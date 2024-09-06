using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Actions
    public InputActionReference movementAction;
    public InputActionReference fireLeftWeaponAction;
    public InputActionReference fireRightWeaponAction;
    public InputActionReference bombingAction;
    public InputActionReference reloadLevel;
    public InputActionReference loadNextLevel;
    public InputActionReference loadPrevLevel;
    public InputActionReference continueAction;

    //Player Weapons
    [SerializeField] GameObject leftWeapon;
    [SerializeField] GameObject rightWeapon;
    [SerializeField] GameObject bomb;

    //Speeds and clamping
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

    //Movement and movement input smoothing 
    [SerializeField] float smoothSpeedDuration = 0.5f; 
    private Vector2 currentMovementInput;
    private Vector2 currentMovementVelocity;
    float xInputValue;
    float yInputValue;

    //Guarantees that lasers are not started in a firing state
    void Start()
    {
        SetWeaponFiringState(false, leftWeapon);
        SetWeaponFiringState(false, rightWeapon);
    }
    
    //Re-Enables handling controls on continue
    void Awake()
    {
        EnableHandlingControls();    
    }


    //Enables all the event subscriptions for player inputs
    void OnEnable()
    {
        fireLeftWeaponAction.action.performed += FireLeft;
        fireLeftWeaponAction.action.canceled += FireLeft;
        fireRightWeaponAction.action.performed += FireRight;
        fireRightWeaponAction.action.canceled += FireRight;
        bombingAction.action.performed += ProcessBombingInput;

        reloadLevel.action.performed += ReloadLevel;
        loadNextLevel.action.performed += LoadNextLevel;
        loadPrevLevel.action.performed += LoadPrevLevel;
        continueAction.action.performed += HandleContinue;

    }

    void OnDisable()
    {
        
        fireLeftWeaponAction.action.performed -= FireLeft;
        fireLeftWeaponAction.action.canceled -= FireLeft;
        fireRightWeaponAction.action.performed -= FireRight;
        fireRightWeaponAction.action.canceled -= FireRight;
        bombingAction.action.performed -= ProcessBombingInput;

        reloadLevel.action.performed -= ReloadLevel;
        loadNextLevel.action.performed -= LoadNextLevel;
        loadPrevLevel.action.performed -= LoadPrevLevel;
        continueAction.action.performed -= HandleContinue;
    }

    //Movement processing is the only non-event-driven input.
    void Update()
    {
        ProcessMovementInput();
    }

    //Stores user input values into variables used to affect ship position and rotation
    void ProcessMovementInput()
    {
        //Storing player input to be smoothed
        Vector2 movementActionInput = movementAction.action.ReadValue<Vector2>();
        
        //Update the current movement input vector with the smoothed form of the movement input, relative to the current input.
        //Updates velocity as part of SmoothDamp function. 
        currentMovementInput = Vector2.SmoothDamp(currentMovementInput, movementActionInput, ref currentMovementVelocity, smoothSpeedDuration);
        
        xInputValue = currentMovementInput.x;
        yInputValue = currentMovementInput.y;

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

        Vector3 newMovementVector = new Vector3(xOffset, yOffset, transform.localPosition.z);

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

    private void FireLeft(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            SetWeaponFiringState(true, leftWeapon);
        }
        else if (obj.canceled)
        {
            SetWeaponFiringState(false, leftWeapon);
        }
    }

    private void FireRight(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            SetWeaponFiringState(true, rightWeapon);
        }
        else if (obj.canceled)
        {
            SetWeaponFiringState(false, rightWeapon);
        }
    }

    void ProcessBombingInput(InputAction.CallbackContext obj)
    {
        if (PlayerStatManager.playerStatManagerInstance.getBombCount() > 0)
        {
            triggerPlayerBomb();
        }
    }

    void SetWeaponFiringState(bool weaponFiringState, GameObject weapon)
    {
        //My initial approach was to set lasers[i].enabled = laserFiringState, but
        //this resulted in an interaction where the in-flight particles were also
        //removed from the world when no longer firing.

        //Disabling the emission module disabled additional particles from firing, while
        // maintaining in-flight particles.
        ParticleSystem.EmissionModule weaponEmitter = weapon.GetComponent<ParticleSystem>().emission;
        weaponEmitter.enabled = weaponFiringState;
    }
    void triggerPlayerBomb()
    {
        PlayerStatManager.playerStatManagerInstance.updateBombs(-1);
        bomb.SetActive(true);
    }

    //Enables controls that were disabled when the player died
    public void EnableHandlingControls()
    {
        bombingAction.action.Enable();
        fireLeftWeaponAction.action.Enable();
        fireRightWeaponAction.action.Enable();
        
        reloadLevel.action.Enable();
        loadPrevLevel.action.Enable();
        loadNextLevel.action.Enable();
    }

    //Disables player controls when the player dies
    public void DisableHandlingControls()
    {
        bombingAction.action.Disable();
        fireLeftWeaponAction.action.Disable();
        fireRightWeaponAction.action.Disable();
        
        reloadLevel.action.Disable();
        loadPrevLevel.action.Disable();
        loadNextLevel.action.Disable();
    }

    //If the continue button is pressed and the player has not died, do nothing.
    //If the player has died (i.e. isTimerCountingDown() returns true, then
    //restart the level and initialize a new round (session maintained).
    private void HandleContinue(InputAction.CallbackContext obj)
    {
        if (GameSessionManager.gameSessionManagerInstance.isTimerCountingDown())
        {
            SceneHandler.sceneHandlerInstance.RestartLevel();
            GameSessionManager.gameSessionManagerInstance.InitializeNewRound();
        }
    }

    private void ReloadLevel(InputAction.CallbackContext obj)
    {
        SceneHandler.sceneHandlerInstance.RestartLevel();
        GameSessionManager.gameSessionManagerInstance.InitializeNewRound();
    }

    private void LoadNextLevel(InputAction.CallbackContext obj)
    {
        SceneHandler.sceneHandlerInstance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void LoadPrevLevel(InputAction.CallbackContext obj)
    {
        SceneHandler.sceneHandlerInstance.LoadLevel(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (40 * transform.forward));
    }
}
