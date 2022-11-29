using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reachEndPoint : MonoBehaviour
{
    private PrisonerManager targetObj;
    private bool reached = false;

    private void Start()
    {
        targetObj = GameObject.FindGameObjectWithTag("PrisonerManager").GetComponent<PrisonerManager>();
    }

    public void PrisonerEscaped()
    {
        if (!reached)
        {
            targetObj.PrisonerEscaped();
            reached = true;
        }
    }
}
