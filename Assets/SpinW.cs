using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinW : MonoBehaviour {

    [Range(-180.0f, 180.0f)]
    public float Speed = 15f;

	private void Start() {
        InvokeRepeating("SpinM", 0, 0.2f);
    }

	private void SpinM() {
        GetComponent<RectTransform>().transform.Rotate(Vector3.forward * Speed);
    }
}
