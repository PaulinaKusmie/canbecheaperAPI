using System.Runtime;
using canbecheaperAPI.Endpoints;
using canbecheaperAPI.Models;
using canbecheaperAPI.Utility;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var conn = builder.Configuration.GetConnectionString("MySQLConnection");

builder.Services.AddDbContext<DbOliwia230Context>(options =>
    options.UseMySql(conn, ServerVersion.AutoDetect(conn))
);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("Mail"));
builder.Services.AddSingleton<MailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapUserEndpoints();
app.MapProductPriceEndpoints();
app.MapPriceEndpoints();
app.MapProductEndpoints();
app.MapTypeEndpoints();
app.MapUnitEndpoints();

app.Run();


