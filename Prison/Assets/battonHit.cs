using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battonHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PrisonerAI"))
        {
            if(other.gameObject.GetComponent<Health>() != null)
            {
                other.gameObject.GetComponent<Health>().Damage(50);
            }
        }
    }
}
