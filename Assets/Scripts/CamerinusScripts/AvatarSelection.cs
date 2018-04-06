using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelection : MonoBehaviour {

    public bool isChangingMe = true;

    public void SetGazedAt(bool isGazed)
    {
        if (isChangingMe)
        {
            if (isGazed)
            {
                StartCoroutine("StartChangingMyAvatar");
            }
            else
            {
                StopCoroutine("StartChangingMyAvatar");
            }
        }
        else
        {
            if (isGazed)
            {
                StartCoroutine("StartChangingYourAvatar");
            }
            else
            {
                StopCoroutine("StartChangingYourAvatar");
            }
        }
    }
    
    IEnumerator StartChangingMyAvatar()
    {
        yield return new WaitForSeconds(5);
        EventManager.TriggerEvent("ChangeMyAvatar");
        SoundManager.Instance.ObjectFound();
    }

    IEnumerator StartChangingYourAvatar()
    {
        yield return new WaitForSeconds(5);
        EventManager.TriggerEvent("ChangeYourAvatar");
    }
}
