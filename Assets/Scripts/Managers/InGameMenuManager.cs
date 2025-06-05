using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour {

    public GameObject DifficultyPicker;
    public GameObject awaitingGameStart;
    public GameObject ActiveMenu;
    public GameObject PauseMenu;
    public GameObject DeathScreenRevive;
    public GameObject DeathScreen;
    public GameObject doYouWishToEnd;
    public GameObject unavailablePopOut;
    public GameObject newHighscoreBlob;

    private Text _Coins;
    private Text _Diamonds;
    private Text SCORE_COUNTER;

    private GameManager GM;

	private void Awake() {
        _Coins = ActiveMenu.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
        _Diamonds = ActiveMenu.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>();
        SCORE_COUNTER = ActiveMenu.transform.GetChild(3).gameObject.GetComponent<Text>();
        GM = this.GetComponent<GameManager>();
	}

	//callbacktype
	public delegate void CallbackHandler();

    //use a coroutine to avoid garbage collection
    public void startGameSequence(CallbackHandler callback) { StartCoroutine(sGs(callback)); }
    public IEnumerator sGs(CallbackHandler callback) {
        var getReady = awaitingGameStart.transform.GetChild(0).gameObject;
        var tapIndicator = awaitingGameStart.transform.GetChild(1).gameObject;

        tapIndicator.SetActive(false);
        getReady.SetActive(true);
        yield return new WaitForSeconds(2);
        getReady.SetActive(false);
        awaitingGameStart.SetActive(false);
        callback();
    }

    public void endGameSequence(CallbackHandler callback) { StartCoroutine(eGs(callback)); }
    public IEnumerator eGs(CallbackHandler callback) {
        ActiveMenu.SetActive(false);
        yield return new WaitForSeconds(1);

        //set score text for death screen
        DeathScreen.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<Text>().text = SCORE_COUNTER.text;

        if (SaveSystem.SAVE_LOADED) {
            DeathScreen.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = SaveSystem.SAVE.HighScore.ToString();
            if (GM.score > SaveSystem.SAVE.HighScore)
                newHighscoreBlob.SetActive(true);
            else
                newHighscoreBlob.SetActive(false);
        }
        callback();
    }

    public void resetScoreCounter() { SCORE_COUNTER.text = "0"; }

    public void hideDifficultyP() { DifficultyPicker.SetActive(false); }

    public void showAwaiting() { awaitingGameStart.SetActive(true); }

    public void showUnavailable()   { unavailablePopOut.SetActive(true); }
    public void hideUnavailable()   { unavailablePopOut.SetActive(false); }

    public void showActiveMenu()    { ActiveMenu.SetActive(true); }
    public void hideActiveMenu()    { ActiveMenu.SetActive(false); }

    public void showPauseMenu()     { PauseMenu.SetActive(true); }
    public void hidePauseMenu()     { PauseMenu.SetActive(false); }

    public void showGameOver()      { DeathScreen.SetActive(true); }
    public void hideGameOver()      { DeathScreen.SetActive(false); }

    public void callTitleScreen()   { GM.UnPause(); SceneManager.LoadScene("Main Menu"); }
    public void callShopMenu()      { GM.UnPause(); SceneManager.LoadScene("Shop Menu"); }
    public void callStatsMenu()     { showUnavailable(); }
    public void callTrophiesMenu()  { showUnavailable(); }

    public void doYouWishToEndGameSTATS()    { showUnavailable(); }
    public void doYouWishToEndGameTROPHY()   { showUnavailable(); }

    public void doYouWishToEndGameSHOP() {
        showDoYouWishToEndGame();
        doYouWishToEnd.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(callShopMenu);
    }

    public void doYouWishToEndGameTITLE() {
        showDoYouWishToEndGame();
		doYouWishToEnd.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(callTitleScreen);
	}

	public void showDoYouWishToEndGame()     { doYouWishToEnd.SetActive(true);  }
	public void doYouWishToEndGameCONTINUE() { doYouWishToEnd.SetActive(false); }

	public void updateActiveMenuScore(int score) { SCORE_COUNTER.text = score.ToString(); }
    public void updateActiveMenuCoins(int coins) { _Coins.text = coins.ToString(); }
    public void updateActiveMenuDiamonds(int diamonds) { _Diamonds.text = diamonds.ToString(); }

}
