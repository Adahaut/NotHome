using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject _uiMap;
    public GameObject _mapButton;
    public Item _itemMap;
    private bool _itemGet;
    public static MapManager Instance;
    [SerializeField] private GameObject _worldMap;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void OpenMap()
    {
        _uiMap.SetActive(true);
        QuestManager.Instance._uiQuest.SetActive(false);
        _mapButton.GetComponent<Image>().color = new Color(132f / 255f, 132f / 255f, 132f / 255f);
        QuestManager.Instance._questButton.GetComponent<Image>().color = Color.white;
    }
    public void GetItem()
    {
        _itemGet = true;
        PC.Instance.GetInventory().AddItem(_itemMap.ItemName(), _itemMap.ItemSprite(), false);
    }
    public void ShowMap()
    {
        if (_itemGet)
        {
            _worldMap.SetActive(true);
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
