using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SwipeHint : MonoBehaviour
{
    [SerializeField] private bool isSwipe = true;
    [SerializeField] private bool showOnPlay = false;
    [SerializeField] private Vector3 startPosition = new Vector3(230f,0f,0f);
    [SerializeField] private Vector3 destPosition = new Vector3(-230,0f,0f);
    [SerializeField] private bool slowTime = true;
    [SerializeField] private bool hideOnZoom = false;
    [SerializeField] private float timeScale = 0.3f;
    [SerializeField] private Image hintImage;
    [SerializeField] private float showSpeed = 0.01f;
    private Coroutine showHintCoroutine = null;
    private TouchHandler touchHandler;
    private CanvasGroup cg;
    private void Start() 
    {
        cg = hintImage.gameObject.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = hintImage.gameObject.AddComponent<CanvasGroup>();
        }
        cg.alpha = 0;
        if (showOnPlay)
        {
            GameController gc = FindObjectOfType<GameController>();
            gc.TouchHandlerInitialized +=InitTouchHandler;
            gc.StartLevel += Show;

        }
    }
    private void InitTouchHandler(TouchHandler touchHandler)
    {
        this.touchHandler = touchHandler;
    }
    private void Show()
    {
        if (hideOnZoom)
        {
            touchHandler.OnZoom += CloseHint;
        }
        else
            touchHandler.PointerUp += CloseHint;
        if (showHintCoroutine != null)
            StopCoroutine(showHintCoroutine);
        showHintCoroutine = StartCoroutine(ShowHint());
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")&&!showOnPlay)
        {
            touchHandler = GameObject.FindObjectOfType<TouchHandler>();
            Show();
            
        }
    }


    private void CloseHint(){
        if(showHintCoroutine!=null){
            StopCoroutine(showHintCoroutine);
            showHintCoroutine=null;
            Time.timeScale = 1f;
            hintImage.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }

    private IEnumerator ShowHint()
    {
        hintImage.gameObject.SetActive(true);
        cg.alpha = 0;
        for(int i=0;i<3;i++){
            hintImage.transform.localPosition = startPosition;
            while(slowTime&&Time.timeScale>timeScale||cg.alpha<1f)
            {
                if(slowTime)
                    Time.timeScale = Time.timeScale-showSpeed*Time.unscaledDeltaTime/0.01f<timeScale?timeScale:Time.timeScale-showSpeed*Time.unscaledDeltaTime/0.01f;
                cg.alpha += showSpeed*Time.unscaledDeltaTime/0.01f;
                yield return new WaitForEndOfFrame();
            }
            if (!isSwipe)
            {
                yield break;
            }
            float step = 0;
            while(step<1)
            {
                hintImage.transform.localPosition = Vector3.Slerp(startPosition, destPosition, step);
                step += 1 * Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }  
            while(cg.alpha>0f)
            {
                    cg.alpha -= showSpeed*Time.unscaledDeltaTime/0.01f;
                yield return new WaitForEndOfFrame();
            } 
        }  
        Time.timeScale=1f;
        yield return null;
    }
    private void OnDestroy()
    {
        try
        {
            GameController gc = FindObjectOfType<GameController>();
            gc.TouchHandlerInitialized -= InitTouchHandler;
            gc.StartLevel -= Show;
            touchHandler.OnZoom -= CloseHint;
            touchHandler.PointerUp -= CloseHint;
        }
        catch { }
    }
}
