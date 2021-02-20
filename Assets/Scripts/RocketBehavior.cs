using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    public AudioSource thrustSound;
    public Rigidbody rocketBody;
    public float forceMultyplier = 5.0f;
    public float rotateMultiplier = 1.0f;
    private bool isPlaying = false;
    private bool togglePlay = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        processInput();
    }

    void processInput()
    {
        hanldeSound();

        handleMovement();
    }

    private void handleMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rocketBody.AddRelativeForce(Vector3.up * forceMultyplier);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward, rotateMultiplier);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward, -rotateMultiplier);
        }
    }

    private void hanldeSound()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            togglePlay = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            togglePlay = false;
        }

        if (togglePlay && !isPlaying)
        {
            thrustSound.Play();
            isPlaying = true;
        }
        

        if(!togglePlay && isPlaying){
            thrustSound.Stop();
            isPlaying = false;
        }
        
    }
}
