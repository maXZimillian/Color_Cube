using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarrierTile : BaseTile
{
    [SerializeField] GameObject barrier;
    [SerializeField] float animSpeed;
    public Colors color;
    private bool isAccessed = false;
    public override bool MoveAccess(Colors colorVal)
    {
        if (!isAccessed)
        {
            if (colorVal == color)
            {
                isAccessed = true;
                DestroyBarrier();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;

        }
    }
    public override void OnEnter(Colors colorVal)
    {

    }
    private void DestroyBarrier()
    {
        StartCoroutine(BarrierDestroyEffect());
    }

    private IEnumerator BarrierDestroyEffect()
    {
        while (barrier.transform.localScale.y > 0)
        {
            barrier.transform.localScale += Vector3.down*animSpeed*Time.deltaTime*100;
            yield return new WaitForEndOfFrame();
        }
        Destroy(barrier);
        yield return null;
    }

}
