using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseCrafter : MonoBehaviour
{
    [SerializeField] private List<Button> _listButton = new();
    [SerializeField] private List<CraftScriptableObject> _listCraft = new();
    [SerializeField] private Image _spriteCraft;
    [SerializeField] private TextMeshProUGUI _textCraft;

    [SerializeField] private InventoryBaseManager _baseInventory;
    private int _indexMove;

    public void OnClick(Button button)
    {
        int index;
        if (button.name.Length > _listButton[0].name.Length)
            index = int.Parse(button.name[6].ToString() + button.name[7].ToString()) - 1;
        else
            index = int.Parse(button.name[6].ToString()) - 1;

        _spriteCraft.gameObject.SetActive(true);
        _textCraft.text = "";
        _spriteCraft.color = _listButton[index].GetComponent<Image>().color;

        SetMaterialsCraft(index);
    }
    public void MoveButton(int direction)
    {
        for (int i = 0;  i < _listButton.Count; i++)
        {
            _listButton[i].transform.position += new Vector3(100 * Screen.width / 1920 * direction, 0, 0);
        }
        if (direction < 0)
        {
            _listButton[_indexMove].transform.position += new Vector3(100 * Screen.width / 1920 * _listButton.Count, 0, 0);
            _indexMove += 1;
            if (_indexMove == _listButton.Count)
                _indexMove = 0;
        }
        else
        {
            if (_indexMove != 0)
            {
                _listButton[_indexMove - 1].transform.position -= new Vector3(100 * Screen.width / 1920 * _listButton.Count, 0, 0);
                _indexMove -= 1;
                if (_indexMove < 0)
                    _indexMove = _listButton.Count - 1;
            }
            else
            {
                _listButton[_listButton.Count - 1].transform.position -= new Vector3(100 * Screen.width / 1920 * _listButton.Count, 0, 0);
                _indexMove = _listButton.Count - 1;
            }
        }
    }

    public void CraftObject()
    {
        print("craft");
        
    }
    private void SetMaterialsCraft(int index)
    {
        if (_listCraft[index]._metal > 0)
            _textCraft.text += _listCraft[index]._metal + " Metal\n\n";
        if (_listCraft[index]._reactor > 0)
            _textCraft.text += _listCraft[index]._reactor + " Reactor\n\n";
        if (_listCraft[index]._suffer > 0)
            _textCraft.text += _listCraft[index]._suffer + " Suffer\n\n";
        if (_listCraft[index]._leaf > 0)
            _textCraft.text += _listCraft[index]._leaf + " Leaf\n\n";
        if (_listCraft[index]._toolKit > 0)
            _textCraft.text += _listCraft[index]._toolKit + " ToolKit\n\n";
        if (_listCraft[index]._seed > 0)
            _textCraft.text += _listCraft[index]._seed + " Seed\n\n";
        if (_listCraft[index]._tissue > 0)
            _textCraft.text += _listCraft[index]._tissue + " Tissue\n\n";
        if (_listCraft[index]._homium > 0)
            _textCraft.text += _listCraft[index]._homium + " Homium\n\n";
    }
}
