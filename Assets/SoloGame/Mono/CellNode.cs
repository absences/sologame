using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellNode : MonoBehaviour
{
    public GameObject[] Numbers;

    private Button btnBase;

    private TextMeshProUGUI numTxt;

    public bool[] NumStatus;

    public int Row, Column;

    public Action<(int, int)> OnClickEvent;
    // Start is called before the first frame update
    void Awake()
    {
        btnBase = transform.Find("btnBase").GetComponent<Button>();

        btnBase.onClick.AddListener(OnBaseClick);

        numTxt = transform.Find("num").GetComponent<TextMeshProUGUI>();

        NumStatus = new bool[Numbers.Length];

        numTxt.raycastTarget = false;
    }

    private void OnBaseClick()
    {
        OnClickEvent?.Invoke((Row, Column));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ResetCell()
    {
        numTxt.text = "";

        foreach (var item in Numbers)
        {
            item.gameObject.SetActive(false);
        }

        NumStatus = new bool[Numbers.Length];
    }
    public void SetStaticTxt(int val)
    {
        numTxt.text = val.ToString();
        numTxt.color = Color.black;

        OnClickEvent = null;
    }
    public int NumberCount//已选中的数量
    {
        get
        {
            int count = 0;
            for (int i = 0; i < NumStatus.Length; i++)
            {
                if (NumStatus[i])
                {
                    count++;
                }
            }
            return count;
        }
    }
   
    public void SetNumberTxt(int val)
    {
        var v = !NumStatus[val - 1];
        NumStatus[val - 1] = v;

        if (NumberCount == 1)
        {
            for (int i = 0; i < Numbers.Length; i++)
            {
                Numbers[i].SetActive(false);

                if (NumStatus[i])
                    numTxt.text = (i + 1).ToString();
            }
          
        }
        else
        {
            numTxt.text = "";
            for (int i = 0; i < Numbers.Length; i++)
            {
                Numbers[i].SetActive(NumStatus[i]);
            }
        }
    }

    public void RefreshStatus(bool valid)
    {
        numTxt.color = valid ? Color.green : Color.red;
    }
}
