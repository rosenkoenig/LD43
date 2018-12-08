using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChildStatsModificator {

    [SerializeField] List<StatModifier> statsModifiers;
    public List<StatModifier> GetStatModifiers { get { return statsModifiers; } }

    public void TryModifyStats(ChildStatsContainer childStatsContainer, bool useDeltaTime = false)
    {
        foreach (var curChildStatInfo in childStatsContainer.GetChildStatInfos)
        {
            foreach (var item in statsModifiers)
            {
                if (curChildStatInfo.childStatID == item._childStatID)
                {
                    curChildStatInfo.currentValue = item.ModifyStat(curChildStatInfo.currentValue, childStatsContainer.GetChildStatInfos, useDeltaTime);
                }
            }
        }
    }
    public void TryModifyStats(ChildCharacter child, bool useDeltaTime = false)
    {
        TryModifyStats(child.statsContainer, useDeltaTime);
    }
}
