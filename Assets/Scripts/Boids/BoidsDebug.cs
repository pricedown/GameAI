using UnityEngine;

namespace Boids
{

    public class BoidsDebug : MonoBehaviour
    {
        public Rect screenArea = new Rect(20, 10, 300, 1000);
    
        public Boids boidManager;
        private BoidParams currentParams;

        void Start()
        {
            currentParams = boidManager.boidParams;
        }

        void OnGUI()
        {
            GUILayout.BeginArea(screenArea, GUI.skin.box);
            GUILayout.Label("Boids Debug Panel");

            GUILayout.Label($"Move Speed: {currentParams.moveSpeed:F2}");
            currentParams.moveSpeed = GUILayout.HorizontalSlider(currentParams.moveSpeed, 0f, 20f);

            GUILayout.Label($"Turn Speedf: {currentParams.turnSpeed:F2}");
            currentParams.turnSpeed = GUILayout.HorizontalSlider(currentParams.turnSpeed, 0f, 10f);

            currentParams.isAlignmentEnabled = GUILayout.Toggle(currentParams.isAlignmentEnabled, "Alignment Enabled");
            GUILayout.Label($"Alignment Radius: {currentParams.alignmentRadius:F2}");
            currentParams.alignmentRadius = GUILayout.HorizontalSlider(currentParams.alignmentRadius, 0f, 10f);
            GUILayout.Label($"Alignment Weight: {currentParams.alignmentWeight:F2}");
            currentParams.alignmentWeight = GUILayout.HorizontalSlider(currentParams.alignmentWeight, 0f, 5f);

            currentParams.isCohesionEnabled = GUILayout.Toggle(currentParams.isCohesionEnabled, "Cohesion Enabled");
            GUILayout.Label($"Cohesion Radius: {currentParams.cohesionRadius:F2}");
            currentParams.cohesionRadius = GUILayout.HorizontalSlider(currentParams.cohesionRadius, 0f, 10f);
            GUILayout.Label($"Cohesion Weight: {currentParams.cohesionWeight:F2}");
            currentParams.cohesionWeight = GUILayout.HorizontalSlider(currentParams.cohesionWeight, 0f, 5f);

            currentParams.isSeparationEnabled = GUILayout.Toggle(currentParams.isSeparationEnabled, "Separation Enabled");
            GUILayout.Label($"Separation Radius: {currentParams.separationRadius:F2}");
            currentParams.separationRadius = GUILayout.HorizontalSlider(currentParams.separationRadius, 0f, 10f);
            GUILayout.Label($"Separation Weight: {currentParams.separationWeight:F2}");
            currentParams.separationWeight = GUILayout.HorizontalSlider(currentParams.separationWeight, 0f, 5f);

            GUILayout.Space(10);
            if (GUILayout.Button("Update Boid Params"))
            {
                boidManager.UpdateParams(currentParams);
                Debug.Log("Updated boid parameters via debug panel.");
            }

            GUILayout.EndArea();
        }
    }

}