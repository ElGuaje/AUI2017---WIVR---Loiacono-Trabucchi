using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour {

    private int levelToLoad;

    // Use this for initialization
    void Start()
    {
        levelToLoad = SceneManager.GetSceneByName("Main Menu").buildIndex;
    }

    public void SetGazedAt(bool isGazed)
    {
        if (isGazed)
        {
            StartCoroutine("StartLoadingScene");
        }
        else
        {
            StopCoroutine("StartLoadingScene");
        }
    }

    IEnumerator StartLoadingScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(levelToLoad);
        
    }
}
