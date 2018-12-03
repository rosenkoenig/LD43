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
    //[SerializeField] bool _isOvertime_interestedPoint;
    [SerializeField] List<StatModifier> _duringActivityModifiers;
    [Header("___________________")]
    //[SerializeField] bool _isOvertime_startActivity;
    [SerializeField] List<StatModifier> _startActivityModifiers;
    //[SerializeField] bool _isOvertime_endActivity;
    [Header("___________________")]
    [SerializeField] List<StatModifier> _endActivityModifiers;

    [Header("___________________")]
    [SerializeField] List<ActivityModifier> _activityModifiers;

    public List<ActivityModifier> GetActivityModifiers { get { return _activityModifiers; } }

    public string GetIPName { get { return _ipName; } }

    public enum StatModificationType { DURING_ACTIVITY, START_ACTIVITY, END_ACTIVITY }

    //public bool IsModifierOvertime_interestedPoint { get { return _isOvertime_interestedPoint; } }
    //public bool IsModifierOvertime_startActivity { get { return _isOvertime_startActivity; } }
    //public bool IsModifierOvertime_endActivity { get { return _isOvertime_endActivity; } }

    //public int Multiplier { get { return _multiplier; } }
    //public bool IsMalusOrBonus { get { return _isMalusOrBonus; } }
    //public bool IsRefill { get { return _isRefill; } }

    public void TryModifyStats(StatModificationType statModificationType, ChildStatsContainer childStatsContainer)
    {
        List<StatModifier> selectedStateModifierList = null;
        bool useDeltaTime = false;
        switch (statModificationType)
        {
            case StatModificationType.DURING_ACTIVITY:
                selectedStateModifierList = _duringActivityModifiers;
                useDeltaTime = true;
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
                    curChildStatInfo.currentValue = item.ModifyStat(curChildStatInfo.currentValue, childStatsContainer._childStatInfos, useDeltaTime);
                }
            }
        }
    }
}
