using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Icons : MonoBehaviour
{
    public Image[] skillFilter;
    public Text[] coolTimeCounter; //남은 쿨타임을 표시할 텍스트

    public float FcoolTime;
    private float FcurrentCoolTime; //남은 쿨타임을 추적 할 변수
    public float IcoolTime;
    private float IcurrentCoolTime; //남은 쿨타임을 추적 할 변수
    public float GcoolTime;
    private float GcurrentCoolTime; //남은 쿨타임을 추적 할 변수
    public float TcoolTime;
    private float TcurrentCoolTime; //남은 쿨타임을 추적 할 변수

    private bool canUseSkill = true; //스킬을 사용할 수 있는지 확인하는 변수
    PlayerLongAttack player;

    void Start()
    {
        //skillFilter.fillAmount = 0;
    }
    private void Awake()
    {
        player = FindObjectOfType<PlayerLongAttack>();
    }

    void Update()
    {
        if(player.FireIcon == true)
        {
            FSkill();
        }else if(player.IceIcon == true)
        {
            ISkill();
        }
        else if (player.GroundIcon == true)
        {
            GSkill();
        }
        else if (player.ThunderIcon == true)
        {
            TSkill();
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
            FcurrentCoolTime = FcoolTime;
            coolTimeCounter[0].text = "" + FcurrentCoolTime;

            StartCoroutine("FCoolTimeCounter");

            player.FireIcon = false;
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
            IcurrentCoolTime = IcoolTime;
            coolTimeCounter[1].text = "" + IcurrentCoolTime;

            StartCoroutine("ICoolTimeCounter");

            player.IceIcon = false;
            
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
            GcurrentCoolTime = GcoolTime;
            coolTimeCounter[2].text = "" + GcurrentCoolTime;

            StartCoroutine("GCoolTimeCounter");
           
            player.GroundIcon = false;
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
            TcurrentCoolTime = TcoolTime;
            coolTimeCounter[3].text = "" + TcurrentCoolTime;

            StartCoroutine("TCoolTimeCounter");

            player.ThunderIcon = false;
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
            skillFilter[0].fillAmount -= 1 * Time.smoothDeltaTime / FcoolTime;

            yield return null;
        }
        coolTimeCounter[0].gameObject.SetActive(false);

        canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈

        yield break;
    }

    //남은 쿨타임을 계산할 코르틴을 만들어줍니다.
    IEnumerator FCoolTimeCounter()
    {
        while (FcurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            FcurrentCoolTime -= 1.0f;
            coolTimeCounter[0].text = "" + FcurrentCoolTime;
        }
        
        yield break;
    }

    IEnumerator ICooltime()
    {
        while (skillFilter[1].fillAmount > 0)
        {
            skillFilter[1].fillAmount -= 1 * Time.smoothDeltaTime / IcoolTime;

            yield return null;
        }
        coolTimeCounter[1].gameObject.SetActive(false);

        canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈

        yield break;
    }

    //남은 쿨타임을 계산할 코르틴을 만들어줍니다.
    IEnumerator ICoolTimeCounter()
    {
        while (IcurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            IcurrentCoolTime -= 1.0f;
            coolTimeCounter[1].text = "" + IcurrentCoolTime;
        }

        yield break;
    }
    IEnumerator GCooltime()
    {
        while (skillFilter[2].fillAmount > 0)
        {
            skillFilter[2].fillAmount -= 1 * Time.smoothDeltaTime / GcoolTime;

            yield return null;
        }
        coolTimeCounter[2].gameObject.SetActive(false);

        canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈

        yield break;
    }

    //남은 쿨타임을 계산할 코르틴을 만들어줍니다.
    IEnumerator GCoolTimeCounter()
    {
        while (GcurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            GcurrentCoolTime -= 1.0f;
            coolTimeCounter[2].text = "" + GcurrentCoolTime;
        }
        
        yield break;
    }
    IEnumerator TCooltime()
    {
        while (skillFilter[3].fillAmount > 0)
        {
            skillFilter[3].fillAmount -= 1 * Time.smoothDeltaTime / TcoolTime;

            yield return null;
        }
        coolTimeCounter[3].gameObject.SetActive(false);

        canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈

        yield break;
    }

    //남은 쿨타임을 계산할 코르틴을 만들어줍니다.
    IEnumerator TCoolTimeCounter()
    {
        while (TcurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            TcurrentCoolTime -= 1.0f;
            coolTimeCounter[3].text = "" + TcurrentCoolTime;
        }
        
        yield break;
    }
}
