using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FinishTile : BaseTile
{
    public Colors color;
    public event Action Win;
    public event Action Lose;
    public override bool MoveAccess(Colors colorVal)
    {
        return true;
    }
    public override void OnEnter(Colors colorVal)
    {
        if(colorVal == color)
        {
            Win?.Invoke();
        }
        else
        {
            Lose?.Invoke();
        }
    }
}
