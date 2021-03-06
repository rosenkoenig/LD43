﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionPanel : Popup {

    [SerializeField]
    UIMissionItem missionItemPrefab = null;

    [SerializeField]
    VerticalLayoutGroup missionItemParent = null;
    List<UIMissionItem> allMissionItems = new List<UIMissionItem>();

    [SerializeField]
    UIMissionSkillItem missionSkillItemPrefab = null;

    [SerializeField]
    VerticalLayoutGroup missionSkillItemParent = null;
    List<UIMissionSkillItem> allMissionSkillItems = new List<UIMissionSkillItem>();

    [SerializeField]
    Text rentAmount, elecAmount, gazAmount, waterAmount, totalEarningAmount;

    public override void Init(object[] args)
    {
        base.Init(args);

        foreach(UIMissionItem item in allMissionItems)
        {
            if(item != null)
                Destroy(item.gameObject);
        }
        allMissionItems.Clear();

        foreach (UIMissionSkillItem item in allMissionSkillItems)
        {
            if(item != null)
                Destroy(item.gameObject);
        }
        allMissionSkillItems.Clear();

        List<RunningMission> runningMissions = GameMaster.Instance.mm.RunningMissions;

        foreach(RunningMission rm in runningMissions)
        {
            UIMissionItem item = CreateMissionItem();
            item.Init(rm.mission.missionName, rm.mission.moneyEarned.ToString());
            allMissionItems.Add(item);

            foreach(StatModifier statModifier in rm.mission.onCompleteSkillStatModifier.GetStatModifiers)
            {
                UIMissionSkillItem skillItem = CreateMissionSkillItem();
                skillItem.Init(rm.child.childName, statModifier._childStatID.StatName, rm.child.statsContainer.GetAChildStatValue(statModifier._childStatID), statModifier._factor);
                allMissionSkillItems.Add(skillItem);
            }
            
        }
        

        missionItemParent.CalculateLayoutInputVertical();
        missionSkillItemParent.CalculateLayoutInputVertical();

        rentAmount.text =   GameMaster.Instance.wallet.rentCost.ToString() + "$";
        elecAmount.text =   GameMaster.Instance.wallet.elecCost.ToString() + "$";
        gazAmount.text =    GameMaster.Instance.wallet.gazCost.ToString() + "$";
        waterAmount.text =  GameMaster.Instance.wallet.waterCost.ToString() + "$";

        float totalEarning = GameMaster.Instance.mm.GetTotalMissionEarnings();
        totalEarning += GameMaster.Instance.GetAllBillsCost;

        totalEarningAmount.text = (totalEarning > 0 ? "+" : "") + totalEarning.ToString() + "$";
    }

    UIMissionItem CreateMissionItem ()
    {
        GameObject inst = GameObject.Instantiate(missionItemPrefab.gameObject, missionItemParent.transform);

        UIMissionItem item = inst.GetComponent<UIMissionItem>();

        return item;
    }

    UIMissionSkillItem CreateMissionSkillItem ()
    {
        GameObject inst = GameObject.Instantiate(missionSkillItemPrefab.gameObject, missionSkillItemParent.transform);

        UIMissionSkillItem item = inst.GetComponent<UIMissionSkillItem>();

        return item;
    }

    public void OnQuitButton  ()
    {
        GameMaster.Instance.uIMaster.OnMissionPanelCloses();
        ClosePopup();
    }
}
