using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
작성: 20181220 이성수(P)

설명: 제단 오브젝트에 사용되는 스크립트
상호작용하면 정신집중 후 여러 버프 획득 후 스테이지 이동
해당 스크립트를 추가할 때 태그를 Altar로 변경할 것
*/

public class Altar : MonoBehaviour
{
	PlayerObject player;

	//[Tooltip("정신집중 완료시 등장할 이펙트 프리팹")]
	//public GameObject EndEffect;

	[Tooltip("집중 시 등장할 이펙트 프리팹")]
	[SerializeField] GameObject concentrateFX;

	[Tooltip("다음 씬으로 넘어가는 지연시간")]
	[SerializeField] float sceneLoadTime = 1f;
	
	// 상호작용 중인가 여부
	[HideInInspector] public bool isInteracting = false;

	[Tooltip("정신집중 시간")]
	[SerializeField] float CompleteTime = 3f;
	[Tooltip("다음 씬 이름을 적을 것")]
	[SerializeField] string nextScene = "";

	float delay = 0f;

	void Start()
	{
		player = FindObjectOfType<PlayerObject>();
	}
	void Update()
	{
		if (isInteracting)
			Interact();
	}
	void Interact()
	{
		concentrateFX.SetActive(true);
		delay += Time.deltaTime;

		// 플레이어 위치가 변하면 정신집중 해제
		if (player.transform.position != PlayerObject.Altarpos)
		{
			delay = 0f;
			isInteracting = false;
			concentrateFX.SetActive(false);
		}

		if (delay >= CompleteTime)
		{
			delay = 0f;
			isInteracting = false;
			concentrateFX.SetActive(false);
			Debug.Log("정신 집중 성공!");

			GameManager.instance.killedColoredMonster = 0;

			PlayerLongAttack.instance.UpAtk += 50f;
			PlayerLongAttack.instance.Atk += 30f;
			player.maxHealth += 200f;
			player.maxMana += 200f;
			player.curHealth += 300f;
			player.curMana += player.maxMana / 2f;

			StartCoroutine("GoNextLevel");
		}
	}

	IEnumerator GoNextLevel()
	{
		GameManager.instance.plMaxHP = player.maxHealth;
		GameManager.instance.plCurHP = player.curHealth;
		GameManager.instance.plMaxMP = player.maxMana;
		GameManager.instance.plCurMP = player.curMana;

		for (int i = 0; i <= 3; i++)
		{
			GameManager.instance.magicCheck[i] = PlayerLongAttack.instance.StageCheck[i];
		}
		GameManager.instance.increasedAtk = PlayerLongAttack.instance.UpAtk;
		GameManager.instance.increasedNormalAtk = PlayerLongAttack.instance.Atk;
		GameManager.instance.Stagenum++;
		yield return new WaitForSeconds(sceneLoadTime);
		SceneManager.LoadScene(nextScene);
	}
	/*
	public IEnumerator Interact()
	{
		alteranim.SetBool("isInteract", true);
		Debug.Log("집중 코루틴 진입");
		
		float delay = 0;
		while(delay <= CompleteTime)
		{
			delay += Time.deltaTime;
			Debug.Log("집중중: " + delay);
			if (player.transform.position != PlayerInteract.pos)
			{
				isInteracting = false;
				alteranim.SetBool("isInteract", false);
				StopCoroutine(intract);
				Debug.Log("집중 코루틴 취소");
			}

			yield return null;
		}

		yield return null;
		Debug.Log("집중 코루틴 성공");
		//GameObject Eff = Instantiate(EndEffect, transform);
		//Destroy(Eff, 2f);
		//gameManager.killedColoredMonster = 0;

		//player.attackDamage += 50f;
		//player.magicDamage += 50f;
		player.maxHealth += 200f;
		player.maxMana += 200f;
		player.curHealth += 300f;
		player.curMana += player.maxMana / 2f;

		isInteracting = false;
		alteranim.SetBool("isInteract", false);
		StartCoroutine("GoNextLevel");
	}

	void GoNextLevel()
	{
		CTime += Time.deltaTime;

		if (CTime >= sceneLoadTime)
		{
			CTime = 0f;
			concentrated = false;
			SceneManager.LoadScene(nextScene);
		}
	}
	*/
}
