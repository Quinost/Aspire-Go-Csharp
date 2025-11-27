using Aspire.AppHost;
using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "guest", secret: true);
var password = builder.AddParameter("password", "guest", secret: true);

var rabbitmq = builder.AddRabbitMQ(Constants.RabbitMqName, username, password)
    .WithManagementPlugin();

var rabbitConnectionString = rabbitmq.Resource.ConnectionStringExpression;

Console.WriteLine($"Go service path: {Path.GetFullPath("../../../../Services")}");

var api = builder.AddProject<Projects.API>(Constants.APIName)
    .WaitFor(rabbitmq)
    .WithEnvironment("RabbitMq__ConnectionString", rabbitConnectionString);

var go = builder.AddContainer("go-service", "go-service")
    .WithDockerfile("../../../../Services", "dockerfile")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WaitFor(api)
    .WithHttpEndpoint(port: 8080, targetPort: 8080, name: "http");

//var goService = builder.AddExecutable("go-service", "wsl", ".",
//        args: ["bash", "-c", "cd '/mnt/d/3.Sources/GO + C#/Services/cmd' && go run ."]);

builder.Build().Run();
