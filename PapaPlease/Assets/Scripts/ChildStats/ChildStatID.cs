using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChildStatID", menuName = "Gameplay/ChildStatID")]
public class ChildStatID : ScriptableObject {

    [SerializeField] string _statName;

    [SerializeField] float _minValue;
    [SerializeField] float _maxValue;

    [SerializeField] bool _isDisplayedAsGauge;

    public string StatName { get { return _statName; } }

    public float MinValue { get { return _minValue; } }
    public float MaxValue { get { return _maxValue; } }

    public bool IsDisplayedAsGauge { get { return _isDisplayedAsGauge; } }

}
