using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildMissionButton : MonoBehaviour {

    [SerializeField]
    Text missionName = null;

    [SerializeField]
    Text[] missionRequisites = null;

    [SerializeField]
    Text[] missionSkillRewards = null;

    [SerializeField]
    Text moneyEarning = null;

    Mission _mission;
    ChildInteractionMission _master;

    public void Init (Mission mission, ChildInteractionMission master)
    {
        _mission = mission;
        _master = master;

        missionName.text = _mission.missionName;

        for (int i = 0; i < missionRequisites.Length; i++)
        {
            if (i < _mission.requisites.Count)
            {
                missionRequisites[i].text = _mission.requisites[i].statIDNeeded.StatName + " " + _mission.requisites[i].amountNeeded;
                missionRequisites[i].gameObject.SetActive(true);
            }
            else
                missionRequisites[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < missionSkillRewards.Length; i++)
        {
            if (i < _mission.onCompleteSkillStatModifier.GetStatModifier.Count)
            {
                missionSkillRewards[i].text = _mission.onCompleteSkillStatModifier.GetStatModifier[i]._childStatID.StatName + " +" + _mission.onCompleteSkillStatModifier.GetStatModifier[i]._factor;
                missionSkillRewards[i].gameObject.SetActive(true);
            }
            else
                missionSkillRewards[i].gameObject.SetActive(false);
        }

        moneyEarning.text = "Money: "+ _mission.moneyEarned.ToString() + " $";
    }

    public void OnClick()
    {
        _master.OnButtonClick(_mission);
    }
}
