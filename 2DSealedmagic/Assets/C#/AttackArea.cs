using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

작성 의도: 플레이어-적군 상호 간의 피격 상호작용을
일괄적으로 관리할 수 있는 스크립트를 만들고자 하였다. (마스터 스크립트?)

설명: '공격 판정' 이 있는 오브젝트에 들어가는 스크립트로 Trigger가 만나면 작동

사용법:	공격판정 오브젝트에 AttackArea 삽입,
		타 스크립트의 공격부분에서 GetComponent<AttackArea>
		데미지랑 누구공격인지 넣을것
		isTrigger를 꼭 체크하기 바람
*/

public class AttackArea : MonoBehaviour
{
	// true일 경우 적의 공격, false일 경우 플레이어의 공격
	[HideInInspector] public bool isEnemyAttack;
	// 생성 함수로부터 데미지를 받을것
	[HideInInspector] public float damage;

	void OnTriggerEnter2D(Collider2D collision)
	{
		// 플레이어의 공격일 경우
		if (!isEnemyAttack)
		{
			if (collision.gameObject.tag == "Enemy")
			{
				Monster mob = collision.GetComponent<Monster>();

				if (mob != null)
				{
					mob.onAttack(damage);

					Destroy(gameObject);
				}
			}
		}

		// 적군의 공격일 경우
		if (isEnemyAttack)
		{
			if (collision.gameObject.tag == "Player")
			{
				PlayerObject pl = collision.GetComponent<PlayerObject>();

				if (pl != null)
				{
					pl.OnDamage(damage);

					Destroy(gameObject);
				}
			}
		}
	}
}
