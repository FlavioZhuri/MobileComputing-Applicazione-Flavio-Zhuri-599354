using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum balType {
	Coins,
	Diamonds
}

public enum shopType {
	Skins,
	Backgrounds,
	Pipes,
	Grounds
}

public enum buttonType {
	buy,
	equip,
	equipped
}

[System.Serializable]
public struct ShopItem {
	public string name;
	public Sprite preview;
	public int cost;
	public balType CostType;
	public shopType ItemCategory;
	public int id;
	public bool Special; //sparkly effect
}

public class ShopManagerEX : MonoBehaviour {

	[Header("Shop Tab Buttons")]
	public GameObject SkinsTabButton;
	public GameObject BackgroundsTabButton;
	public GameObject PipesTabButton;
	public GameObject GroundsTabButton;

	[Space(10)]
	[Header("Shop Item Prefabs")]
	public Object SkinPrefab;
	public Object BackgroundPrefab;
	public Object PipePrefab;
	public Object GroundPrefab;
	public float VerticalSpacing = 2f;
	//deprecated:
	//public int SkinPrefabHeight = 270;
	//public int BackgroundPrefabHeight = 350;
	//public int PipePrefabHeight = 315;
	//public int GroundPrefabHeight = 315;

	[Space(10)]
	[Header("Shop Container")]
	public GameObject ShopInsideContainer;
	public GameObject ConfirmationDialog;
	public GameObject NotEnoughCurrencyDialog;

	[Space(10)]
	[Header("Display Objects")]
	public GameObject CoinsText;
	public GameObject DiamondsText;
	public Sprite CoinsSprite;
	public Sprite DiamondsSprite;

	//[Space(10)]
	//[Header("Debug Values")]
	//public int CoinsValue = Player.Coins;
	//public int DiamondsValue = Player.Diamonds;

	private GameObject ItemManager;
	private List<ShopItem> Skins;
	private List<ShopItem> Backgrounds;
	private List<ShopItem> Pipes;
	private List<ShopItem> Grounds;

	//currently selected shop. will probably be needed later
	private shopType currentShop = shopType.Backgrounds; // we set it to a different value due to the check in the bottom of PopulateShopTab();

	ItemManager IMGR;

	// Assign the item manager on start and
	// Initialize the shop
	private void Start() {

		//get the required data from the ItemManager
		ItemManager = GameObject.Find("_GAME DATA MANAGER");

		//make a variable so we don't spam GetComponent<>();
		IMGR = ItemManager.GetComponent<ItemManager>();
		Skins			= IMGR.GetAllItems(shopType.Skins);
		Backgrounds		= IMGR.GetAllItems(shopType.Backgrounds);
		Pipes			= IMGR.GetAllItems(shopType.Pipes);
		Grounds			= IMGR.GetAllItems(shopType.Grounds);

		//Initialization
		updateText(balType.Coins, -1);
		InitializeButtons();
		PopulateShopTab(shopType.Skins);
		Debug.Log(Time.timeScale);
		return;
	}

	/// <summary>
	/// Updates Display Text and changes values of player variables.
	/// (use on item purchase, or general text update.)
	/// If -1 is passed as value will just update text from the save.
	/// </summary>
	/// <param name="k"></param>
	/// <param name="value"></param>
	private void updateText(balType k, int value) {
		if (!SaveSystem.SAVE_LOADED) {
			Debug.LogError("SAVE IS NOT LOADED @ updateText - ShopManagerEX");
			return;
		}

		if (value == -1) { // if -1, return savedata
			CoinsText.GetComponent<Text>().text = SaveSystem.SAVE.Coins.ToString();
			DiamondsText.GetComponent<Text>().text = SaveSystem.SAVE.Diamonds.ToString();
			return;
		}

		if (k == balType.Coins) {
			SaveSystem.SAVE.Coins = value;
			CoinsText.GetComponent<Text>().text = value.ToString();
		} else if (k == balType.Diamonds) {
			SaveSystem.SAVE.Diamonds = value;
			DiamondsText.GetComponent<Text>().text = SaveSystem.SAVE.Diamonds.ToString();
		} else {
			Debug.LogError("Invalid balType passed to updateText(); in ShopManagerEX.");
		}

		SaveSystem.SaveGame();
	}

	/// <summary>
	/// Attaches event handlers to the shop tab buttons and adds basic tab handling functionality.
	/// </summary>
	private void InitializeButtons() {
		SkinsTabButton.GetComponent<Button>().onClick.AddListener(			delegate { TabOnClick(shopType.Skins); });
		BackgroundsTabButton.GetComponent<Button>().onClick.AddListener(	delegate { TabOnClick(shopType.Backgrounds); });
		PipesTabButton.GetComponent<Button>().onClick.AddListener(			delegate { TabOnClick(shopType.Pipes); });
		GroundsTabButton.GetComponent<Button>().onClick.AddListener(		delegate { TabOnClick(shopType.Grounds); });
	}

	/// <summary>
	/// Toggle a tab's color
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="toggle"></param>
	void ToggleColor(GameObject obj, bool toggle) {
		if (toggle) {
			Color irm;
			ColorUtility.TryParseHtmlString("#516082", out irm);
			obj.GetComponent<Image>().color = irm;
		} else {
			Color irm;
			ColorUtility.TryParseHtmlString("#7186B4", out irm);
			obj.GetComponent<Image>().color = irm;
		}
	}

	/// <summary>
	/// When a tab button is clicked.
	/// </summary>
	void TabOnClick(shopType SelectedShop) {
		CleanShopTab(ShopInsideContainer.transform.GetChild(0).gameObject.GetComponent<Canvas>().gameObject);
		ToggleColor(SkinsTabButton, false);
		ToggleColor(BackgroundsTabButton, false);
		ToggleColor(PipesTabButton, false);
		ToggleColor(GroundsTabButton, false);

		//populate shop container again
		PopulateShopTab(SelectedShop);
		currentShop = SelectedShop; //if needed
	}

	/// <summary>
	/// Purchases an item and subtracts the cost from the value
	/// </summary>
	/// <param name="item"></param>
	public void PurchaseItem(ShopItem item) {
		if (!isPurchased(item.id)) {
			if (item.CostType == balType.Coins) {
				if (SaveSystem.SAVE.Coins >= item.cost) {
					SaveSystem.SAVE.PurchasedItems.Add(item.id);
					SaveSystem.SAVE.Coins -= item.cost;
					Debug.Log("Purchased item \"" + item.name + "\" from the shop, Game Saved. @ ShopManagerEX");
				} else {
					NotEnoughCurrencyDialog.SetActive(true);
					return;
				}
			} else {
				if (SaveSystem.SAVE.Diamonds >= item.cost) {
					SaveSystem.SAVE.PurchasedItems.Add(item.id);
					SaveSystem.SAVE.Diamonds -= item.cost;
					Debug.Log("Purchased item \"" + item.name + "\" from the shop, Game Saved. @ ShopManagerEX");
				} else {
					NotEnoughCurrencyDialog.SetActive(true);
					return;
				}
			}

			SaveSystem.SaveGame();

			updateText(balType.Coins, -1);
		}

		//reload shop tab
		PopulateShopTab(item.ItemCategory);

	}

	/// <summary>
	/// Checks the SaveSystem if an item is purchased
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public bool isPurchased(int id) { return SaveSystem.SAVE.PurchasedItems.Contains(id); }

	/// <summary>
	/// Checks if an item is equipped.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public bool isEquipped(int id) {
		if (
			SaveSystem.SAVE.SelectedSkin == id ||
			SaveSystem.SAVE.SelectedBackground == id ||
			SaveSystem.SAVE.SelectedPipe == id ||
			SaveSystem.SAVE.SelectedGround == id
		) return true; else return false;
	}

	/// <summary>
	/// Will set button color depending on what mode it is in
	/// Will also change the text of the button
	/// </summary>
	/// <param name="btn"></param>
	/// <param name="bt"></param>
	public void setButton(GameObject btn, buttonType bt, ShopItem x) {
		switch (bt) {
			case buttonType.buy: {
					Color irm;
					ColorUtility.TryParseHtmlString("#00FF94", out irm);
					btn.GetComponent<Image>().color = irm;
					btn.transform.GetChild(0).GetComponent<Text>().text = "Buy";
					btn.AddComponent<ItemTransporter>();
					btn.GetComponent<ItemTransporter>().contained_item = x;
					btn.GetComponent<Button>().onClick.AddListener(delegate { onButtonBuy(x); });
					break; }
			case buttonType.equip: {
					Color irm;
					ColorUtility.TryParseHtmlString("#c842f5", out irm);
					btn.GetComponent<Image>().color = irm;
					btn.transform.GetChild(0).GetComponent<Text>().text = "Equip";
					btn.AddComponent<ItemTransporter>();
					btn.GetComponent<ItemTransporter>().contained_item = x;
					btn.GetComponent<Button>().onClick.AddListener(delegate { onButtonEquip(x, x.ItemCategory); });
					break; }
			case buttonType.equipped: {
					Color irm;
					ColorUtility.TryParseHtmlString("#42cef5", out irm);
					btn.GetComponent<Image>().color = irm;
					btn.transform.GetChild(0).GetComponent<Text>().text = "Equipped";
					btn.AddComponent<ItemTransporter>();
					btn.GetComponent<ItemTransporter>().contained_item = x;
					break; }
			default:
				return;
		}
	}

	public void onButtonBuy(ShopItem x) {
		//add transporter for the confirm button
		//ConfirmationDialog.AddComponent<ItemTransporter>();
		//var tmp = ConfirmationDialog.GetComponent<ItemTransporter>().contained_item = x;
		//set dialog title
		ConfirmationDialog.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Do you wish to spend " + x.cost + " " + x.CostType + "?";

		//set yes button action
		ConfirmationDialog.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate {
			ConfirmationDialog.SetActive(false);
			PurchaseItem(x);
		});

		ConfirmationDialog.SetActive(true);
	}

	public void onButtonEquip(ShopItem x, shopType st) {

		if (!SaveSystem.SAVE_LOADED) {
			Debug.LogError("Tried equipping an item while the save was not loaded @ ShopManagerEX");
			return;
		}

		switch (st) {
			case shopType.Skins:
				SaveSystem.SAVE.SelectedSkin = x.id;
				break;
			case shopType.Backgrounds:
				SaveSystem.SAVE.SelectedBackground = x.id;
				break;
			case shopType.Pipes:
				SaveSystem.SAVE.SelectedPipe = x.id;
				break;
			case shopType.Grounds:
				SaveSystem.SAVE.SelectedGround = x.id;
				break;
			default:
				Debug.LogError("Invalid shopType at onButtonEquip @ ShopManagerEX");
				break;
		}

		SaveSystem.SaveGame();
		Debug.Log("Game Saved. -> onButtonEquip @ ShopManagerEX");

		//reload shop tab
		PopulateShopTab(st);
	}


	public void hideConfirmationDialog() { ConfirmationDialog.SetActive(false); }
	public void hideNotEnoughCurrencyDialog() { NotEnoughCurrencyDialog.SetActive(false); }

	/// <summary>
	/// Removes all child elements from a GameObject (in this case the canvas).
	/// </summary>
	/// <param name="x"></param>
	private void CleanShopTab(GameObject x) { foreach (Transform child in x.transform) Destroy(child.gameObject); }

	/// <summary>
	/// This will populate the Shop Inside Container with the corresponding shop array.
	/// </summary>
	private void PopulateShopTab(shopType st) {

		//Clean the shop just in case
		CleanShopTab(ShopInsideContainer.transform.GetChild(0).gameObject.GetComponent<Canvas>().gameObject);

		//Make sure we don't crash if the save is not loaded
		if (!SaveSystem.SAVE_LOADED) {
			Debug.LogError("SAVE IS NOT LOADED @ ShopManagerEX, Creating Save..");
			SaveSystem.LoadGame();
			return;
		}

		//SaveSystem.SAVE.Coins = 727000;
		//SaveSystem.SAVE.Diamonds = 69420;
		//SaveSystem.SaveGame();


		//Get the canvas borders object
		GameObject borders = ShopInsideContainer.transform.GetChild(0).gameObject.GetComponent<Canvas>().gameObject;
		RectTransform bordersRect = borders.GetComponent<RectTransform>();

		int current_index = 0;
		GameObject lastitem = null;

		switch (st) {
			case shopType.Skins:

				//highlight tab
				ToggleColor(SkinsTabButton, true);

				//Populate the shop tab
				foreach (ShopItem x in Skins) {
					{ //double brackets to prevent scope violation

						//set main position to the center of the container canvas
						//container canvas must have a top,stretch RectTransform
						GameObject current_item = (GameObject)Instantiate(
							SkinPrefab,
							new Vector3(
								borders.transform.position.x,
								borders.transform.position.y,
								0
							),
							Quaternion.identity,
							borders.transform
						);

						//spawned object size in canvas
						var LPOX = current_item.GetComponent<RectTransform>().sizeDelta;

						//We set the canvas position of the current element
						current_item.GetComponent<RectTransform>().localPosition = new Vector3(
							0,
							// -bordersRect.rect.y - LPOX.y / 2		= top of container canvas
							// current_index * LPOX.y				= item offset
							// vertical spacing is offsetting the spacing between elements
							// VerticalSpacing * 2					= this is used to move the top of the first element down to not clip with the container
							-bordersRect.rect.y - LPOX.y / 2 - current_index * (LPOX.y + VerticalSpacing) - VerticalSpacing * 2,
							0
						);

						lastitem = current_item;

						//preview
						current_item.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = x.preview;

						//cost
						if (x.cost == 0) //remove price tag if the item is default (ie. it's cost is 0)
							Destroy(current_item.transform.GetChild(1).gameObject);
						else
							current_item.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.cost.ToString();

						//title
						current_item.transform.GetChild(2).GetComponent<Text>().text = x.name;

						//cost type
						if (x.CostType == balType.Coins)
							current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CoinsSprite;

						else if (x.CostType == balType.Diamonds)
							current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = DiamondsSprite;

						//button
						var btnobj = current_item.transform.GetChild(3).gameObject;

						//cost container
						var cst = current_item.transform.GetChild(1).gameObject;

						cst.SetActive(false);
						if (isEquipped(x.id))
							setButton(btnobj, buttonType.equipped, x);
						else if (isPurchased(x.id))
							setButton(btnobj, buttonType.equip, x);
						else {
							cst.SetActive(true);
							setButton(btnobj, buttonType.buy, x);
						}

						current_index++;
					}
				}

				#region Old Code
				//Populate the Skins Shop Tab
				//foreach (ShopItem x in Skins) {
				//	//we cast to gameobject cause unity dumb and can't self convert it
				//	GameObject current_item = (GameObject)Instantiate(
				//		SkinPrefab,
				//		new Vector3(
				//			borders.transform.position.x,
				//			borders.transform.position.y - (current_index - 1) * SkinPrefabHeight,
				//			0
				//		),
				//		Quaternion.identity,
				//		borders.transform
				//	);

				//	lastitem = current_item;

				//	//preview
				//	current_item.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = x.preview;

				//	//cost
				//	if (x.cost == 0) //remove price tag if the item is default (ie. it's cost is 0)
				//		Destroy(current_item.transform.GetChild(1).gameObject);
				//	else
				//		current_item.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.cost.ToString();

				//	//title
				//	current_item.transform.GetChild(2).GetComponent<Text>().text = x.name;

				//	//cost type
				//	if (x.CostType == balType.Coins)
				//		current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CoinsSprite;

				//	else if (x.CostType == balType.Diamonds)
				//		current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = DiamondsSprite;

				//	current_index++;
				//}
				#endregion

				break;

			case shopType.Backgrounds:

				//highlight tab
				ToggleColor(BackgroundsTabButton, true);

				//Populate the shop tab
				foreach (ShopItem x in Backgrounds) {
					{
						GameObject current_item = (GameObject)Instantiate(
							BackgroundPrefab,
							new Vector3(
								borders.transform.position.x,
								borders.transform.position.y,
								0
							),
							Quaternion.identity,
							borders.transform
						);

						//spawned object size in canvas
						var LPOX = current_item.GetComponent<RectTransform>().sizeDelta;

						//We set the canvas position of the current element
						current_item.GetComponent<RectTransform>().localPosition = new Vector3(
							0,
							-bordersRect.rect.y - LPOX.y / 2 - current_index * (LPOX.y + VerticalSpacing) - VerticalSpacing * 2,
							0
						);

						lastitem = current_item;

						//name
						current_item.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.name;

						//preview
						current_item.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = x.preview;

						//cost
						if (x.cost == 0) //remove price tag if the item is default (ie. it's cost is 0)
							Destroy(current_item.transform.GetChild(2).gameObject);
						else
							current_item.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.cost.ToString();

						//cost type
						if (x.CostType == balType.Coins)
							current_item.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CoinsSprite;

						else if (x.CostType == balType.Diamonds)
							current_item.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = DiamondsSprite;

						//button
						var btnobj = current_item.transform.GetChild(3).gameObject;

						//cost container
						var cst = current_item.transform.GetChild(2).gameObject;

						cst.SetActive(false);
						if (isEquipped(x.id))
							setButton(btnobj, buttonType.equipped, x);
						else if (isPurchased(x.id))
							setButton(btnobj, buttonType.equip, x);
						else {
							cst.SetActive(true);
							setButton(btnobj, buttonType.buy, x);
						}

						current_index++;
					}
				}

				#region Old Code
				//foreach (ShopItem x in Backgrounds) {
				//	//we cast to gameobject cause unity dumb and can't self convert it
				//	GameObject current_item = (GameObject)Instantiate(
				//		BackgroundPrefab,
				//		new Vector3(
				//			borders.transform.position.x,
				//			borders.transform.position.y - (current_index - 1) * BackgroundPrefabHeight,
				//			0
				//		),
				//		Quaternion.identity,
				//		borders.transform
				//	);

				//	lastitem = current_item;

				//	//name
				//	current_item.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.name;

				//	//preview
				//	current_item.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = x.preview;

				//	//cost
				//	if (x.cost == 0) //remove price tag if the item is default (ie. it's cost is 0)
				//		Destroy(current_item.transform.GetChild(2).gameObject);
				//	else
				//		current_item.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.cost.ToString();

				//	//cost type
				//	if (x.CostType == balType.Coins)
				//		current_item.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CoinsSprite;

				//	else if (x.CostType == balType.Diamonds)
				//		current_item.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = DiamondsSprite;

				//	current_index++;
				//}
				#endregion

				break;

			case shopType.Pipes:
				//highlight tab
				ToggleColor(PipesTabButton, true);

				//Populate the shop tab
				foreach (ShopItem x in Pipes) {
					{
						GameObject current_item = (GameObject)Instantiate(
							PipePrefab,
							new Vector3(
								borders.transform.position.x,
								borders.transform.position.y,
								0
							),
							Quaternion.identity,
							borders.transform
						);

						//spawned object size in canvas
						var LPOX = current_item.GetComponent<RectTransform>().sizeDelta;

						//We set the canvas position of the current element
						current_item.GetComponent<RectTransform>().localPosition = new Vector3(
							0,
							-bordersRect.rect.y - LPOX.y / 2 - current_index * (LPOX.y + VerticalSpacing) - VerticalSpacing * 2,
							0
						);

						lastitem = current_item;

						//preview
						current_item.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = x.preview;

						//cost
						if (x.cost == 0) //remove price tag if the item is default (ie. it's cost is 0)
							Destroy(current_item.transform.GetChild(1).gameObject);
						else
							current_item.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.cost.ToString();

						//cost type
						if (x.CostType == balType.Coins)
							current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CoinsSprite;

						else if (x.CostType == balType.Diamonds)
							current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = DiamondsSprite;

						//button
						var btnobj = current_item.transform.GetChild(2).gameObject;

						//cost container
						var cst = current_item.transform.GetChild(1).gameObject;

						cst.SetActive(false);
						if (isEquipped(x.id))
							setButton(btnobj, buttonType.equipped, x);
						else if (isPurchased(x.id))
							setButton(btnobj, buttonType.equip, x);
						else {
							cst.SetActive(true);
							setButton(btnobj, buttonType.buy, x);
						}

						current_index++;
					}
				}

				#region Old Code
				//foreach (ShopItem x in Pipes) {
				//	//we cast to gameobject cause unity dumb and can't self convert it
				//	GameObject current_item = (GameObject)Instantiate(
				//		PipePrefab,
				//		new Vector3(
				//			borders.transform.position.x,
				//			bordersRect.position.y - (PipePrefabHeight / 2) - (current_index * PipePrefabHeight),
				//			0
				//		),
				//		Quaternion.identity,
				//		borders.transform
				//	);

				//	lastitem = current_item;

				//	//preview
				//	current_item.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = x.preview;

				//	//cost
				//	if (x.cost == 0) //remove price tag if the item is default (ie. it's cost is 0)
				//		Destroy(current_item.transform.GetChild(1).gameObject);
				//	else
				//		current_item.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.cost.ToString();

				//	//cost type
				//	if (x.CostType == balType.Coins)
				//		current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CoinsSprite;
					
				//	else if (x.CostType == balType.Diamonds)
				//		current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = DiamondsSprite;
					
				//	current_index++;
				//}
				#endregion

				break;

			case shopType.Grounds:
				//highlight tab
				ToggleColor(GroundsTabButton, true);

				//Populate the shop tab
				foreach (ShopItem x in Grounds) {
					{
						GameObject current_item = (GameObject)Instantiate(
							GroundPrefab,
							new Vector3(
								borders.transform.position.x,
								borders.transform.position.y,
								0
							),
							Quaternion.identity,
							borders.transform
						);

						//spawned object size in canvas
						var LPOX = current_item.GetComponent<RectTransform>().sizeDelta;

						//We set the canvas position of the current element
						current_item.GetComponent<RectTransform>().localPosition = new Vector3(
							0,
							-bordersRect.rect.y - LPOX.y / 2 - current_index * (LPOX.y + VerticalSpacing) - VerticalSpacing * 2,
							0
						);

						lastitem = current_item;

						//preview
						current_item.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = x.preview;

						//cost
						if (x.cost == 0) //remove price tag if the item is default (ie. it's cost is 0)
							Destroy(current_item.transform.GetChild(1).gameObject);
						else
							current_item.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.cost.ToString();

						//cost type
						if (x.CostType == balType.Coins)
							current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CoinsSprite;

						else if (x.CostType == balType.Diamonds)
							current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = DiamondsSprite;

						//button
						var btnobj = current_item.transform.GetChild(2).gameObject;

						//cost container
						var cst = current_item.transform.GetChild(1).gameObject;

						cst.SetActive(false);
						if (isEquipped(x.id))
							setButton(btnobj, buttonType.equipped, x);
						else if (isPurchased(x.id))
							setButton(btnobj, buttonType.equip, x);
						else {
							cst.SetActive(true);
							setButton(btnobj, buttonType.buy, x);
						}

						current_index++;
					}
				}

				#region Old Code
					//	foreach (ShopItem x in Grounds) {
					//	//we cast to gameobject cause unity dumb and can't self convert it
					//	GameObject current_item = (GameObject)Instantiate(
					//		GroundPrefab,
					//		new Vector3(
					//			borders.transform.position.x,
					//			bordersRect.position.y - (GroundPrefabHeight / 2) - (current_index * GroundPrefabHeight),
					//			0
					//		),
					//		Quaternion.identity,
					//		borders.transform
					//	);

					//	lastitem = current_item;

					//	//preview
					//	current_item.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = x.preview;

					//	//cost
					//	if (x.cost == 0) //remove price tag if the item is default (ie. it's cost is 0)
					//		Destroy(current_item.transform.GetChild(1).gameObject);
					//	else
					//		current_item.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.cost.ToString();

					//	//cost type
					//	if (x.CostType == balType.Coins) {
					//		current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CoinsSprite;
					//	}
					//	else if (x.CostType == balType.Diamonds) {
					//		current_item.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = DiamondsSprite;
					//	}
					//	current_index++;
					//}
					#endregion

				break;

			default:
				Debug.LogError("Supplied invalid shopType in PopulateShopTabs();");
				break;
		}

		// ############################### STILL BROKEN, DOES NOT SET CORRECT HEIGHT.
		//bordersRect.sizeDelta = new Vector2(
		//	bordersRect.sizeDelta.x,
		// This calculates the height of the borders canvas, so it becomes scrollable by the SkinsTab's ScrollRect
		//	lastitem.GetComponent<RectTransform>().sizeDelta.y * current_index + (lastitem.GetComponent<RectTransform>().sizeDelta.y / 2) * 1.5f
		// I literally have no fucking idea what happened in unity's design department.
		// They were literally like: Yeah, let's just fuck canvases
		// have them on separate coordinate systems
		// for no fucking reason.
		//);

		var RPOX = lastitem.GetComponent<RectTransform>().sizeDelta;
		bordersRect.sizeDelta = new Vector2(
			bordersRect.sizeDelta.x,
			current_index * (RPOX.y + VerticalSpacing) + VerticalSpacing * 4
		);

		//scroll the shop tab to the top after populating it if we're not in the same shop tab.
		if (currentShop != st)
			ShopInsideContainer.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;

		currentShop = st;

	}
}