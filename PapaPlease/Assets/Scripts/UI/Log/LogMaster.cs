using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogMaster : MonoBehaviour {

    [SerializeField]
    VerticalLayoutGroup logParent = null;

    [SerializeField]
    LogItem logItemPrefab = null;

    


    public void Init ()
    {
    }

    

    public void AddLog (string text)
    {
        LogItem li = CreateLogItem();
        li.SetText(text);
        logParent.CalculateLayoutInputVertical();
    }

    LogItem CreateLogItem ()
    {
        GameObject inst = GameObject.Instantiate(logItemPrefab.gameObject, logParent.transform);

        return inst.GetComponent<LogItem>();
    }
}
