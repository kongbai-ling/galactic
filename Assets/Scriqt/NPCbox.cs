using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCbox : MonoBehaviour
{
    public Animator ani;
    private bool isNPCbox = false;
    private void Update()
    {
        if (isNPCbox && Input.GetKeyDown(KeyCode.F))
        {
            UGUIManager.Instance.ShowDialogbox(true);
            UGUIManager.Instance.ShowText();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ani.SetTrigger("IsNPCbox");
            isNPCbox = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ani.SetTrigger("IsNPCboxs");
            UGUIManager.Instance.ShowDialogbox(false);
            isNPCbox = false;

        }
    }
    
}
