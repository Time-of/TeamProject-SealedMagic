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
	[SerializeField] float abilityCooldown;
	[Tooltip("능력 지속시간")]
	[SerializeField] float abilityDuration;
	[Tooltip("능력이 주는 추가 공격력")]
	[SerializeField] float abilityIncreaseDamage;
	[Tooltip("능력이 켜졌을 때 추가 사거리")]
	[SerializeField] float abilityIncreaseRange;

	bool onAbility = false;

	float abCooldown = 0f;
	float atkCooldown = 0f;

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
			if (!isAtk && atkCooldown <= 0f)
			{
				StartCoroutine(Attack());

				StartCoroutine(CooldownAtk(atkDelay));
			}
		}
		isPlayerInAtkRange = false;
	}

	void UseAbility()
	{
		onAbility = true;
		anim.SetBool("onAbility", true);
		StartCoroutine(duringAbility(abilityDuration));
	}

	IEnumerator Attack()
	{
		isAtk = true;
		Vector3 newAtkPos = new Vector3(atkDirection * (atkPos.x + additionalRange) + transform.position.x, atkPos.y + transform.position.y, transform.position.z);

		anim.SetBool("isAttack", true);

		yield return new WaitForSeconds(0.2f);

		GameObject atkArea = Instantiate(atkZone, newAtkPos, Quaternion.identity);
		var area = atkArea.GetComponent<AttackArea>();
		if (area != null)
		{
			area.isEnemyAttack = true;
			area.damage = atkDamage + (onAbility ? abilityIncreaseDamage : 0f);
		}

		GameObject atkFX_S = Instantiate(atkFX_Sprite, newAtkPos, Quaternion.identity);
		GameObject atkFX_P = Instantiate(atkFX_Particle, newAtkPos, Quaternion.identity);
		yield return new WaitForSeconds(0.1f);
		Destroy(atkArea);

		yield return new WaitForSeconds(0.5f);
		Destroy(atkFX_S);
		Destroy(atkFX_P);

		anim.SetBool("isAttack", false);
		isAtk = false;
	}

	IEnumerator duringAbility(float duration)
	{
		float abDur = duration;
		additionalRange = abilityIncreaseRange;
		while (abDur > 0)
		{
			abDur -= 0.5f;
			yield return new WaitForSeconds(0.5f);
			
			if (abDur <= 0.1f)
			{
				onAbility = false;
				additionalRange = 0f;
				anim.SetBool("onAbility", false);
			}
		}
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

	IEnumerator CooldownAtk(float cooldown)
	{
		atkCooldown = cooldown;

		while (atkCooldown > 0)
		{
			atkCooldown -= 0.5f;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
