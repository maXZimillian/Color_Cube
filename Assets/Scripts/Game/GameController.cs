using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public event Action<TouchHandler> TouchHandlerInitialized;
    public event Action StartLevel;
    public event Action ZoomOut;
    public event Action WinGame;
    public event Action GameOver;
    private TouchHandler th;
    public TouchHandler TouchPanel 
    { 
        get 
        { 
            return null;
        } 
        set {
            TouchHandlerInitialized?.Invoke(value);
            value.OnUnactiveTouch += OnStartTouch;
            value.OnZoom += OnZoom;
            th = value;
        } 
    }
    public bool MainSceneExecute { get; private set; } = false;

    private void Awake()
    {
        if (FindObjectOfType<MainMenuController>() != null)
        {
            MainSceneExecute = true;
            FindObjectOfType<MainMenuController>().PlayButtonAnimationEnded += () => { MainSceneExecute = false; };
        }
    }

    void Start()
    {
        FinishTile[] fts = FindObjectsOfType<FinishTile>();
        foreach (FinishTile ft in fts)
        {
            ft.Win += OnWin;
            ft.Lose += OnLose;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (MainSceneExecute)
                Application.Quit();
            else
                SceneManager.LoadScene((int)SceneIndex.MainMenu);
        }
    }

    private void OnZoom()
    {
        ZoomOut?.Invoke();
    }
    private void OnStartTouch()
    {
        StartLevel?.Invoke();
    }

    private void OnWin()
    {
        new DataSaver().Save(SceneManager.GetActiveScene().buildIndex+1);
        WinGame?.Invoke();
        EndgameAction();
    }
    private void OnLose()
    {
        GameOver?.Invoke();
        EndgameAction();
    }

    private void EndgameAction()
    {
        th.OnUnactiveTouch -= OnStartTouch;
        th.OnZoom -= OnZoom;
    }
}
