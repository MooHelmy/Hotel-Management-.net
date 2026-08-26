using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddServices(builder.Configuration);// Configure the HTTP request pipeline.
builder.Services.AddControllers();

var app = builder.Build();
await IdentitySeeder.SeedRolesAndAdminAsync(app.Services);
if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseGlobalExceptionHandling();
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
