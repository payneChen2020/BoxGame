using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SingleCase;
using GameFSM;

public class GameMain : MonoBehaviour
{
    public Transform uiRoot;
    // Start is called before the first frame update
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    void Start()
    {
        UIManager.root = uiRoot;
        Configs.LoadConfigs();
        GameModel.Instance().InitMinePlayerData();
        GraphPoint.Instance().CreateGraph();
        Common.gameCtrl = new GameControl();
        Common.timerCtrl = new TimerControl();
        Application.targetFrameRate = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if(Common.gameCtrl != null)
        {
            Common.gameCtrl.Update();
        }

        if(Common.timerCtrl != null)
        {
            Common.timerCtrl.Update();
        }
    }
}
