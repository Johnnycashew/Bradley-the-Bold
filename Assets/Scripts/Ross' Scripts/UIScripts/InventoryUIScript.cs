using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScript : MonoBehaviour
{

    [Header("Text Element")]
    [SerializeField] private Text showText;

    [Header("Puzzle Check")]
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private PuzzleObject puzzle;

    [Header("Puzzle Interaction")]
    [SerializeField] private GameObject puzzleGameCanvas;
    [SerializeField] private GameObject puzzleGameButton;

    [Header("Puzzle Finished")]
    [SerializeField] private GameObject solvedPuzzle;

    [Header("Finished Indicator")]
    [SerializeField] private Color[] colors;
    [SerializeField] [Range(0.0f, 1.0f)] private float lerpTime;
    [SerializeField] private Image image;
    [SerializeField] private Text[] texts;

    private bool isAnimating = false;

    private Button button;

    private float t = 0.0f;

    private GameManager gameManager;

    private int colorIndex;
    private int length;

    private readonly string piece = "piece";

    private void Start()
    {
        gameManager = GameManager.Instance;

        length = colors.Length;
        solvedPuzzle.GetComponent<Image>().sprite = puzzle.puzzleResultImage;

        if (gameManager.puzzlesSolved.ContainsKey(puzzle))
        {
            if (gameManager.puzzlesSolved[puzzle])
            {
                Destroy(image.gameObject);
                for (int i = texts.Length - 1; i >= 0; i--)
                    Destroy(texts[i].gameObject);

                solvedPuzzle.SetActive(true);
                return;
            }
        }

        button = puzzleGameButton.GetComponent<Button>();
        int count = 0;

        foreach (ItemObject item in gameManager.inventory)
        {
            if (item.name.ToLower().Contains(piece) && item.name.ToLower().Contains(gameObject.name.ToLower()))
            {
                count++;
            }
        }

        showText.text = count.ToString() + "/" + puzzle.puzzlePieces.Length.ToString();

        if (count == puzzle.puzzlePieces.Length)
        {
            puzzleGameButton.SetActive(true);
            button.onClick.AddListener(() => SetPuzzle());

            isAnimating = true;
            foreach (Text text in texts)
            {
                text.text = text.text.Replace('?', '!');
            }
        }

    }

    private void Update()
    {
        if (isAnimating)
        {
            image.color = Color.Lerp(image.color, colors[colorIndex], lerpTime * Time.deltaTime);
            
            foreach (Text text in texts)
                text.color = Color.Lerp(text.color, colors[colorIndex], lerpTime * Time.deltaTime);

            t = Mathf.Lerp(t, 1.0f, lerpTime * Time.deltaTime);

            if (t >= 0.45f)
            {
                t = 0.0f;
                colorIndex++;
                colorIndex = (colorIndex >= length) ? 0 : colorIndex;
            }

        }

        if (gameManager.puzzlesSolved.ContainsKey(puzzle))
        {
            if (gameManager.puzzlesSolved[puzzle])
            {
                solvedPuzzle.GetComponent<Image>().sprite = puzzle.puzzleResultImage;
                solvedPuzzle.SetActive(true);
                return;
            }
        }
    }

    private void SetPuzzle()
    {
        gameManager.puzzleToSolve = puzzle;
        Instantiate(puzzleGameCanvas);
    }

}
