var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var configuration = builder.Configuration;

builder.Services.AddCors(opt =>
{
    var allowedHosts = configuration.GetValue<string?>("AllowedHosts");
    var origins = allowedHosts?.Split(';', StringSplitOptions.RemoveEmptyEntries)
      ?? [];

    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
           .WithOrigins(origins);
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
