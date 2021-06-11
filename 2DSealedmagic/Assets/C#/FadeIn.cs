using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private Image image;

    Altar altar;

    private void Awake()
    {
        image = GetComponent<Image>();
        altar = FindObjectOfType<Altar>();
    }

    // Update is called once per frame
    void Update()
    {
        Fadein();
        StartCoroutine("Fadeouter");
    }

    void Fadein()
    {
        Color color = image.color;

        if (color.a > 0)
        {
            color.a -= Time.deltaTime;
        }

        image.color = color;
    }

    IEnumerator Fadeouter()
    {

        yield return new WaitForSeconds(2f);
        Color color = image.color;

        if (color.a < 1)
        {
            color.a += Time.deltaTime;
        }

        image.color = color;

    }
}
