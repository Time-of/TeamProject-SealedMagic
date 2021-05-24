using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: 몬스터 기본 움직임
나중에 할 것:
몬스터 사망 시 알파값 낮아지게 하기
*/

public class Monster : MonoBehaviour
{
	[Tooltip("적이 움직이는 속도")]
	[SerializeField] float moveSpeed;
	[Tooltip("적의 체력")]
	public float life;
	[Tooltip("몬스터가 스킬을 가지고 있는가?")]
	[SerializeField] bool canUseSkill;
	[Tooltip("사용하는 스킬의 인덱스 번호 (int)")]
	[SerializeField] int skillIndex;
	[Tooltip("사용하는 스킬의 재사용 대기시간 (float)")]
	[SerializeField] float skillCooldown;
	[Tooltip("일반 공격 피해량 (float)")]
	[SerializeField] float normalAttackDamage;
	[Tooltip("일반 공격 재사용 대기시간 (float)")]
	[SerializeField] float normalAttackCooldown;
	[Tooltip("일반 공격 사거리 (float)")]
	[SerializeField] float normalAttackRange;
	[Tooltip("제단을 해제하기 위한 몬스터인지?")]
	[SerializeField] bool isColoredMonster;
	[Tooltip("몬스터 공격 범위 설정 오브젝트")]
	[SerializeField] GameObject monsterAttackArea;

	Animator monsterAnim;
	Rigidbody2D rigid;
	SpriteRenderer spRenderer;

	[Tooltip("빈 칸으로 놔둘 것")]
	public GameObject traceTarget;
	
	[Tooltip("체크하지 말 것")]
	public bool isTracing = false;
	bool isPlayerInAttackRange = false;
	bool isDead = false;
	int moveDir = 1;
	int whereToAtk = 1;

	bool canSkill = true; // 현재 스킬 사용 가능한지 여부
	bool canAttack = true;

	bool isAttacking = false;

	void Awake()
	{
		monsterAnim = gameObject.GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		spRenderer = GetComponent<SpriteRenderer>();

		Invoke("Think", Random.Range(3, 6));
	}

	void FixedUpdate()
	{
		CheckRaycast();
		Move();
	}

	void CheckRaycast()
	{
		if (!isDead)
		{
			LayerMask plform = new LayerMask();
			plform = LayerMask.GetMask("Platform");
			LayerMask spLayer = new LayerMask();
			spLayer = LayerMask.GetMask("Spikes");
			LayerMask plLayer = new LayerMask();
			plLayer = LayerMask.GetMask("Player");

			Vector2 front = new Vector2(rigid.position.x + moveDir, rigid.position.y);
			RaycastHit2D rayPlatform = Physics2D.Raycast(front, Vector2.down, 1f, plform.value);
			RaycastHit2D raySpikes = Physics2D.Raycast(front, Vector2.down, 1f, spLayer.value);
			Debug.DrawRay(front, Vector2.down, Color.red);

			if (moveDir == 1) whereToAtk = 1;
			else if (moveDir == -1) whereToAtk = -1;
			RaycastHit2D rayPlayer = Physics2D.Raycast(transform.position, new Vector2(whereToAtk, 0), normalAttackRange, plLayer.value);
			Debug.DrawRay(transform.position, new Vector2(whereToAtk, 0) * normalAttackRange, Color.green);

			if (!rayPlatform.collider || raySpikes.collider)
			{
				moveDir = moveDir * -1;
				CancelInvoke();
				Invoke("Think", 2);
			}
			else if (isTracing && !isPlayerInAttackRange && !isAttacking)
			{
				Vector2 plPos = traceTarget.transform.position;

				if (plPos.x < transform.position.x)
				{
					moveDir = -1;
				}
				else if (plPos.x >= transform.position.x)
				{
					moveDir = 1;
				}
			}
			else if (isPlayerInAttackRange || isAttacking)
				moveDir = 0;

			if (rayPlayer.collider && !isPlayerInAttackRange)
			{
				isPlayerInAttackRange = true;
				
				StartCoroutine("SkillAttack");
			}
			else if (!rayPlayer.collider && !isAttacking)
			{
				isPlayerInAttackRange = false;
			}
		}
	}
	
	void Move()
	{
		if (moveDir != 0 && !isDead)
		{
			spRenderer.flipX = moveDir == -1 ? true : false;
			monsterAnim.SetBool("isWalk", true);
		}
		else if (moveDir == 0)
			monsterAnim.SetBool("isWalk", false);


		if (!isDead && !isAttacking && !isPlayerInAttackRange)
		{
			rigid.velocity = new Vector2(moveDir, rigid.velocity.y);
		}
		else if (isPlayerInAttackRange || isAttacking || isDead)
		{
			rigid.velocity = Vector2.zero;
		}
	}

	void Think()
	{
		moveDir = Random.Range(-1, 2);
		float ThinkTime = Random.Range(2f, 5f);
		Invoke("Think", ThinkTime);
	}

	public void onAttack(float damage)
	{
		life -= damage;
		StartCoroutine("onDamaged");
		if (life <= 0 && !isDead)
		{
			isDead = true;
			monsterAnim.SetBool("isWalk", false);
			monsterAnim.SetBool("isAttack", false);
			monsterAnim.SetTrigger("isDeath");
			if (isColoredMonster)
			{
				GameManager gameManager = FindObjectOfType<GameManager>();
				gameManager.killedColoredMonster += 1;
			}
			Destroy(gameObject, 1f);
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Bullet")
			onAttack(1); // onAttack 사용 예시. 마법이나 일반 공격에서 monster의 onAttack을 불러올것,,
	}

	IEnumerator onDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);
		rigid.AddForce(Vector2.up * 1.4f, ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.5f);
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	IEnumerator SkillAttack()
	{
		moveDir = 0;
		if (canUseSkill && canSkill)
		{
			isAttacking = true;
			
			Debug.Log("스킬 사용!");
			yield return new WaitForSeconds(1f);
			isAttacking = false; // MonsterSkill.Skill(0);

			StartCoroutine("CoolingSkill");
		}
		else if (!canUseSkill || !canSkill)
		{
			if (canAttack)
			{
				isAttacking = true;
				
				Debug.Log("일반공격 사용!");
				yield return new WaitForSeconds(1f);
				StartCoroutine("normalAttack");

				StartCoroutine("CoolingAttack");
			}
		}
	}

	IEnumerator CoolingSkill()
	{
		canSkill = false;
		yield return new WaitForSeconds(skillCooldown);
		canSkill = true;
		Debug.Log("스킬 쿨 끝");
	}

	IEnumerator CoolingAttack()
	{
		canAttack = false;
		yield return new WaitForSeconds(normalAttackCooldown);
		canAttack = true;
		Debug.Log("일반공격 쿨 끝");
	}

	IEnumerator normalAttack()
	{
		Vector2 front = new Vector2((spRenderer.flipX ? -1 : 1) * (normalAttackRange / 2) + rigid.position.x, rigid.position.y);

		GameObject atkArea = Instantiate(monsterAttackArea, front, Quaternion.identity);
		monsterAnim.SetBool("isAttack", true);

		yield return new WaitForSeconds(0.5f);

		Destroy(atkArea);
		monsterAnim.SetBool("isAttack", false);
		isAttacking = false;
	}

	/*
	void isDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);
		rigid.AddForce(Vector2.up * 1.4f, ForceMode2D.Impulse);
		Invoke("DamagedToNormal", 0.5f);
	}

	void DamagedToNormal()
	{
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	void CooldownSkill()
	{
		canSkill = false;
		skCool -= Time.deltaTime;

		if (skCool <= 0f)
		{
			Debug.Log("스킬 쿨 끝!");
			canSkill = true;
			skCool = skillCooldown;
			//coolingSkill = false;
		}
	}

	void CooldownNormalAttack()
	{
		canAttack = false;
		atkCool -= Time.deltaTime;

		if (atkCool <= 0f)
		{
			Debug.Log("일반공격 쿨 끝!");
			canAttack = true;
			atkCool = normalAttackCooldown;
			//coolingAtk = false;
		}
	}

	void UseSkill()
	{
		if (canUseSkill && canSkill)
		{
			isAttacking = true;
			// MonsterSkill.Skill(0);
			Debug.Log("스킬 사용!");

			//skCool = skillCooldown;
			//coolingSkill = true;
		}
		else if (!canUseSkill || !canSkill)
		{
			if (canAttack)
			{
				isAttacking = true;
				Invoke("NormalAttack", 0.1f);
				Debug.Log("일반공격 사용!");

				//atkCool = normalAttackCooldown;
				//coolingAtk = true;
			}
		}
	}
	
	void NormalAttack()
	{
		Vector2 front = new Vector2((spRenderer.flipX ? -1 : 1) * (normalAttackRange/2) + rigid.position.x, rigid.position.y);

		GameObject atkArea = Instantiate(monsterAttackArea, front, Quaternion.Euler(0,0,0));
		Destroy(atkArea, 0.5f);
		
		monsterAnim.SetBool("isAttack", true);
		Invoke("EndAttack", 0.5f);
	}


	void EndAttack()
	{
		monsterAnim.SetBool("isAttack", false);
		isAttacking = false;
	}
	*/
}
