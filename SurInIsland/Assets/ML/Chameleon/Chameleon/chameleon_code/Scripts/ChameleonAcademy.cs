using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ChameleonAcademy : Academy
{
    [Tooltip("The frequency to auto-change colors (0 means no change)")]
    public float colorChangeFrequency = 0f;

    [Tooltip("The amount the color changes each step")]
    public float colorChangeMagnitude = 5f / 255f;
}
