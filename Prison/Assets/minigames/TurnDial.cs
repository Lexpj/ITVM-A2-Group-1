using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnDial : MonoBehaviour
{

    // Speed of gate
    public float initialSpeed = 1f;
    private float speed;
    // Direction of gate
    // If set to 1, it moves to the right, if set to -1, it moves to the left
    // (optionally, if set to 0, it doesn't move)
    public float direction = 1f; 

    // Margin: important for accuracy of dial vs player
    public float margin = 1.5f;

    // The gate itself and components
    private Transform dial;
    private Camera _cam;
    [SerializeField]public TextMeshProUGUI nr1;
    [SerializeField]public TextMeshProUGUI nr2;
    [SerializeField]public TextMeshProUGUI nr3;

    AudioSource audioSource;
    public float startPitch = 1.2f;
    public AudioClip click;

    private int randint1;
    private int randint2;
    private int randint3;

    private int cur = 1;
    private float dif;
    
    // For the task settings
    public Task task;

    void Awake() {
        _cam = Camera.main;
        randint1 = Random.Range(0,36);
        randint2 = Random.Range(0,36);
        randint3 = Random.Range(0,36);
        Debug.Log(randint1 + " " + randint2 + " " + randint3);
        nr1.text = "" + randint1;
        nr2.text = "" + randint2;
        nr3.text = "" + randint3;
        nr1.color = new Color(255,255,0,255);
        nr2.color = new Color(255,0,0,255);
        nr3.color = new Color(255,0,0,255);
        //nr1.transform.parent.gameObject.SetActive(true);
        //nr2.transform.parent.gameObject.SetActive(true);
        //nr3.transform.parent.gameObject.SetActive(true);
    }

    // AUTOMATIC MOVEMENT::::
    // Start is called before the first frame update
    void Start()
    {
        dial = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = click;
        audioSource.pitch = startPitch;
        speed = initialSpeed;

        nr1.gameObject.GetComponent<Animator>().enabled = true;
        nr2.gameObject.GetComponent<Animator>().enabled = false;
        nr3.gameObject.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotationToAdd = new Vector3(0, 0, speed*direction);
        dial.Rotate(rotationToAdd);

        if (Input.GetKeyDown(KeyCode.Space)) {
            direction *= -1;
            speed *= (float) 1.2f;
            audioSource.pitch *= 1.2f;
            CheckIfAngleCorrect();
            cur += 1;
        }
    }

    void GameOver() {
        direction = 0;
        audioSource.loop = false;
        Debug.Log("GAME OVER");
        task.Failed();
        
        // reset task
        speed = initialSpeed;
        direction = 1f;
        audioSource.pitch = startPitch;
        cur = 0;
        audioSource.loop = true;
        randint1 = Random.Range(0,36);
        randint2 = Random.Range(0,36);
        randint3 = Random.Range(0,36);
        Debug.Log(randint1 + " " + randint2 + " " + randint3);
        dial.transform.eulerAngles = new Vector3(
            dial.transform.localRotation.eulerAngles.x,
            dial.transform.localRotation.eulerAngles.y,
            0
        );
        nr1.text = "" + randint1;
        nr2.text = "" + randint2;
        nr3.text = "" + randint3;
        nr1.color = new Color(255,255,0,255);
        nr2.color = new Color(255,0,0,255);
        nr3.color = new Color(255,0,0,255);
        
        // Animatino
        nr1.gameObject.GetComponent<Animator>().enabled = true;
        nr2.gameObject.GetComponent<Animator>().enabled = false;
        nr3.gameObject.GetComponent<Animator>().enabled = false;

        //UnityEditor.EditorApplication.isPlaying = false;
    }

    void Win() {
        direction = 0;
        audioSource.loop = false;
        task.Completed();
        //UnityEditor.EditorApplication.isPlaying = false;

    }

    void CheckIfAngleCorrect() {    
        var dialNr = dial.transform.localRotation.eulerAngles.z/(360/39);
        if (cur == 1) {
            dif = randint1-dialNr;
            if (dif < 0) {
                dif *= -1;
            }
            if (dif <= margin) {
                nr1.color = new Color(0,255,0,255);
                nr2.color = new Color(255,255,0,255);
                nr1.gameObject.GetComponent<Animator>().enabled = false;
                nr2.gameObject.GetComponent<Animator>().enabled = true;
            } else {
                GameOver();
            }
        }
        else if (cur == 2) {
            dif = randint2-dialNr;
            if (dif < 0) {
                dif *= -1;
            }
            if (dif <= margin) {
                nr2.color = new Color(0,255,0,255);
                nr3.color = new Color(255,255,0,255);
                nr2.gameObject.GetComponent<Animator>().enabled = false;
                nr3.gameObject.GetComponent<Animator>().enabled = true;
            } else {
                GameOver();
            }
        }
        else if (cur == 3) {
            dif = randint3-dialNr;
            if (dif < 0) {
                dif *= -1;
            }
            if (dif <= margin) {
                nr3.color = new Color(0,255,0,255);
                nr3.gameObject.GetComponent<Animator>().enabled = false;
                Win();
            } else {
                GameOver();
            }
        }
    }
}
