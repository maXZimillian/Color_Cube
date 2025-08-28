using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseTile: MonoBehaviour
{
    private Vector2 position;
    private Tilemap tilemap;
    public Vector2 Position { get { return position; } private set { } }

    public abstract bool MoveAccess(Colors colorVal);
    public abstract void OnEnter(Colors colorVal);

    private void Awake()
    {
        tilemap = transform.GetComponentInParent<Tilemap>();
        Vector3Int pos = tilemap.WorldToCell(transform.position);
        position = new Vector2(pos.x, pos.y);
    }
}
