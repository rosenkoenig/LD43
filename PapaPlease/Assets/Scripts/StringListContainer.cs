using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringListContainer", menuName = "Basics/StringListContainer")]
public class StringListContainer : ScriptableObject {

    [SerializeField] List<string> _stringList;
    public List<string> GetStringList { get { return _stringList; } }
}
