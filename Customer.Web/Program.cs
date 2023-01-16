using Auth0.AspNetCore.Authentication;
using Customer.Web.Product.Services;



var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetValue<bool>("Product:Services:UseFake", false))
{
    builder.Services.AddTransient<IProductServices, FakeProductServices>();
}
else
{
    builder.Services.AddHttpClient<ProductServices>();
    builder.Services.AddTransient<IProductServices, ProductServices>();
}

builder.Services.AddAuth0WebAppAuthentication(options => {
    options.Domain = builder.Configuration["Auth:Domain"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
