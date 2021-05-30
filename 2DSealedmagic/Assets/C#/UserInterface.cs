using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
작성: 20181220 이성수(P)

설명: 게임플레이 UI
*/

public class UserInterface : MonoBehaviour
{
	[Header("UI")]
	[Tooltip("체력 바 UI")]
	[SerializeField] Image healthBar;
	[Tooltip("마나 바 UI")]
	[SerializeField] Image manaBar;
	[Tooltip("메뉴 UI")]
	[SerializeField] GameObject MenuUI;
	[Tooltip("메뉴 후경 이미지 오브젝트")]
	[SerializeField] GameObject backImage;
	PlayerObject player;

	void Start()
	{
		player = FindObjectOfType<PlayerObject>();
	}

	void Update()
	{
		//healthBar.value = player.curHealth / player.maxHealth;
		//manaBar.value = player.curMana / player.maxMana;
		healthBar.fillAmount = player.curHealth / player.maxHealth;
		manaBar.fillAmount = player.curMana / player.maxMana;
		if (Input.GetButtonDown("Cancel"))
		{
			if (MenuUI.activeSelf)
			{
				MenuUI.SetActive(false);
				backImage.SetActive(false);
			}
			else
			{
				MenuUI.SetActive(true);
				backImage.SetActive(true);
			}
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
