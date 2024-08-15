using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grindstone : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void playparticleSystem(){
        particleSystem.Play();
    }
}
