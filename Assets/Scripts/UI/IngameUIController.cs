using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIController : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] float popupShowSpeed;
    [SerializeField] GameObject previewPanel;
    [SerializeField] GameObject[] externHidingButtons;
    [SerializeField] GameObject[] pointsCollectListeners;

    void Start()
    {
        GameController gameController = GameObject.FindObjectOfType<GameController>();
        gameController.WinGame += ShowWinPopup;
        gameController.GameOver += ShowLosePopup;
        gameController.StartLevel += HidePreviewPanel;
        gameController.ZoomOut += ShowPreviewPanel;
        if (gameController.MainSceneExecute)
        {
            HideExternButtons();
            MainMenuController mc = FindObjectOfType<MainMenuController>();
            mc.PlayButtonAnimationEnded += ShowExternButtons;
        }
    }

    private void ShowPreviewPanel()
    {
        previewPanel.SetActive(true);
    }

    private void HidePreviewPanel()
    {
        previewPanel.SetActive(false);
    }

    private void ShowWinPopup(){
        StartCoroutine(ShowPopup(winPanel));
    }

    private void ShowLosePopup(){
        StartCoroutine(ShowPopup(losePanel));
    }

    public void ShowAnyPopup(GameObject popup){
        popup.SetActive(true);
        HideExternButtons();
    }

    private void HideExternButtons()
    {
        foreach (GameObject obj in externHidingButtons)
            obj.SetActive(false);
    }
    private void ShowExternButtons()
    {
        foreach (GameObject obj in externHidingButtons)
            obj.SetActive(true);
    }

    IEnumerator ShowPopup(GameObject popup)
    {
        CanvasGroup cg = popup.AddComponent<CanvasGroup>();
        cg.alpha = 0;
        popup.SetActive(true);
        HideExternButtons();
        while (cg.alpha < 1f)
        {
            cg.alpha += popupShowSpeed * Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null; 
    }
}
