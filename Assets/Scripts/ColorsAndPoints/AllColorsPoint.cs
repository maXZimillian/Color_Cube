using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AllColorsPoint : ColorPoint
{
    [SerializeField] private Color[] colors;
    [SerializeField] private Material[] materialsOnStop;
    [SerializeField] private float speed;
    public override event Action<Color, Colors> OnColorTrigger;
    private Material material;
    private float hue = 0;
    private Coroutine colorCoroutine;
    private void OnEnable()
    {
        material = new Material(GetComponent<Renderer>().material);
        material.color = Color.HSVToRGB(0, 1, 1);
        GetComponent<Renderer>().material = material;
        if (colorCoroutine != null)
            StopCoroutine(colorCoroutine);
        colorCoroutine=StartCoroutine(ChangeColor());

    }
    private IEnumerator ChangeColor()
    {
        bool stoppedRed = false;
        bool stoppedYellow = false;
        bool stoppedBlue = false;
        while (true)
        {
            hue += speed * Time.deltaTime;
            if (hue > 0.95f)
            {
                hue = 0;
                stoppedRed = stoppedYellow = stoppedBlue = false;

            }
            if (hue >0.01f && hue <0.05f && !stoppedRed)
            {
                GetComponent<Renderer>().material = materialsOnStop[0];
                yield return new WaitForSeconds(1.5f);
                GetComponent<Renderer>().material = material;
                stoppedRed = true;
            }
            if (hue >0.16f && hue<0.19f&& !stoppedYellow)
            {
                GetComponent<Renderer>().material = materialsOnStop[1];
                yield return new WaitForSeconds(1.5f);
                GetComponent<Renderer>().material = material;
                stoppedYellow = true;
            }
            if (hue > 0.58f && hue < 0.61f && !stoppedBlue)
            {
                GetComponent<Renderer>().material = materialsOnStop[2];
                yield return new WaitForSeconds(1.5f);
                GetComponent<Renderer>().material = material;
                stoppedBlue = true;
            }
            material.color = Color.HSVToRGB(hue, 1, 1);
            yield return new WaitForEndOfFrame();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float hue = this.hue * 360f;
            if (hue > 290f || hue <= 30f)
            {
                OnColorTrigger?.Invoke(colors[0], Colors.red);
            }
            else
            if (hue > 30f && hue <= 140f)
            {
                OnColorTrigger?.Invoke(colors[1], Colors.yellow);
            }
            else
            if (hue > 140f && hue <= 290f)
            {
                OnColorTrigger?.Invoke(colors[2], Colors.blue);
            }
            AnimateDestroy();
        }
    }

    private void AnimateDestroy()
    {
        Destroy(gameObject);
    }
}
