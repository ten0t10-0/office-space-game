using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OfficeObjectDependency
{
    public int ObjectIndexChild;
    public int ObjectIndexParent;

    public OfficeObjectDependency(int childIndex, int parentIndex)
    {
        ObjectIndexChild = childIndex;
        ObjectIndexParent = parentIndex;
    }
}
