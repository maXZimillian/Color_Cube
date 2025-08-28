using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPopupController : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private bool stopTimeOnShow;
    [SerializeField] private GameObject[] externHidingButtons = null;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject exitButton;
    private float prevousTimeScale=0f;
    private void OnEnable() {
        if(stopTimeOnShow){
            prevousTimeScale=Time.timeScale;
            Time.timeScale = 0f;
        }
        if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings && nextLevelButton != null)
        {
            nextLevelButton.SetActive(false);
            if(exitButton != null)
                exitButton.SetActive(true);
        }
    }
    public void HidePopup(){
        if(stopTimeOnShow)
            Time.timeScale = prevousTimeScale;
        popup.SetActive(false);
        if (externHidingButtons != null)
        {
            foreach(GameObject obj in externHidingButtons)
                obj.SetActive(true);
        }
    }
    public void NextLevel(){
        if (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else
        {
            LoadMainMenu();
        }
    }
    public void ReloadScene(){
        if(stopTimeOnShow)
            Time.timeScale = 1f;
        StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().buildIndex));
    }
    public void LoadMainMenu(){
        if(stopTimeOnShow)
            Time.timeScale = 1f;
        StartCoroutine(LoadLevelAsync((int)SceneIndex.MainMenu));
    }

    private IEnumerator LoadLevelAsync(int sceneIndex)
    {
        yield return SceneManager.LoadSceneAsync(sceneIndex);
    }
}
