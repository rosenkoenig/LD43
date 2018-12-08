using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChildStatID", menuName = "Gameplay/ChildStatID")]
public class ChildStatID : ScriptableObject {

    [SerializeField] string _statName;

    [SerializeField] float _maxValue;

    [UnityEngine.Serialization.FormerlySerializedAs("_isDisplayedAsGauge")]
    [SerializeField] bool _isBetweenNegativeAndPositive;

    [SerializeField] float _startValue;

    [SerializeField] float _playerValue;

    [Header("Display gauge")]
    [SerializeField] string _addedJaugeText;

    [SerializeField] bool _isTitleGauge;
    [SerializeField] Color _titleGaugeColor;
    
    [SerializeField] bool _displayReverseValue;

    [Header("Low Stat warning log")]
    [SerializeField] bool _isLowStatWarningLogActivated;
    [SerializeField] string _lowStatWarningLogText;

    public string StatName { get { return _statName; } }

    public float MinValue { get { return _isBetweenNegativeAndPositive ? -_maxValue : 0; } }
    public float MaxValue { get { return _maxValue; } }

    public float GetStartValue { get { return _startValue; } }

    public float GetPlayerValue { get { return _playerValue; } }

    public string GetAddedJaugeText { get { return _addedJaugeText; } }

    public bool IsDoubleGauge { get { return _isBetweenNegativeAndPositive; } }

    public bool IsTitleGauge { get { return _isTitleGauge; } }

    public Color GetTitleGaugeColor { get { return _titleGaugeColor; } }

    public bool IsDisplayReverseValue { get { return _displayReverseValue; } }

    public bool IsLowStatWarningLogActivated { get { return _isLowStatWarningLogActivated; } }

    public string LowStatWarningLogText { get { return _lowStatWarningLogText; } }

}
