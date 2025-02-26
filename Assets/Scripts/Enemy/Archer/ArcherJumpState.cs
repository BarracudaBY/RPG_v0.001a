using UnityEngine;

public class ArcherJumpState : EnemyState
{
	private EnemyArcher enemy;
	public ArcherJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}
	
	public override void Enter()
	{
		base.Enter();
		
		rb.linearVelocity = new Vector2(enemy.jumpVelocity.x * -enemy.facingDir,enemy.jumpVelocity.y );
		
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		
		enemy.anim.SetFloat("yVelocitry",rb.linearVelocity.y);
		
		if(rb.linearVelocity.y < 0 && enemy.IsGroundDetected())
		stateMachine.ChangeState(enemy.battleState);
	}
}
