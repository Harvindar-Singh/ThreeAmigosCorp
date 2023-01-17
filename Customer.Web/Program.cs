using Customer.Web.Product.Services;
using Auth0.AspNetCore.Authentication;
using Customer.Web.Services.Values;

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetValue<bool>("Services:Values:UseFake", false))
{
    builder.Services.AddTransient<IValuesService, FakeValuesService>();
}
else
{
    builder.Services.AddHttpClient<ValuesService>();  
    builder.Services.AddTransient<IValuesService, ValuesService>();
}

builder.Services.AddAuth0WebAppAuthentication(options => {
    options.Domain = builder.Configuration["Auth:Domain"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
});

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IProductServices, FakeProductServices>();
}
else
{
    builder.Services.AddHttpClient<IProductServices, ProductServices>();
}
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
