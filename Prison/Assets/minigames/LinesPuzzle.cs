using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;



public class LinesPuzzle : MonoBehaviour
{

    class Tile
    {
        public int x;
        public int y;
        public int orientation = 0;
        public int[] lines = {0,0,0,0};

        public string currentPrefab = "vertical";
        IDictionary<string, GameObject> prefabs;
        public GameObject tile;

        public void setXY(int curx, int cury) {
            x = curx;
            y = cury;
        }
        public void setDic(IDictionary<string, GameObject> dic) {
            prefabs = dic;
        }
        public void init() {
            Vector3 pos = new Vector3(x,-y,0);
            tile = Instantiate(prefabs[currentPrefab], pos, Quaternion.identity) as GameObject;
            tile.transform.parent = GameObject.Find("Grid").transform;
        }

        private bool checkSameArray(int[] a, int[] b) {
            for (int i = 0; i < 4; i++) {
                if (a[i] != b[i]) {
                    return false;
                }
            }
            return true;
        }
        public void shuffle() {
            if (checkSameArray(lines, new int[] {0,1,0,1}) || checkSameArray(lines,new int[] {1,0,1,0})) {
                orientation = Random.Range(0,1);
            }
            else if (checkSameArray(lines,new int[] {0,0,0,0}) || checkSameArray(lines,new int[] {1,1,1,1})) {
                orientation = 0;
            }
            else {
                orientation = Random.Range(0,3);
            }
        }

        public void rotate(int rot) {
            if (checkSameArray(lines,new int[] {0,1,0,1}) || checkSameArray(lines,new int[] {1,0,1,0})) {
                orientation = (orientation + 2 + rot) % 2;
            }
            else if (!(checkSameArray(lines,new int[] {0,0,0,0}) || checkSameArray(lines, new int[] {1,1,1,1}))) {
                orientation = (orientation + 4 + rot) % 4;
            }
        }

        public void render() {
            int[] conf = {
                lines[(0+orientation)%4],  // Right
                lines[(3+orientation)%4],  // Bottom
                lines[(2+orientation)%4],  // Left
                lines[(1+orientation)%4],  // Top
            };
            string links = conf[0].ToString() + conf[1].ToString() + conf[2].ToString() + conf[3].ToString();
            IDictionary<string, string> trans = new Dictionary<string, string>() {
                {"0000", "empty"},
                {"0001", "stubup"},
                {"0010", "stubleft"},
                {"0011", "cornerrightdown"},
                {"0100", "stubdown"},
                {"0101", "vertical"},
                {"0110", "cornerrightup"},
                {"0111", "tleft"},
                {"1000", "stubright"},
                {"1001", "cornerleftdown"},
                {"1010", "horizontal"},
                {"1011", "tup"},
                {"1100", "cornerleftup"},
                {"1101", "tright"},
                {"1110", "tdown"},
                {"1111", "cross"},
            };
            
            // Change object
            if (!isSamePrefab(trans[links])) {
                currentPrefab = trans[links];
                Destroy(tile);
                init();
            }

        }

        public bool isSamePrefab(string newPrefab) {
            if (currentPrefab == newPrefab) {
                return true;
            }
            return false;
        }

        public void onMouseDown() {
            Debug.Log(x + " " + y);
        }
    }

    // Cam
    private Camera _cam;

    // Prefabs of the different tiles
    public GameObject CornerLeftDown;
    public GameObject CornerLeftUp;
    public GameObject CornerRightDown;
    public GameObject CornerRightUp;
    public GameObject Cross;
    public GameObject Vertical;
    public GameObject Horizontal;
    public GameObject StubDown;
    public GameObject StubRight;
    public GameObject StubLeft;
    public GameObject StubUp;
    public GameObject TDown;
    public GameObject TRight;
    public GameObject TLeft;
    public GameObject TUp;
    public GameObject Empty;

    AudioSource audioSource;
    public AudioClip rotationSound;
    public AudioClip win;


    // Grid settings
    public int width = 3;
    public int height = 3;
    private Tile[,] tiles = new Tile[3,3];
    
    public float density = 0.7f;


    // Start is called before the first frame update
    void Awake() {
        _cam = Camera.main;
    }

    void Start() {
        IDictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>(){
            {"cornerleftdown", CornerLeftDown},
            {"cornerleftup", CornerLeftUp},
            {"cornerrightdown", CornerRightDown},
            {"cornerrightup", CornerRightUp},
            {"cross", Cross},
            {"vertical", Vertical},
            {"horizontal", Horizontal},
            {"stubdown", StubDown},
            {"stubright", StubRight},
            {"stubup", StubUp},
            {"stubleft", StubLeft},
            {"tdown", TDown},
            {"tright", TRight},
            {"tleft", TLeft},
            {"tup", TUp},
            {"empty", Empty}
        };
        float configChance;
        audioSource = GetComponent<AudioSource>();

        // New tiles 
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1); j++) {
                tiles[i,j] = new Tile();
                tiles[i,j].setXY(j*3,i*3);
                tiles[i,j].setDic(prefabs);
                tiles[i,j].init();
            }
        }   

        // Generate puzzle
        // Horizontal lines
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

    void Update() {
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1); j++) {
                tiles[i,j].render();
            }
        }
        Vector3 mousepos = getMousePos();
        if (mousepos != new Vector3(-1,-1,-1)) {
            tiles[(int)mousepos.y,(int)mousepos.x].rotate((int) 1);
            audioSource.PlayOneShot(rotationSound,1);
            //Debug.Log(tiles[(int)mousepos.y,(int)mousepos.x].orientation);
            //Debug.Log(tiles[(int)mousepos.y,(int)mousepos.x].lines);
        }
        bool won = checkState();
        if (won) {
            Debug.Log("YAAYAYAYAYAY");
            audioSource.PlayOneShot(win,1);
        }
        //var mousepos = _cam.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(mousepos);
    }




    Vector3 getMousePos() {
        if (Input.GetMouseButtonDown(0)) { // returns true when the mouse left button is pressed
            Vector3 mousePos = Input.mousePosition;   
            mousePos.z=Camera.main.nearClipPlane;
            int xz = (int) mousePos.x/180;
            int yz = (int) mousePos.y/180;
            yz = 2-yz;
            Debug.Log(mousePos.x + " " + mousePos.y);
            //Debug.Log(xz + " " + yz);
            return new Vector3(xz,yz,0);
        }
        return new Vector3(-1,-1,-1);
    }
        
}
