using PlatformService.DataContext;
using PlatformService.Extensions;
using PlatformService.SyncDataServices.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices(builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build();

await PrepDb.PrepPopulation(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcPlatformService>();

app.MapGet("/protos/platforms.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});
    
app.Run();