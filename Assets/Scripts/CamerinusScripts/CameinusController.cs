using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gvr;
using UnityEngine.EventSystems;

public class CameinusController : MonoBehaviour {

    public GameObject head;
    public GameObject playerCamera;
    public GameObject playerCanvas;

    private UnityAction changeMyAvatar;
    private UnityAction changeYourAvatar;

    private Camera cam;

    // Use this for initialization
    void Start()
    {
        changeMyAvatar = new UnityAction(ChangeMyAvatar);
        playerCamera.SetActive(true);
        EventManager.StartListening("ChangeMyAvatar", changeMyAvatar);
    }

    //added for changingroom
    private void Update()
    {
        //head.transform.SetPositionAndRotation(head.transform.position, playerCamera.transform.rotation);

        head.transform.rotation = Quaternion.Euler(playerCamera.transform.rotation.eulerAngles.x, 
            180 - playerCamera.transform.rotation.eulerAngles.y, - playerCamera.transform.rotation.eulerAngles.z);

    }

    private void ChangeMyAvatar()
    {
        RaycastResult r = GvrPointerInputModule.CurrentRaycastResult;
        if (r.gameObject.GetComponent<AvatarSelection>() != null)
        {
            Quaternion rot = r.gameObject.transform.rotation;
            string s = r.gameObject.transform.GetChild(0).name;
            string s1 = head.transform.GetChild(0).name;

            if (s.LastIndexOf("(Clone)") != -1)
                s = s.Substring(0, s.LastIndexOf("(Clone)"));

            if (s1.LastIndexOf("(Clone)") != -1)
                s1 = s1.Substring(0, s1.LastIndexOf("(Clone)"));

            GameObject l = Resources.Load<GameObject>("Camerinus/" + s);
            //added for avatar selection memory
            PlayerPrefs.SetString("ChosenAvatar", "Camerinus/" + s);
            PlayerPrefs.Save();
            //----------
            Debug.Log(s);
            GameObject o = Instantiate(l, new Vector3(0, 0, 0), head.transform.rotation);
            Destroy(head.transform.GetChild(0).gameObject);
            o.transform.parent = head.transform;
            o.transform.localPosition = new Vector3(0, 0, 0);
            
            GameObject l1 = Resources.Load<GameObject>("Camerinus/" + s1);
            GameObject o1 = Instantiate(l1, new Vector3(0, 0, 0), rot);
            o1.transform.position = new Vector3(r.gameObject.transform.position.x, r.gameObject.transform.position.y, r.gameObject.transform.position.z);
            o1.transform.parent = r.gameObject.transform;
            Destroy(r.gameObject.transform.GetChild(0).gameObject);
        }
    }
}
