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
		HPText.text = "HP: " + PlayerState.Instance.HP.ToString();
		AmmoText.text = "Ammo: " + PlayerState.Instance.Ammo.ToString();
		XPText.text = "XP: " + PlayerState.Instance.XP.ToString();

		AimedAtText.text = AimedObjectString;
	}
}
