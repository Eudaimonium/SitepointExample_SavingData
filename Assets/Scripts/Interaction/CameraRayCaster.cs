using UnityEngine;
using System.Collections;

public class CameraRayCaster : MonoBehaviour 
{
	public Camera camera;
	// Use this for initialization
	void Start () 
	{
		if (camera == null)
			camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Get middle of the screen
		int x = Screen.width / 2;
		int y = Screen.height /2;

		Ray InteractableRay = camera.ScreenPointToRay(new Vector3(x,y,0));

		RaycastHit hit;

		//Cast on all "Items" (layer 10)
		if (Physics.Raycast(InteractableRay, out hit, 5, 1<<10))
		{
			//All Items should use IInteractable interface, and implement LookAt and Interact functions.
			IInteractable obj = hit.transform.gameObject.GetComponent<IInteractable>();
			obj.LookAt();
			if (Input.GetButtonDown("Use"))
				obj.Interact();
		}
		else
		//Cast on all transition items (layer 8)
		if (Physics.Raycast(InteractableRay, out hit, 5, 1<<8))
		{
			//All transition items will have TransitionScript, so we grab that script and execute it's functions.
			TransitionScript obj = hit.transform.gameObject.GetComponent<TransitionScript>();
			obj.AimAt();
			if (Input.GetButtonDown("Use"))
				obj.Interact();
		}
		else //if we haven't hit anything, clear the string so we don't have leftover messages when not aiming at anything.
			HUDScript.AimedObjectString = "";


	}
}
