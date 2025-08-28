using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offsetZ;
    [SerializeField] private float smooth = 5.0f;
    private bool active = false;
    private Vector3 startPosition;
    private Vector3 destinationPosition;

    private void Start()
    {
        startPosition = transform.position;
        GameController gc = FindObjectOfType<GameController>();
        gc.StartLevel += ActivateCamera;
        gc.ZoomOut += DeactivateCamera;
    }
    private void ActivateCamera()
    {
        active = true;
    }
    private void DeactivateCamera()
    {
        active = false;
    }
    private void LateUpdate()
    {
        if (active)
        {
            destinationPosition = new Vector3(target.position.x, 10, target.position.z + offsetZ);
        }
        else
        {
            destinationPosition = startPosition;
        }
        transform.position = Vector3.Lerp(transform.position, destinationPosition, Time.deltaTime * smooth);
    }
}
