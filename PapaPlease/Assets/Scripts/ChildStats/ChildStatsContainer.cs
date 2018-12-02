using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChildStatsContainer {

    public List<ChildStatInfo> _childStatInfos { get; private set; }
    [SerializeField] ChildStatIDsContainer _childStatsIdContainer;
    [SerializeField] List<ChildInitialSkillsPack> _childInitialSkillsPacks;
    void Awake()
    {
        _childStatInfos = new List<ChildStatInfo>();
        foreach (var curStatID in _childStatsIdContainer.GetChildStatIDs)
        {
            _childStatInfos.Add(new ChildStatInfo() { childStatID = curStatID });
        }
        ChildInitialSkillsPack selectedInitialSkillPack = _childInitialSkillsPacks[UnityEngine.Random.Range(0, _childInitialSkillsPacks.Count)];
        selectedInitialSkillPack.GenerateChildStats(this);
    }

    [System.Serializable]
    public class ChildStatInfo
    {
        public ChildStatID childStatID;
        public float currentValue;
    }

}
