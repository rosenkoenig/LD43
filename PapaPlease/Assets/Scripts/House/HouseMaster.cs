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
    public List<Transform> allChairPivots;

    [SerializeField]
    RoomExtension roomExtensionPrefab;


    [SerializeField]
    GameObject roomExtensionEndPrefab;
    GameObject roomExtensionEndInst;

    [SerializeField]
    Transform roomExtensionPivot;


    public Transform playerTableSpawnPoint = null;
    public Transform playerDaySpawnPoint = null;

    [SerializeField]
    DoorLocked doorLocked = null;

    private void Start()
    {
        allInterestPoints = new List<InterestPoint>(GetComponentsInChildren<InterestPoint>());
    }

    public void Init ()
    {
        UpdateTableRoomSize();
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
            if (ip.iPtype != gm.ipTypeFun && ip.activity.State == ActivityState.RUNNING)
                anActivityIsRunning = true;
        }

        return !anActivityIsRunning;
    }


    #region Table
    [SerializeField]
    float roomExtensionWidth = 1f;

    List<RoomExtension> roomExtensions = new List<RoomExtension>();
    public void UpdateTableRoomSize ()
    {
        int childCount = gm.vm.GetChildrenNumber;

        foreach(RoomExtension re in roomExtensions)
        {
            Destroy(re.gameObject);
        }
        roomExtensions = new List<RoomExtension>();
        allChairPivots = new List<Transform>();

        int roomNeeded = Mathf.CeilToInt((float)childCount / 2f);
        Debug.Log(childCount + " - " + roomNeeded);
        for (int i = 0; i < roomNeeded; i++)
        {
            GameObject inst = GameObject.Instantiate(roomExtensionPrefab.gameObject, roomExtensionPivot.position + roomExtensionPivot.forward * (i * roomExtensionWidth), roomExtensionPivot.rotation);
            RoomExtension curExt = inst.GetComponent<RoomExtension>();
            roomExtensions.Add(curExt);

            allChairPivots.Add(curExt.chairsPivot[0]);
            allChairPivots.Add(curExt.chairsPivot[1]);
        }

        if(roomExtensionEndInst == null)
        {
            roomExtensionEndInst = GameObject.Instantiate(roomExtensionEndPrefab.gameObject, roomExtensionPivot.position + roomExtensionPivot.forward * (roomExtensions.Count * roomExtensionWidth), roomExtensionPivot.rotation);
        }
        else
        {
            roomExtensionEndInst.transform.position = roomExtensionPivot.position + roomExtensionPivot.forward * (roomExtensions.Count * roomExtensionWidth);
        }

    }

    public void SetDoorLockedClosed (bool closed)
    {
        if (closed)
            doorLocked.Close();
        else
            doorLocked.Open();
    }


    public void SetChildrenOnTable ()
    {
        List<ChildCharacter> children = gm.vm.allChildren;

        Debug.Log("child coun = " + children.Count + " & all chair pivot count = "+allChairPivots.Count);
        for (int i = 0; i < children.Count; i++)
        {
            if (i >= allChairPivots.Count) break;
            children[i].SetOnTable(allChairPivots[i]);
        }
    }

    
    #endregion
}
