using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddReverseProxy()
    .LoadFromMemory(new[]
    {
        new RouteConfig
        {
            RouteId = "app1_route",
            ClusterId = "app1_cluster",
            Match = new RouteMatch
            {
                Path = "/app1/{**catch-all}"
            }
        }

        ,
        new RouteConfig
        {
            RouteId = "app2_route",
            ClusterId = "app2_cluster",
            Match = new RouteMatch
            {
                Path = "/app2/{**catch-all}"
            }
        }
    },
    new[]
    {
        new ClusterConfig
        {
            ClusterId = "app1_cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
            {
                    "destination1",
                    new DestinationConfig {
                        Address =  "http://localhost:4200"
                    }
                }
            }
        },
        new ClusterConfig
        {
            ClusterId = "app2_cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                {
                    "destination1",
                    new DestinationConfig {
                        Address = "http://localhost:4201"
                    }
                }
            }
        }
    });
var app = builder.Build();

app.MapDefaultEndpoints();
app.MapReverseProxy();
app.MapGet("/", () => Results.Redirect("/app2", permanent: true));

app.Run();
