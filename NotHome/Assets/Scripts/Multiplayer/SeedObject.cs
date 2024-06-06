﻿using Mirror;
using UnityEngine;

[System.Serializable]
public class SeedObject : NetworkBehaviour
{
    public Seed seedStruct = new Seed();

    public Sprite seedImage;
    public Sprite fruitImage;

    [SyncVar] private bool _growStarted = false;
    [SyncVar] public float _timeToGrow;
    [SyncVar] public float _currentTimer;

    [SyncVar(hook = nameof(OnPositionChanged))]
    private Vector3 _syncedPosition;

    private Vector3 initYPos;

    public void StartGrow(Vector3 pos)
    {
        if(!_growStarted)
        {
            _growStarted = true;
            _currentTimer = 0f;
            _timeToGrow = seedStruct._growingTime;

            initYPos = pos;
            if (isServer)
            {
                _syncedPosition = pos;
                RpcSetPosition(pos); 
            }
        }
    }

    private void Update()
    {
        if (_growStarted)
        {
            if (_currentTimer < _timeToGrow)
            {
                _currentTimer += Time.deltaTime;

                //Update Scale
                float t = _currentTimer / _timeToGrow; 
                float yScale = Mathf.Lerp(0.25f, 0.75f, t);

                Vector3 newScale = new Vector3(.2f, yScale, .2f);
                transform.localScale = newScale;

                //Update Position
                float newYPos = Mathf.Lerp(initYPos.y, initYPos.y + 0.25f, t);

                Vector3 newPos = transform.position;
                newPos.y = newYPos;

                if (isServer)
                {
                    _syncedPosition = newPos;
                }

                transform.position = newPos;
            }
            else
            {
                _growStarted = false;
            }
        }
    }

    private void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    [ClientRpc]
    private void RpcSetPosition(Vector3 pos)
    {
        if (!isServer)
        {
            print("test");
            transform.position = pos;
        }
    }

}
