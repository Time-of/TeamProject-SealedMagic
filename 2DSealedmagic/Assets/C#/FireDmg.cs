using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDmg : MonoBehaviour
{
    [Tooltip("불 지속 데미지")]
    private int dot = 5;
    [Tooltip("불 중첩 횟수")]
    [SerializeField] private int count = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "FireBullet")
        {
            if (count == 1)
            {
                StartCoroutine(DotDam());
                count--;
            }
        }

    }

    // 코루틴을 통해 계속 불 데미지가 지속된다(아직 시간을 안 정해줌)
    IEnumerator DotDam()
    {
        while (true)
        {
            GetComponent<Monster>().onAttack(dot);
            yield return new WaitForSeconds(1);
        }
    }
}
