using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public string[] lines;

    private bool canActivate;

    public bool isPerson = true;

    void Start()
    {
        
    }

    void Update()
    {
        if(canActivate && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines, isPerson);
        }
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Player")
        {
            canActivate = true;
        }
    }

      private void OnTriggerExit2D (Collider2D other)
    {
        if(other.tag == "Player")
        {
            canActivate = false;
        }
    }
}
