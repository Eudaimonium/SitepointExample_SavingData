using UnityEngine;
using System.Collections;

public class XPBoxScript : MonoBehaviour, IInteractable
{
	//Amount of XP to add per one use
	public int XPAmount = 50;


	#region IInteractable implementation
	public void LookAt ()
	{
		HUDScript.AimedObjectString = "+ " + XPAmount.ToString() + " XP";
	}
	public void Interact ()
	{
		PlayerState.Instance.localPlayerData.XP += XPAmount;
	}
	#endregion
}
