using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: 오브젝트 (화살 발사기, 문, 발판 등) 스크립트
필요하지 않은 정보는 작성하지 않는다.

봉인석의 index: 사용 가능하게 될 마법의 인덱스 번호 작성 (0~3)

-- 필요한 정보 --
ArrowDispenser: gameObj, Direction
Arrow: amount
Plate: gameObj
Door: null
SealedStone: HP, index
PotionHP/PotionMP: gameObj(FX), amount

-- ObjectType에 오브젝트 종류를 기재 --
화살발사기: ArrowDispenser
화살: Arrow
발판: Plate
문: Door
봉인석: SealedStone
물약: PotionHP, PotionMP
*/

public class Object : MonoBehaviour
{
	[Tooltip("오브젝트 타입을 입력")]
	[SerializeField] string ObjectType;
	[Tooltip("방향에 따라 Right, Left 입력 / 불필요시 작성 안해도 됨")]
	[SerializeField] public string Direction;
	[Tooltip("오브젝트 타입에 따른 오브젝트를 넣을 것")]
	[SerializeField] GameObject gameObj;
	[Tooltip("오브젝트의 피해량, 회복량 등의 수치")]
	[SerializeField] float amount;
	[Tooltip("오브젝트 속도")]
	[SerializeField] float speed;
	[Tooltip("오브젝트 체력 (SealedStone)")]
	[SerializeField] float HP;
	[Tooltip("인덱스 번호")]
	[SerializeField] int index;

	bool onTrigger = false;
	float floatingDir = 0f;

	Vector2 pos;

	void Start()
	{
		if (ObjectType == "ArrowDispenser")
			StartCoroutine(ArrowDispenser(Direction));
		if (ObjectType == "Arrow")
			StartCoroutine("Arrow");
		if (ObjectType == "PotionHp" || ObjectType == "PotionMP")
		{
			pos = transform.position;
			StartCoroutine("SetFloatingDir");
		}
	}

	IEnumerator ArrowDispenser(string Dir)
	{
		while(true)
		{
			yield return new WaitForSeconds(2f);
			GameObject arrow = Instantiate(gameObj, transform.position, Quaternion.identity);
			if (Dir == "Right") arrow.transform.localScale = new Vector3(1, 1, 1);
			else if (Dir == "Left") arrow.transform.localScale = new Vector3(-1, 1, 1);

			if (arrow != null)
				Destroy(arrow, 2f);
		}
	}

	IEnumerator Arrow()
	{
		int dir = (int)transform.localScale.x;
		AttackArea atkArea = GetComponent<AttackArea>();
		atkArea.isEnemyAttack = true;
		atkArea.damage = amount;
		while (true)
		{
			transform.Translate(dir * speed * Time.deltaTime, 0, 0);
			yield return null;
		}
	}
	
	IEnumerator SetFloatingDir()
	{
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(Floater(0.3f));
		while (true)
		{
			floatingDir = 1f;
			yield return new WaitForSeconds(1f);
			floatingDir = -1f;
			yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator Floater(float speed)
	{
		while (true)
		{
			if (floatingDir == 1f)
				transform.position = Vector2.Lerp(transform.position, pos + Vector2.up * 1f, Time.deltaTime * speed);

			if (floatingDir == -1f)
				transform.position = Vector2.Lerp(transform.position, pos, Time.deltaTime * speed);

			yield return null;
		}
	}

	/*
	 * 해당 부분은 HP가 3이고 1씩 감소하는 것으로 기획되어 제거됨
	 * 대체 스크립트가 OnTriggerEnter2D에 작성됨
	public void onAttack(float dmg)
	{
		HP -= dmg;
		if (HP <= 0)
		{
			//Player.CanUseMagic[index] = true;
			//(파괴 이펙트 생성 스크립트 추가)

			Destroy(gameObject);
		}
	}
	*/

	void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerObject player = collision.GetComponent<PlayerObject>();
		if (ObjectType == "Plate" && collision.gameObject.tag == "Player" && !onTrigger)
		{
			onTrigger = true;
			gameObj.SetActive(false);
		}

		if (ObjectType == "Arrow")
		{
			if (collision.gameObject.tag == "Player")
			{
				player.OnDamage(amount); // 트랙 데미지
				Destroy(gameObject);
			}
			
			if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Floor")
			{
				Destroy(gameObject);
			}
		}

		if (ObjectType == "SealedStone")
		{
			if (collision.gameObject.tag == "Attack")
			{
				HP -= 1;
				if (HP <= 0)
				{
					//player.CanUseMagic(index);
					// (있다면)(파괴 이펙트 생성 스크립트)

					Destroy(gameObject);
				}
			}
		}

		if (ObjectType == "PotionHp" || ObjectType == "PotionMP")
		{
			if(collision.gameObject.tag == "Player")
			{
				PlayerObject pl = FindObjectOfType<PlayerObject>();
				
				if(ObjectType == "PotionHp")
				{
					pl.curHealth += amount;
					if (pl.curHealth >= pl.maxHealth)
						pl.curHealth = pl.maxHealth;
				}
				else if(ObjectType == "PotionMP")
				{
					pl.curMana += amount;
					if (pl.curMana >= pl.maxMana)
						pl.curMana = pl.maxMana;
				}

				//GameObject Pfx = Instantiate(gameObj, pl.transform);
				//Destroy(Pfx, 1f);

				Destroy(gameObject);
			}
		}
	}
}
