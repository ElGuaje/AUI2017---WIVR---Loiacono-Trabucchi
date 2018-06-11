using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour {

    public Sprite[] allSprites;
    public List<Sprite> spriteList;
    public string objectSet1Path;
    public string objectSet2Path;
    public string objectSet3Path;
    public string objectSet4Path;
    public string objectSet5Path;

    private int dropdownImages;

    void Start()
    {
        dropdownImages = PlayerPrefs.GetInt("DropdownImages");

        if (dropdownImages == 0)
        {
            allSprites = Resources.LoadAll<Sprite>(objectSet1Path);
        }
        else if (dropdownImages == 1)
        {
            allSprites = Resources.LoadAll<Sprite>(objectSet2Path);
        }
        else if (dropdownImages == 2)
        {
            allSprites = Resources.LoadAll<Sprite>(objectSet3Path);
        }
        else if (dropdownImages == 3)
        {
            allSprites = Resources.LoadAll<Sprite>(objectSet4Path);
        }
        else if (dropdownImages == 4)
        {
            allSprites = Resources.LoadAll<Sprite>(objectSet5Path);
        }
    }

}
