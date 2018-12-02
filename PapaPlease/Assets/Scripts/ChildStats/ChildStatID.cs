using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChildStatID", menuName = "Gameplay/ChildStatID")]
public class ChildStatID : ScriptableObject {

    [SerializeField] string _statName;

    [SerializeField] float _maxValue;

    [SerializeField] float _startValue;

    [SerializeField] string _addedJaugeText;

    [UnityEngine.Serialization.FormerlySerializedAs("_isDisplayedAsGauge")]
    [SerializeField] bool _isBetweenNegativeAndPositive;

    public string StatName { get { return _statName; } }

    public float MinValue { get { return _isBetweenNegativeAndPositive ? -_maxValue : 0; } }
    public float MaxValue { get { return _maxValue; } }

    public float GetStartValue { get { return _startValue; } }

    public string GetAddedJaugeText { get { return _addedJaugeText; } }

    public bool IsDisplayedAsGauge { get { return _isBetweenNegativeAndPositive; } }

}
