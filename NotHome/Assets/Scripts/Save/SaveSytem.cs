using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSytem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private QuestManager _questManager;
    [SerializeField] private NewFieldManager _newFieldManager;

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
            //amelioration + liste inventaire + liste champs + quetes
            // variables a save
            playerPositions = playerTransform.position,
            _actualQuest = _questManager._actualQuest,
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
        //variables a load
        playerTransform.position = savedData.playerPositions;
        _questManager._actualQuest = savedData._actualQuest;
    }

}

[System.Serializable]
public class PlantData
{
    public int index;
    public bool isPlanted;
    public int seedId;
    public float remainingGrowTime;
}

public class SavedData
{
    public Vector3 playerPositions;
    public QuestScriptableObject _actualQuest;
    public List<PlantData> plants;
}

