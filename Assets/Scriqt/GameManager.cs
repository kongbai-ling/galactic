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
    [SerializeField]private float DefenseLv;
    public float Defenselv { get { return DefenseLv; } set { DefenseLv = value;} }
    [SerializeField]private float PowerLv;
    public float Powerlv { get { return PowerLv; } set { PowerLv = value; } }
    [SerializeField]private float SpeedLv;
    public float Speedlv { get { return SpeedLv; } set { SpeedLv = value; } }
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
            UGUIManager.Instance.EXP = (int)EXP;
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
    //拷贝任务数据
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
    public ItemList ItemFallManager()
    {
        float total = 0;
        foreach (ItemList item in Ltemdata.items)
        {
            total += item.weight;
        }
        total = Random.Range(0, total);
        float cumulative = 0;
        foreach (ItemList itemer in Ltemdata.items)
        {
            cumulative += itemer.weight;
            if(total <= cumulative)
            {
                return itemer;
            }
        }
        return null;
    }
}
