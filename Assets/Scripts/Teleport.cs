using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Teleport : Photon.MonoBehaviour {

    private Vector3 startingPosition;

    public Vector3 myPlayerPosition;
    public Material inactiveMaterialNotMine;
    public Material inactiveMaterialMine;
    public Material gazedAtMaterial;
    public bool gazedAt;
    public int playerCube;
    public int cubeNumber;
    public int spriteIndex;
    public int realIndex;
    public bool canTeleport = true;

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
            if (photonView.isMine)
            {
                GetComponent<SpriteRenderer>().material = inactiveMaterialMine;
                EventManager.TriggerEvent("GazedAtEvent");
            }
            else
            {
                GetComponent<SpriteRenderer>().material = inactiveMaterialNotMine;
                EventManager.TriggerEvent("GazedAtEvent");
            }
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
    public void TeleportRandomly() {

        if (canTeleport)
        {
            Vector3 direction = Random.onUnitSphere;

            if (direction.y < -0.1f)
            {
                direction.y = -direction.y;
            }

            float distance = 4f;
            transform.position = new Vector3(myPlayerPosition.x + direction.x * distance, 3 + direction.y * distance,
                         myPlayerPosition.z + direction.z * distance);
            gameObject.GetComponent<SpriteMover>().Recalibrate();
        }
    }

    [PunRPC]
    public void Deactivate()
    {
        if (gameObject.activeInHierarchy)
        {
            PhotonNetwork.Instantiate("Bubbles", transform.position, Quaternion.identity, 0);
            SoundManager.Instance.ObjectFound();
            EventManager.TriggerEvent("ElementFound");
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("MemoryElement"))
        {
            photonView.RPC("TeleportRandomly", PhotonTargets.All);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(playerCube);
            stream.SendNext(spriteIndex);
            stream.SendNext(realIndex);

        }
        else if (stream.isReading)
        {
            // Network player, receive data
            this.playerCube = (int)stream.ReceiveNext();
            this.spriteIndex = (int)stream.ReceiveNext();
            this.realIndex = (int)stream.ReceiveNext();
            this.GetComponent<SpriteRenderer>().sprite = GameObject.FindGameObjectWithTag("SpriteLoader").GetComponent<SpriteLoader>().allSprites[realIndex];
        }
    }

    public void RequestOwnership(int viewID)
    {
        photonView.TransferOwnership(viewID);
    }
}
