using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFootsteps : MonoBehaviour
{
	Vector3 oldPosition;
	float speed;
    float oldSpeed;
    float olderSpeed;
    bool steps;
    public AudioClip[] audioSources;
    private AudioSource randomSound;
    private AudioSource randomSound2;
    private Rigidbody2D myBody;
    // Start is called before the first frame update
    void Start()
    {
    	oldPosition = transform.position;
        steps = false;
        speed = 0;
        oldSpeed = 0;
        olderSpeed = 0;
        myBody = gameObject.GetComponent<Rigidbody2D>();
        randomSound = gameObject.GetComponent<AudioSource>();
        //randomSound2 = randomSound;
    }

    // Update is called once per frame
    void Update()
    {
        speed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
        if (!steps)
        {
            if (!myBody.IsSleeping())
            //if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) 
            //    || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || 
            //    Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) 
            //    || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
     	  //speed = Vector3.Distance(oldPosition, transform.position) * 100f;
     	  //oldPosition = transform.position;
          //if (speed != 0)
          {
            //oldSpeed = speed;
            StartCoroutine(playEngineSound());
          } //else
          //{
            //randomSound.Stop();
            //steps = false;
            //oldSpeed = 0;
          //}
     	  Debug.Log("Speed: " + speed.ToString("F2"));
        }
        if (myBody.IsSleeping()|| (speed == 0 && oldSpeed == 0 && olderSpeed == 0)){
        //if(!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) 
        //        || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || 
        //        Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) 
        //        || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))) {
            randomSound.Stop();
            //steps = false;
        //}
        }
        olderSpeed = oldSpeed;
        oldSpeed = speed;
    }

    IEnumerator playEngineSound()
    {
        //randomSound = randomSound2;
        randomSound.clip = audioSources[Random.Range(0, audioSources.Length)];
        randomSound.Play();

        steps = true;
        yield return new WaitForSeconds(randomSound.clip.length);
        steps = false;
    }
}
