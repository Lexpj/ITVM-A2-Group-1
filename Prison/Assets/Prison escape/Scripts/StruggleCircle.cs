using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StruggleCircle : MonoBehaviour
{ 
    private float struggleCounter = 0f;
    [SerializeField] private GameObject struggleCircleInner;

    // Update is called once per frame
    private void Update()
    {
        struggleCounter -= 0.5f;

        if (struggleCounter < 0)
        {
            struggleCounter = 0f;
        }

        struggleCircleInner.transform.localScale = new Vector3(struggleCounter / 100, struggleCounter / 100, 1);
    }

    public void ButtonPressed()
    {
        struggleCounter += 27f;
        struggleCircleInner.transform.localScale = new Vector3(struggleCounter / 100, struggleCounter / 100, 1);
    }

    public float getCounter()
    {
        return struggleCounter;
    }

    public void resetStruggleCounter()
    {
        struggleCounter = 0f;
    }
}
