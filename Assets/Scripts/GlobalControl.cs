using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Master class, needs to be in every level.
/// </summary>
public class GlobalControl : MonoBehaviour 
{
	public static GlobalControl Instance;

    //TUTORIAL
    public PlayerStatistics savedPlayerData = new PlayerStatistics();

    public List<SavedDroppableList> SavedLists;


    public delegate void SaveDelegate(object sender, EventArgs args);
    public static event SaveDelegate SaveEvent;

	//Copy or our player, if we ever need it game-wide
	public GameObject Player;

	//Transition target is set by TransitionScript component, when interacted with. 
	//This enables us to spawn the player at custom location when next scene is loaded.
	//To use this, first go to your destination scene, and make an empty GameObject and position it
	//where you would like for player to spawn, and use Copy on transform component. 
	//Next, go to your source scene, make empty GameObject, and paste component values. This will position the 
	//game object at some arbitrary position. Assign this game object as transition target to the TransitionScript.
	//Your player will be moved to that location after next scene is loaded.
	public Transform TransitionTarget;



	//Pseudo-singleton concept from Unity dev tutorial video:
    void Awake()
    {
        Application.targetFrameRate = 144;

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

		if (TransitionTarget == null)
			TransitionTarget = gameObject.transform;

        
    }
    
    public void InitializeSceneList()
    {
        if (SavedLists == null)
        {
            print("Saved lists was null");
            SavedLists = new List<SavedDroppableList>();
        }

        bool found = false;

        //We need to find if we already have a list of saved items for this level:
        for (int i = 0; i < SavedLists.Count; i++)
        {
            if (SavedLists[i].SceneID == SceneManager.GetActiveScene().buildIndex)
            {
                found = true;
                print("Scene was found in saved lists!");
            }
        }

        //If not, we need to create it:
        if (!found)
        {           
            SavedDroppableList newList = new SavedDroppableList(SceneManager.GetActiveScene().buildIndex);
            SavedLists.Add(newList);

            print("Created new list!");
        }
    }

    public SavedDroppableList GetListForScene()
    {
        for (int i = 0; i < SavedLists.Count; i++)
        {
            if (SavedLists[i].SceneID == SceneManager.GetActiveScene().buildIndex)
                return SavedLists[i];
        }

        return null;
    }

    public PlayerStatistics LocalCopyOfData;
    public bool IsSceneBeingLoaded = false;
    public bool IsSceneBeingTransitioned = false;

    public void FireSaveEvent()
    {
        GetListForScene().SavedPotions = new List<SavedDroppablePotion>();
        GetListForScene().SavedSword = new List<SavedDroppableSword>();
        //If we have any functions in the event:
        if (SaveEvent != null)
            SaveEvent(null, null);
    }

    public void SaveData()
    {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        FireSaveEvent();  

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary");
        FileStream SaveObjects = File.Create("saves/saveObjects.binary");

        LocalCopyOfData = PlayerState.Instance.localPlayerData;

        formatter.Serialize(saveFile, LocalCopyOfData);
        formatter.Serialize(SaveObjects, SavedLists);

        saveFile.Close();
        SaveObjects.Close();

        print("Saved!");
    }

    public void LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);
        FileStream saveObjects = File.Open("Saves/saveObjects.binary", FileMode.Open);

        LocalCopyOfData = (PlayerStatistics)formatter.Deserialize(saveFile);
        SavedLists = (List<SavedDroppableList>)formatter.Deserialize(saveObjects);
        
        saveFile.Close();
        saveObjects.Close();

        print("Loaded");
    }
}