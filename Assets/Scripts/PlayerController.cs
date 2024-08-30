using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction movementAction;
    [SerializeField] InputAction firingAction;
    [SerializeField] InputAction bombingAction;
    [SerializeField] GameObject[] lasers;
    [SerializeField] GameObject bomb;

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

    PlayerStatManager playerStatManager;
    void Start()
    {
        playerStatManager = FindObjectOfType<PlayerStatManager>();
    }

    void OnEnable()
    {
        movementAction.Enable();
        firingAction.Enable();
        bombingAction.Enable();

    }

    void OnDisable()
    {
        movementAction.Disable();
        firingAction.Disable();
        bombingAction.Disable();
    }

    void Update()
    {
        //Handle player input controlling position and rotation of space ship
        ProcessMovementInput();
        
        //Handle player input firing basic weapons
        ProcessFiringInput();
        ProcessBombingInput();
    }

    void ProcessBombingInput()
    {
        if (bombingAction.ReadValue<float>() == 1.0 && playerStatManager.getBombCount() > 1)
        {
            triggerPlayerBomb();
        }
    }

    void ProcessFiringInput()
    {
        //Since firingAction is set up as a value, a button press returns float of 1.0
        if(firingAction.ReadValue<float>() == 1.0)
        {

            SetLaserFiringState(true);
        } 
        else
        {
            SetLaserFiringState(false);
        }
    }

    void triggerPlayerBomb()
    {
        playerStatManager.updateBombs(playerStatManager.getBombCount()-1);
        bomb.SetActive(true);
    }

    void SetLaserFiringState(bool laserFiringState)
    {
        //My initial approach was to set lasers[i].enabled = laserFiringState, but
        //this resulted in an interaction where the in-flight particles were also
        //removed from the world when no longer firing.
        
        //Disabling the emission module disabled additional particles from firing, while
        // maintaining in-flight particles.
        for (int i = 0; i < lasers.Length; i++)
        {
            ParticleSystem.EmissionModule laserEmitter = lasers[i].GetComponent<ParticleSystem>().emission;
            laserEmitter.enabled = laserFiringState;
        }
    }

    void SetBombFiringState(bool bombFiringState)
    {
        //Since firingAction is set up as a value, a button press returns float of 1.0
        if (bombingAction.ReadValue<float>() == 1.0 && playerStatManager.getBombCount() > 1)
        {
            triggerPlayerBomb();
            bomb.SetActive(true);
        }
    }

    //Stores user input values into variables used to affect ship position and rotation
    void ProcessMovementInput()
    {
        xInputValue = movementAction.ReadValue<Vector2>().x;
        yInputValue = movementAction.ReadValue<Vector2>().y;

        ProcessTranslation();
        ProcessRotation();
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

    public Vector3 GetPlayerShipPosition()
    {
        return this.gameObject.transform.parent.position;
    }
    
}
