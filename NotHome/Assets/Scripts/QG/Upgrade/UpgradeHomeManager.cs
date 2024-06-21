using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeHomeManager : MonoBehaviour
{
    private int _levelBuilding = 1;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private TextMeshProUGUI _ressourcesNeeded;
    public List<DictionnaryElement<string, List<DictionnaryElement<string, int>>>> _upgarde;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private string _nameBuilding;
    [HideInInspector] public ParticleSystem _particleLevelUp;
    private FixSpaceship _fixSpaceship;

    [SerializeField] List<GameObject> _UpgradesVisuals = new List<GameObject>();

    private void Start()
    {
        _textLevel.text = "Level " + _levelBuilding.ToString();
    }

    private void OnEnable()
    {
        UpdateRessourcesText();
    }

    public int GetLevel() { return _levelBuilding; }

    private void UpdateRessourcesText()
    {
        _ressourcesNeeded.text = "";
        if (_levelBuilding - 1 < _upgarde.Count)
        {
            for (int i = 0; i < _upgarde[_levelBuilding - 1].Value.Count; i++)
            {
                _ressourcesNeeded.text += _upgarde[_levelBuilding - 1].Value[i].Value + " X " + _upgarde[_levelBuilding - 1].Value[i].Key + "\n\n";
            }
        }
    }

    public void SetEffectBuilding()
    {
        switch (_nameBuilding)
        {
            case "Field":
                EffectField();
                break;
            case "TDC":
                EffectTDC();
                break;
            case "QG":
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
        switch (_levelBuilding)
        {
            case 2:
                QuestManager.Instance.SetQuestUpLevel2();
                QuestManager.Instance.QuestComplete(3);
                _mapManager._canOpenUiMap = true;
                QG_Manager.Instance.SetMaxHealthBar(1.20f);
                break;
            case 3:
                QuestManager.Instance.SetQuestUpLevel3();
                DoorExit.Instance.QGLevel3();
                QG_Manager.Instance.SetMaxHealthBar(1.20f);
                break;
            case 4:
                //_bridge.SetActive(true);
                break;
        }
    }
    public void EffectTDC()
    {
        if (_levelBuilding == 2)
        {
            QuestManager.Instance.SetQuestUpLevel2();
        }
        else if (_levelBuilding >= 3)
        {
            DroneManager._canUseDrone = true;
            QuestManager.Instance.SetQuestUpLevel3();
        }  
    }
    public void EffectField()
    {
        switch (_levelBuilding)
        {
            case 2:
                _UpgradesVisuals[0].SetActive(true);
                QuestManager.Instance.SetQuestUpLevel2();
                break;
            case 3:
                _UpgradesVisuals[1].SetActive(true);
                QuestManager.Instance.SetQuestUpLevel3();
                break;
            case 4:
                _UpgradesVisuals[2].SetActive(true);
                break;
            default:
                break;
        }
    }
    public void UpdateBuilding(GameObject button)
    {
        if (_upgarde.Count >= _levelBuilding)
        {
            int number = 0;
            List<int> listIndex = new();
            List<int> listMat = new();
            for (int i = 0; i < _inventoryManager._slotList.Count; i++)
            {
                if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained() != null)
                {
                    for (int j = 0; j < _upgarde[_levelBuilding - 1].Value.Count; j++)
                    {
                        if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _upgarde[_levelBuilding - 1].Value[j].Key)
                        {
                            number++;
                            if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().Number() < _upgarde[_levelBuilding - 1].Value[j].Value)
                            {
                                Debug.Log("Pas assez de materiaux");
                                return;
                            }
                            else
                            {
                                listIndex.Add(i);
                                listMat.Add(_upgarde[_levelBuilding - 1].Value[j].Value);
                            }
                        }
                    }
                }
            }
            if (number == _upgarde[_levelBuilding - 1].Value.Count)
            {
                _levelBuilding++;
                SetEffectBuilding();
                UpdateRessourcesText();
                _textLevel.text = "Level " + _levelBuilding.ToString();
                for (int i = 0; i < listIndex.Count; i++)
                {
                    _inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().
                        SetNumberAndName(_inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().Number() - listMat[i], 
                        _inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>()._name);
                }
                _particleLevelUp.Play();
            }
            else
            {
                Debug.Log("Pas assez de materiaux");
            }
        }
        else
        {
            Debug.Log("Level max");
        }
    }
}
