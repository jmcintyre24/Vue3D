using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Attached GameObject.
/// </summary>
public class ObjectManipulator : MonoBehaviour
{
    // Booleans used to mark if an object can move or rotate.
    public bool canMove = false, doRotate = false;
    [Header("Movement & Scaling Settings")]
    [SerializeField]
    float movementSpeed = 0.1f;
    [SerializeField]
    float rotationSpeed = 50.0f;
    [SerializeField]
    float minScale = 0.1f, maxScale = 5.0f;
    int m_RotID = -1;

    Vector3 m_PreviousPos;
    Vector3 m_ScaleOfRoot
    {
        get
        {
            return transform.localScale;
        }
        set
        {
            transform.localScale = value;
        }
    }

    #region Unity Functions
    private void Update()
    {
        // Ensure there is an instance of the GameState, then check the current state to ensure the object is being tracked or can move.
        if (!GameState.instance || GameState.instance.current != GameState.EState.TargetFound)
            return;

        // Does the user still have the rotate buttons held?
        if (doRotate)
        {
            transform.Rotate(Rotate(m_RotID) * rotationSpeed * Time.deltaTime);
        }

        // Did the user click in the movable space?
        if (canMove)
        {
            CheckForMovement();
            CheckForPinchToScale();
        }
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    #region MUTATORS
    public void SetRotationSpeed(float v)
    {
        rotationSpeed = v;
    }

    public void SetMovementSpeed(float v)
    {
        movementSpeed = v;
    }

    public void ToggleMovement(bool v)
    {
        canMove = v;
    }

    public void RotateBegin(int rotationID)
    {
        m_RotID = rotationID;
        doRotate = true;
    }

    public void RotateEnd()
    {
        doRotate = false;
    }
    #endregion

    // 1 = x, 2 = y, 3 = z
    public Vector3 Rotate(int eulerRotation)
    {
        switch (eulerRotation)
        {
            case -3:
                return -Vector3.forward;
            case -2:
                return -Vector3.up;
            case -1:
                return -Vector3.right;
            case 1:
                return Vector3.right;
            case 2:
                return Vector3.up;
            case 3:
                return Vector3.forward;
            default:
                break;
        }

        return Vector3.zero;
    }

    // Set's the root back to it's default values
    public void ResetObject()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
    #endregion

    #region PRIVATE_FUNCTIONS
    // Called Per Frame, checks if input has been given for movement.
    private void CheckForMovement()
    {
        // Happens only on key push
        if (Input.GetButtonDown("Primary"))
        {
            if (Input.touchSupported)
            {
                m_PreviousPos = Input.GetTouch(0).position; // Reset the previous position so it doesn't move right away
            }
            else
            {
                m_PreviousPos = Input.mousePosition;
            }

        }

        // Check if the LeftMouse
        if (Input.GetButton("Primary"))
        {
            // Get screen position of the Mouse or Fingers
            Vector3 r;
            if (Input.touchSupported)
            {
                r = Input.GetTouch(0).position;
            }
            else
            {
                r = Input.mousePosition;
            }

            // Get the direction from the last position we had.
            Vector3 dir = (-r + m_PreviousPos);
            dir = new Vector3(dir.x, 0.0f, dir.y);

            // Update the previous position to the one we're at to ensure we're tracking the changes in movement.
            m_PreviousPos = r;

            // Move the object
            transform.position += movementSpeed * dir * Time.deltaTime;
        }
    }

    // Called Per Frame, checks if input has been given for scaling.
    private void CheckForPinchToScale()
    {
        if (Input.GetButton("Primary"))
        {
            Vector3 newScale = m_ScaleOfRoot * (1.0f + Input.GetAxis("Mouse ScrollWheel"));
            if (newScale.x > minScale && newScale.x < maxScale)
            {
                m_ScaleOfRoot *= 1.0f + Input.GetAxis("Mouse ScrollWheel");
            }
        }

        // Ensure there are two fingers down
        if (Input.touchCount == 2)
        {
            // Get the first and second input from touch
            Touch inputZero = Input.GetTouch(0);
            Touch inputOne = Input.GetTouch(1);

            // Find their previous positions
            Vector2 inputZeroPrevPosition = inputZero.position - inputZero.deltaPosition;
            Vector2 inputOnePrevPosition = inputOne.position - inputOne.deltaPosition;

            // Get the distance (magnitude) of the finger's previous and current positions.
            float prevMagnitude = Vector2.Distance(inputZeroPrevPosition, inputOnePrevPosition);
            float currMagnitude = Vector2.Distance(inputZero.position, inputOne.position);

            // Find the difference between the current magnitude and the old magnitude.
            float difference = currMagnitude - prevMagnitude;

            // Let's temporary store the variable so we can check if it breaks out Min's and Max's
            Vector3 newScale = m_ScaleOfRoot * (1.0f + (difference * Time.deltaTime));
            if (newScale.x > minScale && newScale.x < maxScale)
            {
                // Scale the root GameObject up.
                m_ScaleOfRoot *= 1.0f + (difference * Time.deltaTime);
            }
        }
    }
#endregion
}
