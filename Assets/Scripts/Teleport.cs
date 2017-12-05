using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Teleport : Photon.MonoBehaviour {

    private Vector3 startingPosition;

    public Vector3 myPlayerPosition;
    public Material inactiveMaterial;
    public Material gazedAtMaterial;
    public bool gazedAt;
    public int playerCube;
    public int cubeNumber;
    public int spriteIndex;

    void Start() {
        startingPosition = transform.localPosition;
        gazedAt = false;
    }

    public void SetGazedAt(bool isGazed) {
        if (isGazed && photonView.isMine)
        {
            gazedAt = true;
            GetComponent<SpriteRenderer>().material = gazedAtMaterial;
            EventManager.TriggerEvent("GazedAtEvent");
        }
        else
        {
            gazedAt = false;
            GetComponent<SpriteRenderer>().material = inactiveMaterial;
            EventManager.TriggerEvent("GazedAtEvent");
        }
        /*if (!photonView.isMine)
        {
            photonView.RPC("RPCSetGazedAt", PhotonTargets.Others, isGazed);
        }*/
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

 /*   [PunRPC]
    public void RPCSetGazedAt(bool isGazed)
    {
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

    }*/

    [PunRPC]
    public void TeleportRandomly() {
        float sign = Mathf.Sign(myPlayerPosition.x - transform.position.x);
          
        Vector3 direction = Random.onUnitSphere;
        direction.x = Mathf.Clamp(direction.x, 0.5f, 1f);
        direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
        float distance = 2.5f;
        transform.position = new Vector3(myPlayerPosition.x + direction.x * distance * sign, myPlayerPosition.y + direction.y * distance,
                     myPlayerPosition.z + direction.z * distance);
    }

    [PunRPC]
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        photonView.RPC("TeleportRandomly", PhotonTargets.All);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(playerCube);
            stream.SendNext(spriteIndex);
        }
        else if (stream.isReading)
        {
            // Network player, receive data
            this.playerCube = (int)stream.ReceiveNext();
            this.spriteIndex = (int)stream.ReceiveNext();
            this.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("SpriteLoader").GetComponent<SpriteLoader>().allSprites[spriteIndex];
        }
    }

    public void RequestOwnership(int viewID)
    {
        photonView.TransferOwnership(viewID);
    }
}
