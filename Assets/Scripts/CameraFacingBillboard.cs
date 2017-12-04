using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject p in players)
        {
            if (p.GetPhotonView().isMine)
            {
                m_Camera = p.GetComponentInChildren<Camera>();
            }
        }
    }

    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}