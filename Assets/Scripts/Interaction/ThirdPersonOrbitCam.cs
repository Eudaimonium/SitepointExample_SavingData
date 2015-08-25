using UnityEngine;
using System.Collections;

public class ThirdPersonOrbitCam : MonoBehaviour 
{
	public Transform player;
	public Texture2D crosshair;
	
	public Vector3 pivotOffset = new Vector3(0.0f, 1.0f,  0.0f);
	public Vector3 camOffset   = new Vector3(0.0f, 0.7f, -3.0f);

	public float smooth = 10f;

	public Vector3 aimPivotOffset = new Vector3(0.0f, 1.7f,  -0.3f);
	public Vector3 aimCamOffset   = new Vector3(0.8f, 0.0f, -1.0f);

	public float horizontalAimingSpeed = 400f;
	public float verticalAimingSpeed = 400f;
	public float maxVerticalAngle = 30f;
	public float flyMaxVerticalAngle = 60f;
	public float minVerticalAngle = -60f;
	
	public float mouseSensitivity = 0.3f;

	public float sprintFOV = 100f;

	public float WallDistanceCorrection = 0.3f;
	
	private PlayerControl playerControl;
	private float angleH = 0;
	private float angleV = 0;
	private Transform cam;

	
	private Vector3 smoothPivotOffset;
	private Vector3 smoothCamOffset;
	private Vector3 targetPivotOffset;
	private Vector3 targetCamOffset;

	private float defaultFOV;
	private float targetFOV;

	private Vector3 targetWallOffset, wallOffset;

	void Awake()
	{
		cam = transform;
		playerControl = player.GetComponent<PlayerControl> ();
        
		smoothPivotOffset = pivotOffset;
		smoothCamOffset = camOffset;
		targetWallOffset = Vector3.zero;

		defaultFOV = cam.GetComponent<Camera>().fieldOfView;
	}

	void LateUpdate()
	{
		if (PlayerControl.PlayerHasControl)
		{

			angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed * Time.deltaTime;
			angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed * Time.deltaTime;

		}
			// fly
			if(playerControl.IsFlying())
			{
				angleV = Mathf.Clamp(angleV, minVerticalAngle, flyMaxVerticalAngle);
			}
			else
			{
				angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
			}


			Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
			Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
			cam.rotation = aimRotation;

			if(playerControl.IsAiming())
			{
				targetPivotOffset = aimPivotOffset;
				targetCamOffset = aimCamOffset;
			}
			else
			{
				targetPivotOffset = pivotOffset;
				targetCamOffset = camOffset;
			}

			if(playerControl.isSprinting())
			{
				targetFOV = sprintFOV;
			}
			else
			{
				targetFOV = defaultFOV;
			}
			cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp (cam.GetComponent<Camera>().fieldOfView, targetFOV,  Time.deltaTime);

			// Test for collision
			Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
			Vector3 tempOffset = targetCamOffset;

			//New calculation: only one ray cast against objects.

			Ray camPositionRay = new Ray(baseTempPosition, aimRotation * tempOffset);
			RaycastHit hit;
			wallOffset = Vector3.zero;

			if (Physics.Raycast(camPositionRay, out hit, 2f, 1))
			{
				tempOffset.z = -(hit.distance);
				wallOffset = new Vector3(hit.normal.x * WallDistanceCorrection, 
				                         hit.normal.y * WallDistanceCorrection, 
				                         hit.normal.z * WallDistanceCorrection);
			}
			else
			{
				if (playerControl.IsAiming())
				{
					tempOffset.z = aimCamOffset.z;
				}
				else
					tempOffset.z = camOffset.z;
			}


			targetCamOffset.z = tempOffset.z;

			// fly
			if(playerControl.IsFlying())
			{
				targetCamOffset.y = 0;
			}
			
			targetWallOffset = Vector3.Lerp (targetWallOffset, wallOffset, smooth * Time.deltaTime);

			smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
			smoothCamOffset = Vector3.Lerp(smoothCamOffset, targetCamOffset, smooth * Time.deltaTime);

			cam.position =  targetWallOffset + player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

	}


	// Crosshair
	void OnGUI () 
	{
		float mag = Mathf.Abs ((aimPivotOffset - smoothPivotOffset).magnitude);
		if (playerControl.IsAiming() &&  mag < 0.05f)
			GUI.DrawTexture(new Rect(Screen.width/2-(crosshair.width*0.5f), 
			                         Screen.height/2-(crosshair.height*0.5f), 
			                         crosshair.width, crosshair.height), crosshair);

	}
}
