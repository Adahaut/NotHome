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
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void OpenMap()
    {
        if (_canOpenUiMap)
        {
            _uiMap.SetActive(true);
            QuestManager.Instance._uiQuest.SetActive(false);
            _mapButton.GetComponent<Image>().color = new Color(132f / 255f, 132f / 255f, 132f / 255f);
            QuestManager.Instance._questButton.GetComponent<Image>().color = Color.white;
        }
    }
    public void GetItem(Button button)
    {
        _itemGet = true;
        PC.Instance.GetInventory().AddItem(_itemMap.ItemName(), _itemMap.ItemSprite(), false);
        button.gameObject.SetActive(false);
    }
    public void ShowMap()
    {
        if (_itemGet)
        {
            _worldMap.color = Color.red;
            for (int i = 0; i < PC.Instance.GetInventory()._slotList.Count; i++)
            {
                if (PC.Instance.GetInventory()._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == "ItemMap")
                {
                    PC.Instance.GetInventory()._slotList[i].GetComponent<InventorySlot>().SetNumber(0);
                }
            }
        }
    }
}
