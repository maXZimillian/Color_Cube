using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] int additiveScenesCount = 1;    
    [SerializeField] Text prevLevel;
    [SerializeField] Text currLevel;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject upsidePanel;
    [SerializeField] private float animationTime;
    private int maxLevelToOpen = 1;
    private int levelToOpen = 1;
    private int levelsCount;

    public event Action<int> CurrentLevelChanged;
    public event Action PlayButtonPressed;
    public event Action PlayButtonAnimationEnded;

    private void Start()
    {
        //new DataSaver().Reset();
        levelsCount = SceneManager.sceneCountInBuildSettings;
        int currentLevel = ValidateLevelValue(new DataSaver().Load().currentLevel);
        levelToOpen = maxLevelToOpen = ValidateLevelValue(currentLevel);
        ChangeTextValues();
        CurrentLevelChanged?.Invoke(levelToOpen);
        StartCoroutine(LoadLevelAsync(levelToOpen));

    }

    private int ValidateLevelValue(int value)
    {
        if (value <additiveScenesCount)
        {
            value = additiveScenesCount;
        }
        if (value > levelsCount - 1)
        {
            value = levelsCount - 1;
        }
        return value;
    }

    private void ChangeTextValues()
    {
        prevLevel.text = levelToOpen - additiveScenesCount != 0 ? (levelToOpen - additiveScenesCount).ToString() : "";
        currLevel.text = (levelToOpen - additiveScenesCount+1).ToString();
    }

    public void ChangeLevel(int additiveValue)
    {
        levelToOpen = ValidateLevelValue(levelToOpen + additiveValue);
        if (levelToOpen > maxLevelToOpen)
        {
            levelToOpen = maxLevelToOpen;
        }
        int prevLevel = levelToOpen - 1;
        ChangeTextValues();
        CurrentLevelChanged?.Invoke(levelToOpen);        
    }

    public void PlayButtonClick(){
        PlayButtonPressed?.Invoke();
        if(upsidePanel!=null)
            StartCoroutine(Lerp(upsidePanel, Vector3.up * 1000));
        if(playButton!=null)
            StartCoroutine(Lerp(playButton, Vector3.down * 1000));
        StartCoroutine(PlayButtonDeferredAction());
        playButton.SetActive(false);
    }
    private IEnumerator PlayButtonDeferredAction()
    {
        yield return new WaitForSeconds(animationTime);
        PlayButtonAnimationEnded();
        canvas.enabled = false;
    }

    private IEnumerator Lerp(GameObject obj,Vector3 offset)
    {
        Vector3 destPosition = obj.transform.position+offset;
        while ((obj.transform.position-destPosition).magnitude>0.1f)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, destPosition, 2f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ExitGame(){
        Application.Quit();
    }

    private IEnumerator LoadLevelAsync(int sceneIndex)
    {
        yield return SceneManager.LoadSceneAsync(sceneIndex,LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));       
    }

}
