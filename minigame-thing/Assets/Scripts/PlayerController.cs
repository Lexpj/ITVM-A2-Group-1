using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    private Transform player;

    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        if (movementInput != Vector2.zero) {
            int count = rb.Cast(
                movementInput,
                movementFilter,
                castCollisions,
                moveSpeed = collisionOffset
            );
            if (count == 0) {
                rb.MovePosition(rb.position + movementInput * moveSpeed);
            }
        }
        // WIN:
        if (player.transform.position.y >= 6.5)
            Debug.Log("WINNER!!!");
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
}
