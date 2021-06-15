using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalTabScript : MonoBehaviour
{

    [Header("Information Displayed")]
    public GameObject mapInformation;
    public GameObject noteInformation;
    public GameObject puzzleInformation;

    [Header("Tabs")]
    [SerializeField] private RectTransform mapButtonTransform;
    [SerializeField] private RectTransform mapTextTransform;
    [SerializeField] private RectTransform puzzleButtonTransform;
    [SerializeField] private RectTransform puzzleTextTransform;

    private Vector3 leftSide = new Vector3(1, 1, 1);
    private Vector3 rightSide = new Vector3(-1, 1, 1);

    [Header("Note Pages")]
    public GameObject[] notes;

    private void Start()
    {
        NotebookManager.Instance.PopulatePages();
    }

    public void MapButtonClicked()
    {
        mapInformation.SetActive(true);
        noteInformation.SetActive(false);
        puzzleInformation.SetActive(false);

        mapButtonTransform.localScale = leftSide;
        mapTextTransform.localScale = leftSide;
        puzzleButtonTransform.localScale = leftSide;
        puzzleTextTransform.localScale = leftSide;

        NotebookManager.Instance.tabIndex = 2;
    }

    public void NoteButtonClicked()
    {
        mapInformation.SetActive(false);
        noteInformation.SetActive(true);
        puzzleInformation.SetActive(false);

        mapButtonTransform.localScale = rightSide;
        mapTextTransform.localScale = rightSide;
        puzzleButtonTransform.localScale = rightSide;
        puzzleTextTransform.localScale = rightSide;

        NotebookManager.Instance.tabIndex = 0;
    }

    public void PuzzleButtonClicked()
    {
        mapInformation.SetActive(false);
        noteInformation.SetActive(false);
        puzzleInformation.SetActive(true);

        mapButtonTransform.localScale = rightSide;
        mapTextTransform.localScale = rightSide;
        puzzleButtonTransform.localScale = leftSide;
        puzzleTextTransform.localScale = leftSide;

        NotebookManager.Instance.tabIndex = 1;
    }

}
