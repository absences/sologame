using SudokuBase;
using System.Collections.Generic;
using UnityEngine;

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
                var value = cellInfo.Value;
                cell.ResetCell();
                if (value == 0)
                {
                    var list = cellInfo.initrest;

                    cellInfo.ClearValue();

                  

                    foreach (var n in list)
                    {
                        cell.SetNumberTxt(n);//设置预选数字
                    }
                    cell.RefreshStatus(list.Count == 1);
                  

                    //if (list.Count == 1)//可选 初次设置提示的值将写入 否则会被清理
                    //{
                    //    cellInfo.SetValue(list[0]);
                    //}
                   
                }
                else
                {
                    cell.SetNumberTxt(value);
                }
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
                var value = cellInfo.Value;
                cell.ResetCell();
                if (value != 0)
                {
                    cell.SetNumberTxt(value);
                }
            }
        }
    }
}
