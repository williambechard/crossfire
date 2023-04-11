using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnifyText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] textMeshes;

    [SerializeField]
    private string _initText;
    
    private string _text;

    public string Text
    {
        get { return _text;}
        set
        {
            _text = value;
            foreach (var textMesh in textMeshes)
            {
                textMesh.text = value;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textMesh in textMeshes)
        {
            textMesh.text = _initText;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
