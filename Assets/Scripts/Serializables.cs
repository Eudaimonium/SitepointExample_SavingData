using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//TUTORIAL
[Serializable]
public class PlayerStatistics
{
    public int SceneID;
    public float PositionX, PositionY, PositionZ;

    public float HP;
    public float Ammo;
    public float XP;
}


[Serializable]
public class SavedDroppablePotion
{
    public float PositionX, PositionY, PositionZ;
}

[Serializable]
public class SavedDroppableSword
{
    public float PositionX, PositionY, PositionZ;
}

[Serializable]
public class SavedDroppableList
{
    public int SceneID;
    public List<SavedDroppablePotion> SavedPotions;
    public List<SavedDroppableSword> SavedSword;

    public SavedDroppableList(int newSceneID)
    {
        this.SceneID = newSceneID;
        this.SavedPotions = new List<SavedDroppablePotion>();
        this.SavedSword = new List<SavedDroppableSword>();
    }
}