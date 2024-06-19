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
    }
    public void OpenMap()
    {
        //if (_canOpenUiMap)
        //{
            _uiMap.SetActive(true);
            QuestPlayerUI.Instance._uiQuest.SetActive(false);
            _mapButton.GetComponent<Image>().color = new Color(132f / 255f, 132f / 255f, 132f / 255f);
            QuestPlayerUI.Instance._questButton.GetComponent<Image>().color = Color.white;
        //}
    }
    public void GetItem(Item usb)
    {
        if (!_itemGet)
        {
            _itemGet = true;
            _playerController.GetInventory().AddItem(usb.ItemName(), usb.ItemSprite(), false);
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
                    _playerController.GetInventory()._slotList[i].GetComponent<InventorySlot>().SetNumber(0);
                }
            }
        }
    }
}
