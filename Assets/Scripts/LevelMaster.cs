using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMaster : MonoBehaviour
{

    public GameObject PotionPrefab;
    public GameObject SwordPrefab;
    
	void Start ()
    {
        GlobalControl.Instance.InitializeSceneList();

	    if (GlobalControl.Instance.IsSceneBeingLoaded || GlobalControl.Instance.IsSceneBeingTransitioned)
        {
            SavedDroppableList localList = GlobalControl.Instance.GetListForScene();

            if (localList != null)
            {
                print("Saved potions count: " + localList.SavedPotions.Count);

                for (int i = 0; i < localList.SavedPotions.Count; i++)
                {
                    GameObject spawnedPotion = (GameObject)Instantiate(PotionPrefab);
                    spawnedPotion.transform.position = new Vector3(localList.SavedPotions[i].PositionX,
                                                                    localList.SavedPotions[i].PositionY,
                                                                    localList.SavedPotions[i].PositionZ);
                }

                for (int i = 0; i < localList.SavedSword.Count; i++)
                {
                    GameObject spawnedSword = (GameObject)Instantiate(SwordPrefab);
                    spawnedSword.transform.position = new Vector3(localList.SavedSword[i].PositionX,
                                                                    localList.SavedSword[i].PositionY,
                                                                    localList.SavedSword[i].PositionZ);
                }

            }
            else
                print("Local List was null!");
        }
	}
	
}
