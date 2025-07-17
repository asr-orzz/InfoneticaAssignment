namespace WorkflowEngine.Models;

public class WorkflowDefinition
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<State> States { get; set; } = new();
    public List<ActionTransition> Actions { get; set; } = new();
}
