using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    // public TextMeshProUGUI timeDisplay; // Display Time
    // public TextMeshProUGUI dayDisplay; // Display Day
    [SerializeField] public Light2D light2D; // this is the post processing volume
 
    public float tick; // Increasing the tick, increases second rate
    public float seconds; 
    public int mins;
    public int hours;
    public int days = 1;
 
    public bool activateLights; // checks if lights are on
    public GameObject exteriorLights; // all the lights we want on when its dark
    public GameObject interiorLights;
    public SpriteRenderer[] stars; // star sprites 
    // Start is called before the first frame update
    void Awake(){
        seconds = 0;
        mins = 0;
        hours = UnityEngine.Random.Range(1, 24);
        if(hours >= 19 || hours <= 6) {
                light2D.intensity = 0.05f;
                activateLights = true;
        }else if(hours >= 7){ 
                light2D.intensity = 1f;
                activateLights = false;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (LightsManager.GetInstance() != null)
        {
            exteriorLights = LightsManager.GetInstance().exteriorLights;
            interiorLights = LightsManager.GetInstance().interiorLights;

            // Manage exterior and interior lights based on time
            if (activateLights)
            {
                if (exteriorLights != null) exteriorLights.SetActive(true);
                if (interiorLights != null) interiorLights.SetActive(true);
            }
            else
            {
                if (exteriorLights != null) exteriorLights.SetActive(false);
                if (interiorLights != null) interiorLights.SetActive(false);
            }
        }
    }

    // FixedUpdate is called at a consistent rate, independent of frame rate
    void FixedUpdate()
    {
        CalcTime(); // Calculate time progression
        DisplayTime(); // Optional: display the time
    }
 
    public void CalcTime() // Used to calculate sec, min and hours
    {
        seconds += Time.fixedDeltaTime * tick * Time.timeScale;
         
        if (seconds >= 60) // 60 sec = 1 min
        {
            seconds = 0;
            mins += 1;
        }
 
        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
        }
 
        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
            days += 1;
        }
        ControlPPV(); // changes post processing volume after calculation
    }
 
    public void ControlPPV() // used to adjust the post processing slider.
    {
        if(hours >= 18 && hours < 19) // dusk at 6:00 PM - 7:00 PM
        {
            light2D.intensity = Mathf.Clamp(1 - (float)mins / 60, 0.05f, 1f); // Adjust intensity, capped at 0.2
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, (float)mins / 60); // change the alpha value of the stars
            }

            if (!activateLights && mins > 45) // if lights haven't been turned on and it's pretty dark
            {
                activateLights = true;
                StartCoroutine(AudioManager.GetInstance().FadeOutAmbience(AudioManager.GetInstance().dayAmbience, 0.5f));
                AudioManager.GetInstance().SetAmbienceDay(false, 1f);
                
            }
        }

        if(hours >= 6 && hours < 7) // Dawn from 6:00 AM - 7:00 AM
        {
            light2D.intensity = Mathf.Clamp((float)mins / 60, 0.05f, 1f); // Adjust intensity, capped at 1
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 1 - (float)mins / 60); // make stars invisible
            }

            if (activateLights && mins > 10) // if lights are on and it's pretty bright
            {
                activateLights = false;
                StartCoroutine(AudioManager.GetInstance().FadeOutAmbience(AudioManager.GetInstance().dayAmbience, 0.5f));
                AudioManager.GetInstance().SetAmbienceDay(true, 1f);
                
            }
        }
    }
 
    public void DisplayTime() // Shows time and day in ui
    {
        //timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins); // The formatting ensures that there will always be 0's in empty spaces
        // dayDisplay.text = "Day: " + days; // display day counter
    }


}
