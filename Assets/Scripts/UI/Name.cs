using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Name : MonoBehaviour
{
    private TextMeshProUGUI nameText;

    void Start()
    {
        nameText = transform.GetComponent<TextMeshProUGUI>();
    }

    public void SetName(string name)
    {
        nameText.SetText(name);
    }
}
