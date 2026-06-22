using DPAT_Finite_State_Machine.Models;
using DPAT_Finite_State_Machine.Validators;
using Xunit;

namespace DPAT_Finite_State_Machine.Tests;

// Tests for the diagram validators — the core business rules of the FSM tool.
public class ValidatorTests
{
    // ----- DeterminismValidator -----------------------------------------------------------

    [Fact]
    public void Determinism_ReportsError_WhenTwoTransitionsShareTriggerAndGuard()
    {
        var source = Fsm.Simple("s", SimpleStateEnum.Initial);
        var a = Fsm.Simple("a");
        var b = Fsm.Simple("b");
        var trigger = Fsm.Trigger("push");

        // Same trigger AND same guard on two outgoing transitions => non-deterministic.
        var t1 = Fsm.Connect(source, a, trigger, condition: "x > 0");
        var t2 = Fsm.Connect(source, b, trigger, condition: "x > 0");
        var diagram = new Diagram([source, a, b], [t1, t2]);

        var validator = new DeterminismValidator();
        diagram.Accept(validator);

        Assert.False(validator.IsValid);
        Assert.Contains("non-deterministic", Assert.Single(validator.Errors));
    }

    [Fact]
    public void Determinism_IsValid_WhenOutgoingTransitionsHaveDistinctTriggers()
    {
        var source = Fsm.Simple("s", SimpleStateEnum.Initial);
        var a = Fsm.Simple("a");
        var b = Fsm.Simple("b");

        // Different triggers => the choice is deterministic, even with identical guards.
        var t1 = Fsm.Connect(source, a, Fsm.Trigger("push"), condition: "x > 0");
        var t2 = Fsm.Connect(source, b, Fsm.Trigger("pull"), condition: "x > 0");
        var diagram = new Diagram([source, a, b], [t1, t2]);

        var validator = new DeterminismValidator();
        diagram.Accept(validator);

        Assert.True(validator.IsValid);
    }

    // ----- ReachableValidator -------------------------------------------------------------

    [Fact]
    public void Reachable_ReportsError_WhenStateCannotBeReachedFromInitial()
    {
        var initial = Fsm.Simple("init", SimpleStateEnum.Initial);
        var reached = Fsm.Simple("reached");
        var orphan = Fsm.Simple("orphan");

        var t = Fsm.Connect(initial, reached);
        var diagram = new Diagram([initial, reached, orphan], [t]);

        var validator = new ReachableValidator();
        diagram.Accept(validator);

        Assert.False(validator.IsValid);
        Assert.Contains("orphan", Assert.Single(validator.Errors));
    }

    [Fact]
    public void Reachable_TreatsSubstate_AsReachable_ThroughCompoundParent()
    {
        // subB has no direct edge but is reachable via subA's parent compound.
        var initial = Fsm.Simple("init", SimpleStateEnum.Initial);
        var subA = Fsm.Simple("subA");
        var subB = Fsm.Simple("subB");
        var compound = Fsm.Compound("comp", subStates: [subA, subB]);

        var t = Fsm.Connect(initial, subA);
        var diagram = new Diagram([initial, compound, subA, subB], [t]);

        var validator = new ReachableValidator();
        diagram.Accept(validator);

        Assert.True(validator.IsValid);
    }

    [Fact]
    public void Reachable_ReportsError_WhenConditionalTransitionIsShadowedByUnconditionalOne()
    {
        var source = Fsm.Simple("s", SimpleStateEnum.Initial);
        var always = Fsm.Simple("always");
        var guarded = Fsm.Simple("guarded");

        // An unconditional transition always fires first, so the guarded one is dead code.
        var unconditional = Fsm.Connect(source, always);
        var conditional = Fsm.Connect(source, guarded, condition: "x > 0");
        var diagram = new Diagram([source, always, guarded], [unconditional, conditional]);

        var validator = new ReachableValidator();
        diagram.Accept(validator);

        Assert.Contains(validator.Errors, e => e.Contains("unreachable") && e.Contains("guarded"));
    }

    // ----- InitialFinalStateValidator -----------------------------------------------------

    [Fact]
    public void InitialFinal_ReportsError_WhenNoInitialStateExists()
    {
        var only = Fsm.Simple("a");
        var final = Fsm.Simple("end", SimpleStateEnum.Final);
        var diagram = new Diagram([only, final], []);

        var validator = new InitialFinalStateValidator();
        diagram.Accept(validator);

        Assert.Contains(validator.Errors, e => e.Contains("no initial state"));
    }

    [Fact]
    public void InitialFinal_ReportsError_WhenMultipleInitialStatesExist()
    {
        var first = Fsm.Simple("i1", SimpleStateEnum.Initial);
        var second = Fsm.Simple("i2", SimpleStateEnum.Initial);
        var final = Fsm.Simple("end", SimpleStateEnum.Final);
        var diagram = new Diagram([first, second, final], []);

        var validator = new InitialFinalStateValidator();
        diagram.Accept(validator);

        Assert.Contains(validator.Errors, e => e.Contains("2 initial states"));
    }

    [Fact]
    public void InitialFinal_ReportsError_WhenFinalStateHasOutgoingTransition()
    {
        var initial = Fsm.Simple("init", SimpleStateEnum.Initial);
        var final = Fsm.Simple("end", SimpleStateEnum.Final);

        // A final state is an end state and may not have outgoing transitions.
        var illegal = Fsm.Connect(final, initial);
        var diagram = new Diagram([initial, final], [illegal]);

        var validator = new InitialFinalStateValidator();
        diagram.Accept(validator);

        Assert.Contains(validator.Errors, e => e.Contains("outgoing transitions"));
    }

    // ----- TransitionTargetValidator ------------------------------------------------------

    [Fact]
    public void TransitionTarget_ReportsError_WhenTransitionTargetsUnknownState()
    {
        var source = Fsm.Simple("s", SimpleStateEnum.Initial);
        var ghost = Fsm.Simple("ghost"); // referenced by the transition but absent from the diagram

        var dangling = Fsm.Connect(source, ghost);
        var diagram = new Diagram([source], [dangling]);

        var validator = new TransitionTargetValidator();
        diagram.Accept(validator);

        Assert.Contains("unknown state", Assert.Single(validator.Errors));
    }

    [Fact]
    public void TransitionTarget_ReportsError_WhenTransitionTargetsInitialState()
    {
        var initial = Fsm.Simple("init", SimpleStateEnum.Initial);
        var other = Fsm.Simple("other");

        // Transitioning back into an initial state is not allowed.
        var illegal = Fsm.Connect(other, initial);
        var diagram = new Diagram([initial, other], [illegal]);

        var validator = new TransitionTargetValidator();
        diagram.Accept(validator);

        Assert.Contains("initial state", Assert.Single(validator.Errors));
    }
}
