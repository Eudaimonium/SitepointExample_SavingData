using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

	public float walkSpeed = 0.15f;
	public float runSpeed = 0.75f;
	public float sprintSpeed = 1.5f;
	public float flySpeed = 4.0f;

	public float turnSmoothing = 3.0f;
	public float aimTurnSmoothing = 15.0f;
	public float speedDampTime = 0.1f;

	public float jumpHeight = 5.0f;
	public float jumpCooldown = 1.0f;

	private float timeToNextJump = 0;
	
	private float speed;

	private Vector3 lastDirection;

	private Animator anim;
	private int speedFloat;
	private int jumpBool;
	private int hFloat;
	private int vFloat;
	private int aimBool;
	private int flyBool;
	private int groundedBool;
	public Transform cameraTransform;

	private float h;
	private float v;

	private bool aim;

	private bool run;
	private bool sprint;

	private bool isMoving;

	public static bool PlayerHasControl = true;

	// fly
	private bool fly = false;
	private float distToGround;
	private float sprintFactor;

	//Combat
	public static bool isInCombatStance = false;
	public static bool leaveCombatStance = false;

	public Transform upperBodyTransform;

    public static MovementScheme PlayerMovementScheme;

    public static float runCooldownTimer = 0f;


	void Awake()
	{
		anim = GetComponent<Animator> ();

		if (cameraTransform == null)
			cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

		speedFloat = Animator.StringToHash("Speed");
		jumpBool = Animator.StringToHash("Jump");
		hFloat = Animator.StringToHash("H");
		vFloat = Animator.StringToHash("V");
		aimBool = Animator.StringToHash("Aim");
		// fly
		flyBool = Animator.StringToHash ("Fly");
		groundedBool = Animator.StringToHash("Grounded");
		distToGround = GetComponent<Collider>().bounds.extents.y;
		sprintFactor = sprintSpeed / runSpeed;

	}

	void Start()
	{
		if (cameraTransform == null)
			cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

		if (GlobalControl.Instance.TransitionTarget != null)
			gameObject.transform.position = GlobalControl.Instance.TransitionTarget.position;


        if (GlobalControl.Instance.IsSceneBeingLoaded)
        {
            PlayerState.Instance.localPlayerData = GlobalControl.Instance.LocalCopyOfData;

            transform.position = new Vector3(
                            GlobalControl.Instance.LocalCopyOfData.PositionX,
                            GlobalControl.Instance.LocalCopyOfData.PositionY,
                            GlobalControl.Instance.LocalCopyOfData.PositionZ + 0.1f);            
        }
	}

	bool IsGrounded() 
	{
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	void Update()
	{
        if (Input.GetKey(KeyCode.F5))
        {
            PlayerState.Instance.localPlayerData.SceneID = Application.loadedLevel;
            PlayerState.Instance.localPlayerData.PositionX = transform.position.x;
            PlayerState.Instance.localPlayerData.PositionY = transform.position.y;
            PlayerState.Instance.localPlayerData.PositionZ = transform.position.z;

            GlobalControl.Instance.SaveData();
        }

        if (Input.GetKey(KeyCode.F9))
        {
            GlobalControl.Instance.LoadData();
            GlobalControl.Instance.IsSceneBeingLoaded = true;

            int whichScene = GlobalControl.Instance.LocalCopyOfData.SceneID;

            Application.LoadLevel(whichScene);
        }


		// fly

		if (PlayerControl.PlayerHasControl)
		{
            if (runCooldownTimer < 0.1)
                run = Input.GetButton("Run");
            else
            {
                run = false;
                runCooldownTimer -= Time.deltaTime;
            }

			if(Input.GetButtonDown ("Fly"))
				fly = !fly;
			aim = Input.GetButton("Aim");


		}


			h = Input.GetAxis("Horizontal");
			v = Input.GetAxis("Vertical");
			
			isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;
	}

	void FixedUpdate()
	{
			anim.SetBool (aimBool, IsAiming());
			anim.SetFloat(hFloat, h);
			anim.SetFloat(vFloat, v);
			
			// Fly
			anim.SetBool (flyBool, fly);
			GetComponent<Rigidbody>().useGravity = !fly;
			anim.SetBool (groundedBool, IsGrounded ());
			if(fly)
				FlyManagement(h,v);

			else
			{

				MovementManagement (h, v, run, sprint);
				JumpManagement ();
			}
	}

	// fly
	void FlyManagement(float horizontal, float vertical)
	{
		Vector3 direction = Rotating(horizontal, vertical);
		GetComponent<Rigidbody>().AddForce(direction * flySpeed * 100 * (sprint?sprintFactor:1));
	}

	void JumpManagement()
	{
		if (GetComponent<Rigidbody>().velocity.y < 10) // already jumped
		{
			anim.SetBool (jumpBool, false);
			if(timeToNextJump > 0)
				timeToNextJump -= Time.deltaTime;
		}
		if (Input.GetButtonDown ("Jump"))
		{
			if (!isInCombatStance)
			{
				anim.SetBool(jumpBool, true);
				if(speed > 0 && timeToNextJump <= 0 && !aim)
				{
					GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0);
					timeToNextJump = jumpCooldown;
				}
			}
		}
	}

	void MovementManagement(float horizontal, float vertical, bool running, bool sprinting)
	{
		Rotating(horizontal, vertical);

		if(isMoving)
		{
            /*
			if(sprinting)
			{
				speed = sprintSpeed;
                PlayerState.UseFocus(0.2f, 2f);
			}
			else */
            if (running)
            {
                if (PlayerMovementScheme == MovementScheme.Interior)
                {
                    speed = runSpeed;
                    sprint = false;
                }
                else
                {
                    speed = sprintSpeed;
                    sprint = true;
                }
            }
            else
            {
                if (PlayerMovementScheme == MovementScheme.Interior)
                    speed = walkSpeed;
                else
                    speed = runSpeed;

                sprint = false;
            }

			anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
		}
		else
		{
			speed = 0f;
            anim.SetFloat(speedFloat, 0f, speedDampTime, Time.deltaTime);
		}
		GetComponent<Rigidbody>().AddForce(Vector3.forward*speed);
	}

	Vector3 Rotating(float horizontal, float vertical)
	{
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		if (!fly)
			forward.y = 0.0f;
		forward = forward.normalized;

		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		Vector3 targetDirection;

		float finalTurnSmoothing;

		if(IsAiming())
		{
			targetDirection = forward;
			finalTurnSmoothing = aimTurnSmoothing;
		}
		else
		{
			targetDirection = forward * vertical + right * horizontal;
			finalTurnSmoothing = turnSmoothing;
		}

		if((isMoving && targetDirection != Vector3.zero) || IsAiming())
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
			// fly
			if (fly)
				targetRotation *= Quaternion.Euler (90, 0, 0);

			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
			lastDirection = targetDirection;
		}
		//idle - fly or grounded
		if(!(Mathf.Abs(h) > 0.9 || Mathf.Abs(v) > 0.9))
		{
			Repositioning();
		}

		return targetDirection;
	}	

	private void Repositioning()
	{
		Vector3 repositioning = lastDirection;
		if(repositioning != Vector3.zero)
		{
			repositioning.y = 0;
			Quaternion targetRotation = Quaternion.LookRotation (repositioning, Vector3.up);
			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
		}
	}

	public bool IsFlying()
	{
		return fly;
	}

	public bool IsAiming()
	{
		return aim && !fly;
	}

	public bool isSprinting()
	{
		return sprint && !aim && (isMoving);
	}


}

public enum MovementScheme
{
    Interior,
    Exterior
}
