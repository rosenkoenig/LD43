using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IPTypeInfo
{
    public IPType IPType;
    public bool isAvailable;
}

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

        List<InterestPoint> ipMatchingCategory = allInterestPoints.FindAll(x => x.iPtype == type && x.activity.IsAvailable());

        int idx = Random.Range(0, ipMatchingCategory.Count);

        ip = ipMatchingCategory[idx];

        return ip;
    }
    public InterestPoint GetRandomInterestPoint(IPType type, InterestPoint excludedIP)
    {
        InterestPoint ip = null;

        List<InterestPoint> ipMatchingCategory = allInterestPoints.FindAll(x => x.iPtype == type && x != excludedIP && x.activity.IsAvailable());

        if(ipMatchingCategory.Count > 0)
        {
            int idx = Random.Range(0, ipMatchingCategory.Count);

            ip = ipMatchingCategory[idx];
        }


        return ip;
    }

    public List<IPTypeInfo> GetAllIPTypeInfos ()
    {
        List<IPType> allIpType = new List<IPType>();
        List<IPTypeInfo> allIPTypeInfos = new List<IPTypeInfo>();
        foreach (InterestPoint ip in allInterestPoints)
        {
            if (allIpType.Contains(ip.iPtype) == false)
                allIpType.Add(ip.iPtype);
        }

        List<IPType> availableIpType = GetAllAvailableIPTypes();

        foreach(IPType ipType in allIpType)
        {
            IPTypeInfo info = new IPTypeInfo();
            info.IPType = ipType;

            info.isAvailable = availableIpType.Contains(ipType);

            allIPTypeInfos.Add(info);
        }

        return allIPTypeInfos;
    }

    List<IPType> GetAllAvailableIPTypes ()
    {
        List<IPType> availableIPType = new List<IPType>();

        foreach (InterestPoint ip in allInterestPoints)
        {
            if (ip.activity.IsAvailable() && availableIPType.Contains(ip.iPtype) == false)
                availableIPType.Add(ip.iPtype);
        }        

        return availableIPType;
    }

    public bool NoActivityIsRunning ()
    {
        bool anActivityIsRunning = false;

        foreach(InterestPoint ip in allInterestPoints)
        {
            if (ip.activity.State == ActivityState.RUNNING)
                anActivityIsRunning = true;
        }

        return anActivityIsRunning;
    }
}
