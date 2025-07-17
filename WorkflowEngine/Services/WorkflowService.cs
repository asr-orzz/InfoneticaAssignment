using WorkflowEngine.Models;
using WorkflowEngine.Persistence;

namespace WorkflowEngine.Services;

public class WorkflowService(
    IRepository<WorkflowDefinition> definitions,
    IRepository<WorkflowInstance> instances)
{
    public (bool ok, string? error, WorkflowDefinition? def) CreateDefinition(WorkflowDefinition def)
    {
        var (isValid, error) = DefinitionValidator.Validate(def);
        if (!isValid) return (false, error, null);

        return (true, null, definitions.Save(def));
    }

    public WorkflowDefinition? GetDefinition(string id) => definitions.Find(id);
    public IEnumerable<WorkflowDefinition> AllDefinitions() => definitions.All();

    public WorkflowInstance? StartInstance(string definitionId)
    {
        var def = definitions.Find(definitionId);
        if (def == null) return null;

        var initial = def.States.FirstOrDefault(s => s.IsInitial && s.Enabled);
        if (initial == null) return null;

        var instance = new WorkflowInstance
        {
            DefinitionId = def.Id,
            CurrentStateId = initial.Id
        };

        return instances.Save(instance);
    }

    public (bool ok, string? error, WorkflowInstance? instance) ExecuteAction(string instanceId, string actionId)
    {
        var inst = instances.Find(instanceId);
        if (inst == null) return (false, "Instance not found", null);

        var def = definitions.Find(inst.DefinitionId)!;
        var action = def.Actions.FirstOrDefault(a => a.Id == actionId);
        if (action == null || !action.Enabled)
            return (false, "Action invalid or disabled", null);

        if (!action.FromStates.Contains(inst.CurrentStateId))
            return (false, "Action not allowed from current state", null);

        var targetState = def.States.FirstOrDefault(s => s.Id == action.ToState && s.Enabled);
        if (targetState == null)
            return (false, "Target state is not enabled or doesn't exist", null);

        var currentState = def.States.First(s => s.Id == inst.CurrentStateId);
        if (currentState.IsFinal)
            return (false, "Cannot act from a final state", null);

        inst.CurrentStateId = targetState.Id;
        inst.History.Add((action.Id, DateTime.UtcNow));
        return (true, null, instances.Save(inst));
    }

    public WorkflowInstance? GetInstance(string id) => instances.Find(id);
    public IEnumerable<WorkflowInstance> AllInstances() => instances.All();
}
