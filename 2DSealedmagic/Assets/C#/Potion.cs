using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: 포션 오브젝트 동작
회복포션일 경우 tag를 HealPotion
마나회복 포션일 경우 tag를 ManaPotion으로 변경 후 사용할 것
*/

public class Potion : MonoBehaviour
{
	[Tooltip("포션 획득시 출력할 이펙트 프리팹")]
	[SerializeField] GameObject potionFx;
	[Tooltip("회복량")]
	[SerializeField] float recoveryAmount;
	[Tooltip("포션종류: HealPotion / ManaPotion")]
	[SerializeField] string potionKinds;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player" && potionKinds == "HealPotion")
		{
			PlayerObject player = FindObjectOfType<PlayerObject>();
			player.curHealth += recoveryAmount;
			//GameObject hfx = Instantiate(potionFx, player.transform);
			//Destroy(hfx, 1f);
			Destroy(gameObject);
		}
		if (collision.tag == "Player" && potionKinds == "ManaPotion")
		{
			PlayerObject player = FindObjectOfType<PlayerObject>();
			player.curMana += recoveryAmount;
			//GameObject mfx = Instantiate(potionFx, player.transform);
			//Destroy(mfx, 1f);
			Destroy(gameObject);
		}
	}
}
