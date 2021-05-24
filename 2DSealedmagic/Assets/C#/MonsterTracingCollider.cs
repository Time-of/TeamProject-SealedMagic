using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: 몬스터가 쫓아갈 범위를 설정한 오브젝트에 넣을 스크립트
몬스터 스크립트를 부모로 한다.
범위 안에 들어왔다면 1초 후 플레이어를 쫓는다.
★ 이 스크립트를 넣는 오브젝트의 레이어를 반드시 PlayerCheckRange로 변경할 것 ★
★ plz change layer --> "PlayerCheckRange" when this script added ★
(한글로 쓰면 미리보기 창에 ??로 보여서 씀)
*/

public class MonsterTracingCollider : MonoBehaviour
{
	[Tooltip("부모 오브젝트")]
	public Monster monster;
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			monster.traceTarget = collision.gameObject;
			monster.CancelInvoke();
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			monster.isTracing = true;
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			monster.isTracing = false;
			monster.CancelInvoke();
			monster.Invoke("Think", 0.2f);
		}
	}

}
