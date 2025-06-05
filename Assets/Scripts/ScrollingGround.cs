
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingGround : MonoBehaviour
{
    public Transform target;
    public GameObject GroundContainer;
    public GameObject[] GroundGen;

    private float bg1;
    private float bg2;

    private GameObject[] InstantiatedBg;

    void Start()
    {
        bg1 = 3.5f;
        bg2 = 16.35f;

        InstantiatedBg = new GameObject[2];


        InstantiatedBg[0] = Instantiate(GroundGen[0], new Vector3(bg1, -4.5f, 0), Quaternion.identity);
        InstantiatedBg[1] = Instantiate(GroundGen[0], new Vector3(bg2, -4.5f, 0), Quaternion.identity);
        InstantiatedBg[0].transform.SetParent(GroundContainer.transform, true);
        InstantiatedBg[1].transform.SetParent(GroundContainer.transform, true);

    }

    void FixedUpdate()
    {
        if (this.transform.position.x >= InstantiatedBg[1].transform.position.x - 4f)
        {
            Destroy(InstantiatedBg[0]);
            InstantiatedBg[0] = Instantiate(GroundGen[0], new Vector3(bg2, -4.5f, 0), Quaternion.identity);
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
