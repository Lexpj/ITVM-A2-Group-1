using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Tile 
{
    public int orientation = 0;
    public int[] lines = new int[] {0,0,0,0};
    public int pos;
    private GameObject cross;
    private GameObject cornerleftdown;
    private GameObject empty;
    private GameObject stubleft;
    private GameObject tleft;
    private GameObject vertical; 

    public void setPos(int x) {
        pos = x;
    }
    public void initObjects() {
        cross = GameObject.Find("TileObject ("+pos+")/Tile ("+pos+")/Cross");
        cornerleftdown = GameObject.Find("TileObject ("+pos+")/Tile ("+pos+")/CornerLeftDown");
        empty = GameObject.Find("TileObject ("+pos+")/Tile ("+pos+")/Empty");
        stubleft = GameObject.Find("TileObject ("+pos+")/Tile ("+pos+")/StubLeft");
        tleft = GameObject.Find("TileObject ("+pos+")/Tile ("+pos+")/TLeft");
        vertical = GameObject.Find("TileObject ("+pos+")/Tile ("+pos+")/Vertical");
    }
    public void init() {
        orientation = 0;
        lines = new int[] {0,0,0,0};
    }
    public void calcStart() {
        cross.SetActive(false);
        empty.SetActive(false);
        stubleft.SetActive(false);
        tleft.SetActive(false);
        vertical.SetActive(false);
        cornerleftdown.SetActive(false);
        calcPrefab();
    }

    public void calcPrefab() {
        // also sets static orientation 
        if (isEqual(lines, new int[] {1,1,1,1})) { // Cross
            cross.SetActive(true);
            setObjectRotation(0);
        }
        else if (isEqual(lines, new int[] {0,0,0,0})) { // Empty
            empty.SetActive(true);
            setObjectRotation(0);
        }

        else if (isEqual(lines, new int[] {0,0,0,1})) { // Stubup
            stubleft.SetActive(true);
            setObjectRotation(90);
        }
        else if (isEqual(lines, new int[] {0,0,1,0})) { // Stubleft
            stubleft.SetActive(true);
            setObjectRotation(0);
        }
        else if (isEqual(lines, new int[] {0,1,0,0})) { // Stubdown
            stubleft.SetActive(true);
            setObjectRotation(270);
        }
        else if (isEqual(lines, new int[] {1,0,0,0})) { // Stubright
            stubleft.SetActive(true);
            setObjectRotation(180);
        }

        else if (isEqual(lines, new int[] {1,0,0,1})) { // cornerleftdown
            cornerleftdown.SetActive(true);
            setObjectRotation(270);
        }
        else if (isEqual(lines, new int[] {1,1,0,0})) { // cornerleftup
            cornerleftdown.SetActive(true);
            setObjectRotation(0);
        }
        else if (isEqual(lines, new int[] {0,0,1,1})) { // cornerrightdown
            cornerleftdown.SetActive(true);
            setObjectRotation(180);
        }
        else if (isEqual(lines, new int[] {0,1,1,0})) { // cornerrightup
            cornerleftdown.SetActive(true);
            setObjectRotation(90);
        }

        else if (isEqual(lines, new int[] {0,1,1,1})) { // tleft
            tleft.SetActive(true);
            setObjectRotation(0);
        }
        else if (isEqual(lines, new int[] {1,0,1,1})) { // tup
            tleft.SetActive(true);
            setObjectRotation(90);
        }
        else if (isEqual(lines, new int[] {1,1,0,1})) { // tright
            tleft.SetActive(true);
            setObjectRotation(180);
        }
        else if (isEqual(lines, new int[] {1,1,1,0})) { // Stubup
            tleft.SetActive(true);
            setObjectRotation(270);
        }

        else if (isEqual(lines, new int[] {0,1,0,1})) { // vertical
            vertical.SetActive(true);
            setObjectRotation(0);
        }
        else if (isEqual(lines, new int[] {1,0,1,0})) { // horizontal
            vertical.SetActive(true);
            setObjectRotation(90);
        }

    }
    public void setObjectRotation(int rot) {
        GameObject tile = GameObject.Find("TileObject ("+pos+")");
        tile.transform.eulerAngles = new Vector3(0,0,rot);
    }
    bool isEqual(int[] x, int[] y) {
        for (int i = 0; i < 4; i++) {
            if (x[i] != y[i]) {
                return false;
            }
        }
        return true;
    }
    public void addRotationPrefab(int rot) {
        Vector3 rotationToAdd = new Vector3(0, 0, rot*-90);
        GameObject tile = GameObject.Find("TileObject ("+pos+")");
        tile.transform.Rotate(rotationToAdd);

    }
    public void shuffle() {
        if (isEqual(lines, new int[] {0,1,0,1}) || isEqual(lines, new int[] {1,0,1,0})) {
            orientation = Random.Range(0,1);
        }
        else if (isEqual(lines, new int[] {1,1,1,1}) || isEqual(lines, new int[] {0,0,0,0})) {
            orientation = 0;
        }
        else {
            orientation = Random.Range(0,3);
        }
        addRotationPrefab(orientation);
    }

    public void rotate(int x) {
        if (isEqual(lines, new int[] {0,1,0,1}) || isEqual(lines, new int[] {1,0,1,0})) {
            orientation = (orientation + 2 + x) % 2;
        }
        else if (!(isEqual(lines, new int[] {1,1,1,1}) || isEqual(lines, new int[] {0,0,0,0}))) {
            orientation = (orientation + 4 + x) % 4;
        }
    }
}

public class PuzzleLines : MonoBehaviour
{
    // Cam
    private Camera _cam;

    public AudioSource audioSource;
    public AudioSource ticker;
    public AudioClip rotationSound;
    public AudioClip win;
    public AudioClip tickSound;

    // Grid settings
    public int width = 3;
    public int height = 3;
    private Tile[,] tiles = new Tile[3,3];
    public float density = 0.7f;    

    // Click settings
    public bool clicked;
    public int clickedPos;

    // Time for failure
    private bool firstClick;
    private float startTime;
    public float maxTime = 10.0f;
    private float tickTimePeriod = 0.5f;
    private float tickTime = 0.0f;
    private bool firstIncrease = false;
    private float firstIncreaseTime;
    private bool secondIncrease = false;
    private float secondIncreaseTime;

    // Task
    public Task task;
    
    // Start is called before the first frame update
    void Awake() {
        _cam = Camera.main;
        clicked = false;
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        
        init();
    }
    private void init() {
        // New tiles 
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1); j++) {
                tiles[i,j] = new Tile();
                tiles[i,j].setPos(j + i*3);
                tiles[i,j].initObjects();
            }
        }   
        makePuzzle();
    }
    void makePuzzle() {
        // Generate puzzle
        // Horizontal lines
        float configChance;
        firstClick = false;
        startTime = Time.time;
        tickTimePeriod = 0.5f;
        tickTime = 0.0f;
        firstIncrease = false;
        firstIncreaseTime = maxTime / 3 * 2;
        secondIncrease = false;
        secondIncreaseTime = maxTime / 3;
        clicked = false;
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1); j++) {
                tiles[i,j].init(); // reset lines and orientation
            }
        }   
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1)-1; j++) {
                configChance = Random.Range(0f,1f);
                if (configChance <= density) {
                    tiles[i,j].lines[0] = 1;
                    tiles[i,j+1].lines[2] = 1;
                }
            }
        }  
        // Vertical lines
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1)-1; j++) {
                configChance = Random.Range(0f,1f);
                if (configChance <= density) {
                    tiles[j,i].lines[3] = 1;
                    tiles[j+1,i].lines[1] = 1;
                }
            }
        }  
        // Init the thingies
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1); j++) {
                tiles[i,j].calcStart();
            }
        } 

        // Small chance for shuffle to end up with the same config as the result
        // so that's why shuffle until not endstate
        while (checkState()) {
            for (int i = 0; i < tiles.GetLength(0); i++) {
                for (int j = 0; j < tiles.GetLength(1); j++) {
                    tiles[i,j].shuffle();
                }
            }
        }
    }
    private bool checkSameArray(int a, int b) {
        if (a==b) {
            return true;
        };
        return false;
    }

    bool checkState() {
        bool found = true;

        // Horizontal
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1)-1; j++) {
                if (!(checkSameArray(tiles[i,j].lines[(tiles[i,j].orientation)%4],tiles[i,j+1].lines[(2+tiles[i,j+1].orientation)%4]))) {
                    found = false;
                    break;
                }
            }
        }  
        // Vertical lines
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1)-1; j++) {
                if (!(checkSameArray(tiles[j,i].lines[(3+tiles[j,i].orientation)%4],tiles[j+1,i].lines[(1+tiles[j+1,i].orientation)%4]))) {
                    found = false;
                    break;
                }
            }
        } 

        // If found, play some sound or smth
        return found;
    }

    void Failed() {
        makePuzzle();
        task.Failed();
    }

    void Win() {
        task.Completed();
    }

    void Update() {
        if (firstClick) {
            if (Time.time-startTime > maxTime) {
                Failed();
            }
        
            if (Time.time-startTime > tickTime) { // sound tick
                ticker.PlayOneShot(tickSound,1);
                tickTime += tickTimePeriod;
                if (Time.time-startTime > firstIncreaseTime && !firstIncrease) {
                    firstIncrease = true;
                    tickTimePeriod = tickTimePeriod * 0.5f;
                }
                else if (Time.time-startTime > secondIncreaseTime && !secondIncrease) {
                    secondIncrease = true;
                    tickTimePeriod = tickTimePeriod * 0.5f;
                }
            }
        }

        if (clicked) {
            clicked = false;
            int y = clickedPos/3;
            int x = clickedPos%3;
            tiles[y,x].rotate(1);
            audioSource.PlayOneShot(rotationSound);

            if (!firstClick) {
                firstClick = true;
                startTime = Time.time;
            }
        }

        bool won = checkState();
        if (won && firstClick) {
           Win();
        }

    }
}
