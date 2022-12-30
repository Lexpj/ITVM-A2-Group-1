using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occupied : MonoBehaviour
{
    public bool occupied = false;
    private bool prisonerCaught = false;
    private GameObject caughtPrisoner;

    public bool isOccupied()
    {
        return occupied;
    }

    public void SetOccupied(bool occupy)
    {
        occupied = occupy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (occupied && !prisonerCaught)
        {
            if (other.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<CharacterMovementNoGrid>().isCaptured())
                {
                    Debug.Log("Saved player in cel");
                    prisonerCaught = true;
                    caughtPrisoner = other.gameObject;
                }
            }
            if (other.CompareTag("PrisonerAI"))
            {
                if (other.gameObject.GetComponent<AIMovement>().isCaptured())
                {
                    Debug.Log("Saved AI in cel");
                    prisonerCaught = true;
                    caughtPrisoner = other.gameObject;
                }
            }
        }
    }

    public bool PrisonerInsideCel()
    {
        return prisonerCaught;
    }

    public GameObject CaughtPrisoner()
    {
        return caughtPrisoner;
    }

    public void ResetCel()
    {
        occupied = false;
        prisonerCaught = false;
        caughtPrisoner = null;
        Debug.Log("reset cel");
    }
}
