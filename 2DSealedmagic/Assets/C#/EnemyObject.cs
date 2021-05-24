using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
	public int nextMove;
	public int Hp = 80;

	public Transform[] walkCheck;
	public LayerMask layerMask;

	Rigidbody2D rigid;
	Animator anim;
	SpriteRenderer spriteRenderer;
	CapsuleCollider2D ncollider;


	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		ncollider = GetComponent<CapsuleCollider2D>();


		//Random 처음 한번 호출
		Invoke("Think", 5);

	}

	void Update()
	{
		/*
        // 벽에 충돌 했을떄, Turn
        if(Physics2D.OverlapCircle(walkCheck[1].position, 0.01f, layerMask) ||
            Physics2D.OverlapCircle(walkCheck[2].position, 0.01f, layerMask))
        {
            Turn();
        }*/
	}

	void FixedUpdate()
	{
		// Move
		rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

		//Cliff check
		Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 1f, rigid.position.y);
		Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
		RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));
		if (rayHit.collider != null)
		{
			Turn();
		}

	}

	// random play
	void Think()
	{
		//Random Direction Active
		nextMove = Random.Range(-1, 2);

		// Ememy image Animation
		anim.SetInteger("WalkSpeed", nextMove);

		//Flip Ememy reversal
		if (nextMove != 0)
			spriteRenderer.flipX = nextMove == 1;

		//Random  Random Direction Active (Recursive)
		float nextThinkTime = Random.Range(2f, 5f);
		Invoke("Think", nextThinkTime);
	}

	// turn image reversal
	void Turn()
	{
		nextMove = nextMove * -1;
		//transform.eulerAngles = new Vector3(0, 180, 0);
		spriteRenderer.flipX = nextMove == 1;

		CancelInvoke();// Time stop
		Invoke("Think", 2);// 다시 시작
	}


	public void EnemyDamaged(int damage)
	{
		Hp = Hp - damage;
		rigid.AddForce(new Vector2(1, 0.1f) * 2f, ForceMode2D.Impulse);

		if (Hp == 0)
		{
			//Sprite Alpha
			//spriteRenderer.color = new Color(1, 1, 1, 1);

			//Sprite flipY
			spriteRenderer.flipY = true;

			//Collider Disable
			//ncollider.enabled = false;
			Destroy(gameObject, 0.5f);

			//Die Effect Jump
			rigid.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);

			//Destory
			Invoke("DeActive", 0.4f);

			//Die Animation
			anim.SetTrigger("isDie");
		}

	}

	// Ememy gameObject call 
	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.tag == "Bullet")
		{
			OnDamaged(collision.transform.position);
		}
	}

	void OnDamaged(Vector2 targetPos)
	{
		int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;// Damaged Direction
		rigid.AddForce(new Vector2(dirc, 1) * 6, ForceMode2D.Impulse);
	}

	void DeActive()
	{
		gameObject.SetActive(false);
	}
}
