using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수 (P)
작성 의도:
	MonsterSkill에서 인덱스에 따른 스킬의 정보를 받아오고자 하였다.
	원래는 MonsterSkill에 일괄적으로 등록하려 하였으나 관리도 힘들고
	리소스 낭비도 심할 것 같아서 여기에 등록해 두었다.
설명:
	몬스터 스킬들의 정보를 담는 스크립트, 게임매니저 오브젝트에 들어간다.
*/

public class MobSkillsInfo : MonoBehaviour
{
	[Tooltip("공격의 스프라이트")]
	public GameObject[] FX_Sprite = new GameObject[3];
	[Tooltip("공격 범위 오브젝트 프리팹")]
	public GameObject[] Skill_Area = new GameObject[3];
	[Tooltip("공격 이펙트 파티클")]
	public GameObject[] FX_Particle = new GameObject[3];

	[Tooltip("재사용 대기시간")]
	public float[] cooldown = new float[3];
	[Tooltip("사거리")]
	public float[] range = new float[3];
}
