using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMovement : MonoBehaviour
{
    float speed = .01f;
    bool cantMove;
    public Animator anim;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        gameObject.transform.position = new Vector3(transform.position.x + (horizontal * -speed), -2.4f, transform.position.z + (vertical * -speed));
    }
}
