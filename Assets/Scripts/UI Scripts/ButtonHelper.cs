using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper Class for Button Functionality, can be used for Playing a Sound, Loading a Scene, or Exiting the Application.
/// Also Controls the Back Button/Escape functionality.
/// </summary>
public class ButtonHelper : MonoBehaviour
{
    #region MONOBEHAVIOUR_FUNCTIONS
    private void Update()
    {
        if(Input.GetButton("Cancel"))
        {
            if (!SceneController.instance) { Debug.LogError("ButtonHelper: SceneController is not loaded, preventing cancel!"); return; }

            if(SceneController.instance.activeScene == "MainMenu")
            {
                ExitApplication();
            }
            else
            {
                SceneController.instance.LoadScene("MainMenu");
            }
        }
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    // Uses the SceneController to load a scene.
    public void LoadScene(string sceneName)
    {
        if (!SceneController.instance) { Debug.LogError("ButtonHelper: Ensure you are runnning from the _pre scene to setup the scenecontroller!"); return; }

        // Loads the Game Scene or the Main Menu
        SceneController.instance.LoadScene(sceneName);
    }

    // Quits the application.
    public void ExitApplication()
    {
        Application.Quit();
    }

    // Plays an AudioClip on the UI audio channel.
    public void PlaySFX(AudioClip clip)
    {
        if(!AudioController.instance) { Debug.LogError("ButtonHelper: AudioController instance was not found in the scene!"); return; }

        AudioController.instance.PlayAudio(clip, EAudioChannel.SFX);
    }
    #endregion
}
