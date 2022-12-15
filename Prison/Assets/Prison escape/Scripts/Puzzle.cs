using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public GameObject leftGate;
    public GameObject rightGate;
    private GameObject go;
    private float X = 0f;

    private float height = 12f;
    private float i = 0;

    // Chance of spawning:
    public float chance = 80f;
    public float chanceLeft = 50f;

    private float randomNumber = 0f;
    private float skip = 0;

    // Start is called before the first frame update
    void Start()
    {
        while (i < height) {
            // If last placed wall obstructs:
            if (skip > 0) {
                skip -= 1;
                i += 1;
                continue;
            }

            // Pick random number
            randomNumber = Random.Range(0f,1f);
            
            // If randomNumber <= chance:
            if (randomNumber <= chance) {
                // Random number of left else right
                randomNumber = Random.Range(0f,1f);
                
                // If randomNumber <= chanceLeft:
                if (randomNumber <= chanceLeft) {
                    Generate(i, true);
                }
                else {
                    Generate(i, false);
                }
                skip += 2;
            }
            i += 1;
        }
    }

    void Generate(float y, bool left) {
        Vector3 pos = new Vector3(X,-y,0);

        if (left) {
            go = Instantiate(leftGate, pos, Quaternion.identity) as GameObject;
        }
        else {
            go = Instantiate(rightGate, pos, Quaternion.identity) as GameObject;
        }
        go.transform.parent = GameObject.Find("Grid").transform;
    }

}
