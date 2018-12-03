using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class StatModifier
{
    public ChildStatID _childStatID;

    public float _factor = 1;
    public bool _refill;

    public ChildStatID _relativeChildStatID;

    public float ModifyStat(float statValue, List<ChildStatsContainer.ChildStatInfo> statInfos, bool useDeltaTime = false)
    {
        if (_refill)
            return _childStatID.MaxValue;
        float modifiedValue = 0;
        modifiedValue = _factor;
        if(_relativeChildStatID != null)
        {
            ChildStatsContainer.ChildStatInfo childStatInfo = statInfos.FirstOrDefault(x => x.childStatID == _relativeChildStatID);
            if (childStatInfo != null)
                modifiedValue *= childStatInfo.currentValue;
        }


        if (useDeltaTime)
            statValue += modifiedValue * Time.deltaTime;
        else
            statValue += modifiedValue;

        if (statValue > _childStatID.MaxValue)
            return _childStatID.MaxValue;
        else if (statValue < _childStatID.MinValue)
            return _childStatID.MinValue;

        return statValue;
    }
}
