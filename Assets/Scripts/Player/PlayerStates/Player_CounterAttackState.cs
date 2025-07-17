using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat _combat;
    private bool counteredSomebody;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        _combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = _combat.GetCounterRecoveryDuration();
        counteredSomebody = _combat.CounterAttackPerformed();
        
        anim.SetBool("counterAttackPerformed", counteredSomebody);
    }

    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(0, rb.linearVelocity.y);
        
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
        
        if (stateTimer < 0 && counteredSomebody == false)
            stateMachine.ChangeState(player.idleState);
    }
}
