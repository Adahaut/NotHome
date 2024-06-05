using Mirror;
using UnityEngine;

[System.Serializable]
public class SeedObject : NetworkBehaviour
{
    public Seed seedStruct = new Seed();

    public Sprite seedImage;
    public Sprite fruitImage;

    
    private bool _growStarted = false;
    public float _timeToGrow;

    public float _currentTimer;

    public void StartGrow()
    {
        if(!_growStarted)
        {
            _growStarted = true;
            _currentTimer = 0f;
            _timeToGrow = seedStruct._growingTime;
        }
    }

    private void Update()
    {
        if (_growStarted)
        {
            if (_currentTimer < _timeToGrow)
            {
                _currentTimer += Time.deltaTime;
                //fillBar.fillAmount = _currentTimer / _timeToGrow;
                //Mettre a jour la scale en 3d
            }
            else
            {
                _growStarted = false;
            }
        }
    }

}
