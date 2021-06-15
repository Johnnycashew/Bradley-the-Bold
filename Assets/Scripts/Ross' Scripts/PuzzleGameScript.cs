using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGameScript : MonoBehaviour
{
    #region Variables

    private enum State
    {
        Solving,
        Revealing,
        Flipping,
        Moving,
    }

    private State currentState = State.Solving;

    [Header("Puzzle Elements")]
    private readonly float pieceRotateSpeed = Mathf.PI / 2;

    private GameObject selectedPiece;
    public GameObject puzzlePiecePrefab;

    private List<GameObject> piecesToRemove;

    private PuzzleObject puzzleToSolve;

    [Header("Finished Elements")]
    public GameObject blockingElement;
    public Image finishedPuzzle;
    public Image puzzleResult;
    [Range(1.0f, 5.0f)]
    public float hangTime;

    private GameManager gameManager;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        gameManager = GameManager.Instance;
        puzzleToSolve = gameManager.puzzleToSolve;
        finishedPuzzle.sprite = puzzleToSolve.finishedPuzzleImage;
        puzzleResult.sprite = puzzleToSolve.puzzleResultImage;
        piecesToRemove = new List<GameObject>();
        piecesToRemove.Add(GameObject.Find("PuzzleBackground"));
        piecesToRemove.Add(GameObject.Find("PuzzleBorder"));
        selectedPiece = null;
        int numOfPieces = puzzleToSolve.puzzlePieces.Length;

        for (int i = 0; i < numOfPieces; i++)
        {
            //  Create a new instance of our prefab and reference our components
            GameObject obj = Instantiate(puzzlePiecePrefab, gameObject.transform);
            Image asset = obj.GetComponent<Image>();
            RectTransform rect = obj.GetComponent<RectTransform>();

            //  Change the object's name, for later clarification
            obj.name = puzzleToSolve.puzzlePieces[i].name;

            //  Get our information from the Puzzle for our interface
            asset.sprite = puzzleToSolve.puzzlePieces[i].sprite;
            rect.localPosition = puzzleToSolve.puzzlePieces[i].spawnLocation;
            rect.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            rect.rotation = puzzleToSolve.puzzlePieces[i].spawnRotation;
            rect.sizeDelta = new Vector2(obj.GetComponent<Image>().sprite.rect.width, obj.GetComponent<Image>().sprite.rect.height);

            //  Allow the button to do something
            obj.GetComponent<Button>().onClick.AddListener(() => SetSelectedPiece(obj));

            piecesToRemove.Add(obj);
        }

        blockingElement.transform.SetParent(gameObject.transform.Find("Border"), false);
        blockingElement.transform.SetParent(gameObject.transform, false);

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Solving:
                if (Input.GetMouseButton(1) && selectedPiece != null)
                {
                    string[] items = selectedPiece.name.Split(' ');

                    AttemptSnap(int.Parse(items[items.Length - 1]));

                    selectedPiece = null;
                    Cursor.visible = true;
                }
                if (Input.GetKeyDown(KeyCode.Escape) && selectedPiece != null)
                {
                    selectedPiece = null;
                    Cursor.visible = true;
                }
                break;
            case State.Revealing:
                finishedPuzzle.color = new Color(1.0f, 1.0f, 1.0f, finishedPuzzle.color.a + Time.deltaTime);
                puzzleResult.color = new Color(1.0f, 1.0f, 1.0f, finishedPuzzle.color.a + Time.deltaTime);

                if (finishedPuzzle.color.a >= 1.0f)
                {
                    for (int i = piecesToRemove.Count - 1; i >= 0; i--)
                    {
                        Destroy(piecesToRemove[i]);
                    }
                    currentState = State.Flipping;
                }
                break;
            case State.Flipping:
                var fprt = finishedPuzzle.gameObject.GetComponent<RectTransform>();
                var prrt = puzzleResult.gameObject.GetComponent<RectTransform>();

                if (fprt.localScale.x == 0)
                {
                    prrt.localScale = new Vector3(prrt.localScale.x - Time.deltaTime, 1.0f, 1.0f);

                    if (prrt.localScale.x <= -1)
                    {
                        prrt.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                        currentState = State.Moving;
                    }

                    return;
                }

                fprt.localScale = new Vector3(fprt.localScale.x - Time.deltaTime, 1.0f, 1.0f);

                if (fprt.localScale.x <= 0)
                    fprt.localScale = new Vector3(0.0f, 1.0f, 1.0f);

                break;
            case State.Moving:
                hangTime -= Time.deltaTime;

                if (hangTime <= 0.0f)
                {
                    gameManager.puzzlesSolved.Add(puzzleToSolve, true);

                    if (puzzleToSolve.notebookEntries.Length > 0)
                        foreach (ItemObject item in puzzleToSolve.notebookEntries)
                        {
                            NotebookManager.Instance.AddEntry(item);
                            if (item is NotebookImageEntry)
                                WriteInJournal((NotebookImageEntry)item);
                            if (item is NotebookTextEntry)
                                WriteInJournal((NotebookTextEntry)item);
                        }

                    Destroy(gameObject);
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (selectedPiece == null) return;

        RectTransform rectTransform = selectedPiece.GetComponent<RectTransform>();

        rectTransform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            rectTransform.Rotate(0, 0, pieceRotateSpeed);
        }

        if (Input.GetKey(KeyCode.E))
        {
            rectTransform.Rotate(0, 0, -pieceRotateSpeed);
        }
    }

    public void SetSelectedPiece(GameObject obj)
    {
        selectedPiece = obj;
        selectedPiece.transform.SetParent(gameObject.transform.Find("PuzzleBorder"), false);
        selectedPiece.transform.SetParent(gameObject.transform, false);
        Cursor.visible = false;
    }

    private void AttemptSnap(int pieceNumber)
    {
        RectTransform rect = selectedPiece.GetComponent<RectTransform>();
        if (Math.Abs(rect.transform.localRotation.z) <= puzzleToSolve.acceptableRotation &&
            Vector3.Distance(puzzleToSolve.pieceLocations[pieceNumber - 1], selectedPiece.transform.localPosition) <= puzzleToSolve.acceptableDistance)
        {
            rect.transform.localPosition = puzzleToSolve.pieceLocations[pieceNumber - 1];
            rect.transform.localRotation = Quaternion.Euler(0, 0, 0);
            selectedPiece.GetComponent<Button>().interactable = false;
            selectedPiece.transform.SetParent(gameObject.transform.Find("PuzzleBorder"), false);
            CheckCompletion();
        }
    }

    private void CheckCompletion()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            if (button.gameObject.name == "ExitButton") continue;
            if (button.interactable) return;
        }

        currentState = State.Revealing;
        blockingElement.SetActive(true);
    }

    private void WriteInJournal(NotebookImageEntry item)
    {
        var pages = FindObjectOfType<NotebookNavigationScript>().pages;
        var notes = FindObjectOfType<JournalTabScript>().noteInformation;
        GameObject placement = null;

        notes.SetActive(true);

        foreach (GameObject go in pages)
        {
            var name = go.name.Split(' ');
            var check = name[2];
            var index = item.pageNumber.ToString();

            if (check == index)
                placement = go;
        }

        if (placement == null) return;

        placement.SetActive(true);

        Instantiate(NotebookManager.Instance.imagePrefab, placement.transform);
        var created = GameObject.Find(NotebookManager.Instance.imagePrefab.name + "(Clone)");
        created.name = item.name;
        created.GetComponent<RectTransform>().localPosition = item.position;
        created.GetComponent<RectTransform>().sizeDelta = item.size;
        created.GetComponentInChildren<Image>().sprite = item.sprite;

        notes.SetActive(false);

        if (item.pageNumber != NotebookManager.Instance.notesIndex) return;

        placement.SetActive(false);
    }

    private void WriteInJournal(NotebookTextEntry item)
    {
        var pages = FindObjectOfType<NotebookNavigationScript>().pages;
        var notes = FindObjectOfType<JournalTabScript>().noteInformation;
        GameObject placement = null;

        notes.SetActive(true);

        foreach (GameObject go in pages)
        {
            var name = go.name.Split(' ');
            var check = name[2];
            var index = item.pageNumber.ToString();

            if (check == index)
                placement = go;
        }

        if (placement == null) return;

        placement.SetActive(true);

        Instantiate(NotebookManager.Instance.textPrefab, placement.transform);
        var created = GameObject.Find(NotebookManager.Instance.textPrefab.name + "(Clone)");
        created.name = item.name;
        created.GetComponent<RectTransform>().localPosition = item.position;
        created.GetComponent<RectTransform>().sizeDelta = item.size;
        created.GetComponentInChildren<TextMeshProUGUI>().text = item.text;
        
        notes.SetActive(false);

        if (item.pageNumber == NotebookManager.Instance.notesIndex) return;

        placement.SetActive(false);
    }

}
