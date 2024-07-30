using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Character", menuName = "SO/Character")]
public class CharacterSO : ScriptableObject
{
    public int id;
    public string firstName;
    public string lastName;
    public string biography;
    public CultureEnum culture;
    public Sprite image;
}
