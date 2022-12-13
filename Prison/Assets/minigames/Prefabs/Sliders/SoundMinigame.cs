using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMinigame : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip click;

    public GameObject slider1;
    public GameObject slider2;
    public GameObject slider3;
    public GameObject slider4;

    public GameObject cross1;
    public GameObject check1;
    public GameObject cross2;
    public GameObject check2;
    public GameObject cross3;
    public GameObject check3;
    public GameObject cross4;
    public GameObject check4;
    public GameObject firstCircle;
    public GameObject secondCircle;

    private float correctPitch1;
    private float correctPitch2;
    private float correctPitch3;
    private float correctPitch4;

    private bool played1= true;
    private bool played2= true;
    private bool played3= true;
    private bool played4= true;
    private bool valid = false;

    private float startTime;
    public Task task;

    public void correctTunes(bool v){
        valid = v;
        startTime = Time.time;  
        played1 = false;
        played2 = false;
        played3 = false;
        played4 = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    void init() {
        float[] pitches = new float[] {1f,1.5f,2f};
        correctPitch1 = pitches[Random.Range(0,3)];
        correctPitch2 = pitches[Random.Range(0,3)];
        correctPitch3 = pitches[Random.Range(0,3)];
        correctPitch4 = pitches[Random.Range(0,3)];
        Debug.Log(correctPitch1 + " " + correctPitch2 + " " + correctPitch3 + " " + correctPitch4);
        check1.SetActive(false);
        check2.SetActive(false);
        check3.SetActive(false);
        check4.SetActive(false);
        cross1.SetActive(false);
        cross2.SetActive(false);
        cross3.SetActive(false);
        cross4.SetActive(false);
        firstCircle.SetActive(true);
        secondCircle.SetActive(false);
        slider1.GetComponent<audioSlider>().init();
        slider2.GetComponent<audioSlider>().init();
        slider3.GetComponent<audioSlider>().init();
        slider4.GetComponent<audioSlider>().init();
    }

    void checkIfWon() {
        float faults = 0f;
        float placeholder;
        placeholder = slider1.GetComponent<audioSlider>().pitch - correctPitch1;
        if (placeholder > 0) {
            faults += placeholder;
        } else {
            faults -= placeholder;
        }
        placeholder = slider2.GetComponent<audioSlider>().pitch - correctPitch2;
        if (placeholder > 0) {
            faults += placeholder;
        } else {
            faults -= placeholder;
        }
        placeholder = slider3.GetComponent<audioSlider>().pitch - correctPitch3;
        if (placeholder > 0) {
            faults += placeholder;
        } else {
            faults -= placeholder;
        }
        placeholder = slider4.GetComponent<audioSlider>().pitch - correctPitch4;
        if (placeholder > 0) {
            faults += placeholder;
        } else {
            faults -= placeholder;
        }
        
        if (valid) {
            if (faults == 0) {
                Completed();
            } else {
                Failed();
            }
        }
    }

    void Completed() {
        task.Completed();
    }

    void Failed() {
        init();
        task.Failed();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time-startTime < 1 && !played1) {
            audioSource.pitch = correctPitch1;
            audioSource.PlayOneShot(click,1);
            played1 = true;
            if (valid) {
                if (correctPitch1 == slider1.GetComponent<audioSlider>().pitch) {
                    check1.SetActive(true);
                } else {
                    cross1.SetActive(true);
                }
            }
        } else if (Time.time-startTime > 1 && Time.time-startTime < 2 && !played2) {
            audioSource.pitch = correctPitch2;
            audioSource.PlayOneShot(click,1);
            played2 = true;
            if (valid) {
                if (correctPitch2 == slider2.GetComponent<audioSlider>().pitch) {
                    check2.SetActive(true);
                } else {
                    cross2.SetActive(true);
                }
            }
        } else if (Time.time-startTime > 2 && Time.time-startTime < 3 && !played3) {
            audioSource.pitch = correctPitch3;
            audioSource.PlayOneShot(click,1);
            played3 = true;
            if (valid) {
                if (correctPitch3 == slider3.GetComponent<audioSlider>().pitch) {
                    check3.SetActive(true);
                } else {
                    cross3.SetActive(true);
                }
            }
        } else if (Time.time-startTime > 3 && Time.time-startTime < 4 && !played4) {
            audioSource.pitch = correctPitch4;
            audioSource.PlayOneShot(click,1);
            played4 = true;
            if (valid) {
                if (correctPitch4 == slider4.GetComponent<audioSlider>().pitch) {
                    check4.SetActive(true);
                } else {
                    cross4.SetActive(true);
                }
            }

            checkIfWon();
        } 
    }
}
