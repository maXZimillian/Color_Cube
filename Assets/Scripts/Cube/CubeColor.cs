using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeColor : MonoBehaviour
{
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material mainMaterial;
    [SerializeField] private Material additiveMaterial;
    [SerializeField] private GameObject maskObjectPrefab;
    public Material ColorMaterial { get; private set; }
    public Colors ColorValue { get; private set; }
    private void Awake()
    {
        ColorMaterial = new Material(baseMaterial);
        mainMaterial.color = additiveMaterial.color =  ColorMaterial.color = baseMaterial.color;
        //gameObject.GetComponent<Renderer>().materials[0] = mainMaterial;
        ColorPoint[] triggers = FindObjectsOfType<ColorPoint>();
        foreach(ColorPoint trigger in triggers)
        {
            trigger.OnColorTrigger += ChangeColor;
        }
    }

    private void ChangeColor(Color color, Colors colorValue)
    {

        if (colorValue == Colors.white)
        {
            ColorMaterial.color = baseMaterial.color;
        }
        else
        {
            Color cmy1 = new Color(1f - color.r, 1f - color.g, 1f - color.b);
            Color cmy2 = new Color(1f - ColorMaterial.color.r, 1f - ColorMaterial.color.g, 1f - ColorMaterial.color.b);

            Color tempColor = new Color((cmy1.r + cmy2.r) * 0.8f,
            (cmy1.g + cmy2.g) * 0.8f,
            (cmy1.b + cmy2.b) * 0.8f);
            if (this.ColorValue == Colors.white || cmy1 == cmy2)
            {
                tempColor = cmy1;
            }
            tempColor = new Color(-tempColor.r + 1f, -tempColor.g + 1f, -tempColor.b + 1f);
            ColorMaterial.color = tempColor;
        }
        ChangeColorValue(colorValue);
    }

    private void ChangeColorValue(Colors colorVal)
    {
        if (colorVal == Colors.white || ColorValue == Colors.white|| ColorValue==colorVal)
        {
            ColorValue = colorVal;
        }
        else
        {
            char[] stringVal = (((int)ColorValue).ToString() + ((int)colorVal).ToString()).ToCharArray();
            Array.Sort(stringVal);
            int combinedColorsValue = int.Parse(new string(stringVal));
            if (Enum.IsDefined(typeof(Colors), combinedColorsValue))
            {
                ColorValue = (Colors)combinedColorsValue;
            }
            else
            {
                ColorValue = Colors.dirty;
            }
        }
        if(ColorValue == Colors.brown)
        {
            ColorMaterial.color = new Color(0.35f,0.15f,0);
        }
        ChangeColorEffect();
    }

    private void ChangeColorEffect()
    {
        GameObject maskObject = Instantiate(maskObjectPrefab,transform);
        maskObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        maskObject.transform.localPosition = new Vector3(0, 0, 0);
        additiveMaterial.color = ColorMaterial.color;
        StartCoroutine(ColorEffectCoroutine(maskObject));
    }

    private IEnumerator ColorEffectCoroutine(GameObject maskObj)
    {
        while (maskObj.transform.localScale.x < 5)
        {
            maskObj.transform.localScale += Vector3.one * 5f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mainMaterial.color = ColorMaterial.color;
        Destroy(maskObj);
        yield return null;
    }
}
