using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
작성: 20181220 이성수(P)

설명: 게임플레이 UI
다른 오브젝트에서 UserInterface.instance.GameOver();를 호출해주면 즉시 게임오버가 된다.
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
	[Tooltip("게임오버 UI")]
	[SerializeField] GameObject gameOverUI;
	[Tooltip("UI Canvas")]
	[SerializeField] GameObject UICanvas;
	public Text message;
	public Text message2;
	public float Hp;
	public float Mana;

	Animator anim;
	PlayerObject player;

	public static UserInterface instance;

	void Start()
	{
		player = FindObjectOfType<PlayerObject>();
		anim = UICanvas.GetComponent<Animator>();
		instance = this;
	}

	void Update()
	{
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
		if (GameManager.instance.isGameover)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}

		Init_HP();
		Init_MANA();
		message.text = (Hp).ToString();
		message2.text = (Mana).ToString();
	}

	public void GameOver()
	{
		GameManager.instance.isGameover = true;
		MenuUI.SetActive(false);
		gameOverUI.SetActive(true);
		backImage.SetActive(true);
		anim.SetBool("isGameover", true);
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
