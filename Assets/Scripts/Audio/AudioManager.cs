using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effect;
    void Start()
    {
        FindObjectOfType<CubeMovingController>().OnMove += PlayEffect;
    }

    private void PlayEffect()
    {
        effect.Play();
    }
}
