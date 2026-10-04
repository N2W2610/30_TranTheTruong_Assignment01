using _30_TranTheTruong_Assignment01_BackEnd.Models;
using _30_TranTheTruong_Assignment01_BackEnd.Repositories;
using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ---- Data layer ----
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

// ---- Business layer ----
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddSingleton<ITokenService, TokenService>();

// ---- JWT authentication ----
// ---- JWT authentication ----
var jwt = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            ),

            RoleClaimType = "role",
            NameClaimType = "name",

            // Don't use zero while debugging
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        // DEBUG JWT
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine();
                Console.WriteLine("========== JWT AUTH FAILED ==========");
                Console.WriteLine(context.Exception.GetType().Name);
                Console.WriteLine(context.Exception.Message);
                Console.WriteLine("======================================");
                Console.WriteLine();

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine();
                Console.WriteLine("========== JWT AUTH SUCCESS ==========");

                foreach (var claim in context.Principal?.Claims
                         ?? Enumerable.Empty<Claim>())
                {
                    Console.WriteLine(
                        $"{claim.Type} = {claim.Value}");
                }

                Console.WriteLine("======================================");
                Console.WriteLine();

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// ---- Controllers + OData ----
builder.Services
    .AddControllers()
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents("odata", GetEdmModel()));

// ---- Swagger ----
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Assignment01 Backend API",
        Version = "v1"
    });

    // JWT Authentication trong Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ---- Swagger ----
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Assignment01 API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// =====================================================
// ODATA EDM MODEL
// =====================================================

static IEdmModel GetEdmModel()
{
    var odata = new ODataConventionModelBuilder();

    odata.EntitySet<SystemAccount>("SystemAccounts");
    odata.EntitySet<Category>("Categories");
    odata.EntitySet<NewsArticle>("NewsArticles");
    odata.EntitySet<Tag>("Tags");

    // Primary Key
    odata.EntityType<SystemAccount>()
         .HasKey(x => x.AccountId);

    return odata.GetEdmModel();
}