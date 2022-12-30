using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] CharacterMovementNoGrid playerscript;
    [SerializeField] Transform movePoint;
    [SerializeField] Transform sphere;
    [SerializeField] Transform icon;
    [SerializeField] GuardAI guardscript;
    [SerializeField] Transform guardLocation;
    [SerializeField] private GameObject struggleCircle;

    private bool captured = false;
    private bool broughtToCel = false;

    // Update is called once per frame
    void Update()
    {
        sphere.position = movePoint.position;
        icon.position = movePoint.position;

        if(player.activeSelf)
        {
            if (playerscript.isCaptured() && !broughtToCel)
            {
                player.SetActive(false);
                captured = true;
                struggleCircle.SetActive(true);
            }
            if (!playerscript.isCaptured())
            {
                captured = false;
                broughtToCel = false;
                struggleCircle.SetActive(false);
            }
        }

        if (captured)
        {
            movePoint.position = guardLocation.position;
            HandleStruggling();
        }

        if(!guardscript.IsHolding() && !player.activeSelf)
        {
            StartCoroutine("enablePlayer");
        }
        
    }

    IEnumerator enablePlayer()
    {
        captured = false;
        broughtToCel = true;
        struggleCircle.SetActive(false);

        yield return new WaitForSeconds(1);

        player.SetActive(true);
    }

    IEnumerator enablePlayerAndStopCapture()
    {
        captured = false;
        broughtToCel = true;
        struggleCircle.GetComponent<StruggleCircle>().resetStruggleCounter();
        struggleCircle.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        player.SetActive(true);
        playerscript.capture(false);
        playerscript.setHealthToInjured();
    }

    private void HandleStruggling()
    {
        if (struggleCircle.GetComponent<StruggleCircle>().getCounter() >= 100)
        {
            guardscript.letGo(true);
            StartCoroutine("enablePlayerAndStopCapture");
        }
    }
}
