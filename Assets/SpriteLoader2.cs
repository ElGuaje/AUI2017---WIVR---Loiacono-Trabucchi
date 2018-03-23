using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader2 : MonoBehaviour {

    public Sprite[] allSprites;
    public List<Sprite> spriteList;


    void Start()
    {

        allSprites = Resources.LoadAll<Sprite>(RemoteSettings.GetString("path2"));

        Debug.Log("Hi boyz. We have: " + allSprites.Length);
    }
}
