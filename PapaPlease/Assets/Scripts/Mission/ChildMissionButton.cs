using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildMissionButton : MonoBehaviour
{

    [SerializeField]
    Text missionName = null;

    [SerializeField]
    Text[] missionRequisites = null;

    [SerializeField]
    Text[] missionRequisitesAmounts = null;

    [SerializeField]
    Text[] missionSkillRewards = null;

    [SerializeField]
    Text[] missionSkillRewardsAmounts = null;

    [SerializeField]
    Text moneyEarning = null;

    [SerializeField]
    Color negativeColor = Color.red;

    [SerializeField] Text[] missionRequisitiesNotEnoughBigTexts;

    Mission _mission;
    ChildInteractionMission _master;

    public Mission GetMission { get { return _mission; } }

    [SerializeField]
    Button button;

    public void Init(ChildCharacter child, Mission mission, ChildInteractionMission master, bool isAvailable)
    {
        _mission = mission;
        _master = master;

        missionName.text = _mission.missionName;
        

        for (int i = 0; i < missionSkillRewards.Length; i++)
        {
            if (i < _mission.onCompleteSkillStatModifier.GetStatModifier.Count)
            {
                missionSkillRewards[i].text = _mission.onCompleteSkillStatModifier.GetStatModifier[i]._childStatID.StatName;

                float rewardAmount = _mission.onCompleteSkillStatModifier.GetStatModifier[i]._factor;
                bool rewardIsPositive = rewardAmount >= 0;
                missionSkillRewardsAmounts[i].text = (rewardIsPositive ? "+" : "") + rewardAmount.ToString("F0");
                if (rewardIsPositive == false) missionSkillRewardsAmounts[i].color = negativeColor;

                missionSkillRewards[i].gameObject.SetActive(true);
                missionSkillRewardsAmounts[i].gameObject.SetActive(true);
            }
            else
            {
                missionSkillRewards[i].gameObject.SetActive(false);
                missionSkillRewardsAmounts[i].gameObject.SetActive(false);
            }
        }

        if (_mission.moneyEarned != 0f)
            moneyEarning.text = _mission.moneyEarned.ToString() + "$";
        else
            moneyEarning.gameObject.SetActive(false);

        if (_mission.moneyEarned < 0)
            moneyEarning.color = negativeColor;

        if (isAvailable == false)
            button.interactable = false;

        RefreshAvailability(child, isAvailable);
    }

    public void RefreshAvailability(ChildCharacter child, bool isAvailable)
    {
        if (isAvailable == false)
            button.interactable = false;

        for (int i = 0; i < missionRequisites.Length; i++)
        {
            if (i < _mission.requisites.Count)
            {
                missionRequisites[i].text = _mission.requisites[i].statIDNeeded.StatName;
                missionRequisitesAmounts[i].text = _mission.requisites[i].amountNeeded.ToString("F1");
                missionRequisites[i].gameObject.SetActive(true);
                missionRequisitesAmounts[i].gameObject.SetActive(true);
            }
            else
            {
                missionRequisites[i].gameObject.SetActive(false);
                missionRequisitesAmounts[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < missionRequisitiesNotEnoughBigTexts.Length; i++)
        {
            if (i < _mission.requisites.Count)
            {
                if (!isAvailable && child.statsContainer.GetAChildStatValue(_mission.requisites[i].statIDNeeded) < _mission.requisites[i].amountNeeded)
                {
                    missionRequisitiesNotEnoughBigTexts[i].text = "Not enough " + "\n" + _mission.requisites[i].statIDNeeded.StatName;
                    missionRequisitiesNotEnoughBigTexts[i].gameObject.SetActive(true);
                }
                else
                    missionRequisitiesNotEnoughBigTexts[i].gameObject.SetActive(false);
            }
            else
                missionRequisitiesNotEnoughBigTexts[i].gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        _master.OnButtonClick(_mission);
    }
}
