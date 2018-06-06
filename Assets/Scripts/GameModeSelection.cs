using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelection : MonoBehaviour {

    public int level1Scene;
    public int level2Scene;
    public int level3Scene;

    private int levelToLoad;

	// Use this for initialization
	void Start ()
    {
        levelToLoad = PlayerPrefs.GetInt("LevelAmbient");
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
        if (levelToLoad == 0)
        {
            SceneManager.LoadScene(level1Scene);
        }
        else if (levelToLoad == 1)
        {
            SceneManager.LoadScene(level2Scene);
        }
        else if (levelToLoad == 2)
        {
            SceneManager.LoadScene(level3Scene);
        }

    }
}
