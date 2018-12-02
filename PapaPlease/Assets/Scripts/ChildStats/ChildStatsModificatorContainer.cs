using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChildStatsModificatorContainer", menuName = "Gameplay/ChildStatsModificatorContainer")]
public class ChildStatsModificatorContainer : ScriptableObject {

    public ChildStatsModificator _childStatsModificator;

    public void MakeTryModifyStats(ChildStatsContainer childStatsContainer)
    {
        _childStatsModificator.TryModifyStats(childStatsContainer);
    }

    //List<StatModifier> statsModifiers;

    //public void TryModifyStats(ChildStatsContainer childStatsContainer)
    //{
    //    foreach (var curChildStatInfo in childStatsContainer._childStatInfos)
    //    {
    //        foreach (var item in statsModifiers)
    //        {
    //            if (curChildStatInfo.childStatID == item._childStatID)
    //            {
    //                curChildStatInfo.currentValue = item.ModifyStat(curChildStatInfo.currentValue);
    //            }
    //        }
    //    }
    //}
}
