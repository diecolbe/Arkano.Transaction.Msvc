using Arkano.Transactions.Worker.Extentions;
using Arkano.Transactions.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWorkerDependencies(builder.Configuration);

IHost host = builder.Build();

await host.RunAsync();

