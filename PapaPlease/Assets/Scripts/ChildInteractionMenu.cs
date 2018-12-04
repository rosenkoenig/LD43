using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildInteractionMenu : MonoBehaviour
{

    [SerializeField] ChildOrderButton _childOrderButtonRef;
    [SerializeField] RectTransform childOrderButtonsParent;
    //[SerializeField] Text _childNameText;

    
    //[SerializeField] ChildStatIDsContainer _childStatIdsContainer;
    //[SerializeField] ChildStatGauge _childStatGaugeRef;
    //[SerializeField] ChildStatGauge _childStatSkillRef;
    //[SerializeField] RectTransform _childStatGaugesParent;

    List<ChildOrderButton> _childOrderButtons;
    List<IPTypeInfo> allIPTypeInfos { get { return GameMaster.Instance.hm.GetAllIPTypeInfos(); } }

    //List<ChildStatGauge> _childStatsGaugesList;

    ChildCharacter curChild;

    private void Awake()
    {
        //_childStatsGaugesList = new List<ChildStatGauge>();

        

        //foreach (var item in _childStatIdsContainer.GetChildStatIDs)
        //{
        //    ChildStatGauge newStatGauge = null;
        //    if (item.IsDisplayedAsGauge)
        //        newStatGauge = Instantiate(_childStatGaugeRef, _childStatGaugesParent);
        //    else
        //        newStatGauge = Instantiate(_childStatSkillRef, _childStatGaugesParent);

        //    newStatGauge.InitGauge(item);
        //    _childStatsGaugesList.Add(newStatGauge);
        //}
    }

    public void SetupMenu(ChildCharacter child)
    {
        curChild = child;
        //_childNameText.text = child.childName;

        ClearOrderButtons();

        _childOrderButtons = new List<ChildOrderButton>();
        foreach (var item in allIPTypeInfos)
        {
            ChildOrderButton childOrderButton = Instantiate(_childOrderButtonRef, childOrderButtonsParent);
            childOrderButton.InitButton(item, MakeGiveOrder);
            _childOrderButtons.Add(childOrderButton);
        }


        RefreshButtons(curChild);
    }

    private void RefreshButtons(ChildCharacter child)
    {
        

        for (int i = 0; i < allIPTypeInfos.Count; i++)
        {
            if (i < _childOrderButtons.Count)
            {
                _childOrderButtons[i].InitButton(allIPTypeInfos[i], MakeGiveOrder);
                _childOrderButtons[i]._button.interactable = allIPTypeInfos[i].isAvailable;
            }
        }
        //foreach (var item in _childStatsGaugesList)
        //{
        //    foreach (var curStatInfo in child.statsContainer._childStatInfos)
        //    {
        //        if (curStatInfo.childStatID == item.GetChildStatID)
        //            item.RefreshGauge(curStatInfo);
        //    }
        //}
    }

    void ClearOrderButtons ()
    {
        if (_childOrderButtons == null) return;

        foreach(ChildOrderButton btn in _childOrderButtons)
        {
            Destroy(btn.gameObject);
        }
    }

    public void MakeHideMenuInteractChild()
    {
        GameMaster.Instance.uIMaster.HideMenuInteractChild();
    }

    public void MakeGiveOrder(IPTypeInfo ipTypeInfo)
    {
        Debug.Log("Give Order " + ipTypeInfo.IPType.name + " to " + curChild.childName);
        curChild.GiveOrder(ipTypeInfo.IPType);
        MakeHideMenuInteractChild();
    }

    private void Update()
    {
        if (curChild != null)
        { 
            RefreshButtons(curChild);
        }
    }
}
