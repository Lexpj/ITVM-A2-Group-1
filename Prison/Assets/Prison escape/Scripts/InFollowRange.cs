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
            if (other.CompareTag("Player") || other.CompareTag("PrisonerAI"))
            {
                guardscript.SetTargetInRange(true);
                guardscript.SetTarget(other.transform);
                following = true;
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
