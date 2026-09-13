using ECommerceStore.AutoMaprConfiguration;
using ECommerceStore.Data;
using ECommerceStore.Errors;
using ECommerceStore.Models;
using ECommerceStore.Repositories;
using ECommerceStore.Repositories.Interfaces;
using ECommerceStore.Services;
using ECommerceStore.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add CORS policy to allow requests from Angular app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("https://marketaaa.netlify.app") // Your Angular URL
              .AllowAnyMethod()                    // Allows GET, POST, PUT, etc.
              .AllowAnyHeader()                    // Allows custom headers
              .AllowCredentials();                 // Required if sending cookies/auth tokens
    });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add Swagger with JWT authentication support
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer YOUR_TOKEN"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// connection with database
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerenceStoreConnection")));

// 2. Identity  this registers UserManager<T>, SignInManager<T>, RoleManager<T>, etc.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password / lockout rules � tune to your needs
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 2;
    options.User.RequireUniqueEmail = false;
})
    .AddEntityFrameworkStores<ECommerceContext>()
    .AddDefaultTokenProviders();

// 3. JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true; // set false only for local http testing
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // don't allow extra grace period on expiry
    };
});


// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

builder.Services.AddHttpContextAccessor(); // needed for building full image URLs in the service

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IproductImageService, ProductImageService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();
// roles seeding
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}
//static files images
app.UseStaticFiles(); // serves wwwroot by default

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseExceptionHandler( _ => { });

app.UseHttpsRedirection();

app.UseRouting(); // Must come BEFORE UseCors

app.UseCors("AllowAngularApp"); // Apply the registered policy

app.UseAuthorization();

app.MapControllers();

app.Run();
