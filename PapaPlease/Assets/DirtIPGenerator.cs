using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DirtIPGenerator : MonoBehaviour
{

    [SerializeField] InterestPoint _interestPointRef;
    Pool<InterestPoint> _interestPointsPOOL;

    [SerializeField] float _spawnDirtMinDelay;
    [SerializeField] float _spawnDirtMaxDelay;

    [SerializeField] float _spawnDirtChildrenNumberAddedSpeed;
    
    [SerializeField] bool _randomRotation = true;

    List<Transform> _transformSpawnChildren;

    List<SpawnedIPPack> _curSpawnIPPacks;
    float curDelay;

    private void Start()
    {
        _curSpawnIPPacks = new List<SpawnedIPPack>();
        _transformSpawnChildren = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
            _transformSpawnChildren.Add(transform.GetChild(i));

        _interestPointsPOOL = new Pool<InterestPoint>(_interestPointRef, Pool<InterestPoint>.DelGameObjectItemCreate, 6, transform);
        SetNewDelay();
    }

    private void Update()
    {
        if (GameMaster.Instance.gf.GetGameState == GameState.DAY)
        {
            if (curDelay > 0)
                curDelay -= (1 + _spawnDirtChildrenNumberAddedSpeed * GameMaster.Instance.vm.GetChildrenNumber) * Time.deltaTime;
            if (curDelay <= 0)
            {
                SpawnInterestPoint();
                SetNewDelay();
            }

            ManageUnspawn();
        }
    }

    private void ManageUnspawn()
    {
        List<SpawnedIPPack> toRemove = new List<SpawnedIPPack>();
        foreach (var item in _curSpawnIPPacks)
        {
            if (item._spawnedInterestPoint.GetActivityState == ActivityState.COMPLETE)
            {
                item._spawnedInterestPoint.gameObject.SetActive(false);
                toRemove.Add(item);
            }
        }
        foreach (var item in toRemove)
        {
            _curSpawnIPPacks.Remove(item);
        }
    }

    void SpawnInterestPoint()
    {
        List<Transform> freePositions = new List<Transform>();
        foreach (var curTransform in _transformSpawnChildren)
        {
            if (_curSpawnIPPacks.Any(x => x._spawnedPointPosTransform == curTransform) == false)
            {
                freePositions.Add(curTransform);
            }
        }
        if(freePositions.Count > 0)
        {
            Transform t = freePositions[UnityEngine.Random.Range(0, freePositions.Count)];
            SpawnedIPPack newSpawnIPPack = new SpawnedIPPack();
            newSpawnIPPack._spawnedPointPosTransform = t;
            newSpawnIPPack._spawnedInterestPoint = _interestPointsPOOL.BorrowItem();
            newSpawnIPPack._spawnedInterestPoint.transform.position = t.position;
            newSpawnIPPack._spawnedInterestPoint.transform.rotation = t.rotation;
            if (_randomRotation)
            { 
                newSpawnIPPack._spawnedInterestPoint.transform.eulerAngles = new Vector3(
                    newSpawnIPPack._spawnedInterestPoint.transform.eulerAngles.x,
                    UnityEngine.Random.Range(0f, 359f),
                    newSpawnIPPack._spawnedInterestPoint.transform.eulerAngles.z);
            }

            newSpawnIPPack._spawnedInterestPoint.gameObject.SetActive(true);
            newSpawnIPPack._spawnedInterestPoint.MakeResetActivity();

            _curSpawnIPPacks.Add(newSpawnIPPack);
        }
    }

    void SetNewDelay()
    {
        curDelay = UnityEngine.Random.Range(_spawnDirtMinDelay, _spawnDirtMaxDelay);
    }

    class SpawnedIPPack
    {
        public InterestPoint _spawnedInterestPoint;

        public Transform _spawnedPointPosTransform;
    }
}
