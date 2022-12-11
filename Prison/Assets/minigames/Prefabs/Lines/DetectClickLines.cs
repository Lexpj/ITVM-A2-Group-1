using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClickLines : MonoBehaviour
{
    public int pos;
    public GameObject gameobject;
    private PuzzleLines puzzlelines;

    // Start is called before the first frame update
    void Start()
    {
        puzzlelines = gameobject.GetComponent<PuzzleLines>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void rotateself() {
        Vector3 rotationToAdd = new Vector3(0, 0, -90);
        transform.parent.gameObject.transform.Rotate(rotationToAdd);
    }

    public void setPos(int x) {
        pos = x;
    }
    public void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            rotateself();
            puzzlelines.clicked = true;
            puzzlelines.clickedPos = pos;
        }
    }

}
