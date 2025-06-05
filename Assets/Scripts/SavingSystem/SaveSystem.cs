using System.Collections;
using System.IO;
using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    private static string SaveFile = "save.bin";
	public static GameData SAVE;
    public static bool SAVE_LOADED = false;

	public static void SaveGame() {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath + "/" + SaveFile);
        FileStream stream = new FileStream(path, FileMode.Create);

        //If save is empty load defaults (will also catch when in LoadGame empty)
        if (SAVE == null) {
            SAVE = new GameData();
            SAVE.LoadDefaults();
        }
		
        formatter.Serialize(stream, SAVE);
        stream.Close();

    }

    public static void LoadGame () {
        string path = Path.Combine(Application.persistentDataPath + "/" + SaveFile);

        //If save exists
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //try opening the save file
            try {
                SAVE = formatter.Deserialize(stream) as GameData;
                stream.Close();
                SAVE_LOADED = true;

            //if it fails create a new one
            } catch {
                //close stream if file is missing to avoid exceptions
                stream.Close();
                SaveGame();
                SAVE_LOADED = true;
            }

        //if save doesn't exist create a new one
        } else {
            SaveGame();
            SAVE_LOADED = true;
        }
    }
}
