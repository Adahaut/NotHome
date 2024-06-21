using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject _uiMap;
    public GameObject _mapButton;
    public Item _itemMap;
    private bool _itemGet;
    public static MapManager Instance;
    [SerializeField] private Image _worldMap;
    [HideInInspector] public bool _canOpenUiMap;
    private PlayerController _playerController;
    [SerializeField] private TextMeshProUGUI _textUSB;
    [SerializeField] private List<Item> _listUSB = new();
    [SerializeField] private List<TextMeshProUGUI> _listText = new();
    private List<bool> _listBoolUSB = new();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        for (int i = 0; i < _listUSB.Count; i++)
        {
            _listBoolUSB.Add(false);
        }
    }
    public void OpenMap()
    {
        if (_canOpenUiMap)
        {
            _uiMap.SetActive(true);
            QuestPlayerUI.Instance._uiQuest.SetActive(false);
            _mapButton.GetComponent<Image>().color = new Color(132f / 255f, 132f / 255f, 132f / 255f);
            QuestPlayerUI.Instance._questButton.GetComponent<Image>().color = Color.white;
        }
    }
    public void GetItem(int index)
    {
        if (!_listBoolUSB[index] && _playerController.GetInventory().HasRemainingPlace())
        {
            _itemGet = true;
            _playerController.GetInventory().AddItem(_listUSB[index].ItemName(), _listUSB[index].ItemSprite(), false);
            _listBoolUSB[index] = true;
            _listText[index].text = "Remove USB";
        }
        else
        {
            for (int i = 0; i < _playerController.GetInventory()._slotList.Count; i++)
            {
                if (_playerController.GetInventory()._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _listUSB[index].ItemName())
                {
                    _playerController.GetInventory()._slotList[i].GetComponent<InventorySlot>().SetNumberAndName(0, _playerController.GetInventory()._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName());
                    _listBoolUSB[index] = false;
                    _listText[index].text = "Get USB";
                }
            }
        }
    }
    public void ShowMap()
    {
        if (_itemGet)
        {
            _worldMap.color = Color.red;
            for (int i = 0; i < _playerController.GetInventory()._slotList.Count; i++)
            {
                if (_playerController.GetInventory()._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == "DesertKey")
                {
                    _playerController.GetInventory()._slotList[i].GetComponent<InventorySlot>().SetNumberAndName(0, 
                        _playerController.GetInventory()._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName());
                }
            }
        }
    }
}
