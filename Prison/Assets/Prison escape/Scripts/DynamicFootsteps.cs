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
    public Transform sphere;
    private Vector3 pos;
    private Vector3 prevpos;

    // Start is called before the first frame update
    void Start()
    {
    	oldPosition = transform.position;
        steps = false;
        speed = 0;
        oldSpeed = 0;
        olderSpeed = 0;
        pos = sphere.position;
        prevpos = sphere.position;
        randomSound = GetComponent<AudioSource>();
        //randomSound2 = randomSound;
    }

    bool compareVectors(Vector3 a, Vector3 b) {
        for (int i = 0; i < 3; i++) {
            if (a[i] != b[i]) {
                return false;
            }
        }
        return true;
    }


    // Update is called once per frame
    void Update()
    {
        pos = sphere.position;
        if (!steps) {
            if (!compareVectors(pos,prevpos)) {
                prevpos = sphere.position;
                // playsound
                StartCoroutine(playEngineSound());
            }
            else {
                randomSound.Stop();
            }
        }
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
