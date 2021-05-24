using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLongAttack : MonoBehaviour
{
    public Transform firePoint;
    public GameObject impactEffect;

    public int damage = 20;
    

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("isAttack");
            Shoot();
        }

    }

    void Shoot()
    {
        /*
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);

        if (hitInfo)
        {
            EnemyObject enemy = hitInfo.transform.GetComponent<EnemyObject>();// Ememy 함수 호출
            if(enemy != null)
            {
                enemy.EnemyDamaged(damage);// Damage
            }
        }

        Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
        */
        Instantiate(impactEffect, firePoint.position, firePoint.rotation);
    }

}
