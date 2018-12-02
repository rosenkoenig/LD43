using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChildStatsContainer {

    public List<ChildStatInfo> _childStatInfos { get; private set; }
    [SerializeField] ChildStatIDsContainer _childStatsIdContainer;
    [SerializeField] List<ChildInitialSkillsPack> _childInitialSkillsPacks;
    
    public void Init()
    {
        _childStatInfos = new List<ChildStatInfo>();
        foreach (var curStatID in _childStatsIdContainer.GetChildStatIDs)
        {
            _childStatInfos.Add(new ChildStatInfo() { childStatID = curStatID });
        }

        foreach (var item in _childStatInfos)
        {
            item.currentValue = item.childStatID.GetStartValue;
        }

        ChildInitialSkillsPack selectedInitialSkillPack = _childInitialSkillsPacks[UnityEngine.Random.Range(0, _childInitialSkillsPacks.Count)];
        selectedInitialSkillPack.GenerateChildStats(this);
    }

    public float GetAChildStatValue(ChildStatID refID)
    {
        foreach (var item in _childStatInfos)
        {
            if (item.childStatID == refID)
                return item.currentValue;
        }
        Debug.LogError("child stat not found!", refID);
        return 0;
    }

    public float GetAChildStatValueRatio(ChildStatID refID)
    {
        foreach (var item in _childStatInfos)
        {
            if (item.childStatID == refID)
                return (item.childStatID.MaxValue - item.childStatID.MinValue) * item.currentValue / 100;
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
