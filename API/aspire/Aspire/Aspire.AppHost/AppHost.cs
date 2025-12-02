using Aspire.AppHost;
using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "guest", secret: true);
var password = builder.AddParameter("password", "guest", secret: true);

var rabbitmq = builder.AddRabbitMQ(Constants.RabbitMqName, username, password)
    .WithManagementPlugin();

var postgres = builder.AddPostgres(Constants.PostgresName)
    .WithPgAdmin(c => c.WithHostPort(5050))
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("jobs");

var rabbitConnectionString = rabbitmq.Resource.ConnectionStringExpression;
var postgresConnectionString = postgres.Resource.ConnectionStringExpression;

var api = builder.AddProject<Projects.API>(Constants.APIName)
    .WaitFor(rabbitmq)
    .WaitFor(postgres)
    .WithEnvironment("DatabaseSql__ConnectionString", postgresConnectionString)
    .WithEnvironment("RabbitMq__ConnectionString", rabbitConnectionString)
    .WithArgs("--migrate");

var go = builder.AddContainer(Constants.GoWorkerpoolName, Constants.GoWorkerpoolName)
    .WithDockerfile("../../../../Services", "dockerfile")
    .WithImageTag("latest")
    .WaitFor(rabbitmq)
    .WaitFor(api)
    .WithEnvironment("RabbitMq__ConnectionString", rabbitConnectionString)
    .WithHttpEndpoint(port: 8080, targetPort: 8080, name: "http");


builder.Build().Run();
