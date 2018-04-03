using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelection : MonoBehaviour {

    public int sceneToLoad;

	// Use this for initialization
	void Start ()
    {
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
        SceneManager.LoadScene(sceneToLoad);
    }
}
