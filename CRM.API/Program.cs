using CRM.API.Endpoints;
using CRM.API.Models.DAL;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CRMContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion"));
});

builder.Services.AddScoped<CustomerDAL>();

var app = builder.Build();

app.AddCustomerEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
