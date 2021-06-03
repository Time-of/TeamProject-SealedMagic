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
		필수: damage, isEnemyAttack 넣을 것
		상태이상인 경우: AttackType, dogDamage or speedModify, duration
		isTrigger를 꼭 체크하기 바람
*/

public class AttackArea : MonoBehaviour
{
	//공격이 상태이상 공격이라면 어떤 공격이 포함되는지 (Fire, Ice, Stun)
	[HideInInspector]public string AttackType;
	// true일 경우 적의 공격, false일 경우 플레이어의 공격
	[HideInInspector] public bool isEnemyAttack;
	// 생성 함수로부터 데미지를 받을것
	[HideInInspector] public float damage;
	// 도트 피해일 경우 도트 피해량
	[HideInInspector] public float dotDamage;
	// 상태이상이 속도를 수정할 경우 속도 조정값
	[HideInInspector] public float speedModify;
	// 상태이상의 지속 시간
	[HideInInspector] public float duration;

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

					if (AttackType == "Ice")
					{
						mob.modifySpeed(speedModify, duration);
					}
					if (AttackType == "Fire")
					{
						mob.startDotDamage(dotDamage, duration);
					}
					if (AttackType == "Stun")
					{
						mob.startStun(duration);
					}

					Destroy(gameObject);
				}
				else
				{
					BossMonster boss = collision.GetComponent<BossMonster>();

					if (boss != null)
					{
						boss.onAttack(damage);

						if (AttackType == "Ice")
						{
							boss.modifySpeed(speedModify, duration);
						}
						if (AttackType == "Fire")
						{
							boss.startDotDamage(dotDamage, duration);
						}
						if (AttackType == "Stun")
						{
							boss.startStun(duration);
						}

						Destroy(gameObject);
					}
				}
			}
		}

		// 적군의 공격일 경우
		if (isEnemyAttack)
		{
			if (collision.gameObject.tag == "Player" )
			{
				PlayerObject pl = collision.GetComponent<PlayerObject>();

				if (pl != null)
				{
					pl.OnDamage(damage);

					if (AttackType == "Ice")
					{
						//pl.modifySpeed(speedModify, duration);
					}
					if (AttackType == "Fire")
					{
						//pl.startDotDamage(dotDamage, duration);
					}
					if (AttackType == "Stun")
					{
						//pl.startStun(duration);
					}

					Destroy(gameObject);
				}
			}
		}
	}
}
