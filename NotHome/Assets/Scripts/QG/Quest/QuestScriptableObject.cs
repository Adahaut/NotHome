using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestScriptableObject", order = 1)]
public class QuestScriptableObject : ScriptableObject
{
    public string _title;
    public string _lore;
    public string _objectif;
    public QuestScriptableObject _nextQuest;
    public bool _isComplet;
}
