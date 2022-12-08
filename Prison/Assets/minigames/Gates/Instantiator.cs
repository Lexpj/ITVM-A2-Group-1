using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject leftGate;
    public GameObject rightGate;
    private GameObject go;
    private float X = 0f;

    private float height = 10f;
    private float i = 0;

    // Chance of spawning:
    public float chance = 60f;
    public float chanceLeft = 50f;

    private float randomNumber = 0f;
    private float skipl = 0;
    private float skipr = 0;

    // Start is called before the first frame update
    void Start()
    {
        while (i < height) {
            if (skipl > 0) {
                skipl -= 1;
            }
            if (skipr > 0) {
                skipr -= 1;
            }
            // Pick random number
            randomNumber = Random.Range(0f,1f);
            
            // If randomNumber <= chance:
            if (randomNumber <= chance) {
                // Random number of left else right
                randomNumber = Random.Range(0f,1f);
                
                // If randomNumber <= chanceLeft:
                if (randomNumber <= chanceLeft && skipl == 0) {
                    Generate(i, true);
                    skipl += 3;
                    skipr += 2;
                }
                else if (skipr == 0) {
                    Generate(i, false);
                    skipr += 3;
                    skipl += 2;
                }
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
