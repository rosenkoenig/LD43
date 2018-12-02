using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterestPointModification { ON_COMPLETED, OVERTIME_EMPTY, OVERTIME_FULL }
public class InterestPoint : MonoBehaviour {

    public string ipName = "";
    public Transform pivotPoint;
    [UnityEngine.Serialization.FormerlySerializedAs("type")]
    public IPType iPtype;
    public Activity activity;
    public bool onlyUsableByChild = false;
    
    public void TryMakeGlobalModification(InterestPointModification interestPointModification)
    {
        switch(interestPointModification)
        {
            case InterestPointModification.ON_COMPLETED:
                if (globalStatsModificator_OnCompleted != null)
                    GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OnCompleted);
                break;
            case InterestPointModification.OVERTIME_EMPTY:
                if (globalStatsModificator_OverTimeEmpty != null)
                    GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeEmpty);
                break;
            case InterestPointModification.OVERTIME_FULL:
                if (globalStatsModificator_OverTimeFull != null)
                    GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeFull);
                break;
        }
    }

    public ChildStatsModificatorContainer globalStatsModificator_OnCompleted;
    public ChildStatsModificatorContainer globalStatsModificator_OverTimeEmpty;
    public ChildStatsModificatorContainer globalStatsModificator_OverTimeFull;

    public bool Interact (Character character)
    {
        if(onlyUsableByChild && character.GetComponent<PlayerBehaviour>())
        {
            return false;
        }


        bool interacts = activity.IsAvailable(character);
        if (interacts)
        {
            activity.Begin(character);
            character.currentActivity = activity; 
        }

        return interacts;
    }

    void Update ()
    {
        if (activity.State < ActivityState.COMPLETE)
            GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeEmpty);
        else
            GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeFull);

        activity.Update();
    }
}
