using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterestPointModification { ON_COMPLETED, OVERTIME_EMPTY, OVERTIME_FULL }
public class InterestPoint : MonoBehaviour
{

    public string ipName = "";
    public Transform pivotPoint;
    [UnityEngine.Serialization.FormerlySerializedAs("type")]
    public IPType iPtype;
    public Activity activity;
    public bool onlyUsableByChild = false;

    [Header("Activity progress info")]

    [SerializeField] bool _useActivityProgressInfo;
    [SerializeField] Transform _activityProgressInfoPos;

    [Header("Global stats modificators")]

    public ChildStatsModificatorContainer globalStatsModificator_OnCompleted;
    [UnityEngine.Serialization.FormerlySerializedAs("globalStatsModificator_OverTimeEmpty")]
    public ChildStatsModificatorContainer globalStatsModificator_OverTimeCOMPLETED;
    [UnityEngine.Serialization.FormerlySerializedAs("globalStatsModificator_OverTimeFull")]
    public ChildStatsModificatorContainer globalStatsModificator_OverTimeWAITING;

    ActivityProgressInfo _activityProgressInfo;

    public ActivityState GetActivityState { get { return activity.State; } }

    void Start()
    {
        if (_useActivityProgressInfo)
        {
            _activityProgressInfo = Instantiate(GameMaster.Instance.uIMaster.GetActivityProgressInfoRef, _activityProgressInfoPos.position, _activityProgressInfoPos.rotation,
                _activityProgressInfoPos) as ActivityProgressInfo;
            _activityProgressInfo.gameObject.SetActive(false);
        }
    }

    public void MakeResetActivity()
    {
        activity.MakeResetActivity();
    }

    public void TryMakeGlobalModification(InterestPointModification interestPointModification)
    {
        switch (interestPointModification)
        {
            case InterestPointModification.ON_COMPLETED:
                if (globalStatsModificator_OnCompleted != null)
                    GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OnCompleted);
                break;
            case InterestPointModification.OVERTIME_EMPTY:
                if (globalStatsModificator_OverTimeCOMPLETED != null)
                    GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeCOMPLETED);
                break;
            case InterestPointModification.OVERTIME_FULL:
                if (globalStatsModificator_OverTimeWAITING != null)
                    GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeWAITING);
                break;
        }
    }

    public bool Interact(Character character)
    {
        PlayerBehaviour playerInteractor = character.GetComponent<PlayerBehaviour>();
        if (onlyUsableByChild && playerInteractor == null)
        {
            return false;
        }

        if (playerInteractor != null)
        {
            _activityProgressInfo.transform.LookAt(
                new Vector3(
                    playerInteractor.transform.position.x,
                    _activityProgressInfo.transform.position.y,
                    playerInteractor.transform.position.z) +
                new Vector3(
                    (_activityProgressInfo.transform.position.x - playerInteractor.transform.position.x) * 2,
                    0,
                    (_activityProgressInfo.transform.position.z - playerInteractor.transform.position.z) * 2));
        }

        bool interacts = activity.IsAvailable(character);
        if (interacts)
        {
            activity.Begin(character);
            character.currentActivity = activity;
        }

        return interacts;
    }

    void Update()
    {
        if (activity.State < ActivityState.COMPLETE)
            GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeWAITING, true);
        else
            GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeCOMPLETED, true);

        activity.Update();
        if (_activityProgressInfo != null)
        {
            if (activity.isRunning)
            {
                if (_activityProgressInfo.gameObject.activeSelf == false)
                    _activityProgressInfo.gameObject.SetActive(true);

                _activityProgressInfo.Refresh(activity.GetCompletionRatio);
            }
            else
            {
                if (_activityProgressInfo.gameObject.activeSelf)
                    _activityProgressInfo.gameObject.SetActive(false);
            }
        }
    }
}
