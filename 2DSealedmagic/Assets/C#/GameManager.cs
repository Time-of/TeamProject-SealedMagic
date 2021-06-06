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

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (isGameover)
			{
				SceneManager.LoadScene("Stage1");
			}
			else if (isGameClear)
			{
				SceneManager.LoadScene("Stage1");
			}
		}
	}
}
