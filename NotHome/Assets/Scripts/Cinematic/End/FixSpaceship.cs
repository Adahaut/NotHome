using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSpaceship : MonoBehaviour
{
    [SerializeField] private GameObject _spaceshipToFix;
    [SerializeField] private GameObject _spaceshipFixed;
    [SerializeField] private GameObject _camera;
    private GameObject[] _playersRef;

    private void Start()
    {
        _playersRef = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            FixSpaceShip();
        }
    }

    public void FixSpaceShip()
    {
        for (int i = 0; i < _playersRef.Length; i++)
        {
            _playersRef[i].SetActive(false);
        }

        _spaceshipToFix.SetActive(false);
        _spaceshipFixed.SetActive(true);
        _camera.SetActive(true);      
    }
}
