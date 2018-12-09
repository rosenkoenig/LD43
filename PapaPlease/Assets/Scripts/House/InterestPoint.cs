using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterestPointModification { ON_COMPLETED, OVERTIME_EMPTY, OVERTIME_FULL }
public class InterestPoint : MonoBehaviour
{

    [UnityEngine.Serialization.FormerlySerializedAs("ipName")]
    public string playerActivityName = "";
    public string logActivityName = "";
    public string logName = "";
    public Transform pivotPoint;
    [UnityEngine.Serialization.FormerlySerializedAs("type")]
    public IPType iPtype;
    public Activity activity;
    public bool onlyUsableByChild = false;
    public bool onlyUsableByPlayer = false;
    public bool destroyOnComplete = false;

    [SerializeField]
    Texture[] completionTextures = null;

    [SerializeField]
    Renderer textureTarget = null;

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
        if(destroyOnComplete)
            activity.OnEnd += DestroyThis;

        if (_useActivityProgressInfo)
        {
            _activityProgressInfo = Instantiate(GameMaster.Instance.uIMaster.GetActivityProgressInfoRef, _activityProgressInfoPos.position, _activityProgressInfoPos.rotation,
                _activityProgressInfoPos) as ActivityProgressInfo;
            _activityProgressInfo.gameObject.SetActive(false);
        }
        activity.SetInheritedIPType(iPtype);
    }

    void DestroyThis ()
    {
        Destroy(gameObject);
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
        if (onlyUsableByChild && playerInteractor != null)
        {
            return false;
        }

        if (playerInteractor != null && _activityProgressInfo != null)
        {
            _activityProgressInfo.transform.position = playerInteractor.GetPlayerHeadBehaviour.GetCamera.transform.position +
                playerInteractor.GetPlayerHeadBehaviour.GetCamera.transform.forward * _activityProgressInfo.GetPlayerHeadDistance.z +
                playerInteractor.GetPlayerHeadBehaviour.GetCamera.transform.up * _activityProgressInfo.GetPlayerHeadDistance.y;
            _activityProgressInfo.transform.LookAt(playerInteractor.GetPlayerHeadBehaviour.GetCamera.transform.position +
                playerInteractor.GetPlayerHeadBehaviour.GetCamera.transform.forward * _activityProgressInfo.GetPlayerHeadDistance.z * 2);
            _activityProgressInfo.SetScaleForPlayer();

                //new Vector3(
                //    playerInteractor.transform.position.x,
                //    _activityProgressInfo.transform.position.y,
                //    playerInteractor.transform.position.z) +
                //new Vector3(
                //    (_activityProgressInfo.transform.position.x - playerInteractor.transform.position.x) * 2,
                //    0,
                //    (_activityProgressInfo.transform.position.z - playerInteractor.transform.position.z) * 2));
        }
        else
        {
            if(_activityProgressInfo != null && _activityProgressInfoPos != null)
            {
                _activityProgressInfo.transform.position = _activityProgressInfoPos.position;
                _activityProgressInfo.transform.eulerAngles = _activityProgressInfoPos.eulerAngles;
                _activityProgressInfo.SetScaleForChild();
            }
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
        if (GameMaster.Instance.gf.GetGameState == GameState.DAY)
        {
            if (activity.State < ActivityState.COMPLETE)
                GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeWAITING, true);
            else
                GameMaster.Instance.vm.ApplyGlobalModifier(globalStatsModificator_OverTimeCOMPLETED, true);
        }

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

        //only for painting
        if (textureTarget != null)
        {
            int texId = Mathf.FloorToInt(activity.GetCompletionRatio * (float)(completionTextures.Length-1));
            //Debug.Log(texId);

            Material mat = textureTarget.materials[1];
            mat.mainTexture = completionTextures[texId];
            textureTarget.materials[1] = mat;
        }
    }
}
