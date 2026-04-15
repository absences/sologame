using SudokuBase;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class ChessBoard : MonoBehaviour
{
    public CellGroup[] cellGroups;

    public void ResetCells()
    {
        foreach (var cellGroup in cellGroups)
        {
            cellGroup.ResetCells();
        }
    }

    public CellNode GetCellNode((int, int) pos)
    {
        var gy = pos.Item1 / 3;
        var gx = pos.Item2 / 3;

        var group = cellGroups[gy * 3 + gx];

        var cy = pos.Item1 % 3;
        var cx = pos.Item2 % 3;

        var cell = group.cellNodes[cy * 3 + cx];

        return cell;
    }

    internal void GenerateCells(Dictionary<(int, int), CellInfo> cells, System.Action<(int, int)> onClickCell)
    {
        foreach (var item in cells)//00 01 02
        {
            var pos = item.Key;
            var cellInfo = item.Value;

            var cell = GetCellNode(pos);

            cell.Row = cellInfo.row;
            cell.Column = cellInfo.column;

            if (cellInfo.isInit)
            {
                cell.SetStaticTxt(cellInfo.Value);
            }
            else
            {
                cell.OnClickEvent = onClickCell;
            }
        }
    }
    public void ShowHelp(Dictionary<(int, int), CellInfo> cells)
    {
        foreach (var item in cells)//00 01 02
        {
            var pos = item.Key;
            var cellInfo = item.Value;

            var cell = GetCellNode(pos);

            if (!cellInfo.isInit)
            {
                var list = cellInfo.initrest;

                foreach (var n in list)
                {
                    cell.SetNumberTxt(n);
                }

                // cellInfo.row

                cell.RefreshStatus(list.Count == 1);
            }
        }
    }
    public void HideHelp(Dictionary<(int, int), CellInfo> cells)
    {
        foreach (var item in cells)//00 01 02
        {
            var pos = item.Key;
            var cellInfo = item.Value;

            var cell = GetCellNode(pos);

            if (!cellInfo.isInit)
            {
                cell.ResetCell();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
