using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour {

    public GameObject OptionsMenu;
    public GameObject ExitGM;
    public GameObject UnavailablePopOut;
    public GameObject VolumeSlider;



    //Make a variable so we don't spam GetComponent on value change.
    private Slider v_slider;
	public void Start() { v_slider = VolumeSlider.GetComponent<Slider>(); }

	public void OpenURL()                   { Application.OpenURL("http://kaisu-games.com"); }
    public void OpenOptions()               { OptionsMenu.SetActive(true); }
    public void CloseOptions()              { OptionsMenu.SetActive(false); }
    public void ExitComfirmation()          { ExitGM.SetActive(true); }
    public void ExitComfirmationQuit()      { ExitGM.SetActive(false); }
    public void ExitGame()                  { Application.Quit(); }
    public void LoadGame()                  { SceneManager.LoadScene("Game Scene"); }
    public void LoadShop()                  { SceneManager.LoadScene("Shop Menu"); }
    public void trophies()                  { UnavailablePopOut.SetActive(true); }
    public void Leaderboard()               { UnavailablePopOut.SetActive(true); }



    //Options Menu
    public void VolumeMax()                 { v_slider.value = 100; }
    public void VolumeMute()                { v_slider.value = 0; }
    public void VolumeChanged()             { Debug.Log(v_slider.value); }
    public void RemoveAds()                 { UnavailablePopOut.SetActive(true); }
    public void SetLangENG()                { UnavailablePopOut.SetActive(true); }
    public void SetLangITL()                { UnavailablePopOut.SetActive(true); }
    public void SetLangRUS()                { UnavailablePopOut.SetActive(true); }
    public void CloseUnavailablePopOut()    { UnavailablePopOut.SetActive(false); }

}
