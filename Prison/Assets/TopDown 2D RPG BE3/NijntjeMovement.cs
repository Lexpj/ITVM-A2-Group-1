using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NijntjeMovement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject flashlight;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        Vector3 mouseWorldPosition;
        Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;

        animator.SetFloat("Horizontal", mouseWorldPosition.x);
        animator.SetFloat("Vertical", mouseWorldPosition.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
