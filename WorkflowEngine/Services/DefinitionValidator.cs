using WorkflowEngine.Models;

namespace WorkflowEngine.Services;

public static class DefinitionValidator
{
    public static (bool IsValid, string? ErrorMessage) Validate(WorkflowDefinition def)
    {
        if (def.States.Count(s => s.IsInitial) != 1)
            return (false, "There must be exactly one initial state.");

        if (def.States.GroupBy(s => s.Id).Any(g => g.Count() > 1))
            return (false, "Duplicate state IDs found.");

        if (def.Actions.GroupBy(a => a.Id).Any(g => g.Count() > 1))
            return (false, "Duplicate action IDs found.");

        var stateIds = def.States.Select(s => s.Id).ToHashSet();

        foreach (var action in def.Actions)
        {
            if (!stateIds.Contains(action.ToState) ||
                !action.FromStates.All(stateIds.Contains))
                return (false, $"Action '{action.Id}' references unknown state.");
        }

        return (true, null);
    }
}
