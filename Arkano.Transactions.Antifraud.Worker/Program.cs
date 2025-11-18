using Arkano.Transactions.Antifraud.Worker.Extentions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWorkerDependencies(builder.Configuration);

IHost host = builder.Build();

await host.RunAsync();


