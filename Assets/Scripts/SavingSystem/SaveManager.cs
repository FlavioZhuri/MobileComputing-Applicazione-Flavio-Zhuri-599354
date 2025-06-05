using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    //If the SaveManager is already loaded.
    static bool isLoaded = false;

    //Always keep loaded
    private void Awake() {
        //make sure that we don't infinitely start creating Save Managers
        if (!isLoaded) {
            DontDestroyOnLoad(this.gameObject);
            isLoaded = true;
        } else {
            Destroy(this.gameObject);
        }
    }

    //Load/Create save on start
    void Start() { SaveSystem.LoadGame(); }
}
