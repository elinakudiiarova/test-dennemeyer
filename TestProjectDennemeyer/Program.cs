using Microsoft.EntityFrameworkCore;
using TestProjectDennemeyer.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<TestDennemeyerDbContext>(options =>
    options.UseNpgsql(connectionString));builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TestDennemeyerDbContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.Run();