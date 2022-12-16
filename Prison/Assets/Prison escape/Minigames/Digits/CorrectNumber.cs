using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectNumber : MonoBehaviour
{
    public GameObject nr1;
    public GameObject nr2;
    public GameObject nr3;

    public AudioSource audioSource;
    public AudioClip click;

    private int currentnr1;
    private int currentnr2;
    private int currentnr3;
    private int solutionnr1;
    private int solutionnr2;
    private int solutionnr3;

    public GameObject game;
    public int lightIndex = 0; 
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentnr1 = nr1.GetComponent<DigitScript>().currentDigit;
        currentnr2 = nr2.GetComponent<DigitScript>().currentDigit;
        currentnr3 = nr3.GetComponent<DigitScript>().currentDigit;
        solutionnr1 = game.GetComponent<DigitsMinigame>().solutionNr1;
        solutionnr2 = game.GetComponent<DigitsMinigame>().solutionNr2;
        solutionnr3 = game.GetComponent<DigitsMinigame>().solutionNr3;

        if (lightIndex == 0 && solutionnr1 == currentnr1) {
            if (game.GetComponent<DigitsMinigame>().hardMode) {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0f, 1f);
            } else {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0.0f, 1f);
            }
        } else if (lightIndex == 1 && solutionnr2 == currentnr2) {
            if (game.GetComponent<DigitsMinigame>().hardMode) {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0f, 1f);
            } else {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0.0f, 1f);
            }
        } else if (lightIndex == 2 && solutionnr3 == currentnr3) {
            if (game.GetComponent<DigitsMinigame>().hardMode) {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0f, 1f);
            } else {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0.0f, 1f);
            }
        } else if (lightIndex == 0 && (currentnr1 == solutionnr2 || currentnr1 == solutionnr3)) {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0f, 1f);
        } else if (lightIndex == 1 && (currentnr2 == solutionnr1 || currentnr2 == solutionnr3)) {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0f, 1f);
        } else if (lightIndex == 2 && (currentnr3 == solutionnr2 || currentnr3 == solutionnr1)) {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0f, 1f);
        } else {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.0f, 0.0f, 1f);
        }
    }
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (lightIndex == 0) {
                nr1.GetComponent<DigitScript>().isClicked();
            } else if (lightIndex == 1) {
                nr2.GetComponent<DigitScript>().isClicked();
            } else {
                nr3.GetComponent<DigitScript>().isClicked();
            }
            audioSource.PlayOneShot(click, 1);
            game.GetComponent<DigitsMinigame>().firstClick = true;
        }
        
    }
}
