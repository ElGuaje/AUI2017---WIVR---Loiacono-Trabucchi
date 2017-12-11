using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour {

    public Sprite[] allSprites;

    private string path;

    private void Awake()
    {
        allSprites = new Sprite[RemoteSettings.GetInt("numberOfImages")];
        path = RemoteSettings.GetString("path");
    }

    IEnumerator Start()
    {
        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
        WWW www = new WWW(path);
        Debug.Log(www.error);
        yield return www;
        www.LoadImageIntoTexture(tex);
        Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f); 
        allSprites[0] = mySprite;
    }

}
