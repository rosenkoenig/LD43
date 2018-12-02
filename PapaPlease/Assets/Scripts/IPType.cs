using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IPType", menuName = "Gameplay/IPType")]
public class IPType : ScriptableObject
{

    [SerializeField] string _ipName;

    //[SerializeField] int _multiplier = 1;
    //[SerializeField] bool _isMalusOrBonus;
    //[SerializeField] bool _isRefill;
    [SerializeField] bool _isModifierOvertime_interestedPoint;
    [SerializeField] bool _isModifierOvertime_startActivity;
    [SerializeField] bool _isModifierOvertime_endActivity;

    [SerializeField] List<StatModifier> _interestPointModifiers;
    [SerializeField] List<StatModifier> _startActivityModifiers;
    [SerializeField] List<StatModifier> _endActivityModifiers;

    public string GetIPName { get { return _ipName; } }

    public enum StatModificationType { INTEREST_POINT, START_ACTIVITY, END_ACTIVITY }

    public bool IsModifierOvertime_interestedPoint { get { return _isModifierOvertime_interestedPoint; } }
    public bool IsModifierOvertime_startActivity { get { return _isModifierOvertime_startActivity; } }
    public bool IsModifierOvertime_endActivity { get { return _isModifierOvertime_endActivity; } }

    //public int Multiplier { get { return _multiplier; } }
    //public bool IsMalusOrBonus { get { return _isMalusOrBonus; } }
    //public bool IsRefill { get { return _isRefill; } }

    public void TryModifyStats(StatModificationType statModificationType, ChildStatsContainer childStatsContainer)
    {
        List<StatModifier> selectedStateModifierList = null;
        switch (statModificationType)
        {
            case StatModificationType.INTEREST_POINT:
                selectedStateModifierList = _interestPointModifiers;
                break;
            case StatModificationType.START_ACTIVITY:
                selectedStateModifierList = _startActivityModifiers;
                break;
            case StatModificationType.END_ACTIVITY:
                selectedStateModifierList = _endActivityModifiers;
                break;
        }

        foreach (var curChildStatInfo in childStatsContainer._childStatInfos)
        {
            foreach (var item in selectedStateModifierList)
            {
                if (curChildStatInfo.childStatID == item._childStatID)
                {
                    curChildStatInfo.currentValue = item.ModifyStat(curChildStatInfo.currentValue);
                }
            }
        }
    }

    [System.Serializable]
    public class StatModifier
    {
        public ChildStatID _childStatID;

        public float _factor = 1;
        //public bool _isPercentage;
        public bool _refill;

        public float ModifyStat(float statValue)
        {
            if (_refill)
                return _childStatID.MaxValue;
            float modifiedValue = 0;
            //if (_isPercentage)
            //modifiedValue = (_childStatID.maxValue - _childStatID.minValue) * _factor / 100f;
            //else
            modifiedValue = _factor;

            statValue += modifiedValue;

            if (statValue > _childStatID.MaxValue)
                return _childStatID.MaxValue;
            else if (statValue < _childStatID.MinValue)
                return _childStatID.MinValue;

            return statValue;
        }
    }

}
