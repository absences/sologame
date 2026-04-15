using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanelNode : MonoBehaviour
{
    public GameObject[] cells;

    private Transform Root;
    private Transform Bg;

    private Action<int> OnSelectValue;
    private bool Block = false;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            var c = cells[i].GetComponent<Button>();

            var k = i;
            c.onClick.AddListener(() => OnSelect(k));
        }

        Bg = transform.Find("bg");
        var btn = Bg. GetComponent<Button>();

        btn.onClick.AddListener(CloseThis);

        Root = Bg.Find("Root");
    }
    void CloseThis()
    {
        Bg.gameObject.SetActive(false);
    }
    void OnSelect(int k)
    {
        if (Block)
            return;
        Block = true;
      //  UniTask.Create(async () =>
       // {
            Root.DOScale(0, 0.3f);
         //   await UniTask.WaitForSeconds(0.2f);
            Bg.gameObject.SetActive(false);
            OnSelectValue.Invoke(k);
       // });
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ShowPanel(bool[] status, Action<int> onSelectNode)
    {
        Block = false;

        OnSelectValue = onSelectNode;

        Bg.gameObject.SetActive(true);
        Root.localScale = Vector3.zero;

        Root.DOScale(1, 0.3f);

        for (int i = 0; i < cells.Length; i++)
        {
            //cells[i].SetActive(!status[i]);

            var img = cells[i].GetComponent<Image>();

            img.color = status[i] ? HexToColor("#4B92FFFF") : Color.gray;
        }
    }
    Color HexToColor(string s)
    {
        Color c;

        ColorUtility.TryParseHtmlString(s, out c);

        return c;
    }
}
