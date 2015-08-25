using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState : MonoBehaviour 
{
	public static PlayerState Instance;

	public Transform playerPosition;

    //TUTORIAL
	public float HP;
	public float Ammo;
	public float XP;

	void Awake()
	{
		if (Instance == null)
			Instance = this;

		if (Instance != this)
			Destroy(gameObject);

	}

	//At start, load data from GlobalControl.
	void Start () 
	{

	}

	void Update()
	{

	}

}
