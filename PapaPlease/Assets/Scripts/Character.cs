﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public Activity currentActivity = null;

    public ChildStatsContainer statsContainer;

    public virtual void OnActivityEnds()
    {
        currentActivity = null;
    }
}
