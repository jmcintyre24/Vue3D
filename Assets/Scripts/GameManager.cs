using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// - SINGLETON -
/// Core App System that'll Manage other Core Systems.
/// </summary>
public class GameManager : MonoBehaviour
{
    // For reference Cross-Scenes, let's have a static instance.
    [HideInInspector]
    static public GameManager instance;
    // References to Other Controllers || Managers
    [HideInInspector]
    public ObjectManipulator rootController;
    [HideInInspector]
    public TrackableEventManager targetEventManager;

    // Used to keep track of loading progress as the GameManager finds and waits for other systems to be loaded.
    public float loadProgress = 0.0f;
    public bool isLoaded = false;
    // Track if the user has displayed the structions in the current session.
    public bool displayedInstructions = false;

    #region MONOBEHAVIOUR_FUNCTIONS
    private void Awake()
    {
        // Ensure there is not existing instance of the GameManager, if there is, delete it!
        if (instance) {
            Destroy(this.gameObject);
            return;
        }

        // Set's the instance to this object.
        instance = this;
        DontDestroyOnLoad(this.gameObject); // Mark the Game Object as Do Not Destroy.

        // Subscribe OnSceneLoaded function to the SceneLoaded Event.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    #endregion

    #region PRIVATE_FUNCTIONS
    // Called when a scene is done loading.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If it's the main menu, no reason to gather objects.
        if (scene.name == "MainMenu" || scene.name == "_pre")
            return;

        // When a scene first loads, let's try to gather all the required objects
        StartCoroutine(LoadObjects());
    }

    // This is where scene loading occurs so we can properly keep track of progress for the load menu.
    IEnumerator LoadObjects()
    {
        // Wait till we gather all scene objects and they are loaded
        yield return StartCoroutine(GatherSceneObjects());
        // Gather any other objects here that are not a Singleton on SceneLoad
        // We're done loading!
        loadProgress = 100;

        // Pause here briefly to display the Load Screen at 100%
        yield return new WaitForSeconds(0.75f);
        isLoaded = true;
    }
    
    // Gather's Scene Specific Objects (Object Controllers, AR Manager, 
    IEnumerator GatherSceneObjects()
    {
        // Find the Game Controller in the scene.
        GameObject obj = GameObject.FindWithTag("Object");
        if (!obj) { Debug.LogError("GameManager: Could not find GameObject with the Object tag!"); yield break; };
        rootController = obj.GetComponent<ObjectManipulator>();
        if (!rootController) { Debug.LogError($"GameManager: Could not find the component ObjectController on {rootController.name} GameObject"); };
        loadProgress = 35;
        // Find the AR Manager in the scene.
        obj = GameObject.FindWithTag("ImageTarget");
        if (!obj) { Debug.LogError("GameManager: Could not find GameObject with the ImageTarget tag!"); yield break; };
        targetEventManager = obj.GetComponent<TrackableEventManager>();
        if (!targetEventManager) { Debug.LogError($"GameManager: Could not find the component TrackableEventManager on {rootController.name} GameObject"); };
        loadProgress = 70;
        // Wait Until the AR Manager has been loaded.
        yield return new WaitUntil(() => { return targetEventManager.isLoaded; });
    }
    #endregion
}
