using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    // Sliders and Values for the color the user is creating.
    [SerializeField]
    Slider r, g, b;
    float m_Red, m_Green, m_Blue;
    // The Renderer that is going to get modified.
    [SerializeField]
    MeshRenderer rendererObjToModify;
    Material matToModify;

    #region MONOBEAHVIOUR_FUNCTIONS
    private void Awake()
    {
        // Let's create a new instance of the material so we don't modify the base one.
        matToModify = new Material(rendererObjToModify.material);
        rendererObjToModify.material = matToModify;
        // Subscribe all the slider's so when their values change it approriately changes the material.
        r.onValueChanged.AddListener((float v) => { m_Red = v; SetRGB(); });
        m_Red = r.value;
        g.onValueChanged.AddListener((float v) => { m_Green = v; SetRGB(); });
        m_Green = g.value;
        b.onValueChanged.AddListener((float v) => { m_Blue = v; SetRGB(); });
        m_Blue = b.value;
    }
    #endregion

    public void SetRGB()
    {
        matToModify.color = new Color(m_Red, m_Green, m_Blue);
    }
}
