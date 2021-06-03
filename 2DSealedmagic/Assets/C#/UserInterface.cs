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
	[Tooltip("체력바 수치")]
	public Text message;
	[Tooltip("마나바 수치")]
	public Text message2;
	[Tooltip("받아오는 체력")]
	public float Hp;
	[Tooltip("받아오는 마나")]
	public float Mana;



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
		Init_HP();
		Init_MANA();
		message.text = (Hp).ToString();
		message2.text = (Mana).ToString();
	}
	private void Init_HP()
	{
		Hp = player.curHealth;
		Set_HP();
	}
	private void Init_MANA()
	{
		Mana = player.curMana;
		Set_MANA();
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	private void Set_HP()
	{
		if (Hp <= 0)
		{
			Hp = 0;
		}
		else
		{
			if (player.curHealth > player.maxHealth)
			{
				Hp = player.maxHealth;
			}
		}
	}

	private void Set_MANA()
	{

		if (Mana <= 0)
		{
			Mana = 0;
		}
		else
		{
			if (player.curMana > player.maxMana)
			{
				Mana = player.maxMana;
			}
		}
	}
}
