using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.DataAccess;
using HSS.System.V2.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.Services;
using Microsoft.OpenApi.Models;
using HSS.System.V2.Services.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    string xmlPath2 = Path.Combine(Environment.CurrentDirectory, "HSS.xml");

    option.IncludeXmlComments(xmlPath2);

});
builder.Services.AddContextDI(builder.Configuration);
builder.Services.AddServiceLayerDI(builder.Configuration);

#region Auth Services

var jwtSection = builder.Configuration.GetSection(nameof(JwtHelper));
builder.Services.Configure<JwtHelper>(jwtSection);
var jwtConf = new JwtHelper();
jwtSection.Bind(jwtConf);

Console.WriteLine(jwtConf);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //var jwtConf = builder.Configuration.GetValue<JwtHelper>(nameof(JwtHelper))
    //?? throw new Exception("JWT builder.Configuration is missing");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConf.JwtKey))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//using (var scope = app.Services.CreateScope())
//{
//    using (var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
//    {
//        await dbContext.Database.MigrateAsync();
//        await SeedingData.SeedAsync(dbContext);
//    }
//}

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
