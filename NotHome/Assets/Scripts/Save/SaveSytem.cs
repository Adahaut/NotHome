using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSytem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    private GameObject[] _buildings;

    private void Start()
    {
        if(Menu._loadSavedData) 
        {
            LoadData();
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5))
        {
            SaveData();
        }

        if(Input.GetKeyUp(KeyCode.F9))
        {
            LoadData();
        }
    }

    void SaveData()
    {
        SavedData savedData = new SavedData
        {
            playerPositions = playerTransform.position,
            // variables a save
            //amelioration + liste inventaire (liste de struct) + liste champs + quetes
        };

        string jsonData = JsonUtility.ToJson(savedData);
        string filePath = Application.persistentDataPath + "/SavedData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Sauvegarde effectuée");

    }

    void LoadData()
    {
        string filePath = Application.persistentDataPath + "/SavedData.json";
        string jsonData = System.IO.File.ReadAllText(filePath);

        SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

        //chargement des données
        playerTransform.position = savedData.playerPositions;

        //variables a load
        //script.variable (public) = savedData.variable
    }

}

public class SavedData
{
    public Vector3 playerPositions;
}


