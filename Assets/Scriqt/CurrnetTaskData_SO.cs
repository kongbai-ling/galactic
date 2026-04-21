using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CurrentTask_SO")]
public class CurrnetTaskData_SO : ScriptableObject
{
    public List<CurrnetTaskData> CurrentTaskList;
}
[System.Serializable]
public class CurrnetTaskData
{
    [Header("任务ID")]
    public int TaskID;
    [Header("任务类型")]
    public string TaskType;
    [Header("任务名称")]
    public string TaskName;
    [Header("任务简介")]
    public string TaskText;
    [Header("任务完成条件")]
    [Header("任务完成怪物的类型")]
    public int TaskEnemyesType;
    [Header("任务现在Kill怪物数量")]
    public int CurrnetTaskEnemyeNumber;
    [Header("任务完成怪物数量")]
    public int TaskoverNumber;
}
