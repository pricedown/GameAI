using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class HexGrid : MonoBehaviour
{
    
    [SerializeField] private HexObject hexagonPrefab;
    public int gridRadius = 7;
    public float gap = 0.2f;
    public Dictionary<Hex, HexObject> Map;
    private readonly float _sideLength = 0.5f;
    
    private Quaternion _rot;
    private Vector2 _origin;
    private Vector2 _deltaQ, _deltaR, _deltaS;

    private void Awake()
    {
        Map = new Dictionary<Hex, HexObject>();
        _rot = Quaternion.Euler(0, 0, 30); // rotation to make pointy-tipped
        InitGrid(gridRadius);
    }

    public void InitGrid(int radius)
    {
        foreach (var kv in Map)
            Destroy(kv.Value.gameObject);
        Map = new Dictionary<Hex, HexObject>();
        
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
    
            for (int r = r1; r <= r2; r++)
            {
                Hex hex = new Hex(q, r);
                
                CreateHexObject(hex, hexagonPrefab);
            }
        }
    }

    private HexObject CreateHexObject(Hex hex, HexObject type)
    {
        if (HexOccupied(hex))
            return null;
        
        HexObject hexObj = Instantiate(type, HexToWorld(hex), _rot, transform);
        hexObj.SetHex(hex);

        Map.Add(hex, hexObj);
        
        return hexObj;
    }

    public bool InBounds(int q, int r)
    {
        int minX = -gridRadius;
        int maxX = gridRadius;
        int minY = Mathf.Max(-gridRadius, -gridRadius - q);
        int maxY = Mathf.Min(gridRadius, gridRadius - q);
    
        return q >= minX && q <= maxX && r >= minY && r <= maxY;
    }

    public bool HexOccupied(Hex hex)
    {
        return Map.ContainsKey(hex);
    }

    private Vector2 HexToWorld(Hex hex)
    {
        return CubeToWorld(hex.q, hex.r, hex.s);
    }

    private Vector2 CubeToWorld(int q, int r, int s)
    {
        _deltaQ = Quaternion.Euler(0, 0, 120) * Vector3.down * _sideLength;
        _deltaR = Vector3.down * _sideLength;
        _deltaS = Quaternion.Euler(0, 0, -120) * Vector3.down * _sideLength;
        _origin = transform.position;
        return _origin + transform.localScale * (1+gap)*(_deltaQ * q + _deltaR * r + _deltaS * s);
    }
}