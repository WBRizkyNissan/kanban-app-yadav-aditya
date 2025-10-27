using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using KanbanAPI.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddOpenApi();

//CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// EF core + MySQL connection
var cs =  builder.Configuration.GetConnectionString("KanbanDb");
builder.Services.AddDbContext<KanbanDbContext>(opt =>
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Use the CORS policy
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.MapGet("/api/health", () => Results.Ok("Kanban API is running"));

app.Run();