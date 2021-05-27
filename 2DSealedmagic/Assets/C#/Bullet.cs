using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("날라가는 속도")]
    public float speed = 20f;

    public Rigidbody2D rigid;
 

    void Start()
    {
        rigid.velocity = transform.right * speed;// 날라가는 속도(힘)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GroundBullet")
            Destroy(gameObject, 0.01f);
        else
            Destroy(gameObject, 1);// 어딘가에 부디치면 파괴
    }
}
