using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public event Action OnUnactiveTouch;
    public event Action PointerUp;
    public event Action OnZoom;
    public event Action<Vector3> OnSwipe;
    private Vector2 dragStartPosition;
    private bool active = false;

    float prevMagnitude = 0f;
    bool prevMagnitudeSet = false;
    public float ZoomDeltaTreshold = 10f;
    float totalZoom = 0f;
    CurrentAction currentAction = CurrentAction.Undefined;

    private void Start()
    {
        GameController gc = FindObjectOfType<GameController>();
        gc.StartLevel += Activate;
        gc.WinGame += Deactivate;
        gc.GameOver += Deactivate;
        if (gc.MainSceneExecute)
        {
            FindObjectOfType<MainMenuController>().PlayButtonPressed += OnTouchUnactive;
        }
    }
    enum CurrentAction
    {
        Undefined,
        Zoom
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            var touch1Pos = Input.GetTouch(0).position;
            var touch2Pos = Input.GetTouch(1).position;
            var currentMagnitude = (touch1Pos - touch2Pos).magnitude;
            if (prevMagnitudeSet)
            {
                var zoomDelta = prevMagnitude - currentMagnitude;
                switch (currentAction)
                {
                    case CurrentAction.Undefined:
                        totalZoom += zoomDelta;
                        if (totalZoom > ZoomDeltaTreshold)
                        {
                            currentAction = CurrentAction.Zoom;
                            if (active)
                            {
                                Zoom();
                            }
                        }
                        else
                        {
                            if (totalZoom < -ZoomDeltaTreshold)
                            {
                                currentAction = CurrentAction.Zoom;
                                if (!active)
                                {
                                    ZoomIn();
                                }
                            }
                        }
                        break;
                }
            }
            prevMagnitude = currentMagnitude;
            prevMagnitudeSet = true;
        }
        else
        {
            if (Input.touchCount == 0)
            {
                totalZoom = 0f;
                currentAction = CurrentAction.Undefined;
                prevMagnitudeSet = false;
            }
        }
    }

    private void Zoom()
    {
        OnZoom?.Invoke();
        Deactivate();
    }

    private void ZoomIn()
    {
        OnTouchUnactive();
    }


    private void Activate()
    {
        active = true;
    }

    private void Deactivate()
    {
        active = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragStartPosition = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentAction == CurrentAction.Undefined)
        {
            if (active)
            {
                if (Mathf.Abs(eventData.position.y - dragStartPosition.y) > Mathf.Abs(eventData.position.x - dragStartPosition.x))
                {
                    if (eventData.position.y - dragStartPosition.y > 0)
                        OnSwipe?.Invoke(Vector3.forward);
                    else
                    if (eventData.position.y - dragStartPosition.y < 0)
                        OnSwipe?.Invoke(Vector3.back);
                }
                else
                {
                    if (eventData.position.x - dragStartPosition.x > 0)
                        OnSwipe?.Invoke(Vector3.right);
                    else
                    if (eventData.position.x - dragStartPosition.x < 0)
                        OnSwipe?.Invoke(Vector3.left);
                }
                PointerUp?.Invoke();
            }
            else
            {
                OnTouchUnactive();
            }
        }
    }

    private void OnTouchUnactive()
    {
        OnUnactiveTouch?.Invoke();
    }
}
