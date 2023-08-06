using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector2Reference
{
    public bool UseConstant = true;
    public Vector2Int ConstantValue = new Vector2Int();
    public Vector2Variable Variable;

    public Vector2Int Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { Variable.Value = value; }
    }
}
