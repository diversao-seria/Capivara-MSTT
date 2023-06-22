using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationCurveReference
{
    public bool UseConstant = false;
    public AnimationCurve ConstantValue;
    public AnimationCurveVariable Variable;

    public AnimationCurve Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { Variable.Value = value; }
    }
}
