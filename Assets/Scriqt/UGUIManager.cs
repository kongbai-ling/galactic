using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public void ShowDialogbox(bool isShow)
    {
        if(Dialogbox.activeSelf == isShow||Dialogbox==null) return;
        Dialogbox.SetActive(isShow);
    }
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
    public void PromptBoxShow(string str)
    {
        GameObject go = Instantiate(PromptBox, transform);
        go.transform.Find("PromptText").GetComponent<Text>().text = str;
        go.transform.SetAsLastSibling();
        Destroy(go, 0.8f);
    }
    public void ShowTaskminbox(CurrnetTaskData currnetTaskData)
    {
        CurrentTaskminbox.SetActive(true);
        CurrentTaskname_min.text = currnetTaskData.TaskName;
        CurrentTaskContent_min.text = currnetTaskData.TaskText+currnetTaskData.TaskoverNumber+"/"+currnetTaskData.CurrnetTaskEnemyeNumber;
    }
    public void TrackCurrnetTask(string TaskID)
    {
        int ID= int.Parse(TaskID);
        GameManager.Instance.CurrentTaskID = ID;
        GameManager.Instance.CopyTaskData(TaskData.taskList[ID], CurrnetTaskData.CurrentTaskList[0]);
        ShowTaskminbox(CurrnetTaskData.CurrentTaskList[0]);
    }
   
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
    public void ShowTaskContentbox()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            IsOpenTaskbox = !IsOpenTaskbox;
            Taskbox.SetActive(IsOpenTaskbox);
        }
    }
    public void EXPUGUIManager()
    {
        EXPText.text = "可用点数:"+EXP;
    }
    public void ShowBackpackMenu()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            IsOpenBackpackMenu = !IsOpenBackpackMenu;
            BackpackMenu.SetActive(IsOpenBackpackMenu);
        }
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
