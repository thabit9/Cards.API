using Cards.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Zomato API",
                    Version = "v1",
                    Description = "ToDo Api",
                    Contact = new OpenApiContact
                    {
                        Name = "Thabi Tabana",
                        Email = string.Empty,
                        Url = new Uri("https://chedzaictsolutions.co.za/"),
                    },
                });
            } );

//Inject DbContext
builder.Services.AddDbContext<CardContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("CardsConn")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
                // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                c.RoutePrefix = string.Empty;
            });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
