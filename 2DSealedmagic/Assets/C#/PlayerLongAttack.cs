using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLongAttack : MonoBehaviour
{
    [Tooltip("공격이 발사 되는 위치")]
    public Transform firePoint;
    [Tooltip("발사 할 Bullet")]
    public GameObject[] impactEffect;
    [Tooltip("기본 공격 쿨타임1")]
    private float ccurTime;
    [Tooltip("불 공격 쿨타임1")]
    private float FcurTime;
    [Tooltip("얼음 공격 쿨타임1")]
    private float IcurTime;
    [Tooltip("번개 공격 쿨타임1")]
    private float TcurTime;

    [Tooltip("기본 공격 쿨타임2")]
    public float classicTime;
    [Tooltip("불 공격 쿨타임2")]
    public float fireTime;
    [Tooltip("얼음 공격 쿨타임2")]
    public float iceTime;
    [Tooltip("번개 공격 쿨타임2")]
    public float ThunderTime;

    Animator anim;

    // 공격들을 다 따로 놓은 이유는 쿨타임들을 각자 다르게 하려고 함.
    // 썬더는 아직 구현이 안됬으며, 땅은 따로 처리함.
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Attack();
        FireAttack();
        IceAttack();
        ThunderAttack();
    }

    void Attack()
    {
        if (ccurTime <= 0)// 기본 공격
        {
            if (Input.GetButtonDown("Attack")) // "S" Attack (classic)
            {
                anim.SetTrigger("isAttack");
                Shoot();
                ccurTime = classicTime;
            }
        }
        else
        {
            ccurTime -= Time.deltaTime;
        }
    }

    void FireAttack()
    {
        if (FcurTime <= 0)// 불 공격
        {
            if (Input.GetButtonDown("FireAttack")) // "Z" Attack (fire)
            {
                anim.SetTrigger("isAttack");
                FireShoot();
                FcurTime = fireTime;
            }
        }
        else
        {
            FcurTime -= Time.deltaTime;
        }
    }

    void IceAttack()
    {
        if (IcurTime <= 0) // 얼음 공격
        {
            if (Input.GetButtonDown("IceAttack")) // "X" Attack (ice)
            {
                anim.SetTrigger("isAttack");
                IceShoot();
                IcurTime = iceTime;
            }
        }
        else
        {
            IcurTime -= Time.deltaTime;
        }
    }

    void ThunderAttack()
    {
        if (TcurTime <= 0) // 번개 공격
        {
            if (Input.GetButtonDown("ThunderAttack")) // "V" Attack (ice)
            {
                anim.SetTrigger("isAttack");
                ThunderShoot();
                TcurTime = ThunderTime;
            }
        }
        else
        {
            TcurTime -= Time.deltaTime;
        }
    }

    // bullet 생성

    void Shoot()
    {
        Instantiate(impactEffect[0], firePoint.position, firePoint.rotation);
    }

    void FireShoot()
    {
        Instantiate(impactEffect[1], firePoint.position, firePoint.rotation);
    }

    void IceShoot()
    {
        Instantiate(impactEffect[2], firePoint.position, firePoint.rotation);
    }

    // 추가 21.05.27
    void ThunderShoot()
    {
        GameObject thunderAtk = Instantiate(impactEffect[3], firePoint.position, firePoint.rotation);
        AttackArea area = thunderAtk.GetComponent<AttackArea>();
        area.damage = 40;
        area.isEnemyAttack = false;
        area.AttackType = "Stun";
        area.duration = 2f;
    }
}
