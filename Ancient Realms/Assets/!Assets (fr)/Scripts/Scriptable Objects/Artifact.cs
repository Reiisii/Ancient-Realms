using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Artifact", menuName = "SO/Artifact")]
public class ArtifactsSO : ScriptableObject
{
    public int id;
    public string artifactName;
    public string description;
    public CultureEnum culture;
    public Sprite image;
}
