using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySliders : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip click;

    public GameObject slider1;
    public GameObject slider2;
    public GameObject slider3;
    public GameObject slider4;

    private float pitch1;
    private float pitch2;
    private float pitch3;
    private float pitch4;

    private bool played1;
    private bool played2;
    private bool played3;
    private bool played4;


    // Time
    public float startTime;


    void OnMouseDown() {
        pitch1 = slider1.GetComponent<audioSlider>().pitch;
        pitch2 = slider2.GetComponent<audioSlider>().pitch;
        pitch3 = slider3.GetComponent<audioSlider>().pitch;
        pitch4 = slider4.GetComponent<audioSlider>().pitch;
        played1 = false;
        played2 = false;
        played3 = false;
        played4 = false;
        startTime = Time.time;           
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {  
        //Debug.Log(Time.time);
        if (Time.time-startTime < 1 && !played1) {
            audioSource.pitch = pitch1;
            audioSource.PlayOneShot(click,1);
            played1 = true;
        } else if (Time.time-startTime > 1 && Time.time-startTime < 2 && !played2) {
            audioSource.pitch = pitch2;
            audioSource.PlayOneShot(click,1);
            played2 = true;
        } else if (Time.time-startTime > 2 && Time.time-startTime < 3 && !played3) {
            audioSource.pitch = pitch3;
            audioSource.PlayOneShot(click,1);
            played3 = true;
        } else if (Time.time-startTime > 3 && Time.time-startTime < 4 && !played4) {
            audioSource.pitch = pitch4;
            audioSource.PlayOneShot(click,1);
            played4 = true;
        }
    }
}
