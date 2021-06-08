using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Icons : MonoBehaviour
{
	public Image[] skillFilter;
	public Text[] coolTimeCounter; //남은 쿨타임을 표시할 텍스트

	[Tooltip("스킬 아이콘 쿨타임 초기값")]
	[SerializeField] public float[] coolTime;
	[Tooltip("스킬 아이콘 쿨타임 값")]
	[SerializeField] private float[] currentCoolTime; //남은 쿨타임을 추적 할 변수
	[Tooltip("스킬 해제")]
	public GameObject[] Skillit; 

	private bool canUseSkill = true; //스킬을 사용할 수 있는지 확인하는 변수
	PlayerLongAttack player;

	public static Icons instance;

	

	void Start()
	{
		//skillFilter.fillAmount = 0;
	}
	private void Awake()
	{
		instance = this;
	}

	public void FindPlayer()
	{
		player = FindObjectOfType<PlayerLongAttack>();
	}

	void Update()
	{
		if (player != null)
		{
			if (player.SillIcon[0] == true)
			{
				FSkill();
			}
			else if (player.SillIcon[1] == true)
			{
				ISkill();
			}
			else if (player.SillIcon[2] == true)
			{
				GSkill();
			}
			else if (player.SillIcon[3] == true)
			{
				TSkill();
			}
			SkillIcon();
		}

	}

	public void FSkill()
	{
		if (canUseSkill)
		{
			Debug.Log("Fire Skill");
			coolTimeCounter[0].gameObject.SetActive(true);
			skillFilter[0].fillAmount = 1; //스킬 버튼을 가림
			StartCoroutine("FCooltime");
			currentCoolTime[0] = coolTime[0];
			coolTimeCounter[0].text = "" + currentCoolTime[0];

			StartCoroutine("FCoolTimeCounter");

			player.SillIcon[0] = false;
		}
		else
		{
			Debug.Log("아직 스킬을 사용할 수 없습니다.");
		}
	}

	public void ISkill()
	{
		if (canUseSkill)
		{
			Debug.Log("Ice Skill");
			coolTimeCounter[1].gameObject.SetActive(true);
			skillFilter[1].fillAmount = 1; //스킬 버튼을 가림
			StartCoroutine("ICooltime");
			currentCoolTime[1] = coolTime[1];
			coolTimeCounter[1].text = "" + currentCoolTime[1];

			StartCoroutine("ICoolTimeCounter");

			player.SillIcon[1] = false;

		}
		else
		{
			Debug.Log("아직 스킬을 사용할 수 없습니다.");
		}
	}

	public void GSkill()
	{
		if (canUseSkill)
		{
			Debug.Log("Ground Skill");
			coolTimeCounter[2].gameObject.SetActive(true);
			skillFilter[2].fillAmount = 1; //스킬 버튼을 가림
			StartCoroutine("GCooltime");
			currentCoolTime[2] = coolTime[2];
			coolTimeCounter[2].text = "" + currentCoolTime[2];

			StartCoroutine("GCoolTimeCounter");

			player.SillIcon[2] = false;
		}
		else
		{
			Debug.Log("아직 스킬을 사용할 수 없습니다.");
		}
	}

	public void TSkill()
	{
		if (canUseSkill)
		{
			Debug.Log("Thunder Skill");
			coolTimeCounter[3].gameObject.SetActive(true);
			skillFilter[3].fillAmount = 1; //스킬 버튼을 가림
			StartCoroutine("TCooltime");
			currentCoolTime[3] = coolTime[3];
			coolTimeCounter[3].text = "" + currentCoolTime[3];

			StartCoroutine("TCoolTimeCounter");

			player.SillIcon[3] = false;
		}
		else
		{
			Debug.Log("아직 스킬을 사용할 수 없습니다.");
		}
	}

	IEnumerator FCooltime()
	{
		while (skillFilter[0].fillAmount > 0)
		{
			skillFilter[0].fillAmount -= 1 * Time.smoothDeltaTime / coolTime[0];

			yield return null;
		}
		coolTimeCounter[0].gameObject.SetActive(false);

		canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈

		yield break;
	}

	//남은 쿨타임을 계산할 코르틴을 만들어줍니다.
	IEnumerator FCoolTimeCounter()
	{
		while (currentCoolTime[0] > 0)
		{
			yield return new WaitForSeconds(1.0f);

			currentCoolTime[0] -= 1.0f;
			coolTimeCounter[0].text = "" + currentCoolTime[0];
		}

		yield break;
	}

	IEnumerator ICooltime()
	{
		while (skillFilter[1].fillAmount > 0)
		{
			skillFilter[1].fillAmount -= 1 * Time.smoothDeltaTime / coolTime[1];

			yield return null;
		}
		coolTimeCounter[1].gameObject.SetActive(false);

		canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈

		yield break;
	}

	//남은 쿨타임을 계산할 코르틴을 만들어줍니다.
	IEnumerator ICoolTimeCounter()
	{
		while (currentCoolTime[1] > 0)
		{
			yield return new WaitForSeconds(1.0f);

			currentCoolTime[1] -= 1.0f;
			coolTimeCounter[1].text = "" + currentCoolTime[1];
		}

		yield break;
	}
	IEnumerator GCooltime()
	{
		while (skillFilter[2].fillAmount > 0)
		{
			skillFilter[2].fillAmount -= 1 * Time.smoothDeltaTime / coolTime[2];

			yield return null;
		}
		coolTimeCounter[2].gameObject.SetActive(false);

		canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈

		yield break;
	}

	//남은 쿨타임을 계산할 코르틴을 만들어줍니다.
	IEnumerator GCoolTimeCounter()
	{
		while (currentCoolTime[2] > 0)
		{
			yield return new WaitForSeconds(1.0f);

			currentCoolTime[2] -= 1.0f;
			coolTimeCounter[2].text = "" + currentCoolTime[2];
		}

		yield break;
	}
	IEnumerator TCooltime()
	{
		while (skillFilter[3].fillAmount > 0)
		{
			skillFilter[3].fillAmount -= 1 * Time.smoothDeltaTime / coolTime[3];

			yield return null;
		}
		coolTimeCounter[3].gameObject.SetActive(false);

		canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈

		yield break;
	}

	//남은 쿨타임을 계산할 코르틴을 만들어줍니다.
	IEnumerator TCoolTimeCounter()
	{
		while (currentCoolTime[3] > 0)
		{
			yield return new WaitForSeconds(1.0f);

			currentCoolTime[3] -= 1.0f;
			coolTimeCounter[3].text = "" + currentCoolTime[3];
		}

		yield break;
	}

	void SkillIcon()
    {
		if (PlayerLongAttack.instance.StageCheck[0] == true)
		{
			Skillit[0].SetActive(false);
		}
		else
		{
			Skillit[0].SetActive(true);
		}

		if (PlayerLongAttack.instance.StageCheck[1] == true)
		{
			Skillit[1].SetActive(false);
		}
		else
		{
			Skillit[1].SetActive(true);
		}

		if (PlayerLongAttack.instance.StageCheck[2] == true)
		{
			Skillit[2].SetActive(false);
		}
		else
		{
			Skillit[2].SetActive(true);
		}

		if (PlayerLongAttack.instance.StageCheck[3] == true)
		{
			Skillit[3].SetActive(false);
		}
		else
		{
			Skillit[3].SetActive(true);
		}
	}
}
