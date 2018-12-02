﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildStatsMenu : MonoBehaviour {

    [SerializeField] ChildStatIDsContainer _childStatIdsContainer;
    [SerializeField] ChildStatGauge _childStatGaugeRef;
    [SerializeField] ChildStatGauge _childStatSkillRef;
    [SerializeField] RectTransform _childStatGaugesParent;

    [SerializeField] Text _childNameText;

    List<ChildStatGauge> _childStatsGaugesList;

    ChildCharacter curChild;

    private void Awake()
    {
        _childStatsGaugesList = new List<ChildStatGauge>();

        foreach (var item in _childStatIdsContainer.GetChildStatIDs)
        {
            ChildStatGauge newStatGauge = null;
            if (item.IsDisplayedAsGauge)
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

    private void Update()
    {
        if(curChild != null)
            RefreshGauges(curChild);
    }

    private void RefreshGauges(ChildCharacter child)
    {
        foreach (var item in _childStatsGaugesList)
        {
            foreach (var curStatInfo in child.statsContainer._childStatInfos)
            {
                if (curStatInfo.childStatID == item.GetChildStatID)
                    item.RefreshGauge(curStatInfo);
            }
        }
    }
}