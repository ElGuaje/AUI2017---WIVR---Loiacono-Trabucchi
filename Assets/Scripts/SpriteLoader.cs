using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour {

    public Sprite[] allSprites;

    void Start()
    {
        allSprites = Resources.LoadAll<Sprite>("Emojis");
        Debug.Log("Hi boyz. We have: " + allSprites.Length);
    }

}
