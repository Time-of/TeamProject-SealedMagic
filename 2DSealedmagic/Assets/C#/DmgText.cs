using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;// 투명도 설명하기 위한 TMP

public class DmgText : MonoBehaviour
{
    [Tooltip("데미지가 위로 올라가는 속도")]
    public float moveSpeed;
    [Tooltip("데미지가 투명해지는 속도")]
    public float alphaSpeed;
    [Tooltip("데미지가 파괴되는 시간")]
    public float destroyTime;
    [Tooltip("투명도")]
    Color alpha;
    [Tooltip("받아올 데미지")]
    public float damage;

    TextMeshPro text;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();// 데미지를 받아온다
        alpha = text.color;// 투명도
        Invoke("DestroyObject", destroyTime);
    }


    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void DestroyObject()// 파괴
    {
        Destroy(gameObject);
    }
}
