using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleeing : MonoBehaviour
{
    [SerializeField] AIMovement Prisoner;
    private bool fleeing = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!fleeing)
        {
            if (other.CompareTag("Guard"))
            {
                Prisoner.SetGuardInRange(true);
                Prisoner.SetGuard(other.transform);
                fleeing = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Guard"))
        {
            Prisoner.SetGuardInRange(false);
            fleeing = false;
        }
    }
}
