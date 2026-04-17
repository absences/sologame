using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGroup : MonoBehaviour
{
    public CellNode[] cellNodes;

    internal void ResetCells()
    {
        foreach (var item in cellNodes)
        {
            item.ResetCell();
        }
    }
}
