﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualMother : MonoBehaviour {
    public GameMaster gm = null;

    List<ChildCharacter> activeChilds = new List<ChildCharacter>();

    [SerializeField]
    ChildCharacter childPrefab = null;

    [SerializeField]
    string[] possibleNamesMale, possibleNamesFemale;
    List<string> availableNamesMale = new List<string>(), availableNamesFemale = new List<string>();

    // Use this for initialization
    public void Init ()
    {
        availableNamesMale = new List<string>(possibleNamesMale);
        availableNamesFemale = new List<string>(possibleNamesFemale);
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddChild ()
    {
        InstantiateAndCreateChild();

    }

    ChildCharacter InstantiateAndCreateChild ()
    {
        GameObject inst = GameObject.Instantiate(childPrefab.gameObject);
        ChildCharacter childInst = inst.GetComponent<ChildCharacter>();

        childInst.isMale = Random.Range(0, 2) >= 1;
        childInst.childName = GetRandomName(childInst.isMale);
        childInst.hm = gm.hm;

        activeChilds.Add(childInst);

        return childInst;
    }

    string GetRandomName (bool isMale)
    {
        string usedString = "";
        if(isMale)
        {
            usedString = availableNamesMale[Random.Range(0, availableNamesMale.Count)];
            availableNamesMale.Remove(usedString);
        }
        else
        {
            usedString = availableNamesFemale[Random.Range(0, availableNamesFemale.Count)];
            availableNamesFemale.Remove(usedString);
        }

        return usedString;
    }

    public void GiveBirth ()
    {
        AddChild();
    }

    public ChildCharacter GetChild (string childName)
    {
        return activeChilds.Find(x => x.childName == childName);
    }
}