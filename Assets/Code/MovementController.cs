
using DefaultNamespace;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	private Vector2 speed = Vector2.zero;
	public float acceleration = 0.02f;
	private const float GRAVITY = 0.01f;
	public bool isGrounded = false;
	Collider2D[] overalppingColliders = new Collider2D[10];
	private BoxCollider2D collider;


	private void Awake()
	{
		collider = GetComponent<BoxCollider2D>();
	}

	void LateUpdate ()
	{
		ApplyFriction();
		ApplyGravity();
		ApplyAcceleration();
		if (PressingJump() && isGrounded)
		{
			Jump();
		}
		
		Move();
	}

	private void Move()
	{

		Vector2 postion = transform.position;
		postion += speed * Time.deltaTime * 60f;
		transform.position = postion;


		FireRaycast();
//		int numOfCollisions = GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), overalppingColliders);
//		if (numOfCollisions > 0)
//		{
//			ColliderDistance2D cd2 = GetComponent<Collider2D>().Distance(overalppingColliders[0]);
//			transform.position = transform.position - (Vector3)(cd2.normal*cd2.distance);
//			transform.position = transform.position - (Vector3)GetComponent<Collider2D>().Distance(overalppingColliders[0]).normal;
//			Debug.Log("Distance is "+ GetComponent<Collider2D>().Distance(overalppingColliders[0]).distance );
//			Debug.Log("Normal is "+ GetComponent<Collider2D>().Distance(overalppingColliders[0]).normal.magnitude );
//			
//			OnCollisionEnter2D(overalppingColliders[0]);
////isGrounded
////			for (int i = 0; i < numOfCollisions; i++)
////			{
////				GetComponent<Collider2D>().overalppingColliders[i]
////			}
//		}
//		else
//		{
//			isGrounded = false;
//		}
	}

	private void FireRaycast()
	{
		RaycastSides();
		RaycastDown();
	}
	
	private void RaycastSides()
	{
		Vector2 raycastOrigin = (Vector2)transform.position + collider.offset + (new Vector2(0, collider.size.y / 2f));
		RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.right, collider.size.x / 2f);
		CheckIfCollidingHorizontal(hit, Vector2.right);
		
		hit = Physics2D.Raycast(raycastOrigin, Vector2.left, collider.size.x / 2f);
		CheckIfCollidingHorizontal(hit, Vector2.left);
		
		raycastOrigin = (Vector2)transform.position + collider.offset + (new Vector2(0, -collider.size.y / 2f));
		hit = Physics2D.Raycast(raycastOrigin, Vector2.right, collider.size.x / 2f);
		CheckIfCollidingHorizontal(hit, Vector2.right);
		
		hit = Physics2D.Raycast(raycastOrigin, Vector2.left, collider.size.x / 2f);
		CheckIfCollidingHorizontal(hit, Vector2.left);
	}
	
	
	
	private void CheckIfCollidingHorizontal(RaycastHit2D hit, Vector2 raycastDirection)
	{
		if (hit)
		{
			transform.position = new Vector2(hit.point.x + (collider.size.x/2f*-raycastDirection.x), transform.position.y);
		}
	}

	private void RaycastDown()
	{
		Vector2 raycastOrigin = (Vector2)transform.position + collider.offset + (new Vector2(collider.size.x / 2f,0));
		if (speed.y > 0)
		{
			isGrounded = false;
			return;
		}
		CheckIfGrounded(raycastOrigin);
		if (isGrounded) return;
		
		raycastOrigin = (Vector2)transform.position + collider.offset + (new Vector2(-collider.size.x / 2f,0));
		CheckIfGrounded(raycastOrigin);
	}

	private void CheckIfGrounded(Vector2 raycastOrigin)
	{
		RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, -transform.up, collider.size.y / 2f);
		if (hit && !isGrounded)
		{
			speed.y = 0;
			Debug.Log("omg is grounded " + hit.collider.tag);
			isGrounded = true;
			transform.position = new Vector2(transform.position.x ,hit.point.y + 0.5f);
		}
		else
		{
			isGrounded = false;
		}
	}

	private void ApplyGravity()
	{
		if (!isGrounded)
		{
			speed.y -= GRAVITY * Time.deltaTime * 60f;;
		}
	}

	private void ApplyAcceleration()
	{
		if (PressingRight())
		{
			speed.x += acceleration ;
			if (speed.x > acceleration*10)
			{
				speed.x = acceleration * 10;
			}
		}
		else if (PressingLeft())
		{
			speed.x -= acceleration ;
			
			if (speed.x < -acceleration*10)
			{
				speed.x = -acceleration * 10;
			}
		}
	}

	private void ApplyFriction()
	{
		if (speed.x != 0)
		{
			if (speed.x < 0)
			{
				speed.x += acceleration/2f;
			}
			else if (speed.x > 0)
			{
				speed.x -= acceleration/2f ;
			}
			if (Mathf.Abs(speed.x) < 0.01f)
			{
				speed.x = 0;
			}
		}
	}
	
	private void Jump()
	{
		Debug.Log("jump");
		speed.y = 0.3f;
	}

	private bool PressingJump()
	{
		return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
	}

	private static bool PressingRight()
	{
		return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
	}
	
	private static bool PressingLeft()
	{
		return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
	}


	private void OnCollisionEnter2D(Collider2D other)
	{
		Debug.Log("Collided with "+other.gameObject.tag);
		if (other.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
			speed.y = 0;
		}
	}
//	private void OnTriggerEnter2D(Collider2D other)
//	{
//		Debug.Log("trigger entered "+other.gameObject.tag);
//	}


//	public override void OnCollision2D(Collider2D other)
//	{
//		Debug.Log("WAAAW Collided with "+other.gameObject.tag);
//	}
}
