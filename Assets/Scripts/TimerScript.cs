using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    private float time = 0f;
    private bool isRunning = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (isRunning)
        {
            time = time + Time.deltaTime;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = (int)(time % 60);

            GetComponent<Text>().text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }
        if (!isRunning)
        {

        }
	}

    public float GetTimer () 
    {
        return time;
    }

    public void StopTimer ()
    {
        isRunning = false;
    }
}
