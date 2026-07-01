using LaTiendaAPI.Mappings;
using LaTiendaAPI.Models;
using LaTiendaAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// CONTROLADORES
// ==============================
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// ==============================
// SWAGGER
// ==============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==============================
// BASE DE DATOS
// ==============================
builder.Services.AddDbContext<LatiendaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaTienda"));
});

builder.Services.AddDbContext<LatiendaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaTienda"));
});

// ==============================
// AUTOMAPPER
// ==============================
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ==============================
// JWT
// ==============================
builder.Services.AddScoped<JwtService>();

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddAuthorization();

// ==============================
// CORS
// ==============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ==============================
// PIPELINE
// ==============================

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseCors("ReactPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// ==============================
// REDIRECCIÓN A SWAGGER
// ==============================
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();