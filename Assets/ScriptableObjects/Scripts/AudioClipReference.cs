using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClipReference
{
    public bool UseConstant = true;
    public AudioClip ConstantValue;
    public AudioClipVariable Variable;

    public AudioClip Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
