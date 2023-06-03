using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{

    private TextMeshProUGUI atkAmount;
    private TextMeshProUGUI rangeAmount;

    void Start()
    {
        atkAmount = transform.Find("ATK").GetComponentsInChildren<TextMeshProUGUI>()[1];
        rangeAmount = transform.Find("RANGE").GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    public void SetAttack(int atk)
    {
        atkAmount.SetText(atk.ToString());
    }

    public void SetRange(int range)
    {
        rangeAmount.SetText(range.ToString());
    }
}
