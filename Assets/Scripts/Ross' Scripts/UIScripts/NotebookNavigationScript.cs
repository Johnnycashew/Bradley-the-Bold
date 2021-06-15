using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookNavigationScript : MonoBehaviour
{

    public GameObject[] pages;

    public void JumpToPage(int page)
    {
        foreach (GameObject go in pages)
        {
            var name = go.name.Split(' ');
            var check = name[2];

            if (check == page.ToString())
                go.SetActive(true);
            else
                go.SetActive(false);
        }
        NotebookManager.Instance.notesIndex = page;
    }

}
