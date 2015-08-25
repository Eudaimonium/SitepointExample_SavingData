using UnityEngine;
using System.Collections;

public class HPBoxScript : MonoBehaviour, IInteractable
{
	//Amount of HP to add per one use
	public float HPAmount = 10f;



	#region IInteractable implementation
	public void LookAt ()
	{
		HUDScript.AimedObjectString = "+ " + HPAmount.ToString("0") + " HP";
	}
	public void Interact ()
	{
		PlayerState.Instance.localPlayerData.HP += HPAmount;
	}
	#endregion
}
