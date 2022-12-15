using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CharacterMovementNoGrid : MonoBehaviour
{
    [SerializeField] private Transform aimTransform;
    [SerializeField] private FieldOfView fieldOfView;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;
    [SerializeField] private Health health;
    [SerializeField] private GameObject knockedParticles;

    private void HandleAim()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - aimTransform.position).normalized;

        fieldOfView.SetAimDirection(aimDirection);
        fieldOfView.SetOrigin(transform.position);

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (angle > 45 && angle < 135)
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 1);
        }
        if (angle < -135 || angle > 135)
        {
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", 0);
        }
        if (angle > -45 && angle < 45)
        {
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("Vertical", 0);
        }
        if (angle > -135 && angle < -45)
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", -1);
        }
    }

    private void HandleMovement()
    {
        if(health.GetHealthState() != "Knocked")
        {
            knockedParticles.SetActive(false);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    movement.y = +1f;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    movement.y = -1f;
                }
            }
            else
            {
                movement.y = Input.GetAxisRaw("Vertical");
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    movement.x = -1f;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    movement.x = +1f;
                }
            }
            else
            {
                movement.x = Input.GetAxisRaw("Horizontal");
            }

            movement = movement.normalized;
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        else
        {
            knockedParticles.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAim();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
