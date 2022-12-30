using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InFollowRange : MonoBehaviour
{
    public GuardAI guardscript;
    private bool following = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!following)
        {
            if (other.CompareTag("Player"))
            {
                if (!other.gameObject.GetComponent<CharacterMovementNoGrid>().isCaptured())
                {
                    guardscript.SetTargetInRange(true);
                    guardscript.SetTarget(other.transform);
                    following = true;
                }
            }
            if (other.CompareTag("PrisonerAI"))
            {
                if (!other.gameObject.GetComponent<AIMovement>().isCaptured())
                {
                    guardscript.SetTargetInRange(true);
                    guardscript.SetTarget(other.transform);
                    following = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (following)
        {
            if (other.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<CharacterMovementNoGrid>().isCaptured())
                {
                    guardscript.SetTargetInRange(false);
                    following = false;
                }
            }
            if (other.CompareTag("PrisonerAI"))
            {
                if (other.gameObject.GetComponent<AIMovement>().isCaptured())
                {
                    guardscript.SetTargetInRange(false);
                    following = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PrisonerAI"))
        {
            guardscript.SetTargetInRange(false);
            following = false;
        }
    }
}