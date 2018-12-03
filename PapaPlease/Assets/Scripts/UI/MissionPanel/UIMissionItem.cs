using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionItem : MonoBehaviour {

    [SerializeField]
    Text missionName;

    [SerializeField]
    Text missionMoneyEarning;

    public void Init (string name, string earning)
    {
        missionName.text = name;
        missionMoneyEarning.text = earning + "$";
    }
}
