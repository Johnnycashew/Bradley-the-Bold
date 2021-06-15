using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    private GameManager gameManager;
    private SoundManager soundManager;

    [SerializeField] private GameObject tutorialPrompt;

    [SerializeField] private string introScene;
    [SerializeField] private string playScene;

    private void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
    }

    public void DestroyMenu(bool changeState = false)
    {
        if (changeState)
        {
            gameManager.gameState = GameState.Game;
        }
        else
        {
            gameManager.gameState = GameState.Menu;
        }
        soundManager.PlayExitMenu();
        //  Add logic here to return to previous menu screen
        Destroy(gameObject);
    }

    public void LoadCheckpoint()
    {
        //  Add logic here to load game scene with saved checkpoint data
    }

    public void LoadGameCanvas(GameObject loadScreen)
    {
        //  Add logic here to pull up Load Game Canvas
        Instantiate(loadScreen);
        soundManager.PlayEnterMenu();
    }

    public void LoadGame()
    {
        //  Add logic here to load game scene with saved data
        soundManager.PlayEnterMenu();
    }

    public void MainMenu()
    {
        gameManager.gameState = GameState.Menu;
        gameManager.ClearData();
        soundManager.PlayEnterMenu();
        //  Add logic here to load the Intro Scene
        SceneManager.LoadScene(introScene);
    }

    public void NewGame()
    {
        //  Add logic here to launch game scene with fresh flags and collectibles
        tutorialPrompt.SetActive(true);
        soundManager.PlayEnterMenu();
    }

    public void PauseGameCanvas(GameObject pauseScreen)
    {
        //  Add logic here to pull up Pause Screen Canvas
        Instantiate(pauseScreen);
        gameManager.gameState = GameState.Menu;
        soundManager.PlayEnterMenu();
    }

    public void QuitGame()
    {
        soundManager.PlayEnterMenu();
        Application.Quit();
    }

    public void ResumeGame()
    {
        //  Add more logic to allow the game to resume play
        gameManager.gameState = GameState.Game;

        Cursor.lockState = CursorLockMode.Locked;

        soundManager.PlayExitMenu();
        //  Add logic here to destory the canvas
        Destroy(gameObject);

    }

    public void SaveGameCanvas(GameObject saveScreen)
    {
        soundManager.PlayEnterMenu();
        //  Add logic here to pull up Save Game Canvas
        Instantiate(saveScreen);
    }

    public void SaveGame()
    {
        // Add logic here to save game data
        soundManager.PlayEnterMenu();
    }

    public void SettingsCanvas(GameObject settingsScreen)
    {
        soundManager.PlayEnterMenu();
        //  Add logic to pull up Settings Canvas
        Instantiate(settingsScreen);
    }

    public void StartGame(bool isEnablingTutorial)
    {
        gameManager.isTutorialEnabled = isEnablingTutorial;
        gameManager.gameState = GameState.Game;
        SceneManager.LoadScene(playScene);
        soundManager.PlayEnterMenu();
    }

}
