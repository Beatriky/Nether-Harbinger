using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public string[] lines;

    private bool canActivate;

    public bool isPerson = true;

    public bool shouldActivateQuest;
    public string questToMark;
    public bool markComplete;

    void Update()
    {
        if (canActivate && (Input.GetButtonDown("Fire1") || (Input.GetKeyDown(KeyCode.F))) && !DialogManager.instance.dialogBox.activeInHierarchy)
        {   //we activate the dialog
            DialogManager.instance.ShowDialog(lines, isPerson);
            //activate the quest with the name of the quest
            DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;
        }
    }
}
