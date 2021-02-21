using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    [SerializeField] AudioSource thrustSound;
    [SerializeField] Rigidbody rocketBody;
    [SerializeField] float forceMultyplier = 5.0f;
    [SerializeField] float rotateMultiplier = 1.0f;
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
        handleThrusting();

        handleRotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
       switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("Don't worry"); //TODO remove
                break;
            case "Fual":
                print("You got fuel"); //TODO remove
                break;
            default:
                print("You died"); //TODO remove
                // And kill the player
                break;
        }
    }

    void handleThrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rocketBody.AddRelativeForce(Vector3.up * forceMultyplier);
        }

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

        if (!togglePlay && isPlaying)
        {
            thrustSound.Stop();
            isPlaying = false;
        }
    }

    void handleRotate()
    {

        float deltaForce = rotateMultiplier * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {

            transform.Rotate(Vector3.forward * deltaForce);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * -deltaForce);
        }
    }
}
