using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveUISceneController : MonoBehaviour
{
    [SerializeField] private CubeMovingController cube;
    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }
 
    private IEnumerator LoadSceneAsync()
    {
        yield return SceneManager.LoadSceneAsync((int)SceneIndex.LevelBase, LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync((int)SceneIndex.LevelUI, LoadSceneMode.Additive);
        UserUIComponentsProducer UIComponentsProducer = FindObjectOfType<UserUIComponentsProducer>();
        cube.TouchHandler = UIComponentsProducer.touchPanel;
        FindObjectOfType<GameController>().TouchPanel = UIComponentsProducer.touchPanel;
    }
}
