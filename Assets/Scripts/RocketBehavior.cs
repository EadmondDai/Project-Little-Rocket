﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

enum GameState
{
    playing,
    transending,
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
   
    private bool isPlayingThruster = false;
    private bool togglePlayThruster = false;
    [SerializeField] float transitionTime = 1.0f;
    static private int currentSceneIdx = 0;
    private GameState gameState = GameState.playing;

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
            gameState = GameState.transending;
            Invoke("loadNextScene", transitionTime);
        }
    }

    private void onDead()
    {
        if (gameState == GameState.playing)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(deathClip);
            gameState = GameState.transending;
            Invoke("resetLevel", transitionTime);
        }
    }

    private void loadNextScene()
    {
        currentSceneIdx++;
        print("next scene idx " + currentSceneIdx);
        SceneManager.LoadScene(currentSceneIdx);
    }

    private void resetLevel()
    {
        currentSceneIdx = 0;
        SceneManager.LoadScene(currentSceneIdx);
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
            togglePlayThruster = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            togglePlayThruster = false;
        }

        if (togglePlayThruster && !isPlayingThruster)
        {
            audioSource.PlayOneShot(thrustClip);
            isPlayingThruster = true;
        }

        if (!togglePlayThruster && isPlayingThruster)
        {
            audioSource.Stop();
            isPlayingThruster = false;
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
