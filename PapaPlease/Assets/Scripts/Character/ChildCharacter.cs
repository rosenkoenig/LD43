using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum ChildAIState { ROAMING, MOVING_TO_ACTIVITY, IN_ACTIVITY }
public class ChildCharacter : Character {
    public HouseMaster hm = null;

    [SerializeField]
    NavMeshAgent navAgent;


    InterestPoint currentInterestPoint;
    InterestPoint lastInterestPoint;

    public string childName = "";
    public bool isMale = false;

    [SerializeField]
    ChildAIState state = ChildAIState.ROAMING;

    [SerializeField]
    Vector2 inactivityDurationRange;
    float currentInactivityDuration = 0f;
    float stateBeginTime = 0f;

    [SerializeField]
    IPType IpTypeFun = null;

	// Use this for initialization
	void Start () {
        SetState(ChildAIState.ROAMING);
	}
	
	// Update is called once per frame
    void Update ()
    {
        UpdateStates();
    }
    

    public void SetCurrentInterestPoint (InterestPoint interestPoint)
    {
        currentInterestPoint = interestPoint;
        if (currentInterestPoint)
            MoveTo(currentInterestPoint.pivotPoint);
    }

    void MoveTo (Vector3 destination)
    {
        navAgent.SetDestination(destination);
       /* _targetPoint = destination;
        CalculateNavMesh();*/
    }
    void MoveTo (Transform transform)
    {
        MoveTo(transform.position);
    }

    #region PathFinding
    /*
    private Vector3 _targetPoint;
    private NavMeshPath _path;
    private bool _hasPath;
    private NavMeshAgent _agent;
    private Queue<Vector3> _cornerQueue;
    private Vector3 _currentDestination;
    private Vector3 _direction;
    private float _currentDistance;
    NavMeshObstacle _obstacle;

    void OnEnable()
    {
        InitVars();
    }

    private void CalculateNavMesh()
    {
        _path = new NavMeshPath();
        _obstacle.enabled = false;
        _agent.enabled = true;
        _agent.CalculatePath(_targetPoint, _path);
        SetupPath(_path);
        _agent.enabled = false;
        _obstacle.enabled = true;
    }

    private void InitVars()
    {
        //_targetPoint = GameObject.Find("EndPoint").transform.position; // Set target point here
        _obstacle = GetComponent<NavMeshObstacle>();
        _agent = GetComponent<NavMeshAgent>();
        _path = new NavMeshPath();
    }

    void SetupPath(NavMeshPath path)
    {
        _cornerQueue = new Queue<Vector3>();
        foreach (var corner in path.corners)
        {
            _cornerQueue.Enqueue(corner);
        }

        GetNextCorner();
        _hasPath = true;
    }

    private void GetNextCorner()
    {
        if (_cornerQueue.Count > 0)
        {
            _currentDestination = _cornerQueue.Dequeue();
            _direction = (_currentDestination - transform.position).normalized;
            _hasPath = true;
        }
        else
        {
            _hasPath = false;
        }
    }

    

    private void MoveAlongPath()
    {
        if (_hasPath)
        {
            _currentDistance = Vector3.Distance(transform.position, _currentDestination);

            if (_currentDistance > 1f)
                transform.position += _direction * navAgent.speed * Time.deltaTime;
            else
                GetNextCorner();
        }
    }*/

    #endregion

    #region State Management
    void SetState (ChildAIState newState)
    {
        if (state != newState)
        {
            ChildAIState oldState = state;
            state = newState;
            OnStateChange(state, newState);
        }
    }

    void OnStateChange (ChildAIState oldState, ChildAIState newState)
    {
        currentInactivityDuration = Mathf.Lerp(inactivityDurationRange.x, inactivityDurationRange.y, Random.Range(0f, 1f));

        Debug.Log("New state : " + newState);

        switch(newState)
        {
            case ChildAIState.ROAMING:
                StartRoaming();
                break;
            case ChildAIState.MOVING_TO_ACTIVITY:
                StartMovingToActivity();
                break;
            case ChildAIState.IN_ACTIVITY:
                StartInActivity();
                break;
        }
    }

    void StartRoaming ()
    {
        stateBeginTime = Time.time;
        lastInterestPoint = currentInterestPoint;
        currentInterestPoint = null;
    }

    void StartMovingToActivity ()
    {

    }

    void StartInActivity ()
    {
        currentInterestPoint.Interact(this);
        
    }

    void UpdateStates ()
    {
        switch(state)
        {
            case ChildAIState.ROAMING:
                UpdateRoaming();
                break;
            case ChildAIState.MOVING_TO_ACTIVITY:
                UpdateMovingToActivity();
                break;
            case ChildAIState.IN_ACTIVITY:
                UpdateInActivity();
                break;
        }
    }

    void UpdateRoaming ()
    {
        if (Time.time - stateBeginTime >= currentInactivityDuration)
        {
            //wait a little time before starting a new activity
            GetRoamingInterestPoint();
            if (currentInterestPoint)
                SetState(ChildAIState.MOVING_TO_ACTIVITY);
            else
                StartRoaming();
        }
    }

    void GetRoamingInterestPoint()
    {
        SetCurrentInterestPoint(hm.GetRandomInterestPoint(IpTypeFun, lastInterestPoint));
    }

    void UpdateMovingToActivity ()
    {
        //Debug.Log("remaining distance = " + navAgent.remainingDistance);
       // MoveAlongPath();

        if (navAgent.remainingDistance <= 1f)
        {
            //has reached IP
            HasReachedIP();
        }
        
    }

    void IsInRangeForSnap ()
    {
        if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpCoroutine());
    }

    float factor = 0f;
    float lerpSpeed = 3f;
    Vector3 startPosition;
    Quaternion startRot;
    Coroutine lerpCoroutine = null;
    IEnumerator LerpCoroutine ()
    {        
        factor = 0f;
        startPosition = transform.position;
        startRot = transform.rotation;
        while (factor < 1f)
        {
            Debug.Log("coroutine");
            factor += Time.deltaTime * lerpSpeed;

            factor = Mathf.Clamp01(factor);

            transform.position = Vector3.Lerp(startPosition, currentInterestPoint.pivotPoint.position, factor);
            transform.rotation = Quaternion.Lerp(startRot, currentInterestPoint.pivotPoint.rotation, factor);
            yield return new WaitForEndOfFrame();
        }
    }

    void HasReachedIP ()
    {
        if(currentInterestPoint.activity.IsAvailable(this))
        {

            Debug.Log("begin");
            SetState(ChildAIState.IN_ACTIVITY);
            IsInRangeForSnap();
        }
        else
        {
            SetState(ChildAIState.ROAMING);
        }
    }

    void UpdateInActivity ()
    {
        
    }

    public override void OnActivityEnds()
    {
        base.OnActivityEnds();
        currentActivity = null;

        Debug.Log("ends");
        SetState(ChildAIState.ROAMING);
    }
    #endregion
}
