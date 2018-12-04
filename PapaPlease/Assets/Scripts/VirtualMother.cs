using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualMother : MonoBehaviour
{
    public GameMaster gm = null;

    List<ChildCharacter> activeChilds = new List<ChildCharacter>();
    [SerializeField] List<ChildCharacter> allChilds = new List<ChildCharacter>();


    [SerializeField]
    ChildCharacter childPrefab = null;

    [SerializeField]
    string[] possibleNamesMale, possibleNamesFemale;
    List<string> availableNamesMale = new List<string>(), availableNamesFemale = new List<string>();

    public int baseChildsQuantity = 2;

    [SerializeField] List<ChildStatsModificatorContainer> _passiveChildStatsModificators;

    [SerializeField] float _IpDegradationAddedPerChild;

    [SerializeField] Transform _childsMorningSpawnPositionsParent;

    public float GetChildsIpDegradationAddedFactor { get { return _IpDegradationAddedPerChild * activeChilds.Count; } }

    public int GetChildrenNumber { get { return activeChilds.Count; } }

    public List<ChildCharacter> allChildren { get { return allChilds; } }

    public bool hasCondom = false;

    // Use this for initialization
    public void Init()
    {
        availableNamesMale = new List<string>(possibleNamesMale);
        availableNamesFemale = new List<string>(possibleNamesFemale);
    }

    public void SpawnBaseChilds()
    {
        for (int i = 0; i < baseChildsQuantity; i++)
        {
            GiveBirth();
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayPassiveModificators();
    }

    private void PlayPassiveModificators()
    {
        if (gm.gf.GetGameState == GameState.DAY)
        {
            foreach (var item in _passiveChildStatsModificators)
            {
                foreach (var child in activeChilds)
                {
                    item.MakeTryModifyStats(child.statsContainer, true);
                }
            }
        }
    }

    void AddChild()
    {
        InstantiateAndCreateChild();

    }

    ChildCharacter InstantiateAndCreateChild()
    {
        GameObject inst = GameObject.Instantiate(childPrefab.gameObject);
        ChildCharacter childInst = inst.GetComponent<ChildCharacter>();

        childInst.isMale = UnityEngine.Random.Range(0, 2) >= 1;
        childInst.childName = GetRandomName(childInst.isMale);
        childInst.hm = gm.hm;

        activeChilds.Add(childInst);
        allChilds.Add(childInst);

        childInst.Init();

        return childInst;
    }

    string GetRandomName(bool isMale)
    {
        string usedString = "";
        if (isMale)
        {
            usedString = availableNamesMale[UnityEngine.Random.Range(0, availableNamesMale.Count)];
            availableNamesMale.Remove(usedString);
        }
        else
        {
            usedString = availableNamesFemale[UnityEngine.Random.Range(0, availableNamesFemale.Count)];
            availableNamesFemale.Remove(usedString);
        }

        return usedString;
    }

    public void GiveBirth()
    {
        if(!hasCondom)
            AddChild();
    }

    public ChildCharacter GetChild(string childName)
    {
        return allChilds.Find(x => x.childName == childName);
    }

    public void ApplyGlobalModifier(ChildStatsModificatorContainer statsModificator, bool useDeltaTime = false)
    {
        if (statsModificator != null)
        {
            foreach (var item in activeChilds)
            {
                statsModificator.MakeTryModifyStats(item.statsContainer, useDeltaTime);
            }
        }
    }
    
    public void GetChildrenOutOfTable()
    {
        for (int i = 0; i < activeChilds.Count; i++)
        {
            activeChilds[i].transform.position = _childsMorningSpawnPositionsParent.GetChild(i).position;
            activeChilds[i].LeaveTable();

        }
        foreach (ChildCharacter child in allChildren)
        {
            child.SetPlate(null);
        }
    }

    public void ClearDeadChildren()
    {
        List<ChildCharacter> deads = new List<ChildCharacter>();

        foreach (ChildCharacter child in allChildren)
        {
            if (child.ChildState == ChildAIState.DEAD)
            {
                deads.Add(child);
            }
        }

        foreach (ChildCharacter child in deads)
        {
            allChilds.Remove(child);
            if (activeChilds.Contains(child)) activeChilds.Remove(child);
            Destroy(child.gameObject);
        }

    }
}
