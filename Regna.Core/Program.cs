using Microsoft.EntityFrameworkCore;
using Regna.Core.Context;
using Regna.Core.IServices;
using Regna.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
