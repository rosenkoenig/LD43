using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseMaster : MonoBehaviour
{
    public GameMaster gm = null;

    public List<InterestPoint> allInterestPoints;

    private void Start()
    {
        allInterestPoints = new List<InterestPoint>(GetComponentsInChildren<InterestPoint>());
    }

    public InterestPoint GetRandomInterestPoint()
    {
        InterestPoint ip = null;

        int idx = Random.Range(0, allInterestPoints.Count);

        ip = allInterestPoints[idx];

        return ip;
    }

    public InterestPoint GetRandomInterestPoint(IPType type)
    {
        InterestPoint ip = null;

        List<InterestPoint> ipMatchingCategory = allInterestPoints.FindAll(x => x.type == type && x.activity.IsAvailable());

        int idx = Random.Range(0, ipMatchingCategory.Count);

        ip = ipMatchingCategory[idx];

        return ip;
    }
    public InterestPoint GetRandomInterestPoint(IPType type, InterestPoint excludedIP)
    {
        InterestPoint ip = null;

        List<InterestPoint> ipMatchingCategory = allInterestPoints.FindAll(x => x.type == type && x != excludedIP && x.activity.IsAvailable());

        if(ipMatchingCategory.Count > 0)
        {
            int idx = Random.Range(0, ipMatchingCategory.Count);

            ip = ipMatchingCategory[idx];
        }


        return ip;
    }


}
