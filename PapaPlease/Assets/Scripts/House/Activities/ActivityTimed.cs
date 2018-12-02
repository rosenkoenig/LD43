using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTimed : Activity {

    public float duration;


    protected override void UpdateRunningState()
    {
        base.UpdateRunningState();

        List<ActivityHolder> holdersToEnd = new List<ActivityHolder>();

        foreach (ActivityHolder holder in holders)
        {
            holder.completionPercentage = Mathf.Clamp01((Time.time - holder.startTime) / duration);
            if (Time.time - holder.startTime >= duration)
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
