using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitScript : MonoBehaviour
{   
    public int currentDigit = 8;
    public GameObject seg1;
    public GameObject seg2;
    public GameObject seg3;
    public GameObject seg4;
    public GameObject seg5;
    public GameObject seg6;
    public GameObject seg7;

    public void isClicked() {
        currentDigit = (currentDigit + 1)%10;
    }

    public void setSevenSegmentDisplay() {

        if (currentDigit == 0) {
            seg1.SetActive(true);
            seg2.SetActive(true);
            seg3.SetActive(true);
            seg4.SetActive(true);
            seg5.SetActive(true);
            seg6.SetActive(false);
            seg7.SetActive(true);
        } else if (currentDigit == 1) {
            seg1.SetActive(false);
            seg2.SetActive(true);
            seg3.SetActive(false);
            seg4.SetActive(true);
            seg5.SetActive(false);
            seg6.SetActive(false);
            seg7.SetActive(false);
        } else if (currentDigit == 2) {
            seg1.SetActive(true);
            seg2.SetActive(false);
            seg3.SetActive(false);
            seg4.SetActive(true);
            seg5.SetActive(true);
            seg6.SetActive(true);
            seg7.SetActive(true);
        } else if (currentDigit == 3) {
            seg1.SetActive(false);
            seg2.SetActive(true);
            seg3.SetActive(false);
            seg4.SetActive(true);
            seg5.SetActive(true);
            seg6.SetActive(true);
            seg7.SetActive(true);
        } else if (currentDigit == 4) {
            seg1.SetActive(false);
            seg2.SetActive(true);
            seg3.SetActive(true);
            seg4.SetActive(true);
            seg5.SetActive(false);
            seg6.SetActive(true);
            seg7.SetActive(false);
        } else if (currentDigit == 5) {
            seg1.SetActive(false);
            seg2.SetActive(true);
            seg3.SetActive(true);
            seg4.SetActive(false);
            seg5.SetActive(true);
            seg6.SetActive(true);
            seg7.SetActive(true);
        } else if (currentDigit == 6) {
            seg1.SetActive(true);
            seg2.SetActive(true);
            seg3.SetActive(true);
            seg4.SetActive(false);
            seg5.SetActive(true);
            seg6.SetActive(true);
            seg7.SetActive(true);
        } else if (currentDigit == 7) {
            seg1.SetActive(false);
            seg2.SetActive(true);
            seg3.SetActive(false);
            seg4.SetActive(true);
            seg5.SetActive(false);
            seg6.SetActive(false);
            seg7.SetActive(true);
        } else if (currentDigit == 8) {
            seg1.SetActive(true);
            seg2.SetActive(true);
            seg3.SetActive(true);
            seg4.SetActive(true);
            seg5.SetActive(true);
            seg6.SetActive(true);
            seg7.SetActive(true);
        } else if (currentDigit == 9) {
            seg1.SetActive(false);
            seg2.SetActive(true);
            seg3.SetActive(true);
            seg4.SetActive(true);
            seg5.SetActive(true);
            seg6.SetActive(true);
            seg7.SetActive(true);
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setSevenSegmentDisplay();
    }
}
