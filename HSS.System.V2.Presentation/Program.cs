using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.DataAccess;
using HSS.System.V2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using HSS.System.V2.Presentation.Helpers;
using Microsoft.AspNetCore.Mvc;
using HSS.System.V2.Services.Seeding;
using HSS.System.V2.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<BaseUrls>(builder.Configuration.GetSection(nameof(BaseUrls)));

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigration>();
builder.Services.AddTransient<IConfigureOptions<SwaggerUIOptions>, SwaggerUIConfiguration>();

builder.Services.AddContextDI(builder.Configuration);
builder.Services.AddServiceLayerDI(builder.Configuration);
builder.Services.AddSwaggerGen();

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
app.UseSwaggerUI(options =>
{
    // ???? ????? ?????? ???????
    //options.InjectStylesheet("/SwaggerCss3.x/theme-feeling-blue.css");
    //options.InjectStylesheet("/SwaggerCss3.x/theme-flattop.css");
    options.InjectStylesheet("/SwaggerCss3.x/theme-material.css");
    //options.InjectStylesheet("/SwaggerCss3.x/theme-monokai.css");
    //options.InjectStylesheet("/SwaggerCss3.x/theme-muted.css");
    //options.InjectStylesheet("/SwaggerCss3.x/theme-newspaper.css");
    // options.InjectStylesheet("/SwaggerCss3.x/theme-outline.css");
});
//}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    using (var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
    {
        await dbContext.Database.MigrateAsync();
        await SeedingData.SeedAsync(dbContext);
    }
}

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
