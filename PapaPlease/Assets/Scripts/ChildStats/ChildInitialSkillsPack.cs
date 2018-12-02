using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChildInitialSkillsPack", menuName = "Gameplay/ChildInitialSkillsPack")]
public class ChildInitialSkillsPack : ScriptableObject {

    [SerializeField] ChildStatIDsContainer _skillsContainer;
    [SerializeField] float _budget;

    public void GenerateChildStats(ChildStatsContainer childStatsCont)
    {
        float curBudget = _budget;
        foreach (var curStatID in _skillsContainer.GetChildStatIDs)
        {
            foreach (var curStatInfo in childStatsCont._childStatInfos)
            {
                if (curStatInfo.childStatID == curStatID)
                {
                    float maxAddedValue = curBudget;
                    if (maxAddedValue > curStatID.MaxValue)
                        maxAddedValue = curStatID.MaxValue;

                    float selectedValue = UnityEngine.Random.Range(curStatID.MinValue, maxAddedValue);
                    curBudget -= selectedValue;
                    curStatInfo.currentValue = selectedValue;
                }
            }
        }
    }
}
