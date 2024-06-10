using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFieldSlot : MonoBehaviour
{

    [Header("Slots References")]
    public Image fillBar;
    public TMP_Text seedNameTextUI;
    public Image seedImage;
    public Image fruitImage;

    [HideInInspector] public bool _containSeed = false;
    private bool _growStarted = false;

    

    public void StartGrowing(float seedTime, string seedName, Sprite fruitImage, Sprite seedImage)
    {
        if(!_growStarted)
        {
            //_timeToGrow = seedTime;
            this.seedImage.sprite = seedImage;
            this.fruitImage.sprite = fruitImage;
            seedNameTextUI.text = seedName;
            _growStarted = true;
            _containSeed = true;
        }
    }
}
