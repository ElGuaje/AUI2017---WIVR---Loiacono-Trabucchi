using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour {

    public Sprite[] allSprites;
    public List<Sprite> spriteList;
    public string path;

    void Start()
    {

        allSprites = Resources.LoadAll<Sprite>(RemoteSettings.GetString(path));

    }

}
