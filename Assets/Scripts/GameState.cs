using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// - SINGLETON -
/// Tracks the Current State of the Game.
/// </summary>
public class GameState : MonoBehaviour
{
    // Enumeration Holding the States the GameState can be.
    public enum EState
    {
        Menu,
        Paused,
        StartTracking,
        TargetFound,
        TargetLost
    }

    // Override of base UnityEvent class so we can call a custom UnityEvent that takes in a State.
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<GameState.EState> { };

    // Static Instance so we can access cross-scene.
    [HideInInspector]
    public static GameState instance;

    // Unity Event fired when the state changes.
    public StateChangeEvent OnStateChange;

    // Keep track of the current and previous state.
    EState m_Curr;
    public EState current
    {
        get { return m_Curr; }
        set { ChangeState(value); }
    }

    #region MONOBEHAVIOUR_FUNCTIONS
    private void Awake()
    {
        // Ensure an instance doesn't already exist.
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Subscribe OnSceneLoaded function to the SceneLoaded Event.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    // Called when a scene is done loading.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                current = EState.Menu;
                break;
            default:
                current = EState.Paused;
                break;
        }

        // Resets the Listeners on the Event
        OnStateChange.RemoveAllListeners();
    }
    #endregion

    #region PRIVATE FUNCTIONS
    // Called when the current state changes.
    private void ChangeState(EState state)
    {
        // Set the current state to the passed in state.
        m_Curr = state;

        // Wait until the game is finished loading to invoke that the state has changed.
        StartCoroutine(WaitUntilLoadedToInvoke(state));
    }

    // Waits until the GameManager has finished loading to invoke the OnStateChanged event.
    private IEnumerator WaitUntilLoadedToInvoke(EState state)
    {
        yield return new WaitUntil(() => { return GameManager.instance && GameManager.instance.isLoaded; });
        OnStateChange.Invoke(state);
    }
    #endregion
}
