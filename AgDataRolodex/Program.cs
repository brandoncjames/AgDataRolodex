using AgDataRolodex.Domain.Data.DataContext;
using AgDataRolodex.Domain.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using AgDataRolodex.Contracts.DTO;
using FluentValidation;
using AgDataRolodex.Domain.Managers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// making our API an API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

//DbContexts
builder.Services.AddDbContext<ContactDb>(opt => opt.UseInMemoryDatabase("Contacts"));
builder.Services.AddDbContext<ContactNameDb>(opt => opt.UseInMemoryDatabase("ContactNames"));
builder.Services.AddDbContext<ContactAddressDb>(opt => opt.UseInMemoryDatabase("ContactAddresses"));

//repositories
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactNameRepository, ContactNameRepository>();
builder.Services.AddScoped<IContactAddressRepository, ContactAddressRepository>();

//managers
builder.Services.AddScoped<IContactManager, ContactManager>();

//validators
builder.Services.AddScoped<IValidator<ContactDTO>, ContactDTOValidator>();

//cache
builder.Services.AddMemoryCache();
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// activate swagger.
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgDataRolodex");
    c.RoutePrefix = "";
    });
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
// finally, run the app
app.Run();