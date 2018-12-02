using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(InterestPoint))]
public class InterestPointEditor : Editor
{

    InterestPoint targetInterestPoint;

    SerializedObject serializedObj_globalStatsModificator_OnCompleted;
    SerializedObject serializedObj_globalStatsModificator_OverTimeEmpty;
    SerializedObject serializedObj_globalStatsModificator_OverTimeFull;
    //ChildStatsModificatorContainer statsModificator_OnCompleted;
    //ChildStatsModificatorContainer statsModificator_OverTimeEmpty;
    //ChildStatsModificatorContainer statsModificator_OverTimeFull;

    private ReorderableList _p_statsModificator_OnCompleted_ReordList;
    private ReorderableList _p_statsModificator_OverTimeEmpty_ReordList;
    private ReorderableList _p_statsModificator_OverTimeFull_ReordList;
    
    private void OnValidate() { InitAndRefresh(); }

    private void OnEnable()
    {
        InitAndRefresh();
    }

    private void InitAndRefresh()
    {
        targetInterestPoint = target as InterestPoint;

        if (targetInterestPoint.globalStatsModificator_OnCompleted != null)
        {
            serializedObj_globalStatsModificator_OnCompleted = new SerializedObject(targetInterestPoint.globalStatsModificator_OnCompleted);
            _p_statsModificator_OnCompleted_ReordList = InitReordList(_p_statsModificator_OnCompleted_ReordList, serializedObj_globalStatsModificator_OnCompleted, "ON COMPLETED");
        }
        else
        {
            serializedObj_globalStatsModificator_OnCompleted = null;
            _p_statsModificator_OnCompleted_ReordList = null;
        }

        if (targetInterestPoint.globalStatsModificator_OverTimeCOMPLETED != null)
        {
            serializedObj_globalStatsModificator_OverTimeEmpty = new SerializedObject(targetInterestPoint.globalStatsModificator_OverTimeCOMPLETED);
            _p_statsModificator_OverTimeEmpty_ReordList = InitReordList(_p_statsModificator_OverTimeEmpty_ReordList, serializedObj_globalStatsModificator_OverTimeEmpty, "OverTime EMPTY");
        }

        if (targetInterestPoint.globalStatsModificator_OverTimeWAITING != null)
        {
            serializedObj_globalStatsModificator_OverTimeFull = new SerializedObject(targetInterestPoint.globalStatsModificator_OverTimeWAITING);
            _p_statsModificator_OverTimeFull_ReordList = InitReordList(_p_statsModificator_OverTimeFull_ReordList, serializedObj_globalStatsModificator_OverTimeFull, "OverTime FULL");
        }
    }

    ReorderableList InitReordList(ReorderableList reordListToInit, SerializedObject serializedObjectRef, string headerValue)
    {
        reordListToInit = new ReorderableList(serializedObjectRef,
                serializedObjectRef.FindProperty("_childStatsModificator").FindPropertyRelative("statsModifiers"),
                                           true, true, true, true);
        reordListToInit.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, headerValue); };
        reordListToInit.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = reordListToInit.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, true);
            };

        reordListToInit.elementHeightCallback = (int index) =>
        {
            return 70;
        };

        return reordListToInit;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        serializedObject.Update();

        if (serializedObj_globalStatsModificator_OnCompleted != null)
            serializedObj_globalStatsModificator_OnCompleted.Update();

        if (serializedObj_globalStatsModificator_OverTimeEmpty != null)
            serializedObj_globalStatsModificator_OverTimeEmpty.Update();

        if (serializedObj_globalStatsModificator_OverTimeFull != null)
            serializedObj_globalStatsModificator_OverTimeFull.Update();

        if (GUILayout.Button("Refresh"))
            InitAndRefresh();

        if (_p_statsModificator_OnCompleted_ReordList != null)
            _p_statsModificator_OnCompleted_ReordList.DoLayoutList();
        if (_p_statsModificator_OverTimeEmpty_ReordList != null)
            _p_statsModificator_OverTimeEmpty_ReordList.DoLayoutList();
        if (_p_statsModificator_OverTimeFull_ReordList != null)
            _p_statsModificator_OverTimeFull_ReordList.DoLayoutList();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed stats modificator");

            if (targetInterestPoint.globalStatsModificator_OnCompleted != null)
                Undo.RecordObject(targetInterestPoint.globalStatsModificator_OnCompleted, "Changed stats modificator");

            if (targetInterestPoint.globalStatsModificator_OverTimeCOMPLETED != null)
                Undo.RecordObject(targetInterestPoint.globalStatsModificator_OverTimeCOMPLETED, "Changed stats modificator");

            if (targetInterestPoint.globalStatsModificator_OverTimeWAITING != null)
                Undo.RecordObject(targetInterestPoint.globalStatsModificator_OverTimeWAITING, "Changed stats modificator");

            if (serializedObj_globalStatsModificator_OnCompleted != null)
                serializedObj_globalStatsModificator_OnCompleted.ApplyModifiedProperties();

            if (serializedObj_globalStatsModificator_OverTimeEmpty != null)
                serializedObj_globalStatsModificator_OverTimeEmpty.ApplyModifiedProperties();

            if (serializedObj_globalStatsModificator_OverTimeFull != null)
                serializedObj_globalStatsModificator_OverTimeFull.ApplyModifiedProperties();

            serializedObject.ApplyModifiedProperties();
            InitAndRefresh();
            Repaint();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
