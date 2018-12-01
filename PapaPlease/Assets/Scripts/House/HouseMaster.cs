﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseMaster : MonoBehaviour {
    public GameMaster gm = null;

    public List<InterestPoint> allInterestPoints;

    public InterestPoint GetRandomInterestPoint ()
    {
        InterestPoint ip = null;

        int idx = Random.Range(0, allInterestPoints.Count);

        ip = allInterestPoints[idx];

        return ip;
    }
    public InterestPoint GetRandomInterestPoint (InterestPointCategory category)
    {
        InterestPoint ip = null;

        List<InterestPoint> ipMatchingCategory = allInterestPoints.FindAll(x => x.category == category);

        int idx = Random.Range(0, ipMatchingCategory.Count);

        ip = ipMatchingCategory[idx];

        return ip;
    }


}
