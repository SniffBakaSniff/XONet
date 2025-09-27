var builder = WebApplication.CreateBuilder(args);


// register service
builder.Services.AddSingleton<TicTacToeService>();

// Configure the XONet app.
var app = builder.Build();
app.UseHttpsRedirection();

// Setup the "/" endpoint.
app.MapGet("/", () => "Welcome to XONet.").WithName("GetRoot");

// Automatically find all endpoint mapping methods
var endpointTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => t.IsSealed && t.IsAbstract) // static classes
    .ToList();

foreach (var type in endpointTypes)
{
    var method = type.GetMethod("MapEndpoints", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
    if (method != null)
    {
        method.Invoke(null, new object[] { app });
    }
}

// Run the app.
app.Run();
