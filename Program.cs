using IronDomeAPI.Services;
using IronDomeAPI.Middleware.Global ;
using IronDomeAPI.Middleware.Attack;
using IronDomeAPI.Data;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();


app.UseMiddleware<GlobalLoginMiddleware>();

app.UseWhen(
    context =>
        context.Request.Path.StartsWithSegments("/api/attacks"),
    appBuilder =>
    {
       //appBuilder.UseMiddleware<JwtValiaitionMiddleware>();
        appBuilder.UseMiddleware<AttackLoginMiddleware>();
       
    });


app.MapControllers();

app.Run();
