using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IPType", menuName = "Gameplay/IPType")]
public class IPType : ScriptableObject {

    [SerializeField] string _ipName;

    public string GetIPName { get { return _ipName; } }
	
}
