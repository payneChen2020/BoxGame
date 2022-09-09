using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SingleCase;
using cfg.Config;
using System;
using Constant;

public class TroopsUI : MonoBehaviour
{
    public Transform[] troopsList;

    private Text[] forcesText = new Text[4];
    private Text[] timerText = new Text[4];
    private ProgressBar[] progresses = new ProgressBar[4];

    private float timeCount = 0f;

    void Start()
    {
        transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(OnBackClick);
        
    }

    void OnEnable()
    {
        InitTroopsList();
    }

    private void OnBackClick()
    {
        transform.gameObject.SetActive(false);
    }

    private void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount < ConstData.ForcesUpdateFrame) return;
        timeCount = 0f;

        UpdateTroops();
    }

    private void InitTroopsList()
    {
        List<int> generalOnList = GameModel.Instance().generalOnList;

        for (int ii = 0; ii < generalOnList.Count; ii++)
        {
            if (generalOnList[ii] == 0) continue;

            TroopsCfg cfg = Configs.GetTroopsCfgById(generalOnList[ii]);
            int curForces = GameModel.Instance().generalCurForces[ii];
            DateTime dt = DateTime.Now;
            float percent = ((float)curForces / (float)cfg.Forces);

            forcesText[ii] = troopsList[ii].Find("forces").GetComponent<Text>();
            timerText[ii] = troopsList[ii].Find("tips").GetComponent<Text>();
            progresses[ii] = troopsList[ii].Find("progress").GetComponent<ProgressBar>();

            progresses[ii].SetValue(percent);
            forcesText[ii].text = curForces + "/" + cfg.Forces;
            //timerText[ii].text = "预计完成时间：" + dt.ToShortTimeString().ToString();

            ResTools.SetImage("HeadImg/" + generalOnList[ii], troopsList[ii].Find("Img").GetComponent<Image>());



            troopsList[ii].gameObject.SetActive(true);
        }
    }
    private void UpdateTroops()
    {
        List<int> generalOnList = GameModel.Instance().generalOnList;
        for (int ii = 0 ; ii < generalOnList.Count ; ii++)
        {
            if (generalOnList[ii] == 0) continue;

            TroopsCfg cfg = Configs.GetTroopsCfgById(generalOnList[ii]);
            int curForces = GameModel.Instance().generalCurForces[ii];
            DateTime dt = DateTime.Now;
            float percent = ((float)curForces / (float)cfg.Forces);

            progresses[ii].SetValue(percent);
            forcesText[ii].text = curForces + "/" + cfg.Forces;
            
            //float min = Mathf.Floor((cfg.Forces - curForces) / 60);
            ///float sec = Mathf.
            //timerText[ii].text = "预计完成时间：" + dt.ToShortTimeString().ToString();
        }
    }
}
