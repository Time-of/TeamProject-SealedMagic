using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRange : MonoBehaviour
{
    public Vector3 Pos;
    [Tooltip("땅 공격 쿨타임1")]
    private float GcurTime;
    [Tooltip("땅 공격 쿨타임2")]
    [SerializeField] private float GroundTime;
    [Tooltip("데미지를 주는 이펙트와 그림이펙트")]
    public GameObject[] impactEffect;

    public bool check = false;

    Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }
    void Update()
    {
        GroundAttack();
    }

    void GroundAttack()
    {
        if (GcurTime <= 0) // 땅 공격
        {
            if (Input.GetButtonDown("GroundAttack") && check == true) // "X" Attack (ice)
            {
                anim.SetTrigger("isSkill");
                PlayerObject playerMovement = GameObject.Find("Player").GetComponent<PlayerObject>();
                playerMovement.bCanMove = false; // 캐릭터 이동 비활성화
                GroudAttackShoot();
                GcurTime = GroundTime;
                check = false;
            }
        }
        else
        {
            GcurTime -= Time.deltaTime;
        }
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Pos = collision.transform.position;// Ememy 함수 호출
            check = true;
        }
        
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        check = false;
    }

    void GroudAttackShoot()
    {
        GameObject GroundAtk = Instantiate(impactEffect[0], Vector3.up * 0.5f + Pos, Quaternion.identity);
        Instantiate(impactEffect[1], Vector3.up * 0.5f + Pos, Quaternion.identity);
        AttackArea area = GroundAtk.GetComponent<AttackArea>();

        if (area != null)
        {
            area.damage = 70;
            area.isEnemyAttack = false;
        } 
    }

}
