using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public static NPCScript Instance;
    void Start()
    {
        Instance = this;
    }


    void Update()
    {
        
    }
    public IEnumerator NPCTask()
    {
        yield return null;
    }
}
