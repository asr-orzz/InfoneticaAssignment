using WorkflowEngine.Models;
using WorkflowEngine.Persistence;
using WorkflowEngine.Services;

namespace WorkflowEngine.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddWorkflowEngine(this IServiceCollection services)
    {
        services.AddSingleton<IRepository<WorkflowDefinition>>(
            _ => new InMemoryRepository<WorkflowDefinition>(def => def.Id));
        services.AddSingleton<IRepository<WorkflowInstance>>(
            _ => new InMemoryRepository<WorkflowInstance>(inst => inst.Id));
        services.AddSingleton<WorkflowService>();
    }
}
