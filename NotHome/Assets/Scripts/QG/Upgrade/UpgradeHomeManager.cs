using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeHomeManager : MonoBehaviour
{
    private int _levelBuilding = 1;
    [SerializeField] private TextMeshProUGUI _textLevel;
    public List<DictionnaryElement<string, List<DictionnaryElement<string, int>>>> _upgarde;
    private InventoryManager _inventoryManager;
    [SerializeField] private string _nameBuilding;
    [SerializeField] private ListSlotField _fieldManager;
    [SerializeField] private GameObject _alarm;
    [SerializeField] private ParticleSystem _particleLevelUp;
    [SerializeField] private GameObject _bridge;
    private bool _getAlarm;
    private GameObject _ship;

    private GameObject[] _playersRef;
    [SerializeField] private GameObject _spaceshipToFix;
    [SerializeField] private GameObject _spaceshipFixed;
    [SerializeField] private GameObject _camera;
    [SerializeField] List<GameObject> _UpdgardesVisuals = new List<GameObject>();

    private void Start()
    {
        //_textLevel.text = "Level " + _levelBuilding.ToString();
        //_inventoryManager = PC.Instance.GetInventory();
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
        _playersRef = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < _playersRef.Length; i++)
        {
            _playersRef[i].SetActive(false);
        }

        _spaceshipToFix.SetActive(false);
        _spaceshipFixed.SetActive(true);
        _camera.SetActive(true);
    }
    public void EffectQG()
    {
        switch (_levelBuilding)
        {
            case 2:
                QuestManager.Instance.SetQuestUpLevel2();
                QuestManager.Instance.QuestComplete(3);
                MapManager.Instance._canOpenUiMap = true;
                QG_Manager.Instance.SetMaxHealthBar(1.20f);
                break;
            case 3:
                QuestManager.Instance.SetQuestUpLevel3();
                DoorExit.Instance.QGLevel3();
                QG_Manager.Instance.SetMaxHealthBar(1.20f);
                break;
            case 4:
                _bridge.SetActive(true);
                break;
        }
    }
    public void EffectTDC()
    {
        if (_levelBuilding == 2)
        {
            QuestManager.Instance.SetQuestUpLevel2();
            _getAlarm = true;
        }
        else if (_levelBuilding >= 3)
        {
            //DroneManager._canUseDrone = true;
            QuestManager.Instance.SetQuestUpLevel3();
        }  
    }
    private IEnumerator StopAlarm(float second)
    {
        _alarm.SetActive(true);
        yield return new WaitForSeconds(second);
        _alarm.SetActive(false);
    }
    public void EffectField()
    {
        switch (_levelBuilding)
        {
            case 2:
                _UpdgardesVisuals[0].SetActive(true);
                QuestManager.Instance.SetQuestUpLevel2();
                break;
            case 3:
                _UpdgardesVisuals[1].SetActive(true);
                QuestManager.Instance.SetQuestUpLevel3();
                break;
            case 4:
                _UpdgardesVisuals[2].SetActive(true);
                break;
            default:
                break;
        }
        for (int i = 0; i < _fieldManager._listSeed.Count; i++)
        {
            //_fieldManager._listSeed[i].GetComponent<UseField>()._seedTime *= 0.80f;
        }
        //ListSlotField.Instance._listPosSlot[ListSlotField.Instance._listPosSlot.Count - _upgarde.Count - 1 + _levelBuilding - 1].gameObject.SetActive(true);
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
                _textLevel.text = "Level " + _levelBuilding.ToString();
                for (int i = 0; i < listIndex.Count; i++)
                {
                    _inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().
                        SetNumber(_inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().Number() - listMat[i]);
                }
                _particleLevelUp.Play();
                Cursor.lockState = CursorLockMode.Locked;
                PC.Instance.gameObject.GetComponentInChildren<PlayerInput>().actions.actionMaps[0].Enable();
                PC.Instance.gameObject.GetComponentInChildren<PlayerInput>().actions.actionMaps[2].Disable();
                button.SetActive(false);
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
