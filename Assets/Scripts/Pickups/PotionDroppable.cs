using UnityEngine;
using System.Collections;
using System;

public class PotionDroppable : MonoBehaviour, IInteractable
{
    public void Start()
    {
        GlobalControl.SaveEvent += SaveFunction;
    }

    public void OnDestroy()
    {
        GlobalControl.SaveEvent -= SaveFunction;
    }

    public void SaveFunction(object sender, EventArgs args)
    {
        SavedDroppablePotion potion = new SavedDroppablePotion();
        potion.PositionX = transform.position.x;
        potion.PositionY = transform.position.y;
        potion.PositionZ = transform.position.z;

        GlobalControl.Instance.GetListForScene().SavedPotions.Add(potion);        

    }

    public void Interact()
    {
        Destroy(gameObject);
    }

    public void LookAt()
    {
        HUDScript.AimedObjectString = "Pick up: Potion";
    }   
}
