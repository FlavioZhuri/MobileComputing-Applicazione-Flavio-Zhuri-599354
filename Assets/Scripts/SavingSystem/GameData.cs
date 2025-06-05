using System.Collections.Generic;

[System.Serializable]
public class GameData {

	//currencies
	public int Coins;
	public int Diamonds;

	//stats
	public int HighScore;
	public int Deaths;
	public int CollectedCoinsAllTime;
	public int CollectedDiamondsAllTime;

	//Persist selected items
	public int SelectedSkin;
	public int SelectedBackground;
	public int SelectedPipe;
	public int SelectedGround;

	public List<int> PurchasedItems;

	public void LoadDefaults() {
		Coins = 0;
		Diamonds = 0;
		HighScore = 0;
		Deaths = 0;
		CollectedCoinsAllTime = 0;
		CollectedDiamondsAllTime = 0;

		SelectedSkin = 1;
		SelectedBackground = 100;
		SelectedPipe = 200;
		SelectedGround = 300;

		PurchasedItems = new List<int>() { 1, 100, 200, 300 }; //default items
	}

	public bool IsPurchased(int id) { return PurchasedItems.Contains(id); }
	public void AddItem(int id) { PurchasedItems.Add(id); }
	public void RemoveItem(int id) { PurchasedItems.Remove(id); }
	public void RemoveAllItems(int id) { PurchasedItems.Clear(); }
}
