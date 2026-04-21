using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("任务判定逻辑")]
    private string KillEnrmyType;
    public TaskData_SO TaskData;
    public CurrnetTaskData_SO CurrnetTaskData;
    private int _CurrentTaskID;
    public int CurrentTaskID
    {
        get { return _CurrentTaskID; }
        set { _CurrentTaskID = value; }
    }
    private bool IsOver;
    [Header("PlayerAtt")]
    public float EXP;
    [Header("itme")]
    public ItemData Ltemdata;
    

    void Start()
    {
        Instance = this;
        StartCoroutine(TaskCondition());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator TaskCondition()
    {
        while (!IsOver) {
            CheckKliiTask();
            yield return new WaitForSeconds(1f);
        }
       
    }
    public void EXPAdd(float exp)
    {
        EXP = EXP + exp;
        if (EXP >= 1)
        {
            UGUIManager.Instance.EXP += 1;
            UGUIManager.Instance.EXPUGUIManager();
        }
    }
    public void CheckKliiTask()
    {
        foreach(TaskData task in TaskData.taskList)
        {
            if(task.TaskState == true&&task.TaskOver == false&&task.TaskType=="Kill")
            {
                if(task.CurrnetTaskEnemyeNumber >= task.TaskoverNumber)
                {
                    task.TaskOver = true;
                }
            }
        }
    }
    public void KillEnemyeNumber(int EnemyeType)
    {
        foreach (TaskData task in TaskData.taskList)
        {
            if (task.TaskState == true && task.TaskOver == false)
            {
                if (task.TaskEnemyesType == EnemyeType)
                {
                    task.CurrnetTaskEnemyeNumber++;
                    Debug.Log(CurrentTaskID);
                    CopyTaskData(TaskData.taskList[CurrentTaskID], CurrnetTaskData.CurrentTaskList[0]);
                    UGUIManager.Instance.ShowTaskminbox(CurrnetTaskData.CurrentTaskList[0]);
                    
                }
            }
        }
    }
    public void CopyTaskData(TaskData source, CurrnetTaskData target)
    {
        target.TaskID = source.TaskID;
        target.TaskType = source.TaskType;
        target.TaskName = source.TaskName;
        target.TaskText = source.TaskText;
        target.TaskEnemyesType = source.TaskEnemyesType;
        target.CurrnetTaskEnemyeNumber = source.CurrnetTaskEnemyeNumber;
        target.TaskoverNumber = source.TaskoverNumber;
    }
    public void ItemFallManager()
    {
        float random = Random.Range(0, 100);

    }
}
