using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChildStatsContainer {

    public List<ChildStatInfo> _childStatInfos;

    //public int gauge_sante;
    //public int gauge_moral;

    //public int gauge_appetit;
    //public int gauge_hygiene;
    //public int gauge_divertissement;
    //public int gauge_confort;
    //public int gauge_vessie;


    //public int menage;
    //public int obeissance;
    //public int beaute;
    //public int education;
    //public int physique;
    //public int age;

    public class ChildStatInfo
    {
        public ChildStatID childStatID;
        public float currentValue;
    }

}
