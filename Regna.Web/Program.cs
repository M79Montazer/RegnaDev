using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Regna.Web.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ICoreApiClient,CoreApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
{
    ContractResolver = new DefaultContractResolver(),
};
app.Run();
