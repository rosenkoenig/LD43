using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    public ChildStatID _childStatID;

    public float _factor = 1;
    public bool _refill;

    public float ModifyStat(float statValue)
    {
        if (_refill)
            return _childStatID.MaxValue;
        float modifiedValue = 0;
        modifiedValue = _factor;

        statValue += modifiedValue;

        if (statValue > _childStatID.MaxValue)
            return _childStatID.MaxValue;
        else if (statValue < _childStatID.MinValue)
            return _childStatID.MinValue;

        return statValue;
    }
}
