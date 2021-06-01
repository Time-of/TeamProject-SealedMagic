using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수 (P)

설명:
	보스 몬스터 중 하나인 '킹'
    능력을 사용하면 공격력이 증가하고 공격범위도 늘어난다.
*/

public class Boss_King : BossMonster
{
	[Header("보스 몬스터: King")]
	[Tooltip("능력 재사용 대기시간 (0.5초 단위)")]
	public float abilityCooldown;
	[Tooltip("능력 지속시간")]
	public float abilityDuration;

	// 인스펙터뷰 확인용, 나중에 public지울거임
	public bool onAbility = false;

	float abCooldown = 0f;

	// 공격 스크립트 재정의
	protected override void TryAttack()
	{
		if (abCooldown <= 0f)
		{
			UseAbility();
			StartCoroutine(CooldownAbility(abilityCooldown));
		}
		else
		{
			
		}
	}

	void UseAbility()
	{
		
		anim.SetBool("onAbility", true);
	}

	IEnumerator CooldownAbility(float cooldown)
	{
		abCooldown = cooldown;

		while (abCooldown > 0)
		{
			abCooldown -= 0.5f;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
