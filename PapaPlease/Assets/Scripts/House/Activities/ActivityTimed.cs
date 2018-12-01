using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTimed : Activity {

    public float duration;


    protected override void UpdateRunningState()
    {
        base.UpdateRunningState();

        List<ActivityHolder> holdersToEnd = new List<ActivityHolder>();

        foreach(ActivityHolder holder in holders)
        {
            if (Time.time - holder.startTime >= duration)
            {
                holdersToEnd.Add(holder);
            }
        }    
        
        foreach(ActivityHolder holder in holdersToEnd)
        {
            End(holder.character);
        }    
    }
}
