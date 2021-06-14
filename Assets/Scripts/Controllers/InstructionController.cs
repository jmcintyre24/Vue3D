using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the displaying of Instructions when the user first enters the Game Scene
/// </summary>
public class InstructionController : MonoBehaviour
{
    [SerializeField]
    GameObject[] instructionSets;
    int m_InstructionID = 0;

    private void Start()
    {
        if (GameManager.instance.displayedInstructions == true)
        {
            gameObject.SetActive(false);
            GameState.instance.current = GameState.EState.StartTracking;
        }
    }

    private void Update()
    {
        // Let's ensure the GameManager has been instantiated and is loaded
        if(GameManager.instance && GameManager.instance.isLoaded)
        {
            // Let's check if the instructions had already been displayed, if they haven't, did the user press any button?
            if (!GameManager.instance.displayedInstructions && Input.anyKeyDown)
            {
                // Increment what instruction ID we're on
                if (m_InstructionID < instructionSets.Length - 1)
                {
                    // Disable the current instructions shown
                    instructionSets[m_InstructionID].SetActive(false);
                    // Increment the instructionID and then display those instructions
                    instructionSets[++m_InstructionID].SetActive(true);
                }
                else
                {
                    // Have the instructions shut-off.
                    GameManager.instance.displayedInstructions = true;
                    GameState.instance.current = GameState.EState.StartTracking;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
