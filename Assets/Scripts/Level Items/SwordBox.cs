using UnityEngine;
using System.Collections;

public class SwordBox : MonoBehaviour, IInteractable
{

    public Transform SpawnPosition;

    public GameObject SwordPrefab;

    public void Interact()
    {
        GameObject spawned = (GameObject)Instantiate(SwordPrefab, SpawnPosition.position, SpawnPosition.rotation);
        spawned.transform.rotation = SwordPrefab.transform.rotation;
    }

    public void LookAt()
    {
        HUDScript.AimedObjectString = "[e] Spawn Sword object";
    }


}
