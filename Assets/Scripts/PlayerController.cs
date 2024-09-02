using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public InputActionReference movementAction;
    public InputActionReference fireLeftWeaponAction;
    public InputActionReference fireRightWeaponAction;
    public InputActionReference bombingAction;
    public InputActionReference reloadLevel;
    public InputActionReference loadNextLevel;
    public InputActionReference loadPrevLevel;
    public InputActionReference continueAction;

    [SerializeField] GameObject leftWeapon;
    [SerializeField] GameObject rightWeapon;
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

    private void Start()
    {
        SetWeaponFiringState(false, leftWeapon);
        SetWeaponFiringState(false, rightWeapon);
    }
    void Awake()
    {
        EnableHandlingControls();    
    }


    void OnEnable()
    {
        bombingAction.action.performed += ProcessBombingInput;
        //movementAction.action.started += ProcessMovementInput;
        //firingAction.action.performed += ProcessFiringInput;
        fireLeftWeaponAction.action.performed += FireLeft;
        fireLeftWeaponAction.action.canceled += FireLeft;
        fireRightWeaponAction.action.performed += FireRight;
        fireRightWeaponAction.action.canceled += FireRight;

        reloadLevel.action.performed += ReloadLevel;

        loadNextLevel.action.performed += LoadNextLevel;
        loadPrevLevel.action.performed += LoadPrevLevel;
        continueAction.action.performed += HandleContinue;

        //movementAction.Enable();
        //firingAction.Enable();
        //bombingAction.Enable();


    }

    void OnDisable()
    {
        bombingAction.action.performed -= ProcessBombingInput;
        //movementAction.action.started -= ProcessMovementInput;
        //firingAction.action.performed += ProcessFiringInput;
        fireLeftWeaponAction.action.performed -= FireLeft;
        fireLeftWeaponAction.action.canceled -= FireLeft;
        fireRightWeaponAction.action.performed -= FireRight;
        fireRightWeaponAction.action.canceled -= FireRight;
        reloadLevel.action.performed -= ReloadLevel;
        loadNextLevel.action.performed -= LoadNextLevel;
        loadPrevLevel.action.performed -= LoadPrevLevel;
        continueAction.action.performed -= HandleContinue;
        //movementAction.Disable();
        //firingAction.Disable();
        //bombingAction.Disable();
    }

    void Update()
    {
        //Handle player input controlling position and rotation of space ship
        ProcessMovementInput();
        //Handle player input firing basic weapons
        //ProcessFiringInput();
        //ProcessBombingInput();
    }

    void ProcessBombingInput(InputAction.CallbackContext obj)
    {
        Debug.Log("Processing Bomb");
        if(PlayerStatManager.playerStatManagerInstance.getBombCount() > 0)
        {
            Debug.Log("Triggering Bomb");
            triggerPlayerBomb();
        }
        /*if (bombingAction.action.ReadValue<float>() == 1.0 && playerStatManager.getBombCount() > 1)
        {
            triggerPlayerBomb();
        }*/
    }

    void triggerPlayerBomb()
    {
        PlayerStatManager.playerStatManagerInstance.updateBombs(-1);
        bomb.SetActive(true);
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

        /*for (int i = 0; i < lasers.Length; i++)
        {
            ParticleSystem.EmissionModule laserEmitter = lasers[i].GetComponent<ParticleSystem>().emission;
            laserEmitter.enabled = laserFiringState;
        }*/
    }

    void SetBombFiringState(bool bombFiringState)
    {
        //Since firingAction is set up as a value, a button press returns float of 1.0
        if (bombingAction.action.ReadValue<float>() == 1.0 && PlayerStatManager.playerStatManagerInstance.getBombCount() > 1)
        {
            triggerPlayerBomb();
            bomb.SetActive(true);
        }
    }

    //Stores user input values into variables used to affect ship position and rotation
    void ProcessMovementInput()
    {
        xInputValue = movementAction.action.ReadValue<Vector2>().x;
        yInputValue = movementAction.action.ReadValue<Vector2>().y;

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

    private void HandleContinue(InputAction.CallbackContext obj)
    {
        if(GameSessionManager.gameSessionManagerInstance.isTimerCountingDown())
        {
            
            SceneHandler.sceneHandlerInstance.RestartLevel();
            GameSessionManager.gameSessionManagerInstance.InitializeNewRound();

        }
    }
    public void DisableHandlingControls()
    {
        bombingAction.action.Disable();
        //movementAction.action.started -= ProcessMovementInput;
        //firingAction.action.performed += ProcessFiringInput;
        fireLeftWeaponAction.action.Disable();
        reloadLevel.action.Disable();
        loadNextLevel.action.Disable();
    }

    public void EnableHandlingControls()
    {
        bombingAction.action.Enable();
        //movementAction.action.started -= ProcessMovementInput;
        //firingAction.action.performed += ProcessFiringInput;
        fireLeftWeaponAction.action.Enable();
        reloadLevel.action.Enable();
        loadNextLevel.action.Enable();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (40 * transform.forward));
    }
}
