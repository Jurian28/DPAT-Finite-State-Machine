using DPAT_Finite_State_Machine.Models;
using Action = DPAT_Finite_State_Machine.Models.Action;

namespace DPAT_Finite_State_Machine.Tests;

// Small, explicit builders for assembling state machines inside tests.
internal static class Fsm
{
    public static SimpleState Simple(string id, SimpleStateEnum type = SimpleStateEnum.Simple, string? name = null)
        => new(id, name ?? id, type, new List<Action>());

    public static CompoundState Compound(string id, IEnumerable<IState>? subStates = null, string? name = null)
    {
        var state = new CompoundState(id, name ?? id, new List<Action>());
        if (subStates is not null)
            state.SetSubStates(subStates.ToList());
        return state;
    }

    public static Trigger Trigger(string id) => new(id, id);

    // Wires a transition source -> target, registers it on the source and returns it for the Diagram.
    public static Transition Connect(IState source, IState target, Trigger? trigger = null, string condition = "")
    {
        var transition = new Transition(null, trigger, target, condition);
        source.AddTransition(transition);
        return transition;
    }
}
