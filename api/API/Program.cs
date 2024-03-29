using API;

var builder = WebApplication.CreateBuilder(args);
var corsOrigins = "*";
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath).AddJsonFile("appsettings.json");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy(corsOrigins,
        policy =>
        {
            policy.WithOrigins("*");
            policy.WithMethods("*");
            policy.WithHeaders("*");
        });
});

var app = builder.Build();

await Startup.MigrateDatabase(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();