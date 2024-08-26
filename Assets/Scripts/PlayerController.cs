using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction movementAction;

    //To-Do: Implement firing
    //[SerializeField] InputAction fireAction;
    //To-Do: Implement bombing
    //[SerializeField] InputAction bombAction;

    [SerializeField] float playerSpeed = 20f;
    [SerializeField] float minRelativeXPosition = -11f;
    [SerializeField] float maxRelativeXPosition= 11f;
    [SerializeField] float minRelativeYPosition = -5f;
    [SerializeField] float maxRelativeYPosition = 11f;
    

    //Scalars to be updated based on impression of input/visual responsiveness
    [SerializeField] float positionPitchScalar = -2f;
    [SerializeField] float positionYawScalar = 2f;

    //Scalars to be updated based on impression of input/visual responsiveness
    [SerializeField] float inputPitchScalar = -15f;
    [SerializeField] float inputRollScalar = -20f;

    //Variables used to hold user input
    float xInputValue;
    float yInputValue;

    void OnEnable()
    {
        movementAction.Enable();
        // Other inputactions to be enabled once mapped
        // firingAction.Enable();
        // bombAction.Enable();
    }

    void OnDisable()
    {
        movementAction.Disable();
        // firingAction.Disable();
        // bombAction.Disable();
    }

    void Update()
    {
        ProcessUserInput();
        ProcessTranslation();
        ProcessRotation();
    }

    //Stores user input values into variables used to affect ship position and rotation
    void ProcessUserInput()
    {
        xInputValue = movementAction.ReadValue<Vector2>().x;
        yInputValue = movementAction.ReadValue<Vector2>().y;
    }

    //Moves the ship across the screen according to the input provided by the user
    void ProcessTranslation()
    {

        float xOffset = xInputValue * Time.deltaTime * playerSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, minRelativeXPosition, maxRelativeXPosition);

        float yOffset = yInputValue * Time.deltaTime * playerSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, minRelativeYPosition, maxRelativeYPosition);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    //Modifies the roll, pitch, and yaw of the ship so that the rig is facing as expected.
    void ProcessRotation()
    {
        // x = pitch, y = yaw, z = roll
        float pitchDueToPosition = transform.localPosition.y * positionPitchScalar;
        float pitchDueToControlThrow = yInputValue * inputPitchScalar;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = transform.localPosition.x * positionYawScalar;
        float roll = inputRollScalar * xInputValue;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    
}
