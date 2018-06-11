using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour {

    public int levelToLoad;

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
