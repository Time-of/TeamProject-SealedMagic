using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: 제단에 있는 Interact 함수에 접근하게 해주는 컴포넌트
기획서 상 상호작용 키 (제단 사용 키): F키 (default)
*/

public class PlayerInteract : MonoBehaviour
{
	Altar obj;
	PlayerObject player;
	public static Vector3 pos;

	void Start()
	{
		player = GetComponent<PlayerObject>();
	}

	void Update()
	{
		KeyEnter();
	}

	void KeyEnter()
	{
		GameManager gameManager = FindObjectOfType<GameManager>();
		if (Input.GetKeyDown(KeyCode.F) && obj != null && gameManager.killedColoredMonster >= 3 && !obj.isInteracting)
		{
			obj.isInteracting = true;
			pos = player.transform.position;
			
			//obj.StartCoroutine(obj.intract);
			//Debug.Log("집중 코루틴 시도");
		}
	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Altar")
		{
			obj = FindObjectOfType<Altar>();
		}
	}
	
	void OnTriggerExit2D(Collider2D collision)
	{
		obj = null;
	}
}
