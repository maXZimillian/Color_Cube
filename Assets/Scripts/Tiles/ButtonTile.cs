using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonTile : BaseTile
{
    [SerializeField] private GameObject[] hiddenObjects;
    [SerializeField] private GameObject[] setActiveOnAnimationEnd;
    [SerializeField] private GameObject button;
    [SerializeField] private Material pressedButtonMaterial;
    [SerializeField] private float animationSpeed;
    private bool isAccessed = false;
    private void Start()
    {
        foreach(GameObject hiddenObj in hiddenObjects)
        {
            hiddenObj.SetActive(false);
        }
    }
    public override bool MoveAccess(Colors colorVal)
    {
        return true;
    }
    public override void OnEnter(Colors colorVal)
    {
        if (!isAccessed)
        {
            foreach (GameObject hiddenObj in hiddenObjects)
            {
                StartCoroutine(AnimatedActivation(hiddenObj));
            }
            ButtonEffect();
            isAccessed = true;
        }        
    }

    private void ButtonEffect()
    {
        if (button != null&&pressedButtonMaterial!=null)
        {
            StartCoroutine(SetButtonMaterial());
        }
    }

    private IEnumerator SetButtonMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        button.GetComponent<Renderer>().material = pressedButtonMaterial;
        yield return null;
    }

    private IEnumerator AnimatedActivation(GameObject obj)
    {
        float destYPoint = obj.transform.position.y;
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - 2f, obj.transform.position.z);
        obj.SetActive(true);

        if(obj.GetComponent<IButtonObject>()!=null)
        {
            obj.GetComponent<IButtonObject>().IsActive = false;
        }

        while (obj.transform.position.y < destYPoint)
        {
            obj.transform.position += Vector3.up * animationSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        obj.transform.position = new Vector3(obj.transform.position.x, destYPoint, obj.transform.position.z);

        if(obj.GetComponent<IButtonObject>()!=null)
        {
            obj.GetComponent<IButtonObject>().IsActive = true;
        }

        yield return null;
    }
}

public interface IButtonObject {
    bool IsActive{get;set;}
}
