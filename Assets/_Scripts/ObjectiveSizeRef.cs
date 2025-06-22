using System.Collections;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Sirenix.OdinInspector;

public class ObjectiveSizeRef : MonoBehaviour
{
    [Sirenix.OdinInspector.ReadOnly] public ObjectiveSize Size;

    public float SmallestRange = 2.2f;
    public float SmallRange = 3.5f;
    public float MediumRange = 5f;
    public float BigRange = 8.5f;

    public float GetRange()
    {
        switch(Size)
        {
            case ObjectiveSize.Smallest:
                return SmallestRange;
            case ObjectiveSize.Small:
                return SmallRange;
            case ObjectiveSize.Medium:
                return MediumRange;
            case ObjectiveSize.Big:
                return BigRange;
            default:
                return MediumRange;
        }
    }

}
