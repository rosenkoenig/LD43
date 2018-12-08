using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InActivityFeedback : MonoBehaviour {

    [SerializeField] Text _activityInfoText;
    public void SetText(string txt)
    {
        _activityInfoText.text = txt;
    }
}
