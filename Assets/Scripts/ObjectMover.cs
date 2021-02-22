using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ObjectMover : MonoBehaviour
{
    [SerializeField] Vector3 moveVector;
    [Range(0.1f, 5)] [SerializeField] float durationForMoving;
    [Range(0.1f, 10)] [SerializeField] float moveSpeedFactor;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
    }

    private void handleMovement()
    {
        float cycles = Time.time / durationForMoving;
        float wave = Mathf.Sin(cycles * 2 * Mathf.PI);

        transform.Translate(moveVector * wave * moveSpeedFactor);
    }
}
