using Unity.VisualScripting;
using UnityEngine;

public class DB_SpellCastState : EnemyState
{
	private Enemy_DeathBringer enemy;
	
	private int amoutOfSpells;
	private float spellTimer;
	
	
	public DB_SpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}


	public override void Enter()
	{
		base.Enter();
		
		amoutOfSpells = enemy.amountOfSpels;
		spellTimer =  .5f;
		
	}

	public override void Update()
	{
		base.Update();
		
		spellTimer -= Time.deltaTime;
		
		if(CanCast())
			enemy.CastSpell();
		
		if(amoutOfSpells <= 0)
			stateMachine.ChangeState(enemy.teleportState);
	}

	public override void Exit()
	{
		base.Exit();
		
		enemy.lastTimeCast = Time.time;
	}

	private bool CanCast()
	{
		if(amoutOfSpells > 0 && spellTimer < 0)
		{
			amoutOfSpells--;
			spellTimer = enemy.spellCooldown;
			return true;
		}
		
		return false;
	}

	
}
