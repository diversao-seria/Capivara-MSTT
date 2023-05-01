using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector3Reference
{
    public bool UseConstant = true;
    public Vector3 ConstantValue = new Vector3();
    public Vector3Variable Variable;

    public Vector3 Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
