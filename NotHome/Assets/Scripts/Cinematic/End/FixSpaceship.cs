using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSpaceship : MonoBehaviour
{
    [SerializeField] private GameObject _spaceshipToFix;
    [SerializeField] private GameObject _spaceshipFixed;
    [SerializeField] private GameObject _creditsCanva;
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
        _spaceshipToFix.SetActive(false);
        _spaceshipFixed.SetActive(true);
        _creditsCanva.SetActive(true);

        for (int i = 0; i < _playersRef.Length; i++)
        {
            _playersRef[i].SetActive(false);
        }
    }
}
