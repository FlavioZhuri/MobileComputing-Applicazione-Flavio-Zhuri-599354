using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMover : MonoBehaviour {
    public float Speed = 1f;
    void FixedUpdate() { transform.position += Vector3.left * Speed * Time.deltaTime; }
}
