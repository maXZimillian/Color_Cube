using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : BaseTile
{
    public override bool MoveAccess(Colors colorVal)
    {
        return true;
    }
    public override void OnEnter(Colors colorVal)
    {

    }
}
