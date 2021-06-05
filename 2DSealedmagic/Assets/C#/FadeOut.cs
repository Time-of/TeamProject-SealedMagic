using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    /*여기다가 Det() 넣어서, 공격 마지막에 터지는 이펙트 처리 해줌*/

    [Tooltip("투명도 시간")]
    public float fadeOutTime = 1.0f;
    [Tooltip("투명해질지 여부")]
    public bool fadeinout = true;


    void Start()
    {
        FaOut();
    }


    void FaOut()
    {
        if (fadeinout == true)
            StartCoroutine(aktfadeOut(GetComponent<SpriteRenderer>()));
    }

    IEnumerator aktfadeOut(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;
        while (tmpColor.a > 0f)
        {
            tmpColor.a -= 1f * Time.deltaTime / fadeOutTime;
            _sprite.color = tmpColor;
            if (tmpColor.a <= 0f)
                tmpColor.a = 0f;
            yield return null;
        }
        Destroy(this.gameObject);
        _sprite.color = tmpColor;
    }

    void Det()
    {
        Destroy(gameObject);
    }
}
