using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float JumpVelocity = 0.65f;
    public float Gravity = 1.5f;
    public GameObject GAME_MANAGER;
    public bool isFirstClick = true;
    public bool isGameActive = false;
    public Vector3 s_location;
    public Quaternion s_rotation;

    private Rigidbody2D RB;
    private GameManager GM;
    private DifficultyManager DM;


    void Start() {
        RB = GetComponent<Rigidbody2D>();
        GM = GAME_MANAGER.GetComponent<GameManager>();
        DM = GAME_MANAGER.GetComponent<DifficultyManager>();
        s_location = this.transform.position;
        s_rotation = this.transform.rotation;
        FreezeRigidBody();
    }

    public void InitiateJump() {
        RB.velocity = Vector2.up * JumpVelocity;
        //this.transform.Rotate(0,0,-30);
    }

    //also acts as a reset.
    public void FreezeRigidBody() {
        RB.gravityScale = 0;
        RB.velocity = new Vector3(0f, 0f, 0f);
        RB.angularVelocity = 0f;
    }
    public void UnfreezeRigidBody() { RB.gravityScale = Gravity; }

    void Update() {


        if (Input.GetMouseButtonDown(0))
        {
            if (DM.DifficultyPick)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform.name == "Pause Button")
                {
                    Debug.Log("sd");
                }
                else
                {

                    if (isFirstClick)
                    {
                       GM.StartGame();
                       isFirstClick = false;
                    }

                     if (isGameActive)
                       InitiateJump();

                }
            }
        }


    }

    //hit a trigger (score counter,coin,diamond..)
    private void OnTriggerEnter2D(Collider2D collision) {
        switch (collision.gameObject.tag) {
            case "Coin":
                GM.UpdateCoins();
                Destroy(collision.gameObject);
                break;
            case "Diamond":
                GM.UpdateDiamonds();
                Destroy(collision.gameObject);
                break;
            case "sCounter":
                GM.UpdateScore();
                break;
            default:
                return;
		}
    }

    //hit the ground
    private void OnCollisionEnter2D(Collision2D collision) {
        GM.GameOver();
    }
}
