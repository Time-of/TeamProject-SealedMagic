using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: UI- 옵션의 사운드 바 - 사운드 조절
*/

public class SoundManager : MonoBehaviour
{
	[Tooltip("배경음악")]
	[SerializeField] AudioSource musicSource;
	
	public void SetVolume(float volume)
	{
		musicSource.volume = volume;
	}
}
