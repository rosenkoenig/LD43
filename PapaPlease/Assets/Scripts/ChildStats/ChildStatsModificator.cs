using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChildStatsModificator", menuName = "Gameplay/ChildStatsModificator")]
public class ChildStatsModificator : ScriptableObject {

    [SerializeField] List<StatModifier> statsModifiers;

    public void TryModifyStats(ChildStatsContainer childStatsContainer)
    {
        foreach (var curChildStatInfo in childStatsContainer._childStatInfos)
        {
            foreach (var item in statsModifiers)
            {
                if (curChildStatInfo.childStatID == item._childStatID)
                {
                    curChildStatInfo.currentValue = item.ModifyStat(curChildStatInfo.currentValue);
                }
            }
        }
    }
}
