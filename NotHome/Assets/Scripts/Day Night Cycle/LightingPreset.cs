using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning Preset", menuName = "DayNightCycle")]
public class LightingPreset : ScriptableObject
{
    public Gradient _ambiantColor;
    public Gradient _directionalColor;
    public Gradient _fogColor;
}
