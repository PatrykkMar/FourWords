using Microsoft.EntityFrameworkCore;
using FourWords.Server.Context;
using FourWords.Server.Hubs;
using FourWords.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

// Register application services
builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IRoomService, RoomService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FourWordsDBContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<FourWordsHub>("/fourwords");

app.Run();
