using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject _uiMap;
    public GameObject _mapButton;
    public static MapManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void OpenMap()
    {
        _uiMap.SetActive(true);
        QuestManager.Instance._uiQuest.SetActive(false);
        _mapButton.GetComponent<Image>().color = new Color(132f / 255f, 132f / 255f, 132f / 255f);
        QuestManager.Instance._questButton.GetComponent<Image>().color = Color.white;
    }
}
