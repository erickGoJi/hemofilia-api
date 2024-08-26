using api.main.tecnicah.ActionFilter;
using api.main.tecnicah.Mapper;
using AutoMapper;
using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Models.Email;
using biz.main.tecnicah.Repository.Calendario;
using biz.main.tecnicah.Repository.Catalogos;
using biz.main.tecnicah.Repository.Diagnostico;
using biz.main.tecnicah.Repository.ObjectiveSignUp;
using biz.main.tecnicah.Repository.RequestSupport;
using biz.main.tecnicah.Repository.User;
using biz.main.tecnicah.Services.Email;
using biz.main.tecnicah.Services.Logger;
using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Calendario;
using dal.main.tecnicah.Repository.Catalogos;
using dal.main.tecnicah.Repository.Diagnostico;
using dal.main.tecnicah.Repository.ObjectiveSignUp;
using dal.main.tecnicah.Repository.RequestSupport;
using dal.main.tecnicah.Repository.User;
using dal.main.tecnicah.Services.Email;
using dal.main.tecnicah.Services.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<db_HemofiliaContext>(options => options.UseSqlServer(connectionString));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailConfigurations"));

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost")
                .WithOrigins("http://localhost:4200")
                .WithOrigins("http://localhost:8100")
                .WithOrigins("http://demo-minimalist.com")
                .WithOrigins("http://34.237.214.147")
                .WithOrigins("https://my.premierds.com/")
                .WithOrigins("Ionic://localhost")
                .WithOrigins("capacitor://localhost")
                .WithOrigins("http://localhost:63410")
                .WithOrigins("https://4mypatient-dashboard.tartaro.app")
                .WithOrigins("https://4mypatient-dashboard.tartaro.app/")
                .WithOrigins("https://dashboard.4mypatient.com.mx/")
                .WithOrigins("https://dashboard.4mypatient.com.mx")
                .WithOrigins("https://www.dashboard.4mypatient.com.mx")
                .AllowCredentials();
            }));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<ValidationFilterAttribute>();
#region REPOSITORIES
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<ICatConsultorioRepository, CatConsultorioRepository>();
builder.Services.AddTransient<ICatSpecialityRepository, CatSpecialityRepository>();
builder.Services.AddTransient<ICatDayRepository, CatDayRepository>();
builder.Services.AddTransient<ICatScheduleRepository, CatScheduleRepository>();
builder.Services.AddTransient<ICalendarioRepository, CalendarioRepository>();
builder.Services.AddTransient<ICatOptionDiagnosticRepository, CatOptionDiagnosticRepository>();
builder.Services.AddTransient<ICatClaveDiagnosticRepository, CatClaveDiagnosticRepository>();
builder.Services.AddTransient<IResultRepository, ResultRepository>();
builder.Services.AddTransient<IQuizRepository, QuizRepository>();
builder.Services.AddTransient<ICatStateRepository, CatStateRepository>();
builder.Services.AddTransient<IObjectiveSignUpRepository, ObjectiveSignUpRepository>();
builder.Services.AddTransient<ICatRoleRepository, CatRoleRepository>();
builder.Services.AddTransient<IRequestSupportRepository, RequestSupportRepository>();

#endregion
// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddMvc(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.InputFormatters.Add(new XmlSerializerInputFormatter(config));
    config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
    }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "api.main.hemofilia", Version = "v1.0" });
    // Set the comments path for the Swagger JSON and UI.
    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //c.IncludeXmlComments(xmlPath);
    //c.IncludeXmlComments(GetXmlCommentsPathForModels());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.rebel_wings v1"));
//}
app.UseCors("CorsPolicy");
app.UseSwaggerUI(c =>
{
    app.UseSwagger().UseDeveloperExceptionPage();
#if DEBUG
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.main.hemofilia v1");
#else
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.main.hemofilia v1");
    // c.SwaggerEndpoint("/back/api_hemofilia/swagger/v1/swagger.json", "api.main.tecnicah v1");
#endif
});

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

