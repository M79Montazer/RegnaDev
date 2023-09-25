using Microsoft.EntityFrameworkCore;
using Regna.Core.Context;
using Regna.Core.IServices;
using Regna.Core.Services;
//using AutoMapper.Extensions.Microsoft.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.WebHost.UseUrls("http://localhost:6969");
builder.Services.AddDbContext<RegnaContext>(options =>
{
    options.UseSqlServer(@"Data Source=(local);Initial Catalog=RegnaCoreDb;Integrated Security = true;TrustServerCertificate=True");
});
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddTransient<IMatchService, MatchService>();
builder.Services.AddTransient<ICoreService, CoreService>();
builder.Services.AddTransient<IAssetService, AssetService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseRouting();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=home}/{action=index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
