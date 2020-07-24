using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ChameleonArea : Area
{
    [Tooltip("The platform the chameleon is standing on")]
    public GameObject platform;

    private Material platformMaterial;
    private ChameleonAcademy chameleonAcademy;
    private Color currentColor;
    private float lastChange = 0f;

    public override void ResetArea()
    {
        ChangePlatformColor();
    }

    /// <summary>
    /// Gets the platform color (the chameleon is trying to match it)
    /// </summary>
    public Color PlatformColor
    {
        get
        {
            return currentColor;
        }
    }

    /// <summary>
    /// Gets the chameleon academy
    /// </summary>
    public ChameleonAcademy ChameleonAcademy
    {
        get
        {
            return chameleonAcademy;
        }
    }

    private void Start()
    {
        // Initial setup
        platformMaterial = platform.GetComponent<MeshRenderer>().material;
        ChangePlatformColor();
        chameleonAcademy = FindObjectOfType<ChameleonAcademy>();
    }

    private void FixedUpdate()
    {
        // If the colorChangeFrequency is set, determine if it should be changed
        if (chameleonAcademy.colorChangeFrequency > 0 &&
            (Time.fixedTime - lastChange) > chameleonAcademy.colorChangeFrequency)
        {
            ChangePlatformColor();
        }
    }

    /// <summary>
    /// Changes the platform the chameleon is standing on to a random color
    /// </summary>
    private void ChangePlatformColor()
    {
        lastChange = Time.fixedTime;
        currentColor = UnityEngine.Random.ColorHSV();
        platformMaterial.color = new Color(currentColor.r, currentColor.g, currentColor.b);
    }
}
