using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
작성: 20181220 이성수(P)

설명: 게임 매니저 스크립트
제단을 활성화하기 위한 변수 등 관리중
*/

public class GameManager : MonoBehaviour
{
	public int killedColoredMonster = 0;
	public bool isGameover = false;
	public bool isGameClear = false;
	public GameObject ColoredMonsterFX;

	public static GameManager instance;

	[SerializeField] AudioClip SFX_Potion;
	[SerializeField] AudioClip SFX_Plate;
	[SerializeField] public int Stagenum = 0;

	//[SerializeField] GameObject PlayerPrefab;
	//[SerializeField] Vector2[] PlayerSpawnPos;

	[SerializeField] float playerMaxHP;
	[SerializeField] float playerMaxMP;

	[HideInInspector] public float plMaxHP;
	[HideInInspector] public float plCurHP;
	[HideInInspector] public float plMaxMP;
	[HideInInspector] public float plCurMP;
	[HideInInspector] public bool[] magicCheck = new bool[4];
	[HideInInspector] public float increasedAtk;
	[HideInInspector] public float increasedAtk2;

	PlayerObject player;
	AudioSource audioSource;

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
		audioSource = GetComponent<AudioSource>();
		plMaxHP = 1000;
		plCurHP = 1000;
		plMaxMP = 500;
		plCurMP = 500;
	}

	void Start()
	{
		player = FindObjectOfType<PlayerObject>();
		GetPlayerInfo();
	}

	void Update()
	{
		if (UserInterface.instance.isMainScreen && Input.anyKey)
		{
			UserInterface.instance.DisableMainScreen();
			UserInterface.instance.isMainScreen = false;
			SceneManager.LoadScene("Stage1");
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (isGameover)
			{
				InitPlayer();
				UserInterface.instance.InitGameoverBool();
				isGameover = false;
				SceneManager.LoadScene("Stage1");
			}
			else if (isGameClear)
			{
				InitPlayer();
				UserInterface.instance.InitGameclearBool();
				isGameClear = false;
				SceneManager.LoadScene("Stage1");
			}
		}
	}

	public void FindAndGetInfo()
	{
		player = FindObjectOfType<PlayerObject>();
		GetPlayerInfo();
	}

	public void Sound(string activity)
	{
		switch (activity)
		{
			case "Potion":
				audioSource.clip = SFX_Potion;
				break;
			case "Plate":
				audioSource.clip = SFX_Plate;
				break;
		}

		if (audioSource != null)
			audioSource.Play();
	}

	void GetPlayerInfo()
	{
		if (player != null)
		{
			player.maxHealth = plMaxHP;
			player.curHealth = plCurHP;
			player.maxMana = plMaxMP;
			player.curMana = plCurMP;

			for (int i = 0; i <= 3; i++)
			{
				PlayerLongAttack.instance.StageCheck[i] = magicCheck[i];
			}
			PlayerLongAttack.instance.UpAtk = increasedAtk;
			PlayerLongAttack.instance.Atk = increasedAtk2;
		}
	}

	void InitPlayer()
	{
		if (player != null)
		{
			player.maxHealth = playerMaxHP;
			player.curHealth = playerMaxHP;
			player.maxMana = playerMaxMP;
			player.curMana = playerMaxMP;

			for (int i = 0; i <= 3; i++)
			{
				PlayerLongAttack.instance.StageCheck[i] = false;
			}
			PlayerLongAttack.instance.UpAtk = 0f;
			PlayerLongAttack.instance.Atk = 0f;
		}
	}
}
