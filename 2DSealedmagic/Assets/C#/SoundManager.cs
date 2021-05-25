using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;
using UnityEngine.UI;

/*
�ۼ�: 20181220 �̼���(P)

����: UI- �ɼ��� ���� �� - ���� ����
*/

public class SoundManager : MonoBehaviour
{
	[SerializeField] AudioMixer masterMixer;
	[SerializeField] Slider bgmSlider;
	[SerializeField] Slider sfxSlider;
	[SerializeField] Slider masterSlider;

	public void BGMControl()
	{
		float value = bgmSlider.value;

		// -40�� ��� -80���� ����� ���Ұ� ȿ��
		if (value == -40f) masterMixer.SetFloat("BGM", -80f);
		else
		{
			masterMixer.SetFloat("BGM", value);
		}
	}

	public void SFXControl()
	{
		float value = sfxSlider.value;

		// -40�� ��� -80���� ����� ���Ұ� ȿ��
		if (value == -40f) masterMixer.SetFloat("SFX", -80f);
		else
		{
			masterMixer.SetFloat("SFX", value);
		}
	}

	public void MasterControl()
	{
		float value = masterSlider.value;

		// -40�� ��� -80���� ����� ���Ұ� ȿ��
		if (value == -40f) masterMixer.SetFloat("Master", -80f);
		else
		{
			masterMixer.SetFloat("Master", value);
		}
	}
}