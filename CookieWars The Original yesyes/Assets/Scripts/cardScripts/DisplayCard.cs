using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCard : MonoBehaviour
{
    public KämpferCard Karte;   
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Beschreibung;
    public TextMeshProUGUI Kosten;
    public TextMeshProUGUI Angriff;
    public TextMeshProUGUI Leben;
    public Image artWork;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCardStats();
    }
    public void UpdateCardStats()
    {
        Name.text = Karte.name;
        Beschreibung.text = Karte.Beschreibung;
        Kosten.text = Karte.OperationsKosten.ToString();
        Angriff.text = Karte.AttackPoints.ToString();
        Leben.text = Karte.LifePoints.ToString();
        artWork.sprite = Karte.ArtWork;

    }

}
