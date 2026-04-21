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
    [Header("浠诲姟ID")]
    public int TaskID;
    [Header("浠诲姟鐘舵€")]
    public bool TaskState;
    [Header("浠诲姟绫诲瀷")]
    public string TaskType;
    [Header("浠诲姟鍚嶇О")]
    public string TaskName;
    [Header("浠诲姟绠€浠")]
    public string TaskText;
    [Header("浠诲姟瀹屾垚鏉′欢")]
    [Header("浠诲姟瀹屾垚鎬墿鐨勭被鍨")]
    public int  TaskEnemyesType;
    [Header("浠诲姟鐜板湪Kill鎬墿鏁伴噺")]
    public int CurrnetTaskEnemyeNumber;
    [Header("浠诲姟瀹屾垚鎬墿鏁伴噺")]
    public int TaskoverNumber;
    [Header("TaskOverEnd")]
    [Header("浠诲姟鏄惁瀹屾垚")]
    public bool TaskOver;
    [Header("浠诲姟瀹屾垚濂栧姳")]
    public string TaskReward;


}
