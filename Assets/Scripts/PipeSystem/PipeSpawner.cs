using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour {

    [Header("Difficulty Settings")]
    public float spawnTime;
    public float pipeTightness;
    public float pipeTurbulence;
    public float destroyPipeAfter = 7;
    public float pipeSpeed = 2.5f;
    [Space(10)]
    [Header("RNG Coins")]
    public int chanceCoins = 0;
    [Space(10)]
    [Header("RNG Diamonds")]
    public int chanceDiamonds = 0;
    [Space(10)]
    public GameObject pipePrefab;
    public GameObject pipeContainer;

    public bool isSpawning = false;

    private ItemManager imrg;
    private Sprite selpipe;

    // Start is called before the first frame update
    void Start() {
        if (!SaveSystem.SAVE_LOADED) {
            Debug.LogError("GAME DATA MANAGER IS MISSING, ABORTING @ PIPE SPAWNER");
            return;
		}

        InvokeRepeating("SpawnPipe", 0, spawnTime);

        //get the required data from the ItemManager
        imrg = GameObject.Find("_GAME DATA MANAGER").GetComponent<ItemManager>();

        //get selected pipe from the save
        selpipe = imrg.GetItem(SaveSystem.SAVE.SelectedPipe).preview;
    }

    private void SpawnPipe() {
        if (!isSpawning) return;

        GameObject spawnedPipe = Instantiate(pipePrefab);

        //move to the pipe container
        spawnedPipe.transform.SetParent(pipeContainer.transform, true);

        //set pipe movement speed
        spawnedPipe.GetComponent<PipeMover>().Speed = pipeSpeed;

        //top pipe
        var topPipe = spawnedPipe.transform.GetChild(0).gameObject;
        //sprite
        topPipe.GetComponent<SpriteRenderer>().sprite = selpipe;
        //tightness
        topPipe.transform.position = new Vector3(
            topPipe.transform.position.x,
            pipeTightness / 100,
            topPipe.transform.position.z
        ); 

        //bottom pipe
        var bottomPipe = spawnedPipe.transform.GetChild(1).gameObject;
        //sprite
        bottomPipe.GetComponent<SpriteRenderer>().sprite = selpipe;
        //tightness
        bottomPipe.transform.position = new Vector3(
            bottomPipe.transform.position.x,
            -pipeTightness / 100,
            bottomPipe.transform.position.z
        );

        //coin
        var cn = spawnedPipe.transform.GetChild(3).gameObject;
        //diamond
        var dm = spawnedPipe.transform.GetChild(4).gameObject;

        //callibration
        cn.transform.position = new Vector3(cn.transform.position.x, cn.transform.position.y - 0.6f);
        dm.transform.position = new Vector3(dm.transform.position.x, dm.transform.position.y - 0.6f);

        //rng
        int rngNumber = Random.Range(0, 1000);

        if (rngNumber < chanceCoins+ chanceDiamonds && rngNumber>chanceDiamonds) { cn.SetActive(true); }

        if (rngNumber < chanceDiamonds) {
            dm.SetActive(true);
            if (cn.activeSelf)
                cn.SetActive(false);
		}

        //turbulence
        spawnedPipe.transform.position = transform.position + new Vector3(10, Random.Range(-pipeTurbulence, pipeTurbulence), +100);

        //destroy
        Destroy(spawnedPipe, destroyPipeAfter);
    }
}
