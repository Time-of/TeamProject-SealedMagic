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
Arrow: amount, speed
Plate: gameObj
Door: null
SealedStone: HP, index
PotionHP/PotionMP: gameObj(FX), amount, speed

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
	bool isFloating = false;

	Animator anim;
	SpriteRenderer spRenderer;

	Vector2 currentPos;

	float time = 0f;
	float PosY = 0f;

	void Awake()
	{
		anim = GetComponentInParent<Animator>();
		spRenderer = GetComponent<SpriteRenderer>();
	}

	void Start()
	{
		if (ObjectType == "ArrowDispenser")
			StartCoroutine(ArrowDispenser(Direction));
		if (ObjectType == "Arrow")
			StartCoroutine(Arrow());
		if (ObjectType == "PotionHP" || ObjectType == "PotionMP")
		{
			currentPos = transform.position;
			isFloating = true;
		}
	}

	void Update()
	{
		if (isFloating)
		{
			time += Time.deltaTime * speed;
			PosY = Mathf.Sin(time) * 0.3f; // (* 길이;)
			transform.position = currentPos + new Vector2(0, PosY);
		}
	}

	IEnumerator ArrowDispenser(string Dir)
	{
		while (true)
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

	public void SealedStoneOnAttack()
	{
		if (ObjectType == "SealedStone")
		{
			HP -= 1;
			StartCoroutine(onDamaged());

			if (HP <= 0)
			{
				PlayerLongAttack.instance.StageCheck[index] = true;

				Destroy(gameObject);
			}
		}
	}

	IEnumerator onDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);

		yield return new WaitForSeconds(0.3f);
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerObject player = collision.GetComponent<PlayerObject>();
		if (ObjectType == "Plate" && collision.gameObject.tag == "Player" && !onTrigger)
		{
			onTrigger = true;
			anim.SetBool("isDoor", true);
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

		if (ObjectType == "PotionHP" || ObjectType == "PotionMP")
		{
			if (collision.gameObject.tag == "Player")
			{
				PlayerObject pl = FindObjectOfType<PlayerObject>();

				if (ObjectType == "PotionHP")
				{
					pl.curHealth += amount;
					if (pl.curHealth >= pl.maxHealth)
						pl.curHealth = pl.maxHealth;
				}
				else if (ObjectType == "PotionMP")
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
