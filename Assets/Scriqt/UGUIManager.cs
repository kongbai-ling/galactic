using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class UGUIManager : MonoBehaviour
{
    [Header("Dialogbox")]
    public static UGUIManager Instance;
    public GameObject Dialogbox;
    public TMP_Text text;
    public TextAsset TextAsset;
    public string[] Texts;
    private int currentTextIndex;
    public string[] textss;
    [Header("Task")]
    public GameObject CurrentTaskminbox;
    public TMP_Text CurrentTaskContent_min;
    public TMP_Text CurrentTaskname_min;
    public TaskData_SO TaskData;
    public CurrnetTaskData_SO CurrnetTaskData;
    private bool IsOpenTaskbox = false;
    public GameObject Taskbox;
    public GameObject TaskContentbox;
    public GameObject TaskPane;
    private TMP_Text Taskname;
    private TMP_Text TaskContent;
    private Button TaskButton;
    [Header("BackpackMenu")]
    public GameObject BackpackMenu;
    private bool IsOpenBackpackMenu = false;
    public GameObject BackpackContentbox;
    public GameObject BackpackPane;
    public Text PlayerAtt1;
    public Text PlayerAtt2;
    public Text PlayerAtt3;
    [Header("PlayerEXP")]
    public Text EXPText;
    public int EXP;
    [Header("PromptBox")]
    public GameObject PromptBox;
    void Start()
    {
        Instance = this;
        Texts = TextAsset.text.Split('\n');
    }
    // Update is called once per frame
    void Update()
    {
        ShowTaskContentbox();
        ShowBackpackMenu();
    }
    //打开对话框
    public void ShowDialogbox(bool isShow)
    {
        if(Dialogbox.activeSelf == isShow||Dialogbox==null) return;
        Dialogbox.SetActive(isShow);
    }
    //显示文字并开启打字机
    public void ShowText()
    {
        if (currentTextIndex >= Texts.Length) return;
        textss = Texts[currentTextIndex].Split(',');
        text.text = textss[1];
        text.ForceMeshUpdate();
        StartCoroutine(TypeWriter());
        currentTextIndex++;
        if(textss[0] == "#")
        {
            ShowTaskbox(textss[2]);

        }
    }
    //通用显示提示框
    public void PromptBoxShow(string str)
    {
        GameObject go = Instantiate(PromptBox, transform);
        go.transform.Find("PromptText").GetComponent<Text>().text = str;
        go.transform.SetAsLastSibling();
        StartCoroutine(DestroyWithRealtime(go, 0.8f));

        IEnumerator DestroyWithRealtime(GameObject go, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            Destroy(go);
        }
    }
    //显示当前任务
    public void ShowTaskminbox(CurrnetTaskData currnetTaskData)
    {
        CurrentTaskminbox.SetActive(true);
        CurrentTaskname_min.text = currnetTaskData.TaskName;
        CurrentTaskContent_min.text = currnetTaskData.TaskText+currnetTaskData.TaskoverNumber+"/"+currnetTaskData.CurrnetTaskEnemyeNumber;
    }
    //记录追踪当前任务
    public void TrackCurrnetTask(string TaskID)
    {
        int ID= int.Parse(TaskID);
        GameManager.Instance.CurrentTaskID = ID;
        GameManager.Instance.CopyTaskData(TaskData.taskList[ID], CurrnetTaskData.CurrentTaskList[0]);
        ShowTaskminbox(CurrnetTaskData.CurrentTaskList[0]);
    }
   //更新属性等级
    public void ResetAttLvText(int AttID)
    {
        switch (AttID) 
        {
            case 1:
                PlayerAtt1.text = GameManager.Instance.Defenselv.ToString();
                return;
            case 2:
                PlayerAtt2.text = GameManager.Instance.Powerlv.ToString();
                return;
            case 3:
                PlayerAtt3.text = GameManager.Instance.Speedlv.ToString();
                return;
        
        }

    }

    //创建任务栏
    public void ShowTaskbox(string TaskID)
    {
        GameObject go =  Instantiate(TaskPane, TaskContentbox.transform);
        Taskname = go.transform.Find("Taskname").GetComponent<TMP_Text>();
        TaskContent = go.transform.Find("TaskContent").GetComponent<TMP_Text>();
        TaskButton = go.transform.Find("Track").GetComponent<Button>();
        TaskButton.onClick.AddListener(() => TrackCurrnetTask(TaskID));
        Taskname.text = TaskData.taskList[int.Parse(TaskID)].TaskName;
        TaskContent.text = TaskData.taskList[int.Parse(TaskID)].TaskText;
        TaskData.taskList[int.Parse(TaskID)].TaskState = true;
    }
    //打开关闭任务栏
    public void ShowTaskContentbox()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            IsOpenTaskbox = !IsOpenTaskbox;
            if (IsOpenTaskbox)
            {
                TimeManager.Instance.PauseTime(0);
            }
            else
            {
                TimeManager.Instance.PauseTime(1);
            }
            Taskbox.SetActive(IsOpenTaskbox);
        }
    }
    //更新经验值
    public void EXPUGUIManager()
    {
        EXPText.text = "可用点数:"+EXP;
    }
    //打开关闭背包
    public void ShowBackpackMenu()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            IsOpenBackpackMenu = !IsOpenBackpackMenu;
            if (IsOpenBackpackMenu)
            {
                TimeManager.Instance.PauseTime(0);
                
            }
            else
            {
                TimeManager.Instance.PauseTime(1);
            }
            BackpackMenu.SetActive(IsOpenBackpackMenu);
        }
    }
    //背包添加物品
    public void ShowBackpackItem(ItemList iteminformation)
    {
        
        foreach (var item in BackpackContentbox.GetComponentsInChildren<Useitem>())
        {
            if (item.itemList.itemID == iteminformation.itemID)
            {
                item.itemnumber++;
                item.itemnumberText.text = item.itemnumber.ToString();
                return;
            }  
        }
        GameObject go = Instantiate(BackpackPane, BackpackContentbox.transform);
        go.GetComponent<Useitem>().itemImage.sprite = iteminformation.icon;
        go.GetComponent<Useitem>().itemnumber++;
        go.GetComponent<Useitem>().itemnumberText.text = go.GetComponent<Useitem>().itemnumber.ToString();
        go.GetComponent<Useitem>().itemList = iteminformation;
        return;

    }
    //打字机效果
    public IEnumerator TypeWriter()
    {
        TMP_TextInfo info = text.textInfo;
        int toral = info.characterCount;
        bool complete = false;
        int current = 0;
        while (!complete)
        {
            if (current > toral)
            {
                current = toral;
                yield return new WaitForSecondsRealtime(1f);
                complete = true;
            }
            text.maxVisibleCharacters = current;
            current++;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        yield return null;
    }
}
