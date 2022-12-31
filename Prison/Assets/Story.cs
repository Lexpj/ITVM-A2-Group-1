using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] GameObject image1;
    [SerializeField] GameObject image2;
    [SerializeField] GameObject image3;
    [SerializeField] GameObject image4;
    [SerializeField] GameObject image5;
    [SerializeField] GameObject audioSource;

    private bool playButtonPressed = false;
    private float counter = 0f;

    // Update is called once per frame
    void Update()
    {
        if (playButtonPressed)
        {
            audioSource.SetActive(true);
            counter += Time.deltaTime;

            image.SetActive(true);
            if(counter >= 8)
            {
                image1.SetActive(true);
            }
            if(counter >= 20)
            {
                image2.SetActive(true);
            }
            if(counter >= 35)
            {
                image3.SetActive(true);
            }
            if(counter >= 52)
            {
                image4.SetActive(true);
            }
            if(counter >= 55)
            {
                image5.SetActive(true);
            }
            if(counter >= 61)
            {
                image.SetActive(false);
                image1.SetActive(false);
                image2.SetActive(false);
                image3.SetActive(false);
                image4.SetActive(false);
                image5.SetActive(false);

                audioSource.SetActive(false);
                playButtonPressed = false;
                counter = 0f;
            }
        }
    }

    public void PlayButtonPressed()
    {
        playButtonPressed = true;
    }
}
