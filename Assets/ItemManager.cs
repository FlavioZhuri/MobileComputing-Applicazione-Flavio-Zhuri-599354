using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    [Space(10)]
    [Header("Game Items")]
    public List<ShopItem> Skins;
    public List<ShopItem> Backgrounds;
    public List<ShopItem> Pipes;
    public List<ShopItem> Grounds;

    //used for optimization
    IEnumerable<ShopItem> AllItems;

    //If the ItemManager is already spawned.
    static bool isAlreadyPresent = false;

    //Always keep loaded
    private void Awake() {

        //make sure that we don't infinitely start creating ItemManagers
        if (!isAlreadyPresent) {
            DontDestroyOnLoad(this.gameObject);
            isAlreadyPresent = true;
        } else {
            Destroy(this.gameObject);
        }

        //merge all items
        AllItems = Skins.Concat(Backgrounds.Concat(Pipes.Concat(Grounds)));
    }

	public ShopItem GetItem(int id) {
        ShopItem ret = AllItems.FirstOrDefault(it => it.id == id);
        return ret;
	}

    public List<ShopItem> GetAllItems(shopType st) {
        switch (st) {
            case shopType.Skins:
                return Skins;
            case shopType.Backgrounds:
                return Backgrounds;
            case shopType.Pipes:
                return Pipes;
            case shopType.Grounds:
                return Grounds;
            default:
                return null;
        }
	}
}
