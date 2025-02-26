using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
	#region State
	
	public DB_BattleState battleState { get; private set; }
	public DB_AttackState attackState { get; private set; }
	public DB_IdleState idleState { get; private set; }
	public DB_DeadState deadState { get; private set; }
	public DB_SpellCastState spellCastState { get; private set; }
	public DB_TeleportState teleportState { get; private set; }

	#endregion
	public bool boosFightBegun;
	
	[Header("Spell Cast details")]
	[SerializeField] private GameObject spellPrefab;
	[SerializeField] private float spellStateCooldown;
	[SerializeField] private Vector2 spellOffset;
	public int amountOfSpels;
	public float spellCooldown;
	
	public float lastTimeCast;
	
	
	[Header("Teleport detail")]
	[SerializeField] private BoxCollider2D arena;
	[SerializeField] private Vector2 surroundingCheckSize;
	public float chanceToTeleport;
	public float defaultChanceToTeleport = 25;
	

	protected override void Awake()
	{
		base.Awake();
		
		SetupDefaultFacingDir(-1);
		
		idleState = new DB_IdleState(this, stateMachine, "Idle", this);
		
		battleState = new DB_BattleState(this, stateMachine, "Move", this);
		attackState = new DB_AttackState(this, stateMachine, "Attack", this);
		
		deadState = new DB_DeadState(this, stateMachine,"Idle", this);
		spellCastState = new DB_SpellCastState(this, stateMachine, "SpellCast", this);
		teleportState = new DB_TeleportState(this, stateMachine, "Teleport", this);
	}

	protected override void Start()
	{
		base.Start();
		
		stateMachine.Initialize(idleState);
	}
	
	public override void Die()
	{
		base.Die();
		stateMachine.ChangeState(deadState);
	}
	
	public void CastSpell()
	{
		Player player = PlayerManager.instance.player;
		
		float xOffset = 0;
		
		if(player.rb.linearVelocity.x != 0)
			xOffset = player.facingDir * spellOffset.x;
		
			
		Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset , player.transform.position.y + spellOffset.y );
		
		GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
		newSpell.GetComponent<DBSpell_Controller>().SetupSpell(stats);
	

	}
	
	public void FindPosition()
	{
		float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
		float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);
		
		transform.position = new Vector3(x,y);
		transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y/2));
		
		if(!GroundBelow() || SomethingIsAround())
		{
			Debug.Log("Looking for new position");
			FindPosition();
		}
	}
	
	private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
	private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0,whatIsGround);

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		
		Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
		Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
	}
	
	public bool CanTeleport()
	{
		if(Random.Range(0,100) <= chanceToTeleport)
		{
			chanceToTeleport = defaultChanceToTeleport;
			return true;
			
		}
				
		return false;
	}
	
	public bool CanDoSpellCast()
	{
		if (Time.time >= lastTimeCast + spellStateCooldown)
		{
			return true;
		}
		return false;
		
	}
	

}
