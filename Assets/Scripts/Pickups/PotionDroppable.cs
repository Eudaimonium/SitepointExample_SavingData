using UnityEngine;
using System.Collections;
using System;

public class PotionDroppable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Destroy(gameObject);
    }

    public void LookAt()
    {
        HUDScript.AimedObjectString = "Pick up: Potion";
    }

   
}
