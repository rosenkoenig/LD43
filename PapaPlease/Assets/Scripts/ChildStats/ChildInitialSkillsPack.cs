using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChildInitialSkillsPack", menuName = "Gameplay/ChildInitialSkillsPack")]
public class ChildInitialSkillsPack : ScriptableObject
{

    [SerializeField] ChildStatIDsContainer _skillsContainer;
    [SerializeField] float _budget;
    [SerializeField] float _disparity;

    public void GenerateChildStats(ChildStatsContainer childStatsCont)
    {
        float curBudget = _budget;
        while (curBudget > 0)
        {
            foreach (var curStatID in _skillsContainer.GetChildStatIDs)
            {
                foreach (var curStatInfo in childStatsCont.GetChildStatInfos)
                {
                    if (curStatInfo.childStatID == curStatID)
                    {
                        float randomValue = Mathf.Min(curBudget, UnityEngine.Random.Range(1, _disparity));
                        float remainingValue = curStatID.MaxValue - curStatInfo.currentValue;
                        float selectedValue = Mathf.Min(randomValue, remainingValue);

                        curBudget -= selectedValue;
                        curStatInfo.currentValue += selectedValue;

                        if (curBudget <= 0)
                            return;
                    }
                }
            }
        }
    }
}
