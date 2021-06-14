using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// - SINGLETON -
/// Manages switching between scenes and the loading screen.
/// </summary>
public class SceneController : MonoBehaviour
{
    // For reference Cross-Scenes, let's have a static instance.
    [HideInInspector]
    public static SceneController instance;

    // The current active scene
    public string activeScene;

    // References the essentially loading objs so that the progress can properly be displayed.
    public GameObject loadingCanvas;
    public Slider progressBar;
    public Text progressTXT;

    // Progress Values for Scene Loading & Game Manager loading.
    float m_SceneLoadProgress = 0.0f;
    float m_GameManagerProgress = 0.0f;

    // Keeps track of the loading || unloading that is occuring so we can properly display the loading screen.
    List<AsyncOperation> m_Operations = new List<AsyncOperation>();

    #region MONOBEHAVIOUR_FUNCTIONS
    private void Awake()
    {
        // Checking if there is an instance already
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Load directly into the next scene (MainMenu in our case)
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        activeScene = "MainMenu";
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    // Loads a scene asynchronously based on the passed in string. Scene must be in added into the build! 
    public void LoadScene(string sceneName)
    {
        // Activating the loading screen
        loadingCanvas.SetActive(true);
        // Reset the load progresses
        m_SceneLoadProgress = m_GameManagerProgress = 0.0f;

        // Unloading the Main Menu and then loading the Game Scene
        m_Operations.Add(SceneManager.UnloadSceneAsync(activeScene));
        m_Operations.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));

        // Check if we're loading the MainMenu or another Game Scene so we know whether to track the Game Manager's load progress.
        if (sceneName == "MainMenu")
        {
            StartCoroutine(GetSceneLoadProgress(true));
        }
        else
        {
            StartCoroutine(GetSceneLoadProgress());
            StartCoroutine(GetManagerProgress());
        }

        activeScene = sceneName;
    }
    #endregion

    #region PRIVATE_FUNCTIONS
    // Gather's the progress for the actual scene loading.
    public IEnumerator GetSceneLoadProgress(bool hideLoadingAfter = false)
    {
        // Goes throught the operations and checks if they are done
        for (int i = 0; i < m_Operations.Count; i++)
        {
            while (!m_Operations[i].isDone)
            {
                // Calculating the Scene Loading progress and displaying it on the loading screen
                float totalProgress = 0;
                foreach (AsyncOperation op in m_Operations)
                {
                    totalProgress += op.progress;
                }
                m_SceneLoadProgress = (totalProgress / (float)m_Operations.Count) * 100.0f;
                progressBar.value = Mathf.RoundToInt(m_SceneLoadProgress);
                progressTXT.text = progressBar.value.ToString() + "%";
                yield return null;
            }
        }

        if (hideLoadingAfter)
            loadingCanvas.SetActive(false);
    }

    // Gather's Progress based on what the GameManager's currently waiting for.
    public IEnumerator GetManagerProgress()
    {
        while (!GameManager.instance || !GameManager.instance.isLoaded)
        {
            // Ensure the GameManager is loaded
            if (!GameManager.instance)
                yield return null;
            else
                m_GameManagerProgress = GameManager.instance.loadProgress;

            // Displays the loading progress and the loading bar
            progressBar.value = (m_GameManagerProgress + m_SceneLoadProgress) / 2.0f;
            progressTXT.text = progressBar.value.ToString() + "%";
            yield return null;
        }

        yield return new WaitForSeconds(0.75f);
        loadingCanvas.SetActive(false);
    }
    #endregion
}
