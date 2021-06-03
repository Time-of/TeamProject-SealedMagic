using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수 (P)
작성 의도:
	보스 몬스터의 '기본' 움직임을 만들고자 하였다.
	구체적인 행동은 자식 스크립트로 만든다. (이 스크립트가 부모)
	따라서 최대한 public의 사용을 자제하였다...(재사용성을 위해)
	몬스터를 만들 때 시행착오를 겪었던 사항들은 발전시켜 사용하였다....
설명:
	보스 몬스터 기본 행동 스크립트
	공격은 자식 오브젝트에서 넣자........
	자식오브젝트에서 적당한 공간에 isPlayerInAtkRange = false를 해줘야한다...
	atkZone은 콜라이더 말고 Scale로 범위를 설정하기 바란다....
*/

public class BossMonster : MonoBehaviour
{
	[Header("능력치")]
	[Tooltip("공격 피해량")]
	[SerializeField] protected float atkDamage;
	//[Tooltip("공격속도")]
	//[SerializeField] float atkSpeed;
	[Tooltip("공격 재사용 대기시간 (0.5초 단위)")]
	[SerializeField] protected float atkDelay;
	[Tooltip("이동속도")]
	[SerializeField] float moveSpeed;
	[Tooltip("최대 생명력")]
	[SerializeField] float maxLife;
	[Tooltip("현재 생명력")]
	[SerializeField] float curLife;

	[Header("범위 및 이펙트")]
	[Tooltip("플레이어 인식 범위 설정")]
	[SerializeField] Vector2 recognizePlayerZone;
	[Tooltip("공격 범위 설정")]
	[SerializeField] protected GameObject atkZone;
	[Tooltip("공격 범위가 생길 지점")]
	[SerializeField] protected Vector2 atkPos;
	[Tooltip("공격 이펙트: 스프라이트")]
	[SerializeField] protected GameObject atkFX_Sprite;
	[Tooltip("공격 이펙트: 파티클")]
	[SerializeField] protected GameObject atkFX_Particle;

	[Header("플레이어를 선택할 것")]
	public LayerMask plCheck;

	[Header("데미지 띄우기")]
	[Tooltip("몬스터가 받은 데미지 이미지")]
	[SerializeField] GameObject AttackDamageText;
	[Tooltip("몬스터가 받은 데미지 위치")]
	[SerializeField] Vector3 hudPos;

	//[Header("버프")]
	//public List<BuffBase> onBuff = new List<BuffBase>();

	//public static BossMonster instance;

	protected Animator anim;
	protected Rigidbody2D rigid;
	protected SpriteRenderer spRenderer;
	PlayerObject traceTarget;
	Vector3 atkZoneVec;

	protected bool isAtk = false;
	//protected bool isAtkCooldown = false;
	bool isTracing = false;
	protected bool isDead = false;
	protected bool isPlayerInAtkRange = false;

	protected bool onStunned = false;

	int moveDirection = 0;
	protected int atkDirection = 0;
	protected float additionalRange = 0f;
	protected float changedSpeed = 1f;

	void Awake()
	{
		anim = gameObject.GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		spRenderer = GetComponent<SpriteRenderer>();
		atkZoneVec = new Vector3(atkZone.transform.localScale.x, atkZone.transform.localScale.y, 0f);

		StartCoroutine(SearchPlayer());
	}

	void FixedUpdate()
	{
		CheckRaycast();
		Move();
	}

	// 자식 오브젝트에서 재정의할 것
	protected virtual void TryAttack()
	{
		
	}

	void CheckRaycast()
	{
		if (!isDead || !onStunned)
		{
			LayerMask plfLayer = new LayerMask();
			plfLayer = LayerMask.GetMask("Platform");
			LayerMask spLayer = new LayerMask();
			spLayer = LayerMask.GetMask("Spikes");

			Vector2 front = new Vector2(transform.position.x + moveDirection, transform.position.y);
			RaycastHit2D rayPlatform = Physics2D.Raycast(front, Vector2.down, 1f, plfLayer.value);
			RaycastHit2D raySpikes = Physics2D.Raycast(front, Vector2.down, 1f, spLayer.value);
			Debug.DrawRay(front, Vector2.down, Color.red);

			if (moveDirection == 1) atkDirection = 1;
			else if (moveDirection == -1) atkDirection = -1;

			if (!rayPlatform.collider || raySpikes.collider)
			{
				moveDirection = moveDirection * -1;
			}
			else if (isTracing && !isAtk)
			{
				Vector2 plPos = traceTarget.transform.position;

				if (plPos.x < transform.position.x)
				{
					moveDirection = -1;
				}
				else if (plPos.x >= transform.position.x)
				{
					moveDirection = 1;
				}
			}
			else if (isAtk || isPlayerInAtkRange)
				moveDirection = 0;
		}
	}

	void Move()
	{
		if (moveDirection != 0 && !isDead && !onStunned)
		{
			spRenderer.flipX = moveDirection == -1 ? true : false;
			anim.SetBool("isWalk", true);
		}
		else if (moveDirection == 0 || onStunned)
			anim.SetBool("isWalk", false);


		if (!isDead && !isAtk && !isPlayerInAtkRange && !onStunned)
		{
			rigid.velocity = new Vector2(moveDirection * moveSpeed * changedSpeed, rigid.velocity.y);
		}
		else if (isPlayerInAtkRange || isAtk || isDead || onStunned)
		{
			rigid.velocity = Vector2.zero;
		}
	}

	IEnumerator SearchPlayer()
	{
		while (!isTracing)
		{
			CheckPlayer();
			yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator SearchInAtkRange()
	{
		while (!isDead && !onStunned)
		{
			if (!isPlayerInAtkRange)
			{
				CheckInAtkRange();
			}
			if (isPlayerInAtkRange)
			{
				TryAttack();
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	protected void CheckInAtkRange()
	{
		Vector3 newAtkPos = new Vector3(atkDirection * (atkPos.x + additionalRange) + transform.position.x, atkPos.y + transform.position.y, transform.position.z);
		Collider2D[] PlayerChk = Physics2D.OverlapBoxAll(newAtkPos, atkZoneVec, 0, plCheck);
		if (PlayerChk.Length == 1)
		{
			isPlayerInAtkRange = true;
		}
	}

	void CheckPlayer()
	{
		Collider2D[] PlayerChk = Physics2D.OverlapBoxAll(transform.position, recognizePlayerZone, 0, plCheck);
		if (PlayerChk.Length == 1)
		{
			traceTarget = FindObjectOfType<PlayerObject>();
			StartCoroutine(SearchInAtkRange());
			isTracing = true;
		}
	}

	public void onAttack(float damage)
	{
		GameObject AttackText = Instantiate(AttackDamageText);// 데미지 이미지 출력
		AttackText.transform.position = transform.position + hudPos;// 데미지 위치
		AttackText.GetComponent<DmgText>().damage = damage;// 데미지 값 받기

		curLife -= damage;
		StartCoroutine("onDamaged");

		if (curLife <= 0 && !isDead)
		{
			isDead = true;
			anim.SetBool("isWalk", false);
			anim.SetBool("isAttack", false);

			StartCoroutine("Die");
		}
	}

	IEnumerator onDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);

		yield return new WaitForSeconds(0.5f);
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	IEnumerator Die()
	{
		float i = 1f;
		while (i >= 0f)
		{
			spRenderer.color = new Color(1, 1, 1, i);
			i -= 0.05f;
			yield return new WaitForSeconds(0.05f);
			if (i <= 0.05f)
			{
				Destroy(gameObject);
			}
		}
	}

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

	public void startDotDamage(float damage, float duration)
	{
		StartCoroutine(dotDamageState(damage, duration));
	}

	public void startStun(float duration)
	{
		if (onStunned)
		{
			StopCoroutine("StunState");
			StartCoroutine(StunState(duration));
		}
		else
			StartCoroutine(StunState(duration));
	}

	IEnumerator changeSpeedState(float speed, float duration)
	{
		int i = 0;
		changedSpeed = speed;

		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			i++;

			if (duration < i)
			{
				changedSpeed = 1f;
			}
		}
	}

	IEnumerator dotDamageState(float damage, float duration)
	{
		int i = 0;

		while (i < duration)
		{
			yield return new WaitForSeconds(1f);
			onAttack(damage);
			i++;
		}
	}

	IEnumerator StunState(float duration)
	{
		int i = 0;
		onStunned = true;
		isAtk = false;

		GameObject stunFX = Instantiate(MobSkillsInfo.instance.FX_Stun, transform.position + Vector3.up * 0.75f, Quaternion.identity);
		Destroy(stunFX, duration);

		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			i++;
			if (duration <= i)
			{
				onStunned = false;
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f);
		Gizmos.DrawWireCube(transform.position, recognizePlayerZone);

		Vector3 newAtkPos = new Vector3(atkDirection * (atkPos.x + additionalRange) + transform.position.x, atkPos.y + transform.position.y, transform.position.z);
		Gizmos.color = new Color(0f, 1f, 1f);
		Gizmos.DrawWireCube(newAtkPos, atkZoneVec);
	}
}
