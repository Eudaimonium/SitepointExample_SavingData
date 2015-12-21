using UnityEngine;
using System.Collections;
using System;

public class SwordDroppable : MonoBehaviour, IInteractable
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
        SavedDroppableSword sword = new SavedDroppableSword();
        sword.PositionX = transform.position.x;
        sword.PositionY = transform.position.y;
        sword.PositionZ = transform.position.z;

        GlobalControl.Instance.GetListForScene().SavedSword.Add(sword);

    }

    public void Interact()
    {
        Destroy(gameObject);
    }

    public void LookAt()
    {
        HUDScript.AimedObjectString = "Pick up: Sword";
    }
}
