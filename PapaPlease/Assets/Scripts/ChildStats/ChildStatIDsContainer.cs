using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChildStatIDsContainer", menuName = "Gameplay/ChildStatIDsContainer")]
public class ChildStatIDsContainer : ScriptableObject {

    [SerializeField] List<ChildStatID> _childStatIds;
    public List<ChildStatID> GetChildStatIDs { get { return _childStatIds; } }
}
