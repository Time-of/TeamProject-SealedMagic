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
	[SerializeField] bool hasSkill;
	[Tooltip("사용하는 스킬의 인덱스 번호 (int)")]
	[SerializeField] int skillIndex;
	//[Tooltip("사용하는 스킬의 재사용 대기시간 (float)")]
	//[SerializeField] float skillCooldown;
	[Tooltip("일반 공격 피해량 (float)")]
	[SerializeField] float normalAttackDamage;
	[Tooltip("일반 공격 재사용 대기시간 (float)")]
	[SerializeField] float normalAttackCooldown;
	[Tooltip("일반 공격 사거리 (float)")]
	[SerializeField] float normalAttackRange;
	[Tooltip("제단을 해제하기 위한 몬스터인지?")]
	[SerializeField] bool isColoredMonster;
	[Tooltip("몬스터 일반 공격 범위 오브젝트")]
	[SerializeField] GameObject monsterAttackArea;
	[Tooltip("몬스터 일반공격 이펙트")]
	[SerializeField] GameObject normalAtkFX;

	[Tooltip("몬스터가 받은 데미지 이미지")]
	public GameObject AttackDamageText;
	[Tooltip("몬스터가 받은 데미지 위치")]
	public Transform hudPos;

	Animator monsterAnim;
	Rigidbody2D rigid;
	SpriteRenderer spRenderer;

	[HideInInspector] public GameObject traceTarget;
	[HideInInspector] public bool isTracing = false;

	bool isPlayerInAttackRange = false;

	bool isDead = false;
	float moveDir = 1;
	float changedSpeed = 1;
	float atkDir;
	float delay = 0f;

	bool canSkill = true; // 현재 스킬 사용 가능한지 여부
	public bool CanSkill
	{
		get
		{
			return canSkill;
		}
		set
		{
			canSkill = value;
		}
	}
	bool canAttack = true;

	bool isAttacking = false;
	bool isStunned = false;

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
		TryAttack();
	}

	// 레이캐스트 체크
	void CheckRaycast()
	{
		if (!isDead || !isStunned)
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

			if (moveDir == 1) atkDir = 1;
			else if (moveDir == -1) atkDir = -1;

			RaycastHit2D rayPlayer = Physics2D.Raycast(transform.position, new Vector2(atkDir, 0), normalAttackRange, plLayer.value);
			Debug.DrawRay(transform.position, new Vector2(atkDir, 0) * normalAttackRange, Color.green);

			if (rayPlayer.collider) isPlayerInAttackRange = true;
			else if (!rayPlayer.collider) isPlayerInAttackRange = false;

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
		}
	}

	// 이동
	void Move()
	{
		if (moveDir != 0 && !isDead && !isStunned)
		{
			spRenderer.flipX = moveDir == -1 ? true : false;
			monsterAnim.SetBool("isWalk", true);
		}
		else if (moveDir == 0 || isStunned)
			monsterAnim.SetBool("isWalk", false);


		if (!isDead && !isAttacking && !isPlayerInAttackRange && !isStunned)
		{
			rigid.velocity = new Vector2(moveDir * moveSpeed * changedSpeed, rigid.velocity.y);
		}
		else if (isPlayerInAttackRange || isAttacking || isDead || isStunned)
		{
			rigid.velocity = Vector2.zero;
		}

		if (isDead)
		{
			gameObject.tag = default;
		}
	}
	
	// 공격을 시도
	void TryAttack()
	{
		if(isPlayerInAttackRange && !isStunned && !isDead)
		{
			delay += Time.deltaTime;

			if (delay >= 1f)
			{
				delay = 0f;
				StartCoroutine("SkillAttack");
			}
		}
		
	}

	// 이동속도 변경 상태 시작
	public void modifySpeed(float speed, float duration)
	{
		if (changedSpeed != 1f)
		{
			StopCoroutine("changeSpeedState");
			StartCoroutine(changeSpeedState(speed, duration));
		}
		else
			StartCoroutine(changeSpeedState(speed, duration));
	}
	
	// 도트 피해 상태 시작
	public void startDotDamage(float damage, float duration)
	{
		StartCoroutine(dotDamageState(damage, duration));
	}

	// 스턴 상태 시작
	public void startStun(float duration)
	{
		if (isStunned)
		{
			StopCoroutine("StunState");
			StartCoroutine(StunState(duration));
		}
		else
			StartCoroutine(StunState(duration));
	}

	// speed 매개변수는 1 = 100%
	// 이동속도가 변경된 상태
	IEnumerator changeSpeedState(float speed, float duration)
	{
		//Debug.Log("1. 코루틴실행");
		int i = 0;
		changedSpeed = speed;

		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			i++;
			//Debug.Log("2. i++, i: " + i);
			if (duration < i)
			{
				changedSpeed = 1f;
				//Debug.Log("3. 속도 원래대로, 기다린 시간: " + i);
			}
		}
	}

	// 도트 피해를 입는 상태
	IEnumerator dotDamageState(float damage, float duration)
	{
		int i = 0;
		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			onAttack(damage);
			i++;
		}
	}

	// 기절 상태
	IEnumerator StunState(float duration)
	{
		int i = 0;
		isStunned = true;
		isAttacking = false;
		StopCoroutine("SkillAttack");
		// GameObject stunFX = Instantiate(FX_Stun, trasform.position + vector3.up*2f, Quaternion.identity);
		// Destroy(stunFX, 2f);
		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			i++;
			if (duration < i)
			{
				isStunned = false;
			}
		}
	}

	// 이동 방향을 생각
	void Think()
	{
		moveDir = Random.Range(-1, 2);
		float ThinkTime = Random.Range(1f, 3f);
		Invoke("Think", ThinkTime);
	}

	// 공격을 받음
	public void onAttack(float damage)
	{
		GameObject AttackText = Instantiate(AttackDamageText);// 데미지 이미지 출력
		AttackText.transform.position = hudPos.position;// 데미지 위치
		AttackText.GetComponent<DmgText>().damage = damage;// 데미지 값 받기

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

	// 피해를 입음 표시
	IEnumerator onDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);
		rigid.AddForce(Vector2.up * 1.4f, ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.5f);
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	// 스킬 공격 (스킬이 쿨타임이라면 기본 공격)
	IEnumerator SkillAttack()
	{
		moveDir = 0;
		if (hasSkill && canSkill)
		{
			isAttacking = true;
			canSkill = false;

			MonsterSkill sk = GetComponent<MonsterSkill>();
			sk.UseSkill(skillIndex, (int)atkDir);
			yield return new WaitForSeconds(1f);
			isAttacking = false;
		}
		else if (!hasSkill || !canSkill)
		{
			if (canAttack)
			{
				isAttacking = true;

				yield return new WaitForSeconds(0.1f);
				StartCoroutine("normalAttack");

				StartCoroutine("CoolingAttack");
			}
		}
	}

	// 스킬 재사용 대기시간 대기
	/*
	IEnumerator CoolingSkill()
	{
		canSkill = false;
		yield return new WaitForSeconds(skillCooldown);
		canSkill = true;
	}*/

	// 공격 재사용 대기시간 대기
	IEnumerator CoolingAttack()
	{
		canAttack = false;
		yield return new WaitForSeconds(normalAttackCooldown);
		canAttack = true;
	}

	// 일반 공격
	IEnumerator normalAttack()
	{
		Vector2 front = new Vector2((spRenderer.flipX ? -1 : 1) * (normalAttackRange / 2 * 1.3f) + transform.position.x, transform.position.y);

		GameObject atkArea = Instantiate(monsterAttackArea, front, Quaternion.identity);
		var area = atkArea.GetComponent<AttackArea>();
		if (area != null)
		{
			area.isEnemyAttack = true;
			area.damage = normalAttackDamage;
		}

		GameObject atkFX = Instantiate(normalAtkFX, front, Quaternion.identity);
		monsterAnim.SetBool("isAttack", true);

		// 공격 판정은 0.1초만 존재
		yield return new WaitForSeconds(0.1f);
		Destroy(atkArea);

		yield return new WaitForSeconds(0.5f);
		Destroy(atkFX);

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
