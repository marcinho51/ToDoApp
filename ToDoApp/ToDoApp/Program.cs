using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Services;
using ToDoApp.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<ToDoCreateDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IToDoService, ToDoService>();


builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ToDoDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
