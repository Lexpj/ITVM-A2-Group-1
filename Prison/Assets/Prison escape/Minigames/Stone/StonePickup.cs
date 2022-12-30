using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class StonePickup : MonoBehaviour
{
    private float secondsToCompleteTask = 10f;
    private float counter = 0f;

    private bool disableIcon = false;
    private taskManager targetObj;
    public GameObject T_key;
    private bool player = false;
    
    public AudioSource audioSource;
    public AudioSource audioSource2;
    private GameObject playerPos;

    private Vector3 deltaXYZ;
    [SerializeField] private Transform aimTransform;
    [SerializeField] private GuardAI guard;
    private Transform stonetrans;

    public float speed = 0.05f;
    public bool flying = false;

    private void Start()
    {
        targetObj = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<taskManager>();
        playerPos = GameObject.FindGameObjectWithTag("Player");
        deltaXYZ = new Vector3(0,0,0);
        stonetrans = GetComponent<Transform>();
        guard = GameObject.FindGameObjectWithTag("Guard").GetComponent<GuardAI>();
    }

    private void Update()
    {
        
        if (player)
        {
            if (Input.GetKey(KeyCode.Q) && !flying)
            {
                player = false;

                T_key.SetActive(false);

                // Pick up stone
                audioSource.Play();
                Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
                deltaXYZ = (mousePosition - aimTransform.position).normalized;
                Debug.Log(deltaXYZ);
                flying = true;
                disableIcon = true;
            }
            
        }

        if (flying) {
            if (!(deltaXYZ[0] == 0 && deltaXYZ[1] == 0 && deltaXYZ[2] == 0)) {
                Vector3 rotationToAdd = new Vector3(0, 0, 2);
                stonetrans.Rotate(rotationToAdd);
            }
            stonetrans.position = new Vector3(stonetrans.position.x + deltaXYZ[0]*speed,stonetrans.position.y + deltaXYZ[1]*speed,stonetrans.position.z + deltaXYZ[2]*speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            T_key.SetActive(true);
            player = true;
        }
        else if (other.CompareTag("Guard") && flying) {
            flying = false;
            audioSource.Stop();
            audioSource2.Play();
            guard.startTime = Time.time;
            Debug.Log(guard.startTime);
            deltaXYZ = new Vector3(0,0,0);
        }
        else if (other.CompareTag("Walls")) {
            stonetrans.position = new Vector3(stonetrans.position.x - deltaXYZ[0]*5*speed,stonetrans.position.y - deltaXYZ[1]*5*speed,stonetrans.position.z - deltaXYZ[2]*5*speed);
            deltaXYZ = new Vector3(0,0,0);
            flying = false;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            T_key.SetActive(false);
            player = false;
        }
        
        
    }

}
