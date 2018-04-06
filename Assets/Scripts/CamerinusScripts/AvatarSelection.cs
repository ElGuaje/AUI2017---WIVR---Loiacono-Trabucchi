using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelection : MonoBehaviour {

    public void SetGazedAt(bool isGazed)
    {
        if (isGazed)
        {
            StartCoroutine("StartChangingOutfit");
        }
        else
        {
            StopCoroutine("StartChangingOutfit");
        }
    }

    IEnumerator StartChangingOutfit()
    {
        yield return new WaitForSeconds(5);
    }
}
