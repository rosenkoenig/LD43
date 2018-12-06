using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTimed : Activity {

    protected override void UpdateRunningState()
    {
        base.UpdateRunningState();

        List<ActivityHolder> holdersToEnd = new List<ActivityHolder>();

        foreach (ActivityHolder holder in holders)
        {
            //Debug.Log("progression: " + ((Time.time - holder.startTime) * (1 + holder.GetActivityModifiersRatios(_inheritedIPType.GetActivityModifiers))));
            float curDuration = (Time.time - holder.startTime) * (1 + holder.GetActivityModifiersRatios(_inheritedIPType.GetActivityModifiers)) + holder.bonusCompletion;
            holder.curCompletion = curDuration;
            holder.completionPercentage = Mathf.Clamp01(curDuration / duration);
            if (curDuration >= duration)
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
