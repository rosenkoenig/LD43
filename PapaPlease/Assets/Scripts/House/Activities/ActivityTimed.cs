using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTimed : Activity {

    [Header("___________")]
    public float duration;
    


    protected override void UpdateRunningState()
    {
        base.UpdateRunningState();

        List<ActivityHolder> holdersToEnd = new List<ActivityHolder>();

        foreach (ActivityHolder holder in holders)
        {
            holder.completionPercentage = Mathf.Clamp01((Time.time - holder.startTime) * (1 + holder.GetActivityModifiersRatios(_inheritedIPType.GetActivityModifiers)) / duration);
            if ((Time.time - holder.startTime) * (1 + holder.GetActivityModifiersRatios(_inheritedIPType.GetActivityModifiers)) >= duration)
            {
                holdersToEnd.Add(holder);
                holder.completionPercentage = 1f;
            }
        }

        if (holders.Count > 0)
            GetCompletionRatio = holders[0].completionPercentage;

        foreach (ActivityHolder holder in holdersToEnd)
        {
            End(holder.character);
        }    
    }
}
