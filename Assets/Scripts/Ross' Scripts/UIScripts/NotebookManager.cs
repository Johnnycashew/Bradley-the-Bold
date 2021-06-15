using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookManager : Singleton<NotebookManager>
{

    private const int initialNotesIndex = 1;
    private const int initialTabIndex = 0;

    public GameObject imagePrefab;
    public GameObject textPrefab;

    public int notesIndex;
    public int tabIndex;

    public List<NotebookImageEntry> notebookImageEntries;
    public List<NotebookTextEntry> notebookTextEntries;

    private void Start()
    {
        notesIndex = initialNotesIndex;
        tabIndex = initialTabIndex;
    }

    public void PopulatePages()
    {

        foreach (NotebookImageEntry entry in notebookImageEntries)
        {
            Instantiate(imagePrefab, GameObject.Find("Notebook Page " + entry.pageNumber.ToString()).GetComponent<Transform>());
            var created = GameObject.Find(imagePrefab.name + "(Clone)");
            created.name = entry.name;
            created.GetComponent<RectTransform>().localPosition = entry.position;
            created.GetComponent<RectTransform>().sizeDelta = entry.size;
            created.GetComponentInChildren<Image>().sprite = entry.sprite;
        }

        foreach (NotebookTextEntry entry in notebookTextEntries)
        {
            Instantiate(textPrefab, GameObject.Find("Notebook Page " + entry.pageNumber.ToString()).GetComponent<Transform>());
            var created = GameObject.Find(textPrefab.name + "(Clone)");
            created.name = entry.name;
            created.GetComponent<RectTransform>().localPosition = entry.position;
            created.GetComponent<RectTransform>().sizeDelta = entry.size;
            created.GetComponentInChildren<TextMeshProUGUI>().text = entry.text;
        }

        var journalTabScript = FindObjectOfType<JournalTabScript>();

        foreach (GameObject go in journalTabScript.notes)
        {
            var name = go.name.Split(' ');
            var check = name[2];

            if (check != notesIndex.ToString())
                go.SetActive(false);
        }

        switch (tabIndex)
        {
            case 0:
                journalTabScript.NoteButtonClicked();
                break;
            case 1:
                journalTabScript.PuzzleButtonClicked();
                break;
            case 2:
                journalTabScript.MapButtonClicked();
                break;
            default:
                break;
        }

    }

    public void AddEntry(ItemObject obj)
    {
        if (obj.type == ItemType.NotebookImageEntry)
            notebookImageEntries.Add((NotebookImageEntry)obj);
        if (obj.type == ItemType.NotebookTextEntry)
            notebookTextEntries.Add((NotebookTextEntry)obj);
    }

    public void ClearData()
    {
        notebookImageEntries.Clear();
        notebookTextEntries.Clear();

        notesIndex = initialNotesIndex;
        tabIndex = initialTabIndex;
    }

}
