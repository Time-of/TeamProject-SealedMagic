using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: 봉인석이 부숴지면 플레이어가 magicNum번째 마법 사용조건이 해금
현재로써는 근접공격에만 부숴지게 설정됨
★★★★ 삭제 예정인 스크립트 ★★★★
*/
// 아래 //처리된것 지울것

public class SealedStone : MonoBehaviour
{
	[Tooltip("봉인석의 체력")]
	[SerializeField] float HP;
	[Tooltip("부숴지면 활성화 될 마법의 번호 (0~3)")]
	[SerializeField] int magicNum;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Bullet")
		{
			PlayerObject player = FindObjectOfType<PlayerObject>();
			//HP -= player.attackDamage;
			if (HP <= 0)
			{
				//player.canUseMagic[magicNum] = true;
				Destroy(gameObject);
			}
		}
	}
}
