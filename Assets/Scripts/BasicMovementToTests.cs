using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicMovementToTests : MonoBehaviour {

    public float mSpeed = 10;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {

    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move2(h, v);
    }

    void Move2(float h, float v)
    {
        Vector3 movement = Vector3.zero;
        movement.Set(h, 0f, v);
        movement = movement.normalized * mSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }
}

