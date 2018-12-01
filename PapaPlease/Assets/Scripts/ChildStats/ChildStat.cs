using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChildStat {

    [SerializeField] string _statName;

    public string StatName { get { return _statName; } }
}
