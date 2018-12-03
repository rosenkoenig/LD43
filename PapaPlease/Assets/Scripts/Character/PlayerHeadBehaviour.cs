using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadBehaviour : MonoBehaviour {

    [SerializeField] Rigidbody _rigidB;

    [SerializeField] Camera _cam;

    [SerializeField] Vector2 _camRotateSpeed;
    [SerializeField] float _camRotateMaxVerti;
    [SerializeField] float _camRotateMinVertiUp;
    
    public Camera GetCamera { get { return _cam; } }

    //public Vector2 GetCamRotateSpeed { get { return _camRotateSpeed; } }
    //public float GetCamRotateMaxVerti { get { return _camRotateMaxVerti; } }
    //public float GetCamRotateMinVertiUp { get { return _camRotateMinVertiUp; } }

    bool isFrozen = false;
    Vector3 camInitPos;
    public float curCamVERTIDebug;
    void Awake()
    {
        camInitPos = _cam.transform.localPosition;
    }

    public void RotateHead(Vector2 rotateAxis)
    {

        curCamVERTIDebug = _cam.transform.localEulerAngles.x;

        if (isFrozen)
            return;

        float rotateHori = transform.localEulerAngles.y + rotateAxis.x * _camRotateSpeed.x * Time.fixedDeltaTime;

        _rigidB.MoveRotation(Quaternion.Euler(new Vector3(transform.localEulerAngles.x, rotateHori, transform.localEulerAngles.z)));

        float rotateVerti = _cam.transform.localEulerAngles.x + rotateAxis.y * _camRotateSpeed.y * Time.fixedDeltaTime;
        if (rotateVerti > _camRotateMaxVerti && rotateVerti < 175)
        {
            _cam.transform.localEulerAngles = new Vector3(_camRotateMaxVerti, _cam.transform.localEulerAngles.y, _cam.transform.localEulerAngles.z);
        }
        else if (rotateVerti > 180 && rotateVerti < _camRotateMinVertiUp)
        {
            _cam.transform.localEulerAngles = new Vector3(_camRotateMinVertiUp, _cam.transform.localEulerAngles.y, _cam.transform.localEulerAngles.z);
        }
        else
            _cam.transform.localEulerAngles = new Vector3(rotateVerti, _cam.transform.localEulerAngles.y, _cam.transform.localEulerAngles.z);
    }

    public void LookAtTransform(Transform targ)
    {
        Vector3 dir = (targ.position - _cam.transform.position).normalized;
        _rigidB.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        _cam.transform.rotation = Quaternion.LookRotation(new Vector3(_cam.transform.forward.x, dir.y, _cam.transform.forward.z));
    }

    public void SetFreezeHeadControl(bool v)
    {
        isFrozen = v;
    }

}
