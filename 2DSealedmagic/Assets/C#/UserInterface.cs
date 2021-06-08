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
	[Tooltip("옵션 UI")]
	[SerializeField] GameObject OptionUI;
	[Tooltip("나가기 UI")]
	[SerializeField] GameObject ExitUI;
	[Tooltip("메뉴 후경 이미지 오브젝트")]
	[SerializeField] GameObject backImage;
	[Tooltip("게임오버 UI")]
	[SerializeField] GameObject gameOverUI;
	[Tooltip("게임오버 백 이미지")]
	[SerializeField] GameObject gameOverBackImage;
	[Tooltip("게임클리어 UI")]
	[SerializeField] GameObject gameClearUI;
	[Tooltip("메인화면 UI")]
	[SerializeField] GameObject mainScreen;
	[Tooltip("UI 캔버스 오브젝트")]
	[SerializeField] GameObject UICanvas;
	public Text message;
	public Text message2;
	public float Hp;
	public float Mana;

	public bool isMainScreen = true;

	Animator anim;
	PlayerObject player;

	public static UserInterface instance;

	void Start()
	{
		anim = UICanvas.GetComponent<Animator>();
		instance = this;
	}

	public void FindPlayer()
	{
		player = FindObjectOfType<PlayerObject>();
	}

	void Update()
	{
		if (player != null)
		{
			healthBar.fillAmount = player.curHealth / player.maxHealth;
			manaBar.fillAmount = player.curMana / player.maxMana;


			if (Input.GetButtonDown("Cancel") && !GameManager.instance.isGameover && !GameManager.instance.isGameClear)
			{
				if (backImage.activeSelf)
				{
					MenuUI.SetActive(false);
					OptionUI.SetActive(false);
					ExitUI.SetActive(false);
					backImage.SetActive(false);
				}
				else
				{
					MenuUI.SetActive(true);
					OptionUI.SetActive(false);
					ExitUI.SetActive(false);
					backImage.SetActive(true);
				}
			}

			Init_HP();
			Init_MANA();
			message.text = (Hp).ToString();
			message2.text = (Mana).ToString();
		}
	}

	public void EnableMainScreen()
	{
		mainScreen.SetActive(true);
	}

	public void DisableMainScreen()
	{
		mainScreen.SetActive(false);
	}

	public void GameOver()
	{
		GameManager.instance.isGameover = true;
		MenuUI.SetActive(false);
		gameOverUI.SetActive(true);
		gameOverBackImage.SetActive(true);
		backImage.SetActive(false);
		anim.SetBool("isGameover", true);
	}

	public void InitGameoverBool()
	{
		gameOverUI.SetActive(false);
		gameOverBackImage.SetActive(false);
		anim.SetBool("isGameover", false);
	}

	public void InitGameclearBool()
	{
		gameClearUI.SetActive(false);
		anim.SetBool("isGameClear", false);
	}

	public void GameClear()
	{
		GameManager.instance.isGameClear = true;
		MenuUI.SetActive(false);
		gameClearUI.SetActive(true);
		anim.SetBool("isGameClear", true);
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
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif

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
