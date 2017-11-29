using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Teleport : Photon.MonoBehaviour {

    private Vector3 startingPosition;
    
    public Material inactiveMaterial;
    public Material gazedAtMaterial;
    public bool gazedAt;
    public bool isMine;

    void Start() {
          startingPosition = transform.localPosition;
          isMine = photonView.isMine;
          SetGazedAt(false);
    }

    public void SetGazedAt(bool isGazed) {
        if (isGazed)
        {
            gazedAt = true;
            GetComponent<Renderer>().material = gazedAtMaterial;
            EventManager.TriggerEvent("GazedAtEvent");
        }
        else
        {
            gazedAt = false;
            GetComponent<Renderer>().material = inactiveMaterial;
            EventManager.TriggerEvent("GazedAtEvent");
        }
        if (!photonView.isMine)
        {
            photonView.RPC("RPCSetGazedAt", PhotonTargets.Others, isGazed);
        }
    }

    public void Reset() {
          transform.localPosition = startingPosition;
    }

    public void Recenter() {
      #if !UNITY_EDITOR
          GvrCardboardHelpers.Recenter();
      #else
          GvrEditorEmulator emulator = FindObjectOfType<GvrEditorEmulator>();
          if (emulator == null) {
              return;
          }
          emulator.Recenter();
      #endif  // !UNITY_EDITOR
    }

    [PunRPC]
    public void RPCSetGazedAt(bool isGazed)
    {
        Debug.Log("Oh God!");
        if (isGazed)
        {
            gazedAt = true;
            GetComponent<Renderer>().material = gazedAtMaterial;
            EventManager.TriggerEvent("GazedAtEvent");
        }
        else
        {
            gazedAt = false;
            GetComponent<Renderer>().material = inactiveMaterial;
            EventManager.TriggerEvent("GazedAtEvent");
        }
    }

    [PunRPC]
    public void TeleportRandomly() {
          Vector3 direction = Random.onUnitSphere;
          direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
          float distance = 2 * Random.value + 1.5f;
          transform.localPosition = direction * distance;
    }
}
