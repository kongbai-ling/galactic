using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TaskData_SO", menuName = "TaskData_SO")]
public class TaskData_SO : ScriptableObject
{
    public List<TaskData> taskList;
}
[System.Serializable]
public class TaskData {
    [Header("任务ID")]
    public int TaskID;
    [Header("任务是否触发")]
    public bool TaskState;
    [Header("任务类型")]
    public string TaskType;
    [Header("任务名称")]
    public string TaskName;
    [Header("任务描述")]
    public string TaskText;
    [Header("任务目标类型")]
    public int  TaskEnemyesType;
    [Header("现在任务击杀目标敌人数量")]
    public int CurrnetTaskEnemyeNumber;
    [Header("任务结束敌人数量")]
    public int TaskoverNumber;
    [Header("TaskOverEnd")]
    [Header("任务是否完成")]
    public bool TaskOver;
    [Header("奖励")]
    public string TaskReward;


}
