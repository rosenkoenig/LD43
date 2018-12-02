using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildStatGauge : MonoBehaviour {

    [SerializeField] bool _isFromCenter;

    [SerializeField] Text _statNameText;
    [SerializeField] Text _valueText;

    [SerializeField] RectTransform _gaugePositive;
    [SerializeField] RectTransform _gaugeNegative;
    [SerializeField] RectTransform _gaugeParent;

    ChildStatID _childStatID;

    public ChildStatID GetChildStatID { get { return _childStatID; } }

    public void InitGauge(ChildStatID childStatID)
    {
        _childStatID = childStatID;
        _statNameText.text = childStatID.StatName;
    }

    public void RefreshGauge(ChildStatsContainer.ChildStatInfo childStat)
    {
        _valueText.text = Mathf.Round(childStat.currentValue).ToString();
        if(_isFromCenter)
        {
            if(childStat.currentValue < 0)
            {
                _gaugePositive.sizeDelta = new Vector2 (0, _gaugePositive.sizeDelta.y);
                _gaugeNegative.sizeDelta = new Vector2(_gaugeParent.sizeDelta.x / 2, _gaugeNegative.sizeDelta.y);
            }
            else
            {
                _gaugeNegative.sizeDelta = new Vector2(0, _gaugeNegative.sizeDelta.y);
                _gaugePositive.sizeDelta = new Vector2(_gaugeParent.sizeDelta.x / 2, _gaugePositive.sizeDelta.y);
            }
        }
        else
        {

        }
    }

}
