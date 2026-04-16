using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SudokuBase;
using SudokuFactory;
using SudokuGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoloGame : MonoBehaviour
{
    Sudoku Sdk;

    SudokuBuilder sdkBuilder;

    SudokuQuestion sdkGenerator;

    SudokuMarket currentMarket;

    public TMP_InputField noticeNumber;

    public TextMeshProUGUI notice1;

    public Toggle helpToggle;

    public ChessBoard board;

    private const int numbers = 9;

    public SelectPanelNode selectPanelNode;

    private string tempData;
    // Start is called before the first frame update
    void Start()
    {
        Sdk = new Sudoku();

        sdkBuilder = new SudokuBuilder();
        sdkGenerator = new SudokuQuestion();

        ResetGrids();

        helpToggle.onValueChanged.AddListener(OnHelpToggle);
    }

    private void OnHelpToggle(bool v)
    {
        if (currentMarket == null)
            return;
        Dictionary<(int, int), CellInfo> cells = currentMarket.GetCellInfos();
        if (v)
        {
            board.ShowHelp(cells);
        }
        else
        {
            board.HideHelp(cells);
        }
    }

    void ResetGrids()
    {
        board.ResetCells();
    }

    public void GenSolo()
    {
        ResetGrids();
        //List<List<int>> param = new List<List<int>>()
        //        {
        //            new List<int> {0, 9, 0,     4, 8, 0,     0, 0, 0},
        //            new List<int> {6, 4, 0,     0, 0, 0,     0, 2, 7},
        //            new List<int> {1, 2, 8,     4, 7, 0,     0, 5, 6},

        //            new List<int> {2, 5, 1,     0, 6, 0,     0, 0, 8},
        //            new List<int> {0, 0, 0,     0, 0, 0,     0, 0, 0},
        //            new List<int> {8, 0, 0,     0, 5, 0,     2, 6, 0},

        //            new List<int> {0, 8, 0,     0, 3, 0,     0, 7, 0},
        //            new List<int> {5, 0, 2,     7, 4, 0,     0, 8, 3},
        //            new List<int> {3, 0, 7,     5, 0, 0,     4, 0, 2}
        //    };

        //currentMarket = new SudokuMarket(param);

        currentMarket = sdkGenerator.AutoQuestion(sdkBuilder.MakeSudoku(), int.Parse(noticeNumber.text));

        var questions = currentMarket.initValues;

        UpdateCurrentSudokuInfo();

        var value = helpToggle.isOn;

        Dictionary<(int, int), CellInfo> cells = currentMarket.GetCellInfos();

        board.GenerateCells(cells, OnClickCell);

        if (value)
            board.ShowHelp(cells);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void SaveTemp()
    {
        if (currentMarket == null) return;

        var cells = currentMarket.GetCellInfos();
        var currentValues = new Dictionary<string, int>();
        foreach (var kv in cells)
        {
            currentValues[$"{kv.Key.Item1},{kv.Key.Item2}"] = kv.Value.Value;
        }
        var saveData = new Save
        {
            InitValues = currentMarket.initValues,
            CurrentValues = currentValues
        };
        tempData = JsonConvert.SerializeObject(saveData);
    }
    class Save
    {
       public List<List<int>> InitValues { get; set; }
       public Dictionary<string, int> CurrentValues { get; set; }
    }
    public void ReadTemp()
    {
        //   if (!PlayerPrefs.HasKey("SudokuSave")) return;

        if (string.IsNullOrEmpty(tempData))
            return;

        ResetGrids();

       // string json = PlayerPrefs.GetString("SudokuSave");
        var saveData = JsonConvert.DeserializeObject<Save>(tempData);

        currentMarket = new SudokuMarket(saveData.InitValues);
        var cells = currentMarket.GetCellInfos();
        foreach (var kv in saveData.CurrentValues)
        {
            if (kv.Value != 0)
            {
                var parts = kv.Key.Split(',');
                int row = int.Parse(parts[0]);
                int col = int.Parse(parts[1]);
                cells[(row, col)].SetValue(kv.Value);
            }
        }

        UpdateCurrentSudokuInfo();
        board.GenerateCells(cells, OnClickCell);
        if (helpToggle.isOn)
        {
            board.ShowHelp(cells);
        }
    }


    void OnClickCell((int,int) pos)
    {
        // var r = cell.GetRest();

        var ui = board.GetCellNode(pos);

        var status = ui.NumStatus;

        selectPanelNode.ShowPanel(status,
            (k) =>
                OnSelectNode(pos, k));
    }
    void OnSelectNode((int, int) pos, int k) //k>=0
    {
        Dictionary<(int, int), CellInfo> cells = currentMarket.GetCellInfos();

        var cell = cells[pos];

        var ui = board.GetCellNode(pos);

        var init = cell.initrest;

        ui.SetNumberTxt(k + 1);

        if (ui.NumberCount == 1)
        {
           for ( var i = 0; i < ui.NumStatus.Length; i++ )
            {
                if (ui.NumStatus[i])
                {
                    cell.SetValue(i + 1);

                    var t = init.Contains(i + 1);
                    ui.RefreshStatus(t);

                    break;
                }
            }

        }
        else
        {
            cell.ClearValue();
            ui.RefreshStatus(false);
        }

       
    }
    private void UpdateCurrentSudokuInfo()
    {
        notice1.text = "共同位置加权值：  " + Math.Round(currentMarket.Common, 2) + "\n实际提示数个数为:   " + currentMarket.initLists.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
