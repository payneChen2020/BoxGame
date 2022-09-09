using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SingleCase;
using cfg.Config;
using DG.Tweening;

public class DrawUI : MonoBehaviour
{
    public Transform resultSum;

    private Image bigImg;
    private Text generalName;
    private Text forcesDes;
    void Start()
    {
        transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(OnBackClick);
        transform.Find("btn_draw").GetComponent<Button>().onClick.AddListener(ShowDrawResult);

        bigImg = resultSum.Find("img").GetComponent<Image>();
        generalName = resultSum.Find("name").GetComponent<Text>();
        forcesDes = resultSum.Find("forces").GetComponent<Text>();
    }

    private void OnBackClick()
    {
        transform.gameObject.SetActive(false);
    }

    //����Ӣ�۵ĸ���ֻ��35%
    private void ShowDrawResult()
    {

        //TimeSpan timeStamp = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //int curTimeStamp =  Convert.ToInt16(timeStamp.TotalSeconds);
        //UnityEngine.Random.InitState(20220907);

        if(UnityEngine.Random.value * 100f > 35.0f)
        {
            UIManager.ShowToast("���±���������Ӧ");
            return;
        }
        float rand = UnityEngine.Random.value * 15;
        int gId = (int)Mathf.Ceil(rand);

        bool isGot = GameModel.Instance().generalList.Contains(gId);
        if(isGot)
        {
            UIManager.ShowToast("���±���������Ӧ2");
            return;
        }

        Debug.Log("ShowDrawResult===" + gId+"rand==="+ rand);
        resultSum.gameObject.SetActive(true);

        TroopsCfg cfg = Configs.GetTroopsCfgById(gId);
        ResTools.SetImage("Hero/" + gId, bigImg);
        generalName.text = cfg.Name;
        forcesDes.text = "ͳ�ʣ�" + cfg.Forces;

        float tc = 2f;
        DOTween
            .To(() => tc, a => tc = a, 2f, 2f)
            .OnComplete(() =>
        {
            resultSum.gameObject.SetActive(false);
        });

        GameModel.Instance().generalList.Add(gId);
        GameModel.Instance().SaveMinePlayerData();
    }
}
