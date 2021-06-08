using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 디버그용 스크립트, 게임매니저에 부착 후 활성/비활성
/*
h버튼: 스테이지 1로 이동
j버튼: 스테이지 2로 이동
k버튼: 스테이지 3으로 이동
b버튼: 플레이어가 매우 튼튼해짐
n버튼: 플레이어가 매우 쎄짐
m버튼: 플레이어 모든 마법 해금
*/

public class test : MonoBehaviour
{
	[Header("디버그용 스크립트")]
	[Header("H: 스테이지 1로 이동")]
	[Header("J: 스테이지 2로 이동")]
	[Header("K: 스테이지 3으로 이동")]
	[Header("B: 플레이어가 매우 튼튼해짐")]
	[Header("N: 플레이어가 매우 쎄짐")]
	[Header("M: 플레이어 모든 마법 해금")]
	[SerializeField] bool isDebug;
	void Update()
	{
		if (isDebug)
		{
			if (Input.GetKeyDown(KeyCode.H))
			{
				SceneManager.LoadScene("Stage1");
			}
			else if (Input.GetKeyDown(KeyCode.J))
			{
				SceneManager.LoadScene("Stage2");
			}
			else if (Input.GetKeyDown(KeyCode.K))
			{
				SceneManager.LoadScene("Stage3");
			}
			else if (Input.GetKeyDown(KeyCode.B))
			{
				GameManager.instance.plMaxHP += 10000;
				GameManager.instance.plCurHP += 10000;
				GameManager.instance.plMaxMP += 10000;
				GameManager.instance.plCurMP += 10000;

				GameManager.instance.FindAndGetInfo();
			}
			else if (Input.GetKeyDown(KeyCode.N))
			{
				GameManager.instance.increasedAtk += 1000;
				GameManager.instance.increasedNormalAtk += 200;
				GameManager.instance.FindAndGetInfo();
			}
			else if (Input.GetKeyDown(KeyCode.M))
			{
				for (int i = 0; i <= 3; i++)
				{
					GameManager.instance.magicCheck[i] = true;
				}
				GameManager.instance.FindAndGetInfo();
			}
		}
	}
}
