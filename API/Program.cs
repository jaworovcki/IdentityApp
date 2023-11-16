using API.Data;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//defining out IdentityCore service
builder.Services.AddIdentityCore<User>(options =>
{
    //for password configuration
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    //for email configuration
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddRoles<IdentityRole>() // to be able to add roles
    .AddRoleManager<RoleManager<IdentityRole>>() // to be able to make use of Role Manager
    .AddEntityFrameworkStores<DataContext>() // providing out context
    .AddSignInManager<SignInManager<User>>() // make use of SignIn Manager
    .AddUserManager<UserManager<User>>() // make use of User Manager to create users
    .AddDefaultTokenProviders(); // to be able to crate tokens for email confirmation

builder.Services.AddScoped<JWTService>();

// to be able to authenticate users using JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //validate the token based on the key provided in the appsetting.json
            ValidateIssuerSigningKey = true,
            //the issuer signing key based on the JWT:TokenKey in appsetting.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:TokenKey"])),
            //the issuer is the api project
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            //validate the issuer(who is validate the JWT)
            ValidateIssuer = true,
            ValidateAudience = false,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Authentication verifies the identity of the user
app.UseAuthentication();
//Authorization verifies the user has access to the resource
app.UseAuthorization();

app.MapControllers();

app.Run();