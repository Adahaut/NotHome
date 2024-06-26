using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    private FixSpaceship _fixSpaceship;
    public static UpgradeBuilding Instance;
    public List<LevelBuilding> _levelBuildingList = new();
    [HideInInspector] public bool _canOpenAllSAS;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void SetEffectBuilding(string nameBuilding)
    {
        switch (nameBuilding)
        {
            case "TDC":
                _levelBuildingList[1]._particleSystem.Play();
                EffectTDC();
                break;
            case "QG":
                _levelBuildingList[0]._particleSystem.Play();
                EffectQG();
                break;
            case "Ship":
                EffectShip();
                break;
            default:
                Debug.Log("No name building");
                break;
        }
    }
    public void EffectShip()
    {
        _fixSpaceship = GameObject.Find("SpaceshipPart").GetComponent<FixSpaceship>();
        _fixSpaceship.FixSpaceShip();
    }
    public void EffectQG()
    {
        switch (_levelBuildingList[0]._levelBuilding)
        {
            case 2:
                QuestManager.Instance.SetQuestUpLevel2();
                QuestManager.Instance.QuestComplete(3);
                break;
            case 3:
                QuestManager.Instance.SetQuestUpLevel3();
                _canOpenAllSAS = true;
                break;
            case 4:
                //_bridge.SetActive(true);
                break;
        }
    }
    public void EffectTDC()
    {
        if (_levelBuildingList[1]._levelBuilding == 2)
        {
            QuestManager.Instance.SetQuestUpLevel2();
        }
        else if (_levelBuildingList[1]._levelBuilding >= 3)
        {
            DroneManager._canUseDrone = true;
            QuestManager.Instance.SetQuestUpLevel3();
        }
    }
}
