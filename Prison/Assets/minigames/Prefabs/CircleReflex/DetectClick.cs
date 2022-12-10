using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClick : MonoBehaviour
{
    public int pos;
    private CircleGame gameobject; 
    // Start is called before the first frame update
    void Start()
    {
        gameobject = GameObject.Find("CircleGame").GetComponent<CircleGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setPos(int x) {
        pos = x;
    }

    public void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            gameobject.clicked[gameobject.curpointer] = pos;
            gameobject.clickDetected = true;
        }
    }

}
