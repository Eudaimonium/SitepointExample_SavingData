using UnityEngine;
using System.Collections;

public class AmmoBoxScript : MonoBehaviour, IInteractable
{
	//Amount of ammo to add per one use
	public float AmmoAmount = 100f;


	//Here we now implement an interface. We do this so we can simply say to the camera Raycaster:
	//OK, if you've hit anything that implements the IInteractable, then execute the functions from IInteractable,
	//regardless of what class this actually is, we know it has the functions because it implements the interface.
	//See camera raycaster class for more info. 
	#region IInteractable implementation
	public void LookAt ()
	{
		HUDScript.AimedObjectString = "+ " + AmmoAmount.ToString("0") + " ammo";
	}
	public void Interact ()
	{
		PlayerState.Instance.localPlayerData.Ammo += AmmoAmount;


	}
	#endregion
}
