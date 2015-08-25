using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour 
{
	public Text HPText;
	public Text AmmoText;
	public Text XPText;

	public Text AimedAtText;

	public static string AimedObjectString;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		HPText.text = "HP: " + PlayerState.Instance.localPlayerData.HP.ToString();
		AmmoText.text = "Ammo: " + PlayerState.Instance.localPlayerData.Ammo.ToString();
		XPText.text = "XP: " + PlayerState.Instance.localPlayerData.XP.ToString();

		AimedAtText.text = AimedObjectString;
	}
}
