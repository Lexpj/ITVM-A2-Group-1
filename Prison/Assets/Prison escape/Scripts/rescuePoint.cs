using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rescuePoint : MonoBehaviour
{
    private bool prisonerInCel = false;
    private GameObject capturedPrisoner;
    private string kindCaptured;
    [SerializeField] Occupied celOccupied;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (celOccupied)
        {
            if (!prisonerInCel)
            {
                if (other.CompareTag("Player"))
                {
                    capturedPrisoner = other.gameObject;
                    kindCaptured = "Player";
                }
                if (other.CompareTag("PrisonerAI"))
                {
                    capturedPrisoner = other.gameObject;
                    kindCaptured = "AI";
                }
            }
            else
            {
                if (kindCaptured == "Player")
                {
                    capturedPrisoner.GetComponent<CharacterMovementNoGrid>().capture(false);
                    celOccupied.SetOccupied(false);
                    prisonerInCel = false;
                }
                if (kindCaptured == "AI")
                {
                    capturedPrisoner.GetComponent<AIMovement>().capture(false);
                    celOccupied.SetOccupied(false);
                    prisonerInCel = false;
                }
            }
        }
    }
}
