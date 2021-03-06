﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

enum GameState
{
    playing,
    dying,
    winning,
}

public class RocketBehavior : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip thrustClip;
    [SerializeField] AudioClip deathClip;
    [SerializeField] AudioClip victoryClip;

    [SerializeField] Rigidbody rocketBody;

    [SerializeField] float forceMultyplier = 5.0f;
    [SerializeField] float rotateMultiplier = 1.0f;

    [SerializeField] ParticleSystem thrustParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem victoryParticle;

    [SerializeField] Light thrustLight;

    private bool toggleThruster = false;
    [SerializeField] float transitionTime = 1.0f;
    private GameState gameState = GameState.playing;

    [SerializeField] int sceneLength = 1;

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
        if (gameState != GameState.playing) return;

        handleThrusting();

        handleRotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameState != GameState.playing) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Don't worry"); //TODO remove
                break;
            case "Fual":
                print("You got fuel");
                break;
            case "Finish":
                onReachGoal();
                break;
            default:
                onDead();
                break;
        }
    }

    private void onReachGoal()
    {
        if (gameState == GameState.playing)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(victoryClip);
            victoryParticle.Play();
            gameState = GameState.winning;
            Invoke("loadNextScene", transitionTime);
        }
    }

    private void onDead()
    {
        if (gameState == GameState.playing)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(deathClip);
            deathParticle.Play();
            thrustParticle.Stop();
            gameState = GameState.dying;
            Invoke("resetLevel", transitionTime);
        }
    }

    private void loadNextScene()
    {
        int currentSceneIdx = SceneManager.GetActiveScene().buildIndex;
        currentSceneIdx = (currentSceneIdx + 1) % sceneLength;
        SceneManager.LoadScene(currentSceneIdx);
    }

    private void resetLevel()
    {
        SceneManager.LoadScene(0);
    }

    void handleThrusting()
    {
        //Movement
        if (Input.GetKey(KeyCode.Space))
        {
            rocketBody.AddRelativeForce(Vector3.up * forceMultyplier);
        }

        //Thrust Sound
        if (Input.GetKeyDown(KeyCode.Space))
        {
            toggleThruster = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            toggleThruster = false;
        }

        if(toggleThruster && !thrustLight.enabled)
        {
            thrustLight.enabled = true;
        }

        if(!toggleThruster && thrustLight.enabled)
        {
            thrustLight.enabled = false;
        }

        if (toggleThruster && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustClip);
        }

        if (!toggleThruster && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if(toggleThruster && !thrustParticle.isPlaying)
        {
            thrustParticle.Play();
        }

        if(!toggleThruster && thrustParticle.isPlaying)
        {
            thrustParticle.Stop();
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
