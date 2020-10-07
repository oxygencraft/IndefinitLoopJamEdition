using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10f;
    public new Rigidbody2D rigidbody;

    private Vector2 movement;

    void Update()
    {
        Vector2 input = Vector2.zero;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        movement = input * movementSpeed;
    }

    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + movement * Time.fixedDeltaTime);
    }
}
