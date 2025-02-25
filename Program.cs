
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Project_Manager.Configuration;
using Project_Manager.Data;
using Project_Manager.Service.Configuration;
using Project_Manager.Service.UserService;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Project_Manager.Service.Configuration.TokenGenerator;
using Project_Manager.Model;
using Project_Manager.Service.ProjectService;
using Project_Manager.Service.IssueService;
using Project_Manager.Service.OrganizationService;
using Project_Manager.Service.UserConfiguration;
using Project_Manager.Service.UserOrganizationService;
using Project_Manager.Service.OrganizationUserService;
using Project_Manager.Service.OrganizationProjectService;
using Project_Manager.Service.AvatarService;
using Amazon.S3;
using Amazon.Runtime;
using Project_Manager.ExternalServices.CloudSetting;
using Project_Manager.Service.CommentService;
using Project_Manager.Service.NoteService;
using Project_Manager.ExternalServices.EmailService;
using Quartz;
using Project_Manager.Service.BackgroundJobs;

namespace Project_Manager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    //as  the converter
                    opt.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen( opt =>
            {
                //this serves as the input type
                opt.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date",
                    Example = new OpenApiString(DateTime.Today.ToString("yyyy-MM-dd"))
                });

                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Project Manager", Version = "v1" });
                opt.EnableAnnotations();
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Input valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id ="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
            );

            builder.Services.AddQuartz( qrt =>
            {
                var jobs = new JobKey("SendReminderJob");
                qrt.AddJob<SendReminderJobs>(opt => opt.WithIdentity(jobs));

                qrt.AddTrigger(opts =>
                {
                    opts
                    .ForJob(jobs)
                    .WithIdentity("SendReminderJobTrigger")
                    .WithSimpleSchedule(x => x
                        .WithInterval(TimeSpan.FromDays(1))
                        .RepeatForever());
                });
            });

            //host the quartz
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            // bind the cloud setting with the json
            var cloudSection = builder.Configuration.GetSection("AWS");
            builder.Services.Configure<YandexCloudSetting>(cloudSection);

            // get the binded section here
            var cloudConfig = cloudSection.Get<YandexCloudSetting>();

            //aws for yandex cloud configuration
            AmazonS3Config configS3 = new AmazonS3Config
            {
                ServiceURL = cloudConfig.ServiceURL,
                ForcePathStyle = true,
            };

            // set the credentials
            var credentials = new BasicAWSCredentials(cloudConfig.AccessKey, cloudConfig.SecretKey);
            AmazonS3Client s3Client = new AmazonS3Client(credentials, configS3);

            // this is to register the aws using this package "dotnet add package AWSSDK.Extensions.NETCore.Setup"
            builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
            builder.Services.AddAWSService<IAmazonS3>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserConfig, UserConfig>();
            builder.Services.AddScoped<IIssueService, IssueService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IAvatarService, AvatarService>();
            builder.Services.AddScoped<INoteService, NoteService>();
            builder.Services.AddScoped<IOrganizationService, OrganizationService>();
            builder.Services.AddScoped<IOrganizationUserService, OrganizationUserService>();
            builder.Services.AddScoped<IOrganizationGroupService, OrganizationGroupService>();
            builder.Services.AddSingleton(s3Client);
            builder.Services.AddScoped<ICloudService, CloudService>();
            builder.Services.AddScoped<IEmailService, EmailService>();


            builder.Services.AddIdentity<User, Role>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                }
            ).AddEntityFrameworkStores<ApplicationDbContext>(); 


            var jwtSection = builder.Configuration.GetSection("JwtBearerToken");
            builder.Services.Configure<JwtBearerSetting>(jwtSection);

            var jwtConfig = jwtSection.Get<JwtBearerSetting>();
            var key = Encoding.ASCII.GetBytes(jwtConfig.SecretKey);

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer( opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidIssuer = jwtConfig.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddCors();

            var app = builder.Build();

            using var serviceScope = app.Services.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();
            await app.ConfigureIdentityAsync();
            await app.ConfigureDefaultAvatar();



            app.MapControllers();

            app.Run();
        }
    }
}
