using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InAttackRange : MonoBehaviour
{
    public GuardAI guardscript;
    private bool attacking = false;
    public List<Vector3> positions = new List<Vector3>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Health>() != null)
        {
            if (other.gameObject.GetComponent<Health>().GetHealthState() == "Knocked")
            {
                if (other.CompareTag("Player"))
                {
                    if (!other.gameObject.GetComponent<CharacterMovementNoGrid>().isCaptured() && !guardscript.IsHolding())
                    {
                        other.gameObject.GetComponent<CharacterMovementNoGrid>().capture(true);
                        guardscript.IsCapturing(true);
                        guardscript.kindtocapture("Player");
                    }
                }
                if (other.CompareTag("PrisonerAI"))
                {
                    if (!other.gameObject.GetComponent<AIMovement>().isCaptured() && !guardscript.IsHolding())
                    {
                        other.gameObject.GetComponent<AIMovement>().capture(true);
                        guardscript.IsCapturing(true);
                        guardscript.kindtocapture("AI");
                    }
                }
            }
            else if (!attacking)
            {
                if (other.CompareTag("Player"))
                {
                    if (!other.gameObject.GetComponent<CharacterMovementNoGrid>().isCaptured() && !guardscript.IsHolding())
                    {
                        guardscript.SetTargetInAttackRange(true);
                        guardscript.SetTarget(other.transform);
                        attacking = true;
                    }
                }
                if (other.CompareTag("PrisonerAI"))
                {
                    if (!other.gameObject.GetComponent<AIMovement>().isCaptured() && !guardscript.IsHolding())
                    {
                        guardscript.SetTargetInAttackRange(true);
                        guardscript.SetTarget(other.transform);
                        attacking = true;
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Health>() != null)
        {
            if (other.gameObject.GetComponent<Health>().GetHealthState() == "Knocked")
            {
                if (other.CompareTag("Player"))
                {
                    if (!other.gameObject.GetComponent<CharacterMovementNoGrid>().isCaptured() && !guardscript.IsHolding())
                    {
                        other.gameObject.GetComponent<CharacterMovementNoGrid>().capture(true);
                        guardscript.IsCapturing(true);
                        guardscript.kindtocapture("Player");
                    }
                }
                if (other.CompareTag("PrisonerAI"))
                {
                    if (!other.gameObject.GetComponent<AIMovement>().isCaptured() && !guardscript.IsHolding())
                    {
                        other.gameObject.GetComponent<AIMovement>().capture(true);
                        guardscript.IsCapturing(true);
                        guardscript.kindtocapture("AI");
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PrisonerAI"))
        {
            guardscript.SetTargetInAttackRange(false);
            guardscript.SetTargetInRange(true);
            attacking = false;
        }
    }
}