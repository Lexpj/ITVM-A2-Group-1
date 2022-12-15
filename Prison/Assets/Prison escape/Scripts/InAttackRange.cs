using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAttackRange : MonoBehaviour
{
    public GuardAI guardscript;
    private bool attacking = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!attacking)
        {
            if (other.CompareTag("Player") || other.CompareTag("PrisonerAI"))
            {
                guardscript.SetTargetInAttackRange(true);
                guardscript.SetTarget(other.transform);
                attacking = true;
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
