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

    //Line Renderer component
    [SerializeField] LineRenderer bombRay;

    //Guarantees that lasers are not started in a firing state
    void Start()
    {
        bombRay.enabled = false;
        bombRay.useWorldSpace = false;
        SetLeftWeaponFiringState(false, leftWeapon);
        SetRightWeaponFiringState(false, rightWeapon);
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
        bombingAction.action.canceled += ProcessBombingInput;

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
        bombingAction.action.canceled -= ProcessBombingInput;

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

    //On press, start firing
    //On release, stop firing.
    private void FireLeft(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            SetLeftWeaponFiringState(true, leftWeapon);
        }
        else if (obj.canceled)
        {
            SetLeftWeaponFiringState(false, leftWeapon);
        }
    }

    //On press, start firing
    //On release, stop firing.
    private void FireRight(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            SetRightWeaponFiringState(true, rightWeapon);
        }
        else if (obj.canceled)
        {
            SetRightWeaponFiringState(false, rightWeapon);
        }
    }

    //On press (and while held), start up a line renderer which will show
    //the player where the bomb will go off.
    //Then on release, blow up the bomb.
    //Both only if the player has bombs.
    void ProcessBombingInput(InputAction.CallbackContext obj)
    {
        if (obj.performed && PlayerStatManager.playerStatManagerInstance.getBombCount() > 0)
        {
            toggleBombRangeIndicator(true);
        } 
        else if(obj.canceled && PlayerStatManager.playerStatManagerInstance.getBombCount() > 0)
        {
            toggleBombRangeIndicator(false);   
            triggerPlayerBomb();
        }
    }

    //It was easier to set these up as two different states than it was to manage their
    // audio collectively.
    void SetLeftWeaponFiringState(bool weaponFiringState, GameObject weapon)
    {
        //My initial approach was to set lasers[i].enabled = laserFiringState, but
        //this resulted in an interaction where the in-flight particles were also
        //removed from the world when no longer firing.

        //Disabling the emission module disabled additional particles from firing, while
        // maintaining in-flight particles.
        if (weaponFiringState)
        {
            AudioManager.audioManagerInstance.setLoopState("LeftLaserFiringSFX", true);
            AudioManager.audioManagerInstance.Play("LeftLaserFiringSFX");
        } else
        {
            AudioManager.audioManagerInstance.setLoopState("LeftLaserFiringSFX", false);
        }
        ParticleSystem.EmissionModule weaponEmitter = weapon.GetComponent<ParticleSystem>().emission;
        weaponEmitter.enabled = weaponFiringState;
    }

    void SetRightWeaponFiringState(bool weaponFiringState, GameObject weapon)
    {
        //My initial approach was to set lasers[i].enabled = laserFiringState, but
        //this resulted in an interaction where the in-flight particles were also
        //removed from the world when no longer firing.

        //Disabling the emission module disabled additional particles from firing, while
        // maintaining in-flight particles.
        if (weaponFiringState)
        {
            AudioManager.audioManagerInstance.setLoopState("RightLaserFiringSFX", true);
            AudioManager.audioManagerInstance.Play("RightLaserFiringSFX");
        }
        else
        {
            AudioManager.audioManagerInstance.setLoopState("RightLaserFiringSFX", false);
        }
        ParticleSystem.EmissionModule weaponEmitter = weapon.GetComponent<ParticleSystem>().emission;
        weaponEmitter.enabled = weaponFiringState;
    }

    //When the bomb is triggered, decrement the number of bombs, set it active, and
    //play the sfx.
    void triggerPlayerBomb()
    {
        PlayerStatManager.playerStatManagerInstance.updateBombs(-1);
        bomb.SetActive(true);
        AudioManager.audioManagerInstance.Play("ExplosionSFX");
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

    //If the player presses "R", reload the level.
    private void ReloadLevel(InputAction.CallbackContext obj)
    {
        SceneHandler.sceneHandlerInstance.RestartLevel();
        GameSessionManager.gameSessionManagerInstance.InitializeNewRound();
    }
    
    //If the player presses "K", load the next level (if there is a next level).
    //Logic is handled in the Load Level method.
    private void LoadNextLevel(InputAction.CallbackContext obj)
    {
        SceneHandler.sceneHandlerInstance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //If the player presses "P", load the previous level. Does not go back to menu.
    //Logic is handled in the Load Level method.
    private void LoadPrevLevel(InputAction.CallbackContext obj)
    {
        SceneHandler.sceneHandlerInstance.LoadLevel(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void toggleBombRangeIndicator(bool state)
    {
        bombRay.enabled = state;
    }

    public void LevelComplete()
    {
        SceneHandler.sceneHandlerInstance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SessionComplete()
    {
        GameSessionManager.gameSessionManagerInstance.initializeNewSession();
    }
}
