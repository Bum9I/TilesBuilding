using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private List<Material> tileMaterials = new List<Material>();

    void Awake()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in renderers)
        {
            tileMaterials.Add(meshRenderer.material);
        }
    }

    public void SetColor(bool isAvailable)
    {
        foreach (var material in tileMaterials)
        {
            material.color = isAvailable ? Color.green : Color.red;
        }
    }

    public void ResetColor()
    {
        foreach (var material in tileMaterials)
        {
            material.color = Color.white;
        }
    }
}