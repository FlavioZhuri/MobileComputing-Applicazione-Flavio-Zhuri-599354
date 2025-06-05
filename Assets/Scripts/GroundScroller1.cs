
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScroller1 : MonoBehaviour
{
    public Transform target;
    public Transform Floor;

    public GameObject[] MapGen;

    private float bg1;
    private float bg2;

    private GameObject[] InstantiatedBg;


    void Start()
    {
        bg1 = 0;
        bg2 = 18;

        InstantiatedBg = new GameObject[2];


        InstantiatedBg[0] = Instantiate(MapGen[0], new Vector3(bg1, 0, 0), Quaternion.identity);
        InstantiatedBg[1] = Instantiate(MapGen[Random.Range(1, MapGen.Length)], new Vector3(bg2, 0, 0), Quaternion.identity);
    }

    void FixedUpdate()
    {


        if (Camera.main.transform.position.x < target.position.x + 3)
        {
            Vector3 targetPos = new Vector3(target.position.x + 3, 0, transform.position.z);

            //FloorDistance = transform.position.x - Floor.transform.position.x;

            transform.position = Vector3.Lerp(targetPos, transform.position, 0.4f);

            /* Vector2 CurrentFloorY = new Vector2(0, transform.position.x - FloorDistance);
             Floor.transform.position = CurrentFloorY;*/
        }

        if (this.transform.position.x >= InstantiatedBg[1].transform.position.x)
        {
            Destroy(InstantiatedBg[0]);
            InstantiatedBg[0] = Instantiate(MapGen[Random.Range(1, MapGen.Length)], new Vector3(InstantiatedBg[1].transform.position.x + bg2, 0, 0), Quaternion.identity);
            SwichBg();
        }

    }

    private void SwichBg()
    {
        GameObject temp = InstantiatedBg[0];
        InstantiatedBg[0] = InstantiatedBg[1];
        InstantiatedBg[1] = temp;
    }

}
