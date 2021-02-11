using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Card", menuName = "Card")]
public class KämpferCard : ScriptableObject
{
    public new string name;
    public string Beschreibung;
    public int AttackPoints;
    public int LifePoints;
    public int OperationsKosten;

    public Sprite ArtWork;
}
