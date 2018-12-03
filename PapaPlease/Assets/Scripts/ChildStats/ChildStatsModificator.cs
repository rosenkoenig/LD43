using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChildStatsModificator {

    [SerializeField] List<StatModifier> statsModifiers;
    public List<StatModifier> GetStatModifier { get { return statsModifiers; } }

    public void TryModifyStats(ChildStatsContainer childStatsContainer, bool useDeltaTime = false)
    {
        foreach (var curChildStatInfo in childStatsContainer._childStatInfos)
        {
            foreach (var item in statsModifiers)
            {
                if (curChildStatInfo.childStatID == item._childStatID)
                {
                    curChildStatInfo.currentValue = item.ModifyStat(curChildStatInfo.currentValue, childStatsContainer._childStatInfos, useDeltaTime);
                }
            }
        }
    }
    public void TryModifyStats(ChildCharacter child, bool useDeltaTime = false)
    {
        TryModifyStats(child.statsContainer, useDeltaTime);
    }
}
