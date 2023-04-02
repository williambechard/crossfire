using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Tools : MonoBehaviour
{

    public List<Color> ColorSwatch = new();
    public static Tools Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnValidate()
    {
        ExportColors("colorNames");
    }
    void ExportColors(string fileName)
    {
        // Write to disk
        StreamWriter writer = new StreamWriter("Assets/Resources/" + fileName + ".txt", false);
        foreach (Color c in ColorSwatch)
        {
            writer.WriteLine(ColorUtility.ToHtmlStringRGB(c));
        }
        writer.Close();
    }
}
