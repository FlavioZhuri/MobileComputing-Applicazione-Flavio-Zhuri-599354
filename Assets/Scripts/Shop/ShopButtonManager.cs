using UnityEngine;

public class ShopButtonManager : MonoBehaviour {

	public GameObject ConfirmationDialog;
	public GameObject DiamondShop;
	public GameObject CoinShop;
	public GameObject UnavailablePopOut;

	public void HomeButton()		{ UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu"); }
	public void ShowShopCoins()		{ UnavailablePopOut.SetActive(true); }
	public void ShowShopDiamonds()	{ UnavailablePopOut.SetActive(true); }
	public void CloseUnavailale()	{ UnavailablePopOut.SetActive(false); }
}
