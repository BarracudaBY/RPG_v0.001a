
using UnityEngine;

public class ArrowController : MonoBehaviour
{
	[SerializeField] private int damage;
	[SerializeField] private string targetLayerName = "Player";
	
	[SerializeField] private float xVelocity;
	[SerializeField] private Rigidbody2D rb;
	
	[SerializeField] private bool canMove;
	[SerializeField] private bool flipped;
	
	private CharacterStats myStats;
	
	private void Update()
	{
		if(canMove)
			rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
	}
	
	public void SetupArrow(float _speed, CharacterStats _myStats)
	{
		xVelocity = _speed;
		myStats = _myStats;
	

	}
	
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
		{
			//col.GetComponent<CharacterStats>()?.TakeDamage(damage);
			
			myStats.DoDamage(col.GetComponent<CharacterStats>());
			
			

			StuckInto(col);
		}
		else if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
			StuckInto(col);
	}

	private void StuckInto(Collider2D col)
	{
		GetComponentInChildren<ParticleSystem>().Stop();
		GetComponent<CapsuleCollider2D>().enabled = false;
		canMove = false;
		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		transform.parent = col.transform;
		
		Destroy(gameObject,Random.Range(5,7));
	}

	public void FlipArrow()
	{
		if(flipped)
		return;
		
		xVelocity = xVelocity * -1;
		flipped = true;
		transform.Rotate(0,180,0);
		targetLayerName = "Enemy";
	}
}
