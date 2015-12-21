using UnityEngine;
using System.Collections;
using System;

public class PotionBox : MonoBehaviour, IInteractable
{
    public Transform SpawnPosition;

    public GameObject PotionPrefab;

    public void Interact()
    {
        GameObject spawned = (GameObject)Instantiate(PotionPrefab, SpawnPosition.position, SpawnPosition.rotation);
        spawned.transform.rotation = PotionPrefab.transform.rotation;
    }

    public void LookAt()
    {
        HUDScript.AimedObjectString = "[e] Spawn Potion object";
    }


  
}
