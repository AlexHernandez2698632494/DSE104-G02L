using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehiculosAPI;
using VehiculosAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
builder.Services.AddIdentityCore<Usuario>().AddEntityFrameworkStores<VehiculoDBContext>().AddApiEndpoints();
builder.Services.AddDbContext<VehiculoDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<Usuario>();

app.Run();
