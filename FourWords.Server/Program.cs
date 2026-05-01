using Microsoft.EntityFrameworkCore;
using FourWords.Server.Context;
using FourWords.Server.Helpers;
using FourWords.Server.Hubs;
using FourWords.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FourWordsPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:54861")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

// Register application services
builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddTransient<IRoomService, RoomService>();

//Helpers
builder.Services.AddTransient<ICardManager, CardManager>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FourWordsDBContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();

app.UseCors("FourWordsPolicy");

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
app.MapHub<FourWordsHub>("/fourwordshub");

app.Run();
