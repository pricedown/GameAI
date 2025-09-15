using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class LifeDebug : MonoBehaviour
{
    public HexLife hexLife;
    public Rect screenArea = new Rect(20, 10, 300, 300);

    private float playSpeed = 1;
    private bool isPlaying = false;
    private Coroutine playLoop;

    private IEnumerator CoPlay()
    {
        while (true)
        {
            if (isPlaying)
                hexLife.Step();
            yield return new WaitForSeconds(playSpeed);
        }
    }

    private void Awake()
    {
        playLoop = StartCoroutine(CoPlay());
    }

    float scale;
    void OnGUI()
    {
        GUILayout.BeginArea(screenArea, GUI.skin.box);
        GUILayout.Label("Life Debug Panel");
        
        GUILayout.Label($"Play step delay: {playSpeed:F2}");
        playSpeed = GUILayout.HorizontalSlider(playSpeed, 1f, 0.3f);
         
        if (GUILayout.Button("Play / Stop"))
        {
            isPlaying = !isPlaying;
            Debug.Log("Play / Stop");
        }

        if (GUILayout.Button("Next step"))
        {
            hexLife.Step();
            Debug.Log("Stepped");
        }
        
        GUILayout.Space(10);
        GUILayout.Label($"Grid scale: {hexLife.grid.transform.localScale.x:F2}");
        scale = GUILayout.HorizontalSlider(hexLife.grid.transform.localScale.x, 0, 10.0f);
        if (!Mathf.Approximately(scale, hexLife.grid.transform.localScale.x))
            hexLife.grid.transform.localScale = new Vector3(scale, scale, scale);
        GUILayout.Label($"Grid radius: {hexLife.grid.gridRadius:F2}");
        hexLife.grid.gridRadius = (int)GUILayout.HorizontalSlider(hexLife.grid.gridRadius, 1, 30);
        GUILayout.Label($"Grid gap: {hexLife.grid.gap:F2}");
        hexLife.grid.gap = GUILayout.HorizontalSlider(hexLife.grid.gap, 0, 1.0f);

        GUILayout.Space(10);
        if (GUILayout.Button("Reset grid"))
        {
            hexLife.Clear();
            hexLife.grid.InitGrid(hexLife.grid.gridRadius);
            hexLife.UpdateDisplay();
            
            Debug.Log("Cleared & Reset");
        }
        

        GUILayout.EndArea();
    }
    
    
}