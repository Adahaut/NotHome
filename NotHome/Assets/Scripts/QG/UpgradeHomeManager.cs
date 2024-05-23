using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeHomeManager : MonoBehaviour
{
    private int _levelBuilding = 1;
    [SerializeField] private TextMeshProUGUI _textLevel;
    public List<DictionnaryElement<string, List<DictionnaryElement<string, int>>>> _upgarde;

    private void Start()
    {
        _textLevel.text = "Level " + _levelBuilding.ToString();
    }
    public void UpdateBuilding()
    {
        if (_upgarde.Count >= _levelBuilding)
        {
            for (int i = 0; i < _upgarde[_levelBuilding - 1].Value.Count; i++)
            {
                if (99 < _upgarde[_levelBuilding - 1].Value[i].Value) // 99 = au nombre de materiaux que le joueur possede
                {
                    Debug.Log("Pas assez de materiaux");
                    return;
                }
            }
            _levelBuilding++;
            _textLevel.text = "Level " + _levelBuilding.ToString();
            //Enlever les materiaux au joueur
        }
        else
            Debug.Log("Level max");
    }
}
