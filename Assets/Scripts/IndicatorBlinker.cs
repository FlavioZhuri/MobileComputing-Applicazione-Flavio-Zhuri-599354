using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorBlinker : MonoBehaviour {

    public float speed = 0.2f;
    public Sprite Sprite1;
    public Sprite Sprite2;

    private bool flagState = false;

    private void Start() { InvokeRepeating("Indicate", 0, speed); }

    private void Indicate() {
        if (flagState) {
            this.GetComponent<Image>().sprite = Sprite1;
            flagState = false;
        } else {
            this.GetComponent<Image>().sprite = Sprite2;
            flagState = true;
        }
    }
}
