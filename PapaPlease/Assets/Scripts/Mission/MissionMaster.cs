using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningMission
{
    public Mission mission;
    public ChildCharacter child;
}

public class MissionMaster : MonoBehaviour {

    [SerializeField]
    List<Mission> allMissionSettings;

    List<RunningMission> runningMissions = new List<RunningMission>();
    public List<RunningMission> RunningMissions { get { return runningMissions; } }


    public void StartMission (Mission mission, ChildCharacter child)
    {
        if (runningMissions.Find(x => x.mission == mission) != null) return;

        RunningMission runningMission = new RunningMission();
        runningMission.mission = mission;
        runningMission.child = child;

        runningMissions.Add(runningMission);

        child.AttributeToMission();
    }

    public void ClearAllRunningMissions ()
    {
        runningMissions = new List<RunningMission>();
    }

    public float GetTotalMissionEarnings ()
    {
        float earnings = 0f;
        foreach(RunningMission rm in runningMissions)
        {
            earnings += rm.mission.moneyEarned;
        }

        return earnings;
    }

    void ApplyTotalMoneyEarnings ()
    {
        float totalMoney = GetTotalMissionEarnings();
        totalMoney += GameMaster.Instance.GetAllBillsCost;
        Debug.Log("total money earnings = " + totalMoney);
        if(totalMoney > 0)
        {
            GameMaster.Instance.wallet.EarnMoney(totalMoney);
        }
        else
        {
            GameMaster.Instance.wallet.SpendMoney(totalMoney);
        }


    }

    void ApplyAllMissionsSkillsEarnings ()
    {
        foreach(RunningMission rm in runningMissions)
        {
            rm.mission.onCompleteSkillStatModifier.TryModifyStats(rm.child, false);
            rm.mission.onCompleteHiddenStatModifier._childStatsModificator.TryModifyStats(rm.child, false);
        }
    }

    public void ApplyAll ()
    {
        ApplyAllMissionsSkillsEarnings();
        ApplyTotalMoneyEarnings();
        ClearAllRunningMissions();
    }

    public List<Mission> GetAllAvailableMissions ()
    {
        List<Mission> availableMissions = new List<Mission>();

        foreach(Mission mission in allMissionSettings)
        {
            if (runningMissions.Find(x=> x.mission == mission) == null)
                availableMissions.Add(mission);
        }

        return availableMissions;
    }
}
