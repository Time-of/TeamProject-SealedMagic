using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRange : MonoBehaviour
{
    [Tooltip("땅 공격 쿨타임1")]
    private float GcurTime;
    [Tooltip("땅 공격 쿨타임2")]
    [SerializeField] private float GroundTime;
    [Tooltip("번개 공격 쿨타임1")]
    private float TcurTime;
    [Tooltip("번개 공격 쿨타임2")]
    public float ThunderTime;
    [Tooltip("데미지를 주는 이펙트와 그림이펙트")]
    public GameObject[] impactEffect;
    [Tooltip("Layer 체크")]
    public LayerMask InLayer;
    [Tooltip("땅 hit 크기 설정")]
    public Vector2 size;
    [Tooltip("번개 hit 크기 설정")]
    public Vector2 size2;
    [Tooltip("캐릭터 방향대로 hit 방향전함")]
    public int moveDic = 1;

    Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }
    void Update()
    {
        MoveDic();
        GroundAttack();
        ThunderAttack();
    }

    void MoveDic()
    {
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            moveDic = -1;
        }
        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            moveDic = 1;
        }
    }
    void GroundAttack()
    {
        if (GcurTime <= 0) // 땅 공격
        {
            if (Input.GetButtonDown("GroundAttack")) // "X" Attack (ice)
            {
                anim.SetTrigger("isSkill");
                PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
                playerMovement.bCanMove = false; // 캐릭터 이동 비활성화
                GroudAttackShoot();
                GcurTime = GroundTime;
                
            }
        }
        else
        {
            GcurTime -= Time.deltaTime;
        }
        
    }

    void ThunderAttack()
    {

        if (TcurTime <= 0) // 번개 공격
        {
            if (Input.GetButtonDown("ThunderAttack")) // "V" Attack (ice)
            {
                anim.SetTrigger("isSkill");
                
                PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
                playerMovement.bCanMove = false; // 캐릭터 이동 비활성화
                ThunderShoot();
                TcurTime = ThunderTime;
            }
        }
        else
        {
            TcurTime -= Time.deltaTime;
        }

    }

    // 범위 안에 들어오고 공격 키를 누르면 땅 공격
    void GroudAttackShoot()
    {

        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + new Vector3(4.5f * moveDic, 0, 0) , new Vector2(8, 0.5f), 0, InLayer);
        foreach (Collider2D i in hit)
        {
            
            GameObject GroundAtk = Instantiate(impactEffect[0],i.transform.position, Quaternion.identity);
            Instantiate(impactEffect[1], Vector3.up * 0.5f +  i.transform.position, Quaternion.identity);
            AttackArea area = GroundAtk.GetComponent<AttackArea>();

            if (area != null)
            {
                area.damage = 70;
                area.isEnemyAttack = false;
            }
            break;
        }
        
    }
    // 범위 안에 들어오고 공격 키를 누르면 번개 공격
    void ThunderShoot()
    {
        Collider2D[] Thit = Physics2D.OverlapBoxAll(transform.position + new Vector3(4.5f * moveDic, 1.4f, 0), new Vector2(8, 4.5f), 0, InLayer);
        foreach (Collider2D i in Thit)
        {
            GameObject thunderAtk = Instantiate(impactEffect[2], i.transform.position, Quaternion.identity);
            Instantiate(impactEffect[3], Vector3.up * 2.5f + i.transform.position, Quaternion.identity);
            AttackArea area = thunderAtk.GetComponent<AttackArea>();

            if (area != null)
            {
                area.damage = 40;
                area.isEnemyAttack = false;
                area.AttackType = "Stun";
                area.duration = 2f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(4.5f * moveDic, 0, 0), size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(4.5f * moveDic, 1.4f, 0), size2);
    }

}
