using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChildStatsContainer
{
    [SerializeField] List<ChildStatInfo> _childStatsInfos;
    public List<ChildStatInfo> GetChildStatInfos { get { return _childStatsInfos; } }
    [SerializeField] ChildStatIDsContainer _childStatsIdContainer;
    [SerializeField] List<ChildInitialSkillsPack> _childInitialSkillsPacks;

    public void Init(bool isPlayer = false)
    {
        _childStatsInfos = new List<ChildStatInfo>();
        foreach (var curStatID in _childStatsIdContainer.GetChildStatIDs)
        {
            _childStatsInfos.Add(new ChildStatInfo() { childStatID = curStatID });
        }
        if (isPlayer)
        {
            foreach (var item in _childStatsInfos)
            {
                item.currentValue = item.childStatID.GetPlayerValue;
            }
        }
        else
        {
            foreach (var item in _childStatsInfos)
            {
                item.currentValue = item.childStatID.GetStartValue;
            }

            ChildInitialSkillsPack selectedInitialSkillPack = _childInitialSkillsPacks[UnityEngine.Random.Range(0, _childInitialSkillsPacks.Count)];
            selectedInitialSkillPack.GenerateChildStats(this);
        }
    }

    public float GetAChildStatValue(ChildStatID refID)
    {
        foreach (var item in _childStatsInfos)
        {
            if (item.childStatID == refID)
                return Mathf.Round(item.currentValue)   ;
        }
        Debug.LogError("child stat not found!", refID);
        return 0;
    }

    public float GetAChildStatValueRatio(ChildStatID refID)
    {
        foreach (var item in _childStatsInfos)
        {
            if (item.childStatID == refID)
                //return item.currentValue / (item.childStatID.MaxValue - item.childStatID.MinValue);
                return Mathf.InverseLerp(item.childStatID.MinValue, item.childStatID.MaxValue, item.currentValue);
        }
        Debug.LogError("child stat not found!");
        return 0;
    }

    [System.Serializable]
    public class ChildStatInfo
    {
        public ChildStatID childStatID;
        public float currentValue;
    }

}
