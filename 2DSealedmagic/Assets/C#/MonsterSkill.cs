using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: 몬스터 스킬 스크립트
*/

public class MonsterSkill : MonoBehaviour
{
	public GameObject FX_Sprite;
	public GameObject Skill_Area;
	public GameObject FX_Particle;

	public float cooldown;
	public float range;

	bool canUse = true;

	public void UseSkill(int index, int atkDir)
	{
		if (canUse)
		{
			Debug.Log("유즈스킬");
			StartCoroutine(Skill(index, atkDir));
		}
	}

	IEnumerator Skill(int index, int atkDir)
	{
		Debug.Log("코루틴진입");

		if (index == 0)
		{
			Debug.Log("인덱스빵 진입");
			canUse = false;

			Vector2 Pos = new Vector2(atkDir * range + transform.position.x, transform.position.y);

			GameObject atkFX = Instantiate(FX_Sprite, Pos, Quaternion.identity);
			GameObject atkArea = Instantiate(Skill_Area, Pos, Quaternion.identity);
			var area = atkArea.GetComponent<AttackArea>();
			Debug.Log("생성해씀");
			if (area != null)
			{
				area.isEnemyAttack = true;
				area.damage = 300;
				Debug.Log("값변경");
			}

			yield return new WaitForSeconds(0.1f);
			Destroy(atkArea);
			Debug.Log("범위제거");

			yield return new WaitForSeconds(1f);
			Destroy(atkFX);
			Debug.Log("이펙제거");

			StartCoroutine(Cooldown(cooldown));
		}

	}

	IEnumerator Cooldown(float time)
	{
		Debug.Log("쿨시작");
		yield return new WaitForSeconds(time);
		canUse = true;
		Monster mob = GetComponent<Monster>();
		mob.CanSkill = true;
		Debug.Log("쿨끝");
	}
}
