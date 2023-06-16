using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _atkAmount;
    [SerializeField] private TextMeshProUGUI _rangeAmount;
    [SerializeField] private TextMeshProUGUI _atkRangeAmount;

    public void SetAttack(int atk)
    {
        _atkAmount.SetText(atk.ToString());
    }

    public void SetRange(int range)
    {
        _rangeAmount.SetText(range.ToString());
    }

    public void SetAtkRange(int range)
    {
        _atkRangeAmount.SetText(range.ToString());
    }
}
