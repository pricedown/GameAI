using UnityEngine;

namespace Boids
{
    public class BoidsDebug : MonoBehaviour
    {
        public Rect screenArea = new Rect(20, 10, 200, 100);
        
        void OnGUI()
        {
            GUILayout.BeginArea(screenArea);
            GUILayout.Label("Layout Example");
            if (GUILayout.Button("Another Button"))
            {
                Debug.Log("Clicked");
            }
            GUILayout.EndArea();
        }
    }
}
