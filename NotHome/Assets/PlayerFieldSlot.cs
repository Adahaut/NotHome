using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFieldSlot : MonoBehaviour
{

    [Header("Slots References")]
    [SerializeField] private Image fillBar;
    [SerializeField] private TMP_Text seedNameTextUI;
    [SerializeField] private Image seedImage;
    [SerializeField] private Image fruitImage;

    [HideInInspector] public bool _containSeed = false;
    private bool _growStarted = false;

    private float _timeToGrow;
    private string _seedName;

    private float _currentTimer;

    public void StartGrowing(float seedTime, string seedName, Sprite fruitImage, Sprite seedImage)
    {
        if(!_growStarted)
        {
            _timeToGrow = seedTime;
            _seedName = seedName;
            this.seedImage.sprite = seedImage;
            this.fruitImage.sprite = fruitImage;
            _growStarted = true;
            _containSeed = true;
        }
    }

    private void Update()
    {
        if(_growStarted)
        {
            if(_currentTimer < _timeToGrow)
            {
                _currentTimer += Time.deltaTime;
                fillBar.fillAmount = _currentTimer / _timeToGrow;
                //Mettre a jour la scale en 3d
            }
            else
            {
                _growStarted = false;
            }
        }
    }
}
