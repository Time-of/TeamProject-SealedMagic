using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private Image image;
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Fadein();
       
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
    /*
    public void Fadeout()
    {
        Color color = image.color;

        if (color.a < 1)
        {
            color.a += Time.deltaTime;
        }

        image.color = color;
    }*/
}
