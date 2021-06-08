using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("날라가는 속도")]
    public float speed = 20f;

    public GameObject Impact;
    public bool ClassicCheck = false;
    public Rigidbody2D rigid;
    public Transform pos;
 

    void Start()
    {
        rigid.velocity = transform.right * speed;// 날라가는 속도(힘)
        Invoke("Dest", 0.4f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(ClassicCheck == true)
        {
            Instantiate(Impact, transform.position, transform.rotation);
        }
        Destroy(gameObject);// 어딘가에 부디치면 파괴
    }

    void Dest()
    {
        Destroy(gameObject);
    }

}
