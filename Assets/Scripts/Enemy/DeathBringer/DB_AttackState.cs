using UnityEngine;

public class DB_AttackState : EnemyState
{
	private Enemy_DeathBringer  enemy;
	public DB_AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer  _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}
	
	public override void Enter()
	{
		base.Enter();
		
		enemy.chanceToTeleport += 5;
	}

	public override void Exit()
	{
		base.Exit();

		enemy.lastTimeAttacked = Time.time;
	}

	public override void Update()
	{
		base.Update();

		enemy.SetZeroVelocity();

	   

		if (triggerCalled)
		{
			if(enemy.CanTeleport())
				stateMachine.ChangeState(enemy.teleportState);
			else
				stateMachine.ChangeState(enemy.battleState);

		}
	}
	
	
}
