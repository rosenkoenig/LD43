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

    [SerializeField] Image _titleGaugeToColor;

    ChildStatID _childStatID;

    public ChildStatID GetChildStatID { get { return _childStatID; } }

    public void InitGauge(ChildStatID childStatID)
    {
        _childStatID = childStatID;
        _statNameText.text = childStatID.StatName;
    }

    public void SetGaugeColor(Color col)
    {
        _titleGaugeToColor.color = col;
    }

    public void RefreshGauge(ChildStatsContainer.ChildStatInfo childStatInfo)
    {
        float valueToDisplay = childStatInfo.currentValue;
        if (childStatInfo.childStatID.IsDisplayReverseValue)
        {
            if(childStatInfo.childStatID.IsDoubleGauge && childStatInfo.childStatID.IsTitleGauge == false)
                valueToDisplay = (childStatInfo.childStatID.MaxValue - childStatInfo.childStatID.MinValue) - valueToDisplay;
            else
                valueToDisplay = childStatInfo.childStatID.MaxValue - valueToDisplay;
        }

        _valueText.text = Mathf.Round(valueToDisplay).ToString();
        if (childStatInfo.childStatID.GetAddedJaugeText != "")
            _valueText.text += " " + childStatInfo.childStatID.GetAddedJaugeText;
        if (_isFromCenter)
        {
            if(valueToDisplay < 0)
            {
                _gaugePositive.sizeDelta = new Vector2 (0, _gaugePositive.sizeDelta.y);
                _gaugeNegative.sizeDelta = new Vector2((_gaugeParent.sizeDelta.x / 2) * Mathf.Abs(valueToDisplay) / childStatInfo.childStatID.MaxValue,
                    _gaugeNegative.sizeDelta.y);
            }
            else
            {
                _gaugeNegative.sizeDelta = new Vector2(0, _gaugeNegative.sizeDelta.y);
                _gaugePositive.sizeDelta = new Vector2((_gaugeParent.sizeDelta.x / 2) * valueToDisplay / childStatInfo.childStatID.MaxValue,
                    _gaugePositive.sizeDelta.y);
            }
        }
        else
        {
            _gaugePositive.sizeDelta = new Vector2(_gaugeParent.sizeDelta.x * valueToDisplay / childStatInfo.childStatID.MaxValue,
                    _gaugePositive.sizeDelta.y);
        }
    }

}
