using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile
{
    public GameObject mesh;
    public TextMeshPro textMesh;

    public Tile(GameObject mesh, TextMeshPro text)
    {
        this.mesh = mesh;
        this.textMesh = text;
    }
}
