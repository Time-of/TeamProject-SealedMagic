using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;
using UnityEngine.UI;

/*
작성: 20181220 이성수(P)

설명: UI- 옵션의 사운드 바 - 사운드 조절
*/

public class SoundManager : MonoBehaviour
{
	[Header("오디오믹서 및 슬라이더")]
	[SerializeField] AudioMixer masterMixer;
	[SerializeField] Slider bgmSlider;
	[SerializeField] Slider sfxSlider;
	[SerializeField] Slider masterSlider;

	void Start()
	{
		masterMixer.SetFloat("Master", -20f);
	}

	public void BGMControl()
	{
		float value = bgmSlider.value;

		// -40일 경우 -80으로 만들어 음소거 효과
		if (value == -40f) masterMixer.SetFloat("BGM", -80f);
		else
		{
			masterMixer.SetFloat("BGM", value);
		}
	}

	public void SFXControl()
	{
		float value = sfxSlider.value;

		// -40일 경우 -80으로 만들어 음소거 효과
		if (value == -40f) masterMixer.SetFloat("SFX", -80f);
		else
		{
			masterMixer.SetFloat("SFX", value);
		}
	}

	public void MasterControl()
	{
		float value = masterSlider.value;

		// -40일 경우 -80으로 만들어 음소거 효과
		if (value == -40f) masterMixer.SetFloat("Master", -80f);
		else
		{
			masterMixer.SetFloat("Master", value);
		}
	}
}
