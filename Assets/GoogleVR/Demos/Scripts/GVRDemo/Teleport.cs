using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Teleport : Photon.MonoBehaviour {

    private Vector3 startingPosition;

    public Material inactiveMaterial;
    public Material gazedAtMaterial;

    void Start() {
          startingPosition = transform.localPosition;
          SetGazedAt(false);
    }

    public void SetGazedAt(bool gazedAt) {
          if (inactiveMaterial != null && gazedAtMaterial != null) {
               StartCoroutine("TimerToTeleport");   
               GetComponent<Renderer>().material = gazedAt ? gazedAtMaterial : inactiveMaterial;
               return;
          }
          GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
    }

    public void Reset() {
          transform.localPosition = startingPosition;
          StopCoroutine("TimerToTeleport");
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
          Vector3 direction = Random.onUnitSphere;
          direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
          float distance = 2 * Random.value + 1.5f;
          transform.localPosition = direction * distance;
    }

    IEnumerator TimerToTeleport()
    {
        yield return new WaitForSeconds(5);
        photonView.RPC("TeleportRandomly", PhotonTargets.MasterClient);
    }
}
