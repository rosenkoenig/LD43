using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionSkillItem : MonoBehaviour {

    [SerializeField]
    Text childName;

    [SerializeField]
    Text statName;

    [SerializeField]
    Text statDelta;


    public void Init (string child, string stat, float statValue, float delta)
    {
        childName.text = child;
        statName.text = stat;

        statDelta.text = (statValue).ToString("F0") + " - " + (statValue + delta).ToString("F0") + " (+" + delta.ToString("F0") + ")";
    }
}
