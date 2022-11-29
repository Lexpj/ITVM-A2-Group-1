using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGate : MonoBehaviour
{

    // Speed of gate
    public float speed = 5f;

    // Bounds of gate
    public float maxx = 200;
    public float minx = 100;

    // Direction of gate
    // If set to 1, it moves to the right, if set to -1, it moves to the left
    // (optionally, if set to 0, it doesn't move)
    public float direction = 1f; 

    // The gate itself and components
    private Transform gate;
    private Vector3 _dragOffset;
    private Camera _cam;

    // Obstructed path
    private bool move = true;

    // DRAG MOVEMENT::::::
    void Awake() {
        _cam = Camera.main;
    }

    void OnMouseDown() {
        _dragOffset = gate.position - GetMousePos();
    }

    void OnMouseDrag() {
        if (gate.position.x > maxx) {
            gate.position = new Vector2(maxx, gate.position.y);
        }
        else if (gate.position.x < minx) {
            gate.position = new Vector2(minx, gate.position.y);
        }
        else {
            gate.position = GetMousePos() + _dragOffset;
        }
    }

    Vector3 GetMousePos() {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = 0;
        mousePos.z = 0;
        return mousePos;
    }


    // AUTOMATIC MOVEMENT::::
    // Start is called before the first frame update
    void Start()
    {
        gate = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gate.position.x < minx) {
            gate.position = new Vector2(minx, gate.position.y);
        }
        else if (gate.position.x > maxx) {
            gate.position = new Vector2(maxx, gate.position.y);
        }
        else if (move) {
            gate.position = new Vector2(gate.position.x + (direction*speed), gate.position.y);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        move = false;
    }
    private void OnTriggerExit2D(Collider2D other) {
        move = true;
    }
}
