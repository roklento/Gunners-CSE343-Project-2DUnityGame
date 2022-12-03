using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public Rigidbody2D rgb;
    Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 horizontal;
        float speed = 4f;
        velocity = rgb.velocity;
        horizontal = new Vector2(Input.GetAxis("Horizontal"), 0f);

        velocity.x = horizontal.x * speed;

        rgb.velocity = velocity;
    }
}
