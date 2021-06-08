using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
	SpriteRenderer spRenderer;

	bool isBossDefeat = false;

	public static GameClear instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		spRenderer = GetComponent<SpriteRenderer>();
		spRenderer.color = new Color(1, 1, 1, 0);
	}

	public void BossDead()
	{
		isBossDefeat = true;
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (isBossDefeat == true && collision.gameObject.tag == "Player")
		{
			Debug.Log("게임 클리어!");
			UserInterface.instance.GameClear();
			isBossDefeat = false;
		}
	}
}
