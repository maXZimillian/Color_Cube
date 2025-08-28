using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPointMove : MonoBehaviour, IButtonObject
{
    public bool IsActive
    {
        get
        {
            return false;
        }
        set
        {
            if(value==true)
            {
                if(moveCoroutine!=null)
                    StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(Move());
            }
            else
            {
                if(moveCoroutine!=null)
                {
                    StopCoroutine(moveCoroutine);
                    transform.position = startPos;
                }
            }
        }
    }
    [SerializeField] private float offset;
    [SerializeField] private float speed;
    private Coroutine moveCoroutine = null;
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
        moveCoroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        startPos = transform.position;
        float time = 0;
        while (true)
        {
            time += Time.deltaTime*speed;
            transform.position = new Vector3(startPos.x, startPos.y+Mathf.Sin(time)*offset, startPos.z);

            yield return new WaitForEndOfFrame();
        }
    }
}
