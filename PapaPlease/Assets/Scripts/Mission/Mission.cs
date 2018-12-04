using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionRequisites
{
    public ChildStatID statIDNeeded;
    public float amountNeeded;
}


[CreateAssetMenu(fileName = "MissionFile", menuName = "Gameplay/Mission")]
public class Mission : ScriptableObject {

    public string missionName = "Prostitute";
    public float moneyEarned = 1f;
    public List<MissionRequisites> requisites = new List<MissionRequisites>();
    public ChildStatsModificator onCompleteSkillStatModifier = null;
    public ChildStatsModificatorContainer onCompleteHiddenStatModifier = null;

    public bool RequisitesAreFullFilledFor(ChildCharacter child)
    {
        bool areFullFilled = true;

        foreach (MissionRequisites mr in requisites)
        {
            if (child.statsContainer.GetAChildStatValue(mr.statIDNeeded) < mr.amountNeeded)
            {
                areFullFilled = false;
                break;
            }
        }

        return areFullFilled;
    }
    
}
