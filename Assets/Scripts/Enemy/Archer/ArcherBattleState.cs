using UnityEngine;

public class ArcherBattleState : EnemyState
{
	private Transform player;
	private EnemyArcher enemy;
	 private int moveDir;
	public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();

		player = PlayerManager.instance.player.transform;

		if (player.GetComponent<PlayerStats>().isDead)
			stateMachine.ChangeState(enemy.moveState);

		
	}

	public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.safeDiastance)
            {
                if (CanJump())
                    stateMachine.ChangeState(enemy.jumpState);
                //else - добавляем логику бллиииижнего боя 
                //stateMachine.ChangeState(enemy.closeAttackState);

            }

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                stateMachine.ChangeState(enemy.idleState);
        }

        BattleStateFlipControl();

        // enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    private void BattleStateFlipControl()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            enemy.Flip();
    }

    public override void Exit()
	{
		base.Exit();
	}
																									  
	private bool CanAttack()
	{
		if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
		{
			enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
			enemy.lastTimeAttacked = Time.time;
			return true;
		}

		return false;
	}
	
	private bool CanJump()
	{
		if(enemy.GroundBehind() == false || enemy.WallBehind() == true)
			return false;																							
		
		if(Time.time >= enemy.lastTimeJumped + enemy.jumpcooldown)
		{
			
			
			enemy.lastTimeJumped = Time.time;
			return  true;
		}
		
		return false;
	}
	
	
}
