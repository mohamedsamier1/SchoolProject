using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolProject.Core;
using SchoolProject.Core.MiddleWare;
using SchoolProject.Infrustructure;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Service;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Connection To Sql Server
#region Connection To Sql Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(connectionString));
#endregion

#region Dependancy Ingections
builder.Services.AddInfrustructureDependancies()
.AddServiceDependancies()
.AddServiceRegisteration()
.AddCoreDependancies();
#endregion

#region Localization
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(opt =>
{
    opt.ResourcesPath = "";
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    List<CultureInfo> supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("de-DE"),
        new CultureInfo("fr-FR"),
        new CultureInfo("ar-EG")
    };
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ar-EG");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

#endregion

#region AllowCORS
var CORS = "_cors";
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: CORS,
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();

        });
});
#endregion
var app = builder.Build();
app.UseMiddleware<ErrorHandlerMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#region Localization
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);
#endregion


app.UseCors(CORS);

//app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
