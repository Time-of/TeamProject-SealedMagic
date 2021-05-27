using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDmg : MonoBehaviour
{
    [Tooltip("얼음 공격 슬로우")]
    public float SlowSpeed = 0.5f;
    [Tooltip("얼음 공격 슬로우 지속시간")]
    public float SlowDuration = 4f;
    [Tooltip("얼음 중첩 횟수")]
    [SerializeField] private int count = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (count == 1)
        {
            if (collision.gameObject.tag == "IceBullet")
            {
                GetComponent<Monster>().modifySpeed(SlowSpeed, SlowDuration);
                count--;
            }

        }
    }
}
