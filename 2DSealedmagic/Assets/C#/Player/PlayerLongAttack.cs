using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLongAttack : MonoBehaviour
{

	[Tooltip("공격이 발사 되는 위치")]
	public Transform firePoint;
	[Tooltip("발사 할 Bullet 와 이펙트 등")]
	public GameObject[] impactEffect;
	[Tooltip("기본 공격/스킬 쿨타임")]
	[SerializeField]private float[] ccurTime;
	

	[Tooltip("스킬 확인")]
	public bool[] SillIcon;
	[Tooltip("스테이지별 스킬 확인")]
	public bool[] StageCheck; 

	[Tooltip("기본 공격/스킬 쿨타임 적용")]
	[SerializeField] private float[] classicTime;
	[Tooltip("공격을 했을때 작동을 안했을땐 이미지나 마나가 실행 안되게 만듬")]
	private bool binCk = false;
	[Tooltip("제단에서 올려준 공격력")]
	public float UpAtk = 0f;

	[Tooltip("Layer 체크")]
	public LayerMask InLayer;
	[Tooltip("땅 hit 크기 설정")]
	public Vector2 size;
	[Tooltip("번개 hit 크기 설정")]
	public Vector2 size2;
	[Tooltip("캐릭터 방향대로 hit 방향전함")]
	public int moveDic = 1;

	public static PlayerLongAttack instance;

	Animator anim;
	PlayerObject player;


	// 공격들을 다 따로 놓은 이유는 쿨타임들을 각자 다르게 하려고 함.
	// 썬더는 아직 구현이 안됬으며, 땅은 따로 처리함.
	void Awake()
	{
		anim = GetComponent<Animator>();
		player = FindObjectOfType<PlayerObject>();
		instance = this;
	}
   
    void Update()
	{
		MoveDic();
		Attack();
		FireAttack();
		IceAttack();
		GroundAttack();
		ThunderAttack();
	}
	void MoveDic()
	{
		if (Input.GetAxisRaw("Horizontal") == -1)
		{
			moveDic = -1;
		}
		else if (Input.GetAxisRaw("Horizontal") == 1)
		{
			moveDic = 1;
		}
	}
	void Attack()
	{
		if (ccurTime[0] <= 0)// 기본 공격
		{
			if (Input.GetButtonDown("Attack")) // "S" Attack (classic)
			{
				anim.SetTrigger("isAttack");
				PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
				playerMovement.bCanMove = false; // 캐릭터 이동 비활성화
				Shoot();
				ccurTime[0] = classicTime[0];
			}
		}
		else
		{
			ccurTime[0] -= Time.deltaTime;
		}
	}
	void FireAttack()
	{
		if (StageCheck[0])
		{
			if (ccurTime[1] <= 0)// 불 공격
			{
				if (Input.GetButtonDown("FireAttack") && player.curMana > 0) // "Z" Attack (fire)
				{
					anim.SetTrigger("isSkill");
					PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
					playerMovement.bCanMove = false; // 캐릭터 이동 비활성화
					FireShoot();
					SillIcon[0] = true;
					player.curMana -= 50;
					ccurTime[1] = classicTime[1];
				}
			}
			else
			{
				ccurTime[1] -= Time.deltaTime;
			}
		}

	}
	void IceAttack()
	{
		if (StageCheck[1])
		{
			if (ccurTime[2] <= 0) // 얼음 공격
			{
				if (Input.GetButtonDown("IceAttack") && player.curMana > 0) // "X" Attack (ice)
				{
					anim.SetTrigger("isSkill");
					PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
					playerMovement.bCanMove = false; // 캐릭터 이동 비활성화
					IceShoot();
					SillIcon[1] = true;
					player.curMana -= 30;
					ccurTime[2] = classicTime[2];
				}
			}
			else
			{
				ccurTime[2] -= Time.deltaTime;
			}
		}
	}
	void GroundAttack()
	{
		if (StageCheck[2])
		{
			if (ccurTime[3] <= 0) // 땅 공격
			{
				if (Input.GetButtonDown("GroundAttack") && player.curMana > 0) // "X" Attack (ice)
				{
					GroudAttackShoot();
					if (binCk == true)
					{
						anim.SetTrigger("isSkill");
						PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
						playerMovement.bCanMove = false; // 캐릭터 이동 비활성화
						SillIcon[2] = true;
						player.curMana -= 50;
						ccurTime[3] = classicTime[3];
						binCk = false;
					}

				}
			}
			else
			{
				ccurTime[3] -= Time.deltaTime;
			}
		}

	}
	void ThunderAttack()
	{
		if (StageCheck[3])
		{
			if (ccurTime[4] <= 0) // 번개 공격
			{
				if (Input.GetButtonDown("ThunderAttack") && player.curMana > 0) // "V" Attack (ice)
				{
					ThunderShoot();
					if (binCk == true)
					{
						anim.SetTrigger("isSkill");
						PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
						playerMovement.bCanMove = false; // 캐릭터 이동 비활성화
						SillIcon[3] = true;
						player.curMana -= 40;
						ccurTime[4] = classicTime[4];
						binCk = false;
					}
				}
			}
			else
			{
				ccurTime[4] -= Time.deltaTime;
			}
		}

	}


	// bullet 생성
	void Shoot()
	{
		Bullet Pos = GetComponent<Bullet>();
		GameObject ShootAtk = Instantiate(impactEffect[0], firePoint.position, firePoint.rotation);
		AttackArea area = ShootAtk.GetComponent<AttackArea>();

		if (area != null)
		{
			area.damage = 20 + UpAtk;
			area.isEnemyAttack = false;
		}
	}

	void FireShoot()
	{
		GameObject FireAtk = Instantiate(impactEffect[1], firePoint.position, firePoint.rotation);
		AttackArea area = FireAtk.GetComponent<AttackArea>();

		if (area != null)
		{
			area.damage = 50 + UpAtk;
			area.isEnemyAttack = false;
			area.AttackType = "Fire";
			area.dotDamage = 5;
			area.duration = 5f;
		}
	}

	void IceShoot()
	{
		GameObject IceAtk = Instantiate(impactEffect[2], firePoint.position, firePoint.rotation);
		AttackArea area = IceAtk.GetComponent<AttackArea>();

		if (area != null)
		{
			area.damage = 30 + UpAtk;
			area.isEnemyAttack = false;
			area.AttackType = "Ice";
			area.speedModify = 0.5f;
			area.duration = 4f;
		}
	}

	// 범위 안에 들어오고 공격 키를 누르면 땅 공격
	void GroudAttackShoot()
	{
		Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + new Vector3(7.5f * moveDic, 0, 0), new Vector2(13, 1f), 0, InLayer);
		foreach (Collider2D i in hit)
		{
			GameObject GroundAtk = Instantiate(impactEffect[3], i.transform.position, Quaternion.identity);
			Instantiate(impactEffect[4], Vector3.up * 1.4f + i.transform.position, Quaternion.identity);
			binCk = true;
			AttackArea area = GroundAtk.GetComponent<AttackArea>();

			if (area != null)
			{
				area.damage = 70 + UpAtk;
				area.AttackType = "Earth";
				area.isEnemyAttack = false;
			}
			break;

		}

	}


	// 범위 안에 들어오고 공격 키를 누르면 번개 공격
	void ThunderShoot()
	{
		Collider2D[] Thit = Physics2D.OverlapBoxAll(transform.position + new Vector3(7.5f * moveDic, 3.5f, 0), new Vector2(13, 10f), 0, InLayer);
		foreach (Collider2D i in Thit)
		{
			GameObject thunderAtk = Instantiate(impactEffect[5], i.transform.position, Quaternion.identity);
			Instantiate(impactEffect[6], Vector3.up * 4.9f + i.transform.position, Quaternion.identity);
			binCk = true;
			AttackArea area = thunderAtk.GetComponent<AttackArea>();

			if (area != null)
			{
				area.damage = 40 + UpAtk;
				area.isEnemyAttack = false;
				area.AttackType = "Stun";
				area.duration = 2f;
			}
		}
	}

	// isAttack 애니메이션이 끝난 후 이동 활성화
	public void OnCanMonve()
	{
		PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
		playerMovement.bCanMove = true;
	}
	// 스킬 범위를 보여주는 용도
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position + new Vector3(7.5f * moveDic, 0, 0), size);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(transform.position + new Vector3(7.5f * moveDic, 3.5f, 0), size2);
	}
}
