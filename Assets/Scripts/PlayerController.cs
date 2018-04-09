using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gvr;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{

    public GameObject head;
    public GameObject playerCamera;
    public GameObject playerCanvas;

    private UnityAction changeMyAvatar;
    private UnityAction changeYourAvatar;

    private Camera cam;

    // Used to check if is this user's player or an external player
    public bool isControllable;

    // Use this for initialization
    void Start()
    {
        changeMyAvatar = new UnityAction(ChangeMyAvatar);

        if (isControllable)
        {
            playerCamera.SetActive(true);
            head.SetActive(false);
            EventManager.StartListening("ChangeMyAvatar", changeMyAvatar);
        }
        else
        {
            playerCanvas.SetActive(false);
            playerCamera.SetActive(false);
            EventManager.StartListening("ChangeYourAvatar", changeMyAvatar);
        }
    }

    private void ChangeMyAvatar()
    {
        RaycastResult r = GvrPointerInputModule.CurrentRaycastResult;
        if (r.gameObject.GetComponent<AvatarSelection>() != null)
        {
            string s = r.gameObject.transform.GetChild(0).name;
            string s1 = head.transform.GetChild(0).name;

            if (s.LastIndexOf("(Clone)") != -1)
                s = s.Substring(0, s.LastIndexOf("(Clone)"));

            if (s1.LastIndexOf("(Clone)") != -1)
                s1 = s1.Substring(0, s1.LastIndexOf("(Clone)"));

            GameObject l = Resources.Load<GameObject>("Camerinus/" + s);
            Debug.Log(s);
            GameObject o = Instantiate(l, new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(head.transform.GetChild(0).gameObject);
            o.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1.5f, this.gameObject.transform.position.z);
            if (!isControllable)
            {
                o.transform.rotation = head.transform.rotation;
            }
            o.transform.parent = head.transform;

            GameObject l1 = Resources.Load<GameObject>("Camerinus/" + s1);
            GameObject o1 = Instantiate(l1, new Vector3(0, 0, 0), Quaternion.identity);
            o1.transform.position = new Vector3(r.gameObject.transform.position.x, r.gameObject.transform.position.y, r.gameObject.transform.position.z);
            o1.transform.parent = r.gameObject.transform;
            Destroy(r.gameObject.transform.GetChild(0).gameObject);
        }
    }

}