using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CraftScriptableObject", order = 1)]
public class CraftScriptableObject : ScriptableObject
{
    public List<string> _materialName = new List<string>();
    public List<int> _materialNumber = new List<int>();
    public List<Sprite> _materialSprite = new List<Sprite>();

    public string _resultName;
    public Sprite _resultSprite;
    public bool _isAnEquipement;
}
