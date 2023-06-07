using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI atkAmount;
    [SerializeField] private TextMeshProUGUI rangeAmount;
    [SerializeField] private TextMeshProUGUI atkRangeAmount;

    public void SetAttack(int atk)
    {
        atkAmount.SetText(atk.ToString());
    }

    public void SetRange(int range)
    {
        rangeAmount.SetText(range.ToString());
    }

    public void SetAtkRange(int range)
    {
        atkRangeAmount.SetText(range.ToString());
    }
}
