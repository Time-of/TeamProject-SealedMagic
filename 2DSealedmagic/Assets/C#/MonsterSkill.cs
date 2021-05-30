using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: 몬스터 스킬 스크립트
인덱스에 따라 스킬이 다르게 사용됨
UseSkill(스킬 인덱스, 공격 방향, 스킬 번호(1,2));
*/

public class MonsterSkill : MonoBehaviour
{
	[Header("최대 2개까지만")]
	[Tooltip("공격의 스프라이트")]
	[SerializeField] GameObject[] FX_Sprite = new GameObject[2];
	[Tooltip("공격 범위 오브젝트 프리팹")]
	[SerializeField] GameObject[] Skill_Area = new GameObject[2];
	[Tooltip("공격 이펙트 파티클")]
	[SerializeField] GameObject[] FX_Particle = new GameObject[2];

	[Tooltip("재사용 대기시간")]
	[SerializeField] float[] cooldown = new float[2];
	[Tooltip("사거리")]
	[SerializeField] float[] range = new float[2];

	bool canUse_1 = true;
	bool canUse_2 = true;

	public void UseSkill(int index, int atkDir, int skNum)
	{
		if (canUse_1 && skNum == 1)
		{
			StartCoroutine(Skill(index, atkDir, 1));
		}
		else if (canUse_2 && skNum == 2)
		{
			StartCoroutine(Skill(index, atkDir, 2));
		}
	}

	void DeactiveSkill(int skNum)
	{
		if (skNum == 1) canUse_1 = false;
		else if (skNum == 2) canUse_2 = false;
	}

	IEnumerator Skill(int index, int atkDir, int skNum)
	{
		int ArrayNum = skNum - 1;
		if (index == 0)
		{
			DeactiveSkill(skNum);
			//Debug.Log("skill_index_0");
			Vector2 Pos = new Vector2(atkDir * range[ArrayNum] + transform.position.x, transform.position.y);

			GameObject atkSp = Instantiate(FX_Sprite[ArrayNum], Pos + new Vector2(0, 2f), Quaternion.identity);
			yield return new WaitForSeconds(1.1f);

			atkSp.transform.localPosition = Pos;
			GameObject atkArea = Instantiate(Skill_Area[ArrayNum], Pos, Quaternion.identity);
			GameObject atkFX = Instantiate(FX_Particle[ArrayNum], Pos, Quaternion.identity);
			var area = atkArea.GetComponent<AttackArea>();

			if (area != null)
			{
				area.isEnemyAttack = true;
				area.damage = 300;
			}

			yield return new WaitForSeconds(0.1f);
			Destroy(atkArea);

			yield return new WaitForSeconds(1f);
			Destroy(atkSp);
			Destroy(atkFX);

			StartCoroutine(Cooldown(cooldown[ArrayNum], skNum));
		}
		else if (index == 1)
		{
			DeactiveSkill(skNum);
			Debug.Log("skill_index_1");
			GetComponent<Monster>().OnProtected = true;
			GameObject atkFX = Instantiate(FX_Particle[ArrayNum], transform.position, Quaternion.identity);
			Destroy(atkFX, 1f);
			StartCoroutine(Cooldown(cooldown[ArrayNum], skNum));
		}
	}

	IEnumerator Cooldown(float time, int skNum)
	{
		yield return new WaitForSeconds(time);

		if (skNum == 1)
		{
			canUse_1 = true;
			GetComponent<Monster>().canSkill_1 = true;
		}
		else if (skNum == 2)
		{
			canUse_2 = true;
			GetComponent<Monster>().canSkill_2 = true;
		}
	}
}
