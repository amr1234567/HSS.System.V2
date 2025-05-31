using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace HSS.System.V2.Presentation.Helpers
{
    public class SwaggerConfigration : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions c)
        {
            c.SwaggerDoc("PatientAPI", new OpenApiInfo { Title = "Patient API", Version = "v1" });
            c.SwaggerDoc("RecpetionAPI", new OpenApiInfo { Title = "Recpetion API", Version = "v1" });
            c.SwaggerDoc("EmployeeAuthAPI", new OpenApiInfo { Title = "Employee Auth API", Version = "v1" });
            c.SwaggerDoc("ClinicAPI", new OpenApiInfo { Title = "Clinic API", Version = "v1" });
            c.SwaggerDoc("RadiologyAPI", new OpenApiInfo { Title = "Radiology API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            c.OperationFilter<SwaggerCustomHeaderAttribute>();

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}


                    }
                });
            string xmlPath1 = Path.Combine(Environment.CurrentDirectory, "HSS.xml");

            c.IncludeXmlComments(xmlPath1);

        }
    }

    public class SwaggerUIConfiguration : IConfigureOptions<SwaggerUIOptions>
    {
        public void Configure(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/PatientAPI/swagger.json", "Patient API V1");
            options.SwaggerEndpoint("/swagger/RecpetionAPI/swagger.json", "Reception API V1");
            options.SwaggerEndpoint("/swagger/EmployeeAuthAPI/swagger.json", "Employee Auth API V1");
            options.SwaggerEndpoint("/swagger/ClinicAPI/swagger.json", "Clinic API V1");
            options.SwaggerEndpoint("/swagger/RadiologyAPI/swagger.json", "Radiology API V1");
        }
    }


    public class SwaggerCustomHeaderAttribute : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                },
                Example = new OpenApiString("ar"),
            });
        }
    }

}
