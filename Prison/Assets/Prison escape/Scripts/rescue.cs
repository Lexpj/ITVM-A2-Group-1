using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rescue : MonoBehaviour
{
    public Occupied cel;
    private GameObject[] prisonersToNotify;
    private bool notified = false;
    private AIMovement notifiedPrisoner;
    private string rescuerKind;
    private bool atRescuePoint = false;
    private float counter = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!notified && cel.PrisonerInsideCel())
        {
            notifyPrisoner();
        }

        if (cel.PrisonerInsideCel())
        {
            if (rescuerKind == "Player" && atRescuePoint)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    rescued();
                }
            }
            if (rescuerKind == "PrisonerAI" && atRescuePoint)
            {
                counter += Time.deltaTime;

                if (counter >= 1)
                {
                    rescued();
                    counter = 0f;
                }
            }
        }
        if (!atRescuePoint)
        {
            counter = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        atRescuePoint = true;

        if (other.CompareTag("Player"))
        {
            rescuerKind = "Player";
        }
        else if (other.CompareTag("PrisonerAI"))
        {
            rescuerKind = "PrisonerAI";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        atRescuePoint = false;
        rescuerKind = null;
    }

    private void notifyPrisoner()
    {
        prisonersToNotify = GameObject.FindGameObjectsWithTag("PrisonerAI");
        int bestPrisoner = -1;
        float bestDistance = 10000;

        for (int i = 0; i < prisonersToNotify.Length; i++)
        {
            if (!prisonersToNotify[i].GetComponent<AIMovement>().isCaptured() && !prisonersToNotify[i].GetComponent<AIMovement>().isRescueing())
            {
                float rescueDistance = Vector3.Distance(transform.position, prisonersToNotify[i].transform.position);
                if (rescueDistance < bestDistance)
                {
                    bestPrisoner = i;
                    bestDistance = rescueDistance;
                }
            }
        }

        if(bestPrisoner > -1)
        {
            prisonersToNotify[bestPrisoner].GetComponent<AIMovement>().setRescueing(true, this.gameObject);
            notifiedPrisoner = prisonersToNotify[bestPrisoner].GetComponent<AIMovement>();
            notified = true;
        }
    }

    private void rescued()
    {
        if(cel.PrisonerInsideCel())
        {
            if(cel.CaughtPrisoner() != null)
            {
                if (cel.CaughtPrisoner().CompareTag("Player"))
                {
                    cel.CaughtPrisoner().GetComponent<CharacterMovementNoGrid>().capture(false);
                    cel.ResetCel();
                    if(notifiedPrisoner != null)
                    {
                        notifiedPrisoner.setRescueing(false, null);
                        notifiedPrisoner = null;
                    }
                }
                else if (cel.CaughtPrisoner().CompareTag("PrisonerAI"))
                {
                    cel.CaughtPrisoner().GetComponent<AIMovement>().capture(false);
                    cel.ResetCel();
                    if(notifiedPrisoner != null)
                    {
                        notifiedPrisoner.setRescueing(false, null);
                        notifiedPrisoner = null;
                    }
                }
            }
        }
    }

    public void unnotify()
    {
        notified = false;
    }
    
}
