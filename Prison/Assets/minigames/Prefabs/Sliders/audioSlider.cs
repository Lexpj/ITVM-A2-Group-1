using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSlider : MonoBehaviour
{
    private Vector3 _dragOffset;
    private Camera _cam;
    private Transform slider;

    public AudioSource audioSource;
    public AudioClip ting;
    public float pitch;
    
    void Awake() {
        _cam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Transform>();
    }
    
    public void init() {
        pitch = 1.5f;
        slider.localPosition = new Vector3(0,0,0);
    }
    void OnMouseDown() {
        _dragOffset = slider.position - GetMousePos();
    }

    void OnMouseDrag() {
        slider.position = GetMousePos() + _dragOffset;
    }
    void OnMouseUp()
    {
        if (slider.localPosition.y >= 3) {
            slider.localPosition = new Vector3(0,4.5f,0f);
            pitch = ((slider.localPosition.y+4.5f)/9f) + 1;
        }
        else if (slider.localPosition.y <= -3) {
            slider.localPosition = new Vector3(0,-4.5f,0f);
            pitch = ((slider.localPosition.y+4.5f)/9f) + 1;
        }
        else if (slider.localPosition.y != 0) {
            slider.localPosition = new Vector3(0,0f,0f);
            pitch = ((slider.localPosition.y+4.5f)/9f) + 1;
        }
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(ting,1);

    }
    Vector3 GetMousePos() {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos.x = 0;
        return mousePos;
    }
    // Update is called once per frame
    void Update()
    {
        if (slider.localPosition.y > 4.5) {
            slider.localPosition = new Vector3(0,4.5f,0f);
        }
        else if (slider.localPosition.y < -4.5) {
            slider.localPosition = new Vector3(0,-4.5f,0f);
        }

    }
}