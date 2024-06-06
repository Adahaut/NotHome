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

    float yScale;
    float initYPos;
    float posY;

    public void StartGrow()
    {
        if(!_growStarted)
        {
            _growStarted = true;
            _currentTimer = 0f;
            _timeToGrow = seedStruct._growingTime;
            yScale = 0.25f;
            initYPos = transform.localPosition.y;
        }
    }

    private void Update()
    {
        if (_growStarted)
        {
            if (_currentTimer < _timeToGrow)
            {
                _currentTimer += Time.deltaTime;
                //Modify scale
                yScale = Mathf.Clamp(_currentTimer / _timeToGrow, 0.25f, 0.75f);
                Vector3 newScale = new Vector3(.2f, yScale, .2f);
                transform.localScale = newScale;
                //Modify position
                //posY = Mathf.Clamp(_currentTimer / _timeToGrow, initYPos, initYPos + 0.25f);
                //print(posY);
                //Vector3 newPos = Vector3.zero;
                //newPos.y = yScale;
                //transform.localPosition = newPos;
            }
            else
            {
                _growStarted = false;
            }
        }
    }

}
