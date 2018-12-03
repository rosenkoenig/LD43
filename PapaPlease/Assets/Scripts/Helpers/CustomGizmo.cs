using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGizmo : MonoBehaviour {

    [SerializeField] Color _gizmoColor = Color.blue;
    enum GizmoType { Cube, Sphere, WireCube, WireSphere }
    [SerializeField] GizmoType _gizmoType;
    [SerializeField] Vector3 _posOffset;
    [SerializeField] Vector3 _scale = Vector3.one;

    [SerializeField] bool _onSelectedOnly = false;

    void OnDrawGizmos ()
    {
        if(_onSelectedOnly == false)
            DrawGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        if (_onSelectedOnly)
            DrawGizmos();
    }

    private void DrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        switch (_gizmoType)
        {
            case GizmoType.Cube:
                Gizmos.DrawCube(transform.position + _posOffset, _scale);
                break;
            case GizmoType.Sphere:
                Gizmos.DrawSphere(transform.position + _posOffset, _scale.x);
                break;
            case GizmoType.WireCube:
                Gizmos.DrawWireCube(transform.position + _posOffset, _scale);
                break;
            case GizmoType.WireSphere:
                Gizmos.DrawWireSphere(transform.position + _posOffset, _scale.x);
                break;
        }
    }
}
