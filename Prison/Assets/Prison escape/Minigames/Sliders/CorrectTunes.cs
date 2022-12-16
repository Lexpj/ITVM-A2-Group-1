using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectTunes : MonoBehaviour
{
    public GameObject game;
    public GameObject otherPlayButton;
    public bool valid;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnMouseDown() {
        game.GetComponent<SoundMinigame>().correctTunes(valid);
        otherPlayButton.SetActive(true);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
