using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildStatsMenu : MonoBehaviour {

    [SerializeField] ChildStatIDsContainer _childStatIdsContainer;
    [SerializeField] ChildStatGauge _childStatTitleGaugeRef;
    [SerializeField] ChildStatGauge _childStatGaugeRef;
    [SerializeField] ChildStatGauge _childStatSkillRef;
    [SerializeField] RectTransform _childStatGaugesParent;

    [SerializeField] Text _childNameText;
    [SerializeField] Text _childCurrentState;

    List<ChildStatGauge> _childStatsGaugesList;

    ChildCharacter curChild;

    private void Awake()
    {
        _childStatsGaugesList = new List<ChildStatGauge>();

        foreach (var item in _childStatIdsContainer.GetChildStatIDs)
        {
            ChildStatGauge newStatGauge = null;
            if(item.IsTitleGauge)
            { 
                newStatGauge = Instantiate(_childStatTitleGaugeRef, _childStatGaugesParent);
                newStatGauge.SetGaugeColor(item.GetTitleGaugeColor);
            }
            else if (item.IsDoubleGauge)
                newStatGauge = Instantiate(_childStatGaugeRef, _childStatGaugesParent);
            else
                newStatGauge = Instantiate(_childStatSkillRef, _childStatGaugesParent);

            newStatGauge.InitGauge(item);
            _childStatsGaugesList.Add(newStatGauge);
        }
    }

    public void SetupMenu(ChildCharacter child)
    {
        curChild = child;
        _childNameText.text = child.childName;
        RefreshGauges(curChild);
    }

    public void HighlightAGauge(ChildStatID childStatID, bool b)
    {
        foreach (var item in _childStatsGaugesList)
        {
            if (item.GetChildStatID == childStatID)
                item.MakeHighlight(b);
        }
    }

    private void Update()
    {
        if(curChild != null)
        {

            RefreshGauges(curChild);
            _childCurrentState.text = curChild.GetStateName();
        }

    }

    private void RefreshGauges(ChildCharacter child)
    {
        foreach (var item in _childStatsGaugesList)
        {
            foreach (var curStatInfo in child.statsContainer.GetChildStatInfos)
            {
                if (curStatInfo.childStatID == item.GetChildStatID)
                    item.RefreshGauge(curStatInfo);
            }
        }
    }
}
