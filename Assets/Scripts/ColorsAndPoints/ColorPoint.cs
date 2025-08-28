using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ColorPoint : MonoBehaviour
{
    [SerializeField] private Colors color;
    public virtual event Action<Color, Colors> OnColorTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnColorTrigger?.Invoke(GetComponent<Renderer>().material.color,color);
            AnimateDestroy();
        }
    }

    private void AnimateDestroy()
    {
        Destroy(gameObject);
    }
}
