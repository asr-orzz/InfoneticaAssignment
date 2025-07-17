using WorkflowEngine.Models;
using WorkflowEngine.Services;

namespace WorkflowEngine.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void MapWorkflowEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapPost("/definitions", (WorkflowDefinition def, WorkflowService svc) =>
        {
            var (ok, error, created) = svc.CreateDefinition(def);
            return ok ? Results.Created($"/api/definitions/{created!.Id}", created)
                      : Results.BadRequest(error);
        });

        group.MapGet("/definitions", (WorkflowService svc)
            => Results.Ok(svc.AllDefinitions()));

        group.MapGet("/definitions/{id}", (string id, WorkflowService svc)
            => svc.GetDefinition(id) is { } d ? Results.Ok(d) : Results.NotFound());

        group.MapPost("/definitions/{id}/instances", (string id, WorkflowService svc) =>
        {
            var instance = svc.StartInstance(id);
            return instance is null
                ? Results.NotFound("Invalid or uninitialized definition.")
                : Results.Created($"/api/instances/{instance.Id}", instance);
        });

        group.MapPost("/instances/{instId}/actions/{actionId}", (string instId, string actionId, WorkflowService svc) =>
        {
            var (ok, error, inst) = svc.ExecuteAction(instId, actionId);
            return ok ? Results.Ok(inst) : Results.BadRequest(error);
        });

        group.MapGet("/instances/{id}", (string id, WorkflowService svc) =>
        {
            return svc.GetInstance(id) is { } i ? Results.Ok(i) : Results.NotFound();
        });
    }
}
