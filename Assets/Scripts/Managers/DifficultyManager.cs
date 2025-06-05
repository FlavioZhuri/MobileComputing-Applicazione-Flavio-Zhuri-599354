using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DifficultyManager : MonoBehaviour
{

    public bool DifficultyPick = false;


    public GameManager GM;
    public InGameMenuManager IGM;
    public PipeSpawner PIPE_SPAWNER;

    public GameObject DifficultyChoise;


    private int IntenseCap = 20;
    private int AXCap = 75;
    private int ABYSSCap = 150;


    private void Start()
    {
        DiffUnlock();
    }



    public void DiffUnlock()
    {
        if (SaveSystem.SAVE.HighScore >= IntenseCap) {

            if (SaveSystem.SAVE.HighScore >= IntenseCap && SaveSystem.SAVE.HighScore < AXCap)
            {
                DifficultyChoise.transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (SaveSystem.SAVE.HighScore >= AXCap && SaveSystem.SAVE.HighScore < ABYSSCap)
            {
                DifficultyChoise.transform.GetChild(1).gameObject.SetActive(false);
                DifficultyChoise.transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                DifficultyChoise.transform.GetChild(1).gameObject.SetActive(false);
                DifficultyChoise.transform.GetChild(2).gameObject.SetActive(false);
                DifficultyChoise.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
    }


    public void NormalDiff()
    {
        PIPE_SPAWNER.spawnTime = 2;
        PIPE_SPAWNER.pipeTightness = 500;
        PIPE_SPAWNER.pipeTurbulence = 2.15f;
        PIPE_SPAWNER.pipeSpeed = 2.5f;

        PIPE_SPAWNER.chanceCoins = 650;
        PIPE_SPAWNER.chanceDiamonds = 20;

        GM.coinvalue = 1;

        DifficultyPick = true;
        IGM.hideDifficultyP();
        IGM.showAwaiting();
    }

    public void IntenseDiff()
    {
        PIPE_SPAWNER.spawnTime = 1f;
        PIPE_SPAWNER.pipeTightness = 485f;
        PIPE_SPAWNER.pipeTurbulence = 2.15f;
        PIPE_SPAWNER.pipeSpeed = 5f;

        PIPE_SPAWNER.chanceCoins = 650;
        PIPE_SPAWNER.chanceDiamonds = 40;

        GM.coinvalue = 2;


        DifficultyPick = true;
        IGM.hideDifficultyP();
        IGM.showAwaiting();

    }

    public void AlexithymiaDiff()
    {
        PIPE_SPAWNER.spawnTime = 0.5f;
        PIPE_SPAWNER.pipeTightness = 475f;
        PIPE_SPAWNER.pipeTurbulence = 2.15f;
        PIPE_SPAWNER.pipeSpeed = 7f;

        PIPE_SPAWNER.chanceCoins = 300;
        PIPE_SPAWNER.chanceDiamonds = 60;

        GM.coinvalue = 3;

        DifficultyPick = true;
        IGM.hideDifficultyP();
        IGM.showAwaiting();
    }

    public void AbyssDiff()
    {
        PIPE_SPAWNER.spawnTime = 0.5f;
        PIPE_SPAWNER.pipeTightness = 465f;
        PIPE_SPAWNER.pipeTurbulence = 2.15f;
        PIPE_SPAWNER.pipeSpeed = 8f;

        PIPE_SPAWNER.chanceCoins = 100;
        PIPE_SPAWNER.chanceDiamonds = 100;

        GM.coinvalue = 5;


        DifficultyPick = true;
        IGM.hideDifficultyP();
        IGM.showAwaiting();
    }








}
