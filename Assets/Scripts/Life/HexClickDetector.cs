using UnityEngine;

public class HexClickDetector : MonoBehaviour
{
    public Camera cam;
    public LayerMask hexLayer;

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, hexLayer))
        {
            HexObject hexObj = hit.collider.GetComponent<HexObject>();
            if (hexObj)
                hexObj.OnHexClicked();
        }
    }
}