using System;
using UnityEngine;
using TMPro;

public class HexObject : MonoBehaviour
{
    [SerializeField] private GameObject aliveVisual;
    [SerializeField] private HexLife life;
    private Hex _hex;

    private void Awake()
    {
        life = FindFirstObjectByType<HexLife>();
    }

    public Hex GetHex()
    {
        return _hex;
    }

    public void SetHex(Hex hex)
    {
        this._hex = hex;
    }
    
    public void OnHexClicked()
    {
        life.ToggleCell(_hex);
    }

    public void SetAlive(bool alive)
    {
        aliveVisual.SetActive(alive);
    }
}