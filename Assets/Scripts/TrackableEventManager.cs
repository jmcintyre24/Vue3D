using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// Child class of DefaultTrackableEventHandler to allow for more control on Events when tracking and Setup.
/// </summary>
public class TrackableEventManager : DefaultTrackableEventHandler
{
    // Used to keep track of if this object was succesfully loaded.
    [HideInInspector]
    public bool isLoaded = false;

    [SerializeField]
    GameObject toolTipBox;

    #region MONOBEHAVIOUR_FUNCTIONS
    private void Awake()
    {
        StartCoroutine(Setup());
    }

    private void OnApplicationPause(bool pause)
    {
        // If the applcation is unpaused and we were already loaded, reset the focus mode just in case.
        if (!pause)
        {
            if (isLoaded && CameraDevice.Instance != null)
            {
                CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            }
        }
    }
    #endregion

    #region PROTECTED_FUNCTIONS
    // Called every time tracking is found, changes the current GameState to TargetFound
    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        if(GameState.instance)
            GameState.instance.current = GameState.EState.TargetFound;

        if(toolTipBox)
            toolTipBox.SetActive(false);
    }

    // Called every time tracking is lost, changes the current GameState to TargetLost
    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        if (GameState.instance)
            GameState.instance.current = GameState.EState.TargetLost;

        if (toolTipBox)
            toolTipBox.SetActive(true);
    }
    #endregion

    #region PRIVATE_FUNCTIONS
    private IEnumerator Setup()
    {
        // Let's wait until the main camera device has an instance to continue.
        yield return new WaitUntil(() => { return CameraDevice.Instance != null && TrackerManager.Instance != null && GameState.instance; });
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO); // Make the camera continually focus.
        TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();

        // Subscribe to OnStateChange
        GameState.instance.OnStateChange.AddListener(OnStateChange);
        isLoaded = true;
    }

    private void OnStateChange(GameState.EState state)
    {
        switch (state)
        {
            // Stop tracking if we are in a Menu or Paused.
            case GameState.EState.Menu:
            case GameState.EState.Paused:
                TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
                toolTipBox.SetActive(false);
                break;
            case GameState.EState.StartTracking:
                TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
                toolTipBox.SetActive(true);
                break;
            default:
                break;
        }
    }
    #endregion
}
