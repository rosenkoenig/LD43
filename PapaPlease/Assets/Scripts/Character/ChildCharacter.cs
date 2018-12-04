using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum ChildAIState { WAITING, ROAMING, MOVING_TO_ACTIVITY, IN_ACTIVITY, AT_TABLE, IN_MISSION, DEAD }
public class ChildCharacter : Character {
    public HouseMaster hm = null;

    [SerializeField]
    NavMeshAgent navAgent;
    
    InterestPoint currentInterestPoint;
    InterestPoint lastInterestPoint;

    public string childName = "";
    public bool isMale = false;

    ChildAIState state = ChildAIState.ROAMING;
    public ChildAIState ChildState { get { return state; } }

    [SerializeField]
    Vector2 inactivityDurationRange;
    float currentInactivityDuration = 0f;
    float stateBeginTime = 0f;


    [SerializeField]
    float waitingDistance = 2f;

    [SerializeField]
    IPType IpTypeFun = null;

    [SerializeField]
    float slapFallDownDistance = 1f;
    [SerializeField]
    float delayForHSlapHit = 0.3f;

    [SerializeField]
    float slapFallDownSpeed = 6f;

    [SerializeField]
    float slapKODuration = 1f;

    [SerializeField]
    AnimationCurve slapFallDownCurve;

    bool isFrozen = false;
    bool isSlaped = false;

    private void Awake()
    {
        statsContainer.Init();
    }

    [SerializeField]
    Vector2 timeBetweenAnger = new Vector2(1f, 2f);

    [SerializeField]
    float timeBetweenAngerIncPerObeissance = 1f;

    float nextAngerTime = 0f;
    float lastAngerTime = 0f;

    bool isDoingAnger = false;

    [SerializeField]
    ChildStatID obeissanceStatID = null;

    [SerializeField]
    GameObject isInActivityFeedback = null;

    [SerializeField]
    GameObject[] possibleMaleHaircuts, possibleFemaleHaircurts, possibleConsitution;

    [SerializeField]
    Material[] pantMaterials, sweaterMaterials, shoesMaterial, hairMaterial;

    [SerializeField]
    Material baseSkinMaterial;

    [SerializeField]
    InterestPoint pipiIPPrefab = null;

    [SerializeField]
    ChildStatID vessieID = null;

    [SerializeField]
    ChildStatsModificator onPipiModificator = null;

    [SerializeField]
    IPType vessieIPType = null;

    // Use this for initialization
    void Start () {
        
	}

    public void Init ()
    {

        InitSkins();

        int hcIdx = Random.Range(0, isMale ? possibleMaleHaircuts.Length : possibleFemaleHaircurts.Length);
        GameObject hc = isMale ? possibleMaleHaircuts[hcIdx] : possibleFemaleHaircurts[hcIdx];
        hc.SetActive(true);

        int cIdx = Random.Range(0, possibleConsitution.Length);
        possibleConsitution[cIdx].SetActive(true);

        Vector3 ls = possibleConsitution[cIdx].transform.localScale;
        possibleConsitution[cIdx].transform.localScale = new Vector3(ls.x, Random.Range(.8f,1f), ls.z);

        SkinnedMeshRenderer renderer = possibleConsitution[cIdx].GetComponent<SkinnedMeshRenderer>();
        Material[] newMaterials = renderer.materials;

        //enum materialID { Skin, Sweater, Pant, Shoes }
        newMaterials[0] = baseSkinMaterial;

        int randMatIdx = Random.Range(0, sweaterMaterials.Length);
        newMaterials[1] = sweaterMaterials[randMatIdx];

        randMatIdx = Random.Range(0, pantMaterials.Length);
        newMaterials[2] = pantMaterials[randMatIdx];

        randMatIdx = Random.Range(0, shoesMaterial.Length);
        newMaterials[3] = shoesMaterial[randMatIdx];

        renderer.materials = newMaterials;
        

        randMatIdx = Random.Range(0, hairMaterial.Length);
        if(randMatIdx == 2) randMatIdx = Random.Range(0, hairMaterial.Length);
        hc.GetComponent<Renderer>().material = hairMaterial[randMatIdx];

        isInActivityFeedback.SetActive(false);
    }

    void InitSkins ()
    {
        foreach(GameObject hc in possibleFemaleHaircurts)
        {
            hc.SetActive(false);
        }

        foreach (GameObject hc in possibleMaleHaircuts)
        {
            hc.SetActive(false);
        }

        foreach (GameObject hc in possibleConsitution)
        {
            hc.SetActive(false);
        }
    }
	
	// Update is called once per frame
    void Update ()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        extraRotation();
        UpdateStates();
    }

    public void Freeze(bool state)
    {
        Debug.Log("freeze set to " + state);
        if (state)
        {
            isFrozen = true;
            if(navAgent.isActiveAndEnabled)
                navAgent.isStopped = true;
        }
        else
        {
            isFrozen = false;

            if (navAgent.isActiveAndEnabled)
                navAgent.isStopped = false;
        }
    }

    public void GiveOrder (IPType ipType)
    {
        if (isDoingAnger || state == ChildAIState.AT_TABLE) return;

        SetCurrentInterestPoint(hm.GetRandomInterestPoint(ipType, lastInterestPoint));
        if(!isSlaped)
            Freeze(false);
    }

    void SetCurrentInterestPoint(IPType ipType)
    {
        hm.GetRandomInterestPoint(ipType, lastInterestPoint);
    }
    void SetCurrentInterestPoint(InterestPoint interestPoint)
    {
        currentInterestPoint = interestPoint;
        if (currentInterestPoint)
        {
            Debug.Log(childName + "has received a new IP");
            GameMaster.Instance.AddLog(childName + " goes to " + currentInterestPoint.playerActivityName);
            SetState(ChildAIState.MOVING_TO_ACTIVITY, true);
        }
        else
        {
            Debug.LogWarning(childName + "has received a null IP");
            SetState(ChildAIState.WAITING);
        }
            
    }

    Transform curChairTransform = null;
    public void SetOnTable(Transform chairTransform)
    {
        curChairTransform = chairTransform;
        SetState(ChildAIState.AT_TABLE);
    }

    public void LeaveTable ()
    {
        SetState(ChildAIState.WAITING);
    }

    public void AttributeToMission ()
    {
        GameMaster.Instance.vm.UnsubscribeFromActiveChilds(this);
        SetState(ChildAIState.IN_MISSION);
    }

    public string GetStateName ()
    {
        if (isDoingAnger) return "Is Angry !";
        if (isSlaped) return "Is Recovering";

        switch(state)
        {
            case ChildAIState.IN_ACTIVITY:
                return currentInterestPoint.logActivityName;
                break;
            case ChildAIState.DEAD:
                return "Is Dead";
                break;
            case ChildAIState.AT_TABLE:
                return wasEating ? "Is Eating" : isDoingAnger ? "Is Angry !" : "Is Waiting for Food";
            case ChildAIState.MOVING_TO_ACTIVITY:
                return "Is moving to " + currentInterestPoint.logName;
                break;
            case ChildAIState.ROAMING:
                return "Does pretty Nothing";
            case ChildAIState.WAITING:
                return "Probably thinks about your parenthood";
        }

        return "";
    }

    #region Slap
    public void IsSlapped ()
    {
        if (state == ChildAIState.DEAD) return;

        Freeze(true);
        isSlaped = true;
        if (waitAndApplySlapHitCoroutine != null) StopCoroutine(waitAndApplySlapHitCoroutine);
        waitAndApplySlapHitCoroutine = StartCoroutine(waitAndApplySlapHit());
        if(state != ChildAIState.AT_TABLE)
            LaunchSlapAnim();

        StopPotentialAnger();

        if (state == ChildAIState.IN_ACTIVITY)
        {
            SetState(ChildAIState.WAITING);
        }
        

    }

    Coroutine waitAndApplySlapHitCoroutine = null;
    IEnumerator waitAndApplySlapHit ()
    {
        yield return new WaitForSeconds(delayForHSlapHit);

        PlaySoundEvent(OnSlapEvent);

        float animFactor = 0f;

        if(state != ChildAIState.AT_TABLE)
        {
            Vector3 targetPos = GetHitPosition();
            Vector3 startPos = transform.position;

            while (animFactor < 1f)
            {
                animFactor += Time.deltaTime * slapFallDownSpeed;

                Vector3 pos = Vector3.Lerp(startPos, targetPos, slapFallDownCurve.Evaluate(animFactor));

                transform.position = pos;

                yield return new WaitForEndOfFrame();
            }

            transform.forward = GameMaster.Instance.player.transform.position - transform.position;
        }

        yield return new WaitForSeconds(slapKODuration);

        SetNextAngerTime();
        isSlaped = false;
        Freeze(false);
    }

    Vector3 GetHitPosition ()
    {
        Vector3 fallDownDest = transform.position + (transform.position - GameMaster.Instance.player.transform.position) * slapFallDownDistance;

        NavMeshHit hit;
        NavMesh.SamplePosition(fallDownDest, out hit, waitingDistance, 1);
        Vector3 finalPosition = hit.position;

        return finalPosition;
    }
    #endregion

    #region PathFinding


    void MoveTo (Vector3 destination)
    {
        navAgent.SetDestination(destination);
        SetIsWalking(true);
    }
    void MoveTo (Transform transform)
    {
        MoveTo(transform.position);
    }
    #endregion

    #region Pipi
    bool autoPipiFailed = false;
    void CheckPipi ()
    {
        if (state >= ChildAIState.IN_ACTIVITY || GameMaster.Instance.gf.GetGameState != GameState.DAY) return;
        if (statsContainer.GetAChildStatValue(vessieID) <= vessieID.MinValue)
        {
            DoPipi();
        }
        else if (statsContainer.GetAChildStatValueRatio(vessieID) <= 0.25f)
        {
            if (autoPipiFailed == false && Random.Range(0f, 1f) < statsContainer.GetAChildStatValueRatio(obeissanceStatID))
            {
                InterestPoint ip = GameMaster.Instance.hm.GetRandomInterestPoint(vessieIPType);
                if (ip)
                    SetCurrentInterestPoint(ip);
            }
            else
                autoPipiFailed = true;
        }
        else
            autoPipiFailed = false;
    }

    void DoPipi ()
    {
        SpawnPipi();
        onPipiModificator.TryModifyStats(this, false);
        GameMaster.Instance.AddLog(childName + "has peed on the floor");
    }

    void SpawnPipi ()
    {
        if (pipiIPPrefab == null) return;
        GameObject inst = GameObject.Instantiate(pipiIPPrefab.gameObject, transform.position, transform.rotation);

        GameMaster.Instance.hm.SubscribeInterestPoint(inst.GetComponent<InterestPoint>());
    }
    #endregion

    #region State Management
    void SetState (ChildAIState newState)
    {
        SetState(newState, false);
    }
    void SetState (ChildAIState newState, bool force)
    {
        if (force || state != newState)
        {
            ChildAIState oldState = state;
            state = newState;
            OnStateChange(oldState, newState);
        }
    }

    void OnStateChange (ChildAIState oldState, ChildAIState newState)
    {
        currentInactivityDuration = Mathf.Lerp(inactivityDurationRange.x, inactivityDurationRange.y, Random.Range(0f, 1f));
        stateBeginTime = Time.time;

        Debug.Log(childName + " changes state for : " + newState, this);

        switch(oldState)
        {
            case ChildAIState.WAITING:
                if (waitForIdleCoroutine != null) StopCoroutine(waitForIdleCoroutine);
                break;
            case ChildAIState.MOVING_TO_ACTIVITY:
                if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
                if (waitForIdleCoroutine != null) StopCoroutine(waitForIdleCoroutine);
                break;
            case ChildAIState.IN_ACTIVITY:
                CancelActivity();
                break;
            case ChildAIState.IN_MISSION:
                EndInMission();
                break;
            case ChildAIState.AT_TABLE:
                EndAtTable();
                break;
        }

        switch(newState)
        {
            case ChildAIState.WAITING:
                StartWaiting();
                break;
            case ChildAIState.ROAMING:
                StartRoaming();
                break;
            case ChildAIState.MOVING_TO_ACTIVITY:
                StartMovingToActivity();
                break;
            case ChildAIState.IN_ACTIVITY:
                StartInActivity();
                break;
            case ChildAIState.AT_TABLE:
                StartAtTable();
                break;
            case ChildAIState.IN_MISSION:
                StartInMission();
                break;
            case ChildAIState.DEAD:
                StartDeath();
                break;
        }
    }

    void UpdateStates()
    {
        if(statsContainer.GetAChildStatValueRatio(GameMaster.Instance.healthStat) <= 0)
        {
            SetState(ChildAIState.DEAD);
        }

        CheckIsWalking();
        CheckPipi();

        switch (state)
        {
            case ChildAIState.WAITING:
                UpdateWaiting();
                break;
            case ChildAIState.ROAMING:
                UpdateRoaming();
                break;
            case ChildAIState.MOVING_TO_ACTIVITY:
                UpdateMovingToActivity();
                break;
            case ChildAIState.IN_ACTIVITY:
                UpdateInActivity();
                break;
            case ChildAIState.AT_TABLE:
                UpdateAtTable();
                break;
            case ChildAIState.IN_MISSION:
                UpdateInMission();
                break;
            case ChildAIState.DEAD:
                UpdateDeath();
                break;
        }
    }


    /// <summary>
    /// WAITING STATE WAITING
    /// </summary>
    void StartWaiting  ()
    {
        StartWaitIdleCoroutine();
    }

    void StartWaitIdleCoroutine ()
    {
        if (waitForIdleCoroutine != null) StopCoroutine(waitForIdleCoroutine);
        waitForIdleCoroutine = StartCoroutine(waitForOutOfAnimToBeEnded());
    }

    Coroutine waitForIdleCoroutine = null;
    IEnumerator waitForOutOfAnimToBeEnded()
    {
        yield return new WaitForEndOfFrame();
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)
        {
            yield return new WaitForEndOfFrame();
        }

        switch (state)
        {
            case ChildAIState.WAITING:
                RealStartWaiting();
                break;
            case ChildAIState.MOVING_TO_ACTIVITY:
                RealStartMovingToActivity();
                break;
        }
    }

    void RealStartWaiting ()
    {
        Vector3 waitingDestination = transform.position + Random.insideUnitSphere * waitingDistance;

        NavMeshHit hit;
        NavMesh.SamplePosition(waitingDestination, out hit, waitingDistance, 1);
        Vector3 finalPosition = hit.position;

        MoveTo(finalPosition);
    }

    void UpdateWaiting()
    {
        if (Time.time - stateBeginTime >= currentInactivityDuration && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true)
        {
            //wait a little time before starting a new activity
            SetState(ChildAIState.ROAMING);
        }
    }

    /// <summary>
    /// ROAMING STATE ROAMING
    /// </summary>
    void StartRoaming ()
    {
        SetCurrentInterestPoint(hm.GetRandomInterestPoint(IpTypeFun, lastInterestPoint));
    }

    void UpdateRoaming()
    {



    }

    /// <summary>
    /// MOVING TO ACTIVITY STATE MOVING TO ACTIVITY
    /// </summary>
    void StartMovingToActivity ()
    {
        SetNextAngerTime();
        StartWaitIdleCoroutine();
    }

    void RealStartMovingToActivity ()
    {
        lastAngerTime = Time.time;
        SetNextAngerTime();
        MoveTo(currentInterestPoint.pivotPoint);
    }

    void UpdateMovingToActivity()
    {
        //Debug.Log("remaining distance = " + navAgent.remainingDistance);
        // MoveAlongPath();

        if(currentInterestPoint && currentInterestPoint.iPtype != IpTypeFun)
            CheckAnger();

        if (!isDoingAnger && !isFrozen && Vector3.Distance(transform.position, currentInterestPoint.transform.position) <= 1f)
        {
            //has reached IP
            HasReachedIP();
        }

    }

    void SetNextAngerTime ()
    {
        float rand = Random.Range(0f, 1f);

        //TODO : get la state obeissance
        nextAngerTime = Mathf.Lerp(timeBetweenAnger.x, timeBetweenAnger.y, rand);
        nextAngerTime  += statsContainer.GetAChildStatValue(obeissanceStatID) * timeBetweenAngerIncPerObeissance;
        Debug.Log("next anger time = " + nextAngerTime);
        lastAngerTime = Time.time;
    }

    void CheckAnger()
    {
        if(Time.time - lastAngerTime > nextAngerTime && !isFrozen && !isLerping && !isSlaped)
        {
            StartAnger();
        }
    }

    void StartAnger ()
    {
        Debug.Log("ANGER ANGER ANGER ANGER");
        isDoingAnger = true;

        GameMaster.Instance.AddLog(childName + " is angry and needs a good slap !");

        PlaySoundEvent(OnStartAnger);

        Freeze(true);
        if(state == ChildAIState.AT_TABLE)
        {
            animator.SetBool("IsDoingAnger", isDoingAnger);
            animator.Play("AtTableAnger");
        }
        else
        {
            animator.SetBool("IsDoingAnger", isDoingAnger);
            animator.Play("Anger");
        }

    }

    void UpdateAngerBehaviour ()
    {

    }

    void StopPotentialAnger ()
    {
        if (isDoingAnger)
            PlaySoundEvent(OnStopAnger);

        isDoingAnger = false;

        animator.SetBool("IsDoingAnger", isDoingAnger);
        SetNextAngerTime();
    }

    void HasReachedIP()
    {
        if (currentInterestPoint.activity.IsAvailable(this))
        {
            if (!isLerping)
                StartLerpingToSnap();
        }
        else
        {
            SetState(ChildAIState.WAITING);
        }
    }

    void StartLerpingToSnap()
    {
        if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpCoroutine());
    }

    bool isLerping = false;
    Coroutine lerpCoroutine = null;
    IEnumerator LerpCoroutine()
    {
        float factor = 0f;
        float lerpSpeed = 1f;
        Vector3 startPosition;
        Quaternion startRot;

        factor = 0f;
        startPosition = transform.position;
        startRot = transform.rotation;
        isLerping = true;
        while (factor < 1f)
        {

            factor += Time.deltaTime * lerpSpeed;

            factor = Mathf.Clamp01(factor);

            transform.position = Vector3.Lerp(startPosition, currentInterestPoint.pivotPoint.position, factor);
            transform.rotation = Quaternion.Lerp(startRot, currentInterestPoint.pivotPoint.rotation, factor);
            yield return new WaitForEndOfFrame();
        }

        isLerping = false;
        Debug.Log(childName + " begins activity on " + currentInterestPoint.playerActivityName);
        SetState(ChildAIState.IN_ACTIVITY);
    }


    /// <summary>
    /// IN ACTIVITY STATE IN ACTIVITY
    /// </summary>
    void StartInActivity ()
    {

        GameMaster.Instance.AddLog(childName + " starts " + currentInterestPoint.logActivityName);
        currentInterestPoint.Interact(this);
        isInActivityFeedback.SetActive(true);
    }

    void CancelActivity ()
    {
        if(currentActivity)
        {
            currentActivity.CancelActivity(this);
            currentActivity = null;
            isInActivityFeedback.SetActive(false);
        }
    }

    void UpdateInActivity()
    {
        if (isLerping) return;
        currentInterestPoint.iPtype.TryModifyStats(IPType.StatModificationType.DURING_ACTIVITY, statsContainer);
                
        transform.position = currentInterestPoint.pivotPoint.position;
        transform.rotation = currentInterestPoint.pivotPoint.rotation;
    }

    public override void OnActivityEnds()
    {
        base.OnActivityEnds();
        GameMaster.Instance.AddLog(childName + " ends activity on " + currentInterestPoint.playerActivityName);
        currentActivity = null;

        currentInterestPoint.iPtype.TryModifyStats(IPType.StatModificationType.END_ACTIVITY, statsContainer);
        currentInterestPoint.TryMakeGlobalModification(InterestPointModification.ON_COMPLETED);

        lastInterestPoint = currentInterestPoint;
        currentInterestPoint = null;

        SetState(ChildAIState.WAITING);

        isInActivityFeedback.SetActive(false);
    }

    /// <summary>
    /// AT TABLE STATE AT TABLE
    /// </summary>
    void StartAtTable ()
    {
        navAgent.enabled = false;
        SetNextAngerTime();
        LaunchAtTableAnim();
    }

    bool wasEating = false;
    void UpdateAtTable ()
    {
        CheckAnger();

        transform.position = curChairTransform.position;
        transform.rotation = curChairTransform.rotation;

        bool isEating = myPlate.IsFilled && !isDoingAnger;

        if (isEating && !wasEating)
            PlaySoundEvent(OnStartEating);

        if (!isEating && wasEating)
            PlaySoundEvent(OnStopEating);

        wasEating = isEating;

        animator.SetBool("Eats", isEating);
        if (isEating)
        {
            EatPlate();
        }

    }

    void EndAtTable ()
    {
        navAgent.enabled = true;
        animator.Play("Idle");
    }

    void EatPlate ()
    {
        myPlate.IsBeingEaten(this);
        
    }

    PlateObject myPlate = null;

    public void SetPlate (PlateObject po)
    {
        myPlate = po;
    }

    /// <summary>
    /// IN MISSION STATE IN MISSION
    /// </summary>
    void StartInMission()
    {
        Freeze(true);
        navAgent.enabled = false;
        transform.position = Vector3.forward * 1000f;
    }

    void UpdateInMission()
    {

    }

    void EndInMission ()
    {
        GameMaster.Instance.vm.SubscribeToActiveChilds(this);
    }


    /// <summary>
    /// AT TABLE STATE AT TABLE
    /// </summary>
    void StartDeath ()
    {
        Freeze(true);
        navAgent.enabled = false;
        LaunchDeathAnim();
        PlaySoundEvent(OnDeathEvent);
    }

    void UpdateDeath ()
    {

    }

    #endregion

    Vector3 lastPosition;
    bool isWalking = false;
    bool wasWalking = false;
    void CheckIsWalking()
    {

        isWalking = Vector3.Distance(lastPosition, transform.position) > 0.005f;

        if (isWalking && !wasWalking)
            PlaySoundEvent(OnStartWalking);

        if (!isWalking && wasWalking)
            PlaySoundEvent(OnStopWalking);

        wasWalking = isWalking;

        lastPosition = transform.position;

        SetIsWalking(isWalking);
    }

    #region Animation
    [SerializeField]
    Animator animator;

    void SetIsWalking (bool state)
    {
        animator.SetBool("IsWalking", state);
    }

    public float extraRotationSpeed = 8f;
    void extraRotation()
    {
        if (!isWalking) return;
        Vector3 lookrotation = navAgent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);

    }

    void LaunchSlapAnim ()
    {
        animator.Play("SlapHit");
    }

    public void StartAnimState(string stateName)
    {
       
        animator.Play(stateName);
    }

    void LaunchAtTableAnim()
    {
        animator.Play("AtTableIdle");
    }

    void LaunchDeathAnim ()
    {
        //todo anim death
        animator.Play("Death");
    }
    #endregion

    [Header("Sounds")]
    [SerializeField]
    AK.Wwise.Event OnSlapEvent = null;

    [SerializeField]
    AK.Wwise.Event OnStartAnger = null;
    [SerializeField]
    AK.Wwise.Event OnStopAnger = null;

    [SerializeField]
    AK.Wwise.Event OnStartEating = null;
    [SerializeField]
    AK.Wwise.Event OnStopEating = null;

    [SerializeField]
    AK.Wwise.Event OnStartWalking = null;
    [SerializeField]
    AK.Wwise.Event OnStopWalking = null;

    [SerializeField]
    AK.Wwise.Event OnDeathEvent = null;

    void PlaySoundEvent(AK.Wwise.Event soundEvent)
    {
        soundEvent.Post(gameObject);
    }
}
