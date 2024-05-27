using UnityEngine;

[CreateAssetMenu(fileName = "gun", menuName = "Weapons/gun")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string _name;

    [Header("Shooting")]
    public float _damages;
    public float _maxDistance;
    public bool _isAutomatic;

    [Header("Reloading")]
    public int _magSize;
    public float _fireRate; // shoot cadance = (1 / (1 / (_fireRate * 60)))
    public float _reloadSpeed;
}
