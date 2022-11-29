using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 _dragOffset;
    private Camera _cam;
    private Transform gate;
    
    void Awake() {
        _cam = Camera.main;
    }

    void Start()
    {
        gate = GetComponent<Transform>();
    }

    void OnMouseDown() {
        _dragOffset = gate.position - GetMousePos();
    }

    void OnMouseDrag() {
        gate.position = GetMousePos() + _dragOffset;
    }

    Vector3 GetMousePos() {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
