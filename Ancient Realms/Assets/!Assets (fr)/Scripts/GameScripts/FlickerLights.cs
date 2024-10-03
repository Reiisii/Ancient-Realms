using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLights : MonoBehaviour
{
    [SerializeField] private float flickerSpeed = 0.1f; // Speed of the flicker effect
    [SerializeField] private float minInnerRadius = 0.98f; // Minimum inner radius
    [SerializeField] private float maxInnerRadius = 1.2f; // Maximum inner radius
    [SerializeField] private float minOuterRadius = 2.4f; // Minimum outer radius
    [SerializeField] private float maxOuterRadius = 2.6f; // Maximum outer radius

    private Light2D lightObject;
    private float baseInnerRadius; // Store the initial inner radius
    private float baseOuterRadius; // Store the initial outer radius
    private void Awake(){
        lightObject = gameObject.GetComponent<Light2D>();
    }
    void Start()
    {
        // Initialize with base values
        if (gameObject.GetComponent<Light2D>() != null)
        {
            baseInnerRadius = lightObject.pointLightInnerRadius;
            baseOuterRadius = lightObject.pointLightOuterRadius;
        }
    }

    void Update()
    {
        if (lightObject != null)
        {
            // Flicker the inner and outer radius randomly within a range over time
            lightObject.pointLightInnerRadius = Mathf.Lerp(minInnerRadius, maxInnerRadius, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
            lightObject.pointLightOuterRadius = Mathf.Lerp(minOuterRadius, maxOuterRadius, Mathf.PerlinNoise(Time.time * flickerSpeed + 100f, 0f));
        }
    }
}
