using Microsoft.OpenApi;
using Arkano.Transactions.Aplication.Extentions;
using Arkano.Transactions.Infraestructure.Extentions;
using Arkano.Transactions.Infraestructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddAplicationApi();
builder.Services.AddFabrics();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(documentation =>
{
    documentation.SwaggerDoc("v1", new OpenApiInfo { Title = "Transaction - API", Version = "v1" });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var topicManager = scope.ServiceProvider.GetRequiredService<KafkaTopicManager>();
    await topicManager.CreateTopicsAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction - API");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
