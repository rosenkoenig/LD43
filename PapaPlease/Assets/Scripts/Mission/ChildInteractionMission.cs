using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildInteractionMission : MonoBehaviour {

    bool _isDisplayed = false;
	public bool IsDisplayed { get { return _isDisplayed; } }

    [SerializeField]
    ChildMissionButton buttonPrefab = null;

    [SerializeField]
    Transform buttonListParent = null;

     ChildCharacter _child = null;

    List<ChildMissionButton> childMissionBtns = new List<ChildMissionButton>();

    public void Init (ChildCharacter child)
    {
        //GameMaster.Instance.AddLog("Display Mission selection menu");
        _child = child;

        ClearAllButtons();
        List<Mission> availableMission = GameMaster.Instance.mm.GetAllAvailableMissions();

        

        foreach(Mission mission in availableMission)
        {
            CreateButton(mission);
        }

        gameObject.SetActive(true);
        _isDisplayed = true;
    }

    void ClearAllButtons ()
    {
        foreach(ChildMissionButton btn in childMissionBtns)
        {
            Destroy(btn.gameObject);
        }
        childMissionBtns = new List<ChildMissionButton>();
    }

    void CreateButton (Mission mission)
    {
        GameObject inst = GameObject.Instantiate(buttonPrefab.gameObject, buttonListParent);

        ChildMissionButton btn = inst.GetComponent<ChildMissionButton>();

        bool available = mission.RequisitesAreFullFilledFor(_child);

        btn.Init(mission, this, available);
        childMissionBtns.Add(btn);

    }

    public void CloseMenu ()
    {
        gameObject.SetActive(false);
        _isDisplayed = false;
        _child.Freeze(false);
        GameMaster.Instance.uIMaster.HideMenuInteractChild();
    }

    public void OnButtonClick (Mission mission)
    {
        GameMaster.Instance.mm.StartMission(mission, _child);
        CloseMenu();
    }
}
