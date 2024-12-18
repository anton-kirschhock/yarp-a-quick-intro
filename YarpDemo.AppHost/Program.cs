var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.YarpDemo_Proxy>("yarpdemo-proxy");

var app1 = builder.AddYarnApp("app1", "../app1")
 .WithExternalHttpEndpoints();

var app2 = builder.AddYarnApp("app2", "../app2")
 .WithExternalHttpEndpoints()
 .WaitFor(app1);

builder.Build().Run();
