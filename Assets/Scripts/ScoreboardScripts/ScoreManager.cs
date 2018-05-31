using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

public class ScoreManager : MonoBehaviour {

	// The map we're building is going to look like:
	//
	//	LIST OF USERS -> A User -> LIST OF SCORES for that user
	//

	Dictionary< string, Dictionary<string, string> > playerScores;
    string myPath; 
    int changeCounter = 0;
    StreamReader sr;

	void Start() {
        myPath = Application.persistentDataPath;
        LoadScores();
	}

	void Init() {
		if(playerScores != null)
			return;

		playerScores = new Dictionary<string, Dictionary<string, string>>();
	}

    internal void LoadScores()
    {
        if(File.Exists(myPath + "/ScoreFile.txt"))
        {
            sr = new StreamReader(myPath + "/ScoreFile.txt");
            while (!sr.EndOfStream)
            {
                string[] s = sr.ReadLine().Split(' ');
                SetScore(s[0], s[1], s[2]);
            }
            sr.Close();
        }
        else
        {
            Debug.Log("File ScoreFile non esistente");
            File.Create(myPath + "/ScoreFile.txt");
        }
    }

    public void Reset() {
		changeCounter++;
		playerScores = null;
	}

	public int GetScore(string username, string scoreType) {
		Init ();
		if(playerScores.ContainsKey(username) == false) {
			// We have no score record at all for this username
			return 0;
		}

		if(playerScores[username].ContainsKey(scoreType) == false) {
			return 0;
		}

        string[] score = playerScores[username][scoreType].Split(':');
        int scoreInTime = int.Parse(score[0]) * 60 + int.Parse(score[1]);
        
		return scoreInTime;
	}

    public string GetTime(string username)
    {
        Init();
        if (playerScores.ContainsKey(username) == false)
        {
            // We have no score record at all for this username
            return " ";
        }

        if (playerScores[username].ContainsKey("Time") == false)
        {
            return " ";
        }

        return playerScores[username]["Time"];
    }

    public void SetScore(string username, string gameMode, string time) {
		Init ();

		changeCounter++;

		if(playerScores.ContainsKey(username) == false) {
			playerScores[username] = new Dictionary<string, string>();
		}

		playerScores[username][gameMode] = time;
	}

	public string[] GetPlayerNames() {
		Init ();
		return playerScores.Keys.ToArray();
	}
	
	public string[] GetPlayerNames(string sortingScoreType) {
		Init ();
		return playerScores.Keys.OrderBy( n => GetScore(n, sortingScoreType) ).ToArray();

	}

	public int GetChangeCounter() {
		return changeCounter;
	}

}
