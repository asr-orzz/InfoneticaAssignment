using WorkflowEngine.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddWorkflowEngine();

var app = builder.Build();

app.MapGet("/", () => "Workflow Engine is running 🚀");
app.MapWorkflowEndpoints();

app.Run();
