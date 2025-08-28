using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class CubeMovingController : MonoBehaviour
{
    public Vector2 position;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private CubeColor color;
    [SerializeField] private GameObject stepParticles;
    [SerializeField] private Material stepParticlesMaterial;
    private Vector3 actionBuffer;
    private Coroutine moveCoroutine;
    public TouchHandler TouchHandler { get { return null; }set { value.OnSwipe += CheckMovementTo; } }
    public event Action OnMove;

    BaseTile[] tiles;


    void Start()
    {
        tiles = FindObjectsOfType<BaseTile>();
        FindLocalPosition();
    }

    private void FindLocalPosition()
    {
        Tilemap tilemap = FindObjectOfType<Tilemap>();
        Vector3Int cellPos = tilemap.WorldToCell(transform.position);
        position = new Vector2(cellPos.x, cellPos.y);
    }

    private void CheckMovementTo(Vector3 direction)
    {
        
        if (moveCoroutine == null)
        {
            BaseTile destTile=null;
            foreach (BaseTile tile in tiles)
            {
                if (tile.Position.x == position.x + direction.x && tile.Position.y == position.y + direction.z)
                {
                    destTile = tile;
                    break;
                }
            }
            if (destTile != null&&destTile.MoveAccess(color.ColorValue))
            {
                position = new Vector2(position.x + direction.x, position.y + direction.z);
                moveCoroutine = StartCoroutine(Move(direction));
                OnMove?.Invoke();
                destTile.OnEnter(color.ColorValue);
            }
        }
        else
        {
            actionBuffer = direction;
        }
    }

    private void InstantiateParticles()
    {
        if (stepParticles != null && stepParticlesMaterial != null)
        {
            Color partColor = color.ColorMaterial.color;
            stepParticlesMaterial.color = new Color(partColor.r, partColor.g, partColor.b, stepParticlesMaterial.color.a);
            GameObject step = Instantiate(stepParticles, transform.parent);
            step.transform.position = transform.position;
        }
    }

    private IEnumerator Move(Vector3 direction)
    {
        Vector3 startPos = transform.position;
        float remainingAngle = 90;
        Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * moveSpeed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return new WaitForEndOfFrame();
        }
        gameObject.transform.position = startPos+new Vector3(direction.x,direction.y,direction.z);
        InstantiateParticles();
        moveCoroutine = null;
        if (actionBuffer != Vector3.zero)
        {
            CheckMovementTo(actionBuffer);
            actionBuffer = Vector3.zero;
        }
        yield return null;
    }
}
