using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject _PLAYER;              // the player object
    public GameObject _MAIN_CAMERA;         // for the ground scroller
    public GameObject _PIPE_SPAWNER;        // for toggling the pipe spawner
    public GameObject _PIPE_CONTAINER;      // for stopping all pipes once game over
    public GameObject _GROUND_CONTAINER;    // for changing active ground
    public GameObject _BACKGROUND;          // the background image itself not the canvas

    public bool isPlayerRevived = false;
    public int score = 0;
    public int collectedDiamondsThisRun = 0;
    public int collectedCoinsThisRun = 0;

    public int coinvalue = 1000;

    //used for preservation of the time scale on pause
    private float time_scale = 1;

    //In game menu manager
    private InGameMenuManager MM;
    private ItemManager IMGR;
    private PlayerMovement plm;


    void Awake() {
        MM = this.GetComponent<InGameMenuManager>();
        plm = _PLAYER.GetComponent<PlayerMovement>();
    }

	private void Start() {
        if (!SaveSystem.SAVE_LOADED) {
            Debug.LogError("GAME DATA MANAGER IS MISSING @ GAME MANAGER");
            return;
        }

        //ItemManager
        IMGR = GameObject.Find("_GAME DATA MANAGER").GetComponent<ItemManager>();

        //enabled items
        _PLAYER.GetComponent<SpriteRenderer>().sprite = IMGR.GetItem(SaveSystem.SAVE.SelectedSkin).preview;
        _BACKGROUND.GetComponent<Image>().sprite = IMGR.GetItem(SaveSystem.SAVE.SelectedBackground).preview;
        _MAIN_CAMERA.GetComponent<ScrollingGround>().GroundGen[0].GetComponent<SpriteRenderer>().sprite = IMGR.GetItem(SaveSystem.SAVE.SelectedGround).preview;
    }

	public void StartGame() {


        MM.startGameSequence(delegate {
            plm.isGameActive = true;
            plm.UnfreezeRigidBody();

            //call first jump
            plm.InitiateJump();
            MM.showActiveMenu();
            if (SaveSystem.SAVE_LOADED) {
                MM.updateActiveMenuCoins(SaveSystem.SAVE.Coins);
                MM.updateActiveMenuDiamonds(SaveSystem.SAVE.Diamonds);
			}

            _PIPE_SPAWNER.GetComponent<PipeSpawner>().isSpawning = true;
        });
    }

    //Final Game Over Screen
    public void GameOver() {
        if (!plm.isGameActive)
            return;
        _PIPE_SPAWNER.GetComponent<PipeSpawner>().isSpawning = false;

        //stop all already spawned pipes from moving on game over
        foreach(Transform pipe in _PIPE_CONTAINER.transform) pipe.gameObject.GetComponent<PipeMover>().Speed = 0;

        plm.isGameActive = false;
        //disable the ground scroller

        if (SaveSystem.SAVE_LOADED) {
            if (SaveSystem.SAVE.HighScore < score) {
                SaveSystem.SAVE.HighScore = score;
            }
            SaveSystem.SAVE.CollectedCoinsAllTime += collectedCoinsThisRun;
            SaveSystem.SAVE.CollectedDiamondsAllTime += collectedDiamondsThisRun;
            SaveSystem.SaveGame();
            Debug.Log("Game Saved. @ GAME MANAGER");
        } else {
            Debug.LogError("SAVE IS NOT LOADED @ GAME MANAGER");
		}

        MM.endGameSequence(delegate {
            Time.timeScale = 0;
            MM.showGameOver();
        });
	}

    public void Restart() {
        //clear pipes
        foreach (Transform pipe in _PIPE_CONTAINER.transform) Destroy(pipe.gameObject);

        Time.timeScale = time_scale;
        plm.isGameActive = false;
        plm.isFirstClick = true;

        _PLAYER.transform.position = plm.s_location;
        _PLAYER.transform.rotation = plm.s_rotation;
        plm.FreezeRigidBody();

        isPlayerRevived = false;
        score = 0;
        collectedDiamondsThisRun = 0;
        collectedCoinsThisRun = 0;

        MM.hideGameOver();
        MM.resetScoreCounter();
        MM.awaitingGameStart.SetActive(true);
        MM.awaitingGameStart.transform.GetChild(1).gameObject.SetActive(true);      //tap
        MM.awaitingGameStart.transform.GetChild(0).gameObject.SetActive(false);     //get ready
    }

    public void Pause()     { time_scale = Time.timeScale; Time.timeScale = 0; }
    public void UnPause()   { Time.timeScale = time_scale; }

    public void UpdateScore() {
        score++;
        MM.updateActiveMenuScore(score);
    }

    public void UpdateDiamonds() {
        collectedDiamondsThisRun+=1000;
        SaveSystem.SAVE.Diamonds+=1000;
        MM.updateActiveMenuDiamonds(SaveSystem.SAVE.Diamonds);
	}

    public void UpdateCoins() {
        collectedCoinsThisRun+=100;
        SaveSystem.SAVE.Coins+=100;
        MM.updateActiveMenuCoins(SaveSystem.SAVE.Coins);
    }
}
