using System.Collections.Generic;
using Constant;
using cfg.Config;
using SingleCase;
using UnityEngine;

namespace GameFSM
{
    public class GameControl
    {

        public GameControl()
        {
            CreateCamps();
        }

        //阵营列表
        private Dictionary<int , CampControl> campCtrlArr = new Dictionary<int, CampControl>();
        private bool ifPause = false;
        //节点状态机列表
        //事先填入一个站位
        private List<TowerControl> towerCtrlArr = new List<TowerControl>();
        private  readonly int mineCamp = 2;

        //阵营状态机初始化
        private void CreateCamps()
        {
            Debug.Log("GameCtrl CreateCamps");
            for(int ii = 0 ; ii < ConstData.Camp_ID_Arr.Length ; ii++)
            {
                //先默认一个玩家控制单位
                CampControl obj = new CampControl(ConstData.Camp_ID_Arr[ii] ,  ConstData.Camp_ID_Arr[ii] != 2);
                campCtrlArr.Add(ConstData.Camp_ID_Arr[ii], obj);
            }

            GraphPosConfig tCfg = Configs.GetTowerCfgs();

            foreach(GraphPosCfg cfg in tCfg.DataList)
            {
                TowerControl obj = new TowerControl();
                obj.InitData(cfg);

                towerCtrlArr.Add(obj);
            }

            Watcher.Dispatch(EventCmd.Game_Config_inited , tCfg);
        }

        // Update is called once per frame 
        public void Update()
        {
            if (ifPause) return;


            foreach(int id in ConstData.Camp_ID_Arr)
            {
                campCtrlArr[id].Update();
            }
        }



        // 游戏状态机启动
        public void Resume()
        {
            Debug.Log("GameCtrl Resume");
            ifPause = false;
        }
        // 游戏状态机暂停
        public void Pause()
        {
            ifPause = true;
        }

        //改变部队状态
        public void TurnTroopStatus(int id , TroopStatus  sta)
        {
            for (int ii = 0; ii < ConstData.Camp_ID_Arr.Length; ii++)
            {
                if(campCtrlArr[ConstData.Camp_ID_Arr[ii]].TurnTroopStatus(id , sta))
                {
                    return;
                }
            }
        }

        //攻击据点
        public bool AttackTower(int campId , int troopId, int towerId)
        {
            int curForces = campCtrlArr[campId].GetTroopForces(troopId);

            int ret = towerCtrlArr[towerId - 1].BeAttacked(campId , curForces);

            if(ret <= 0)
            {
                //己方部队已耗尽
                //无法找到该部队数据
                campCtrlArr[campId].TroopsBeDestroyed(troopId);
                return false;
            }
            else
            {
                //消灭驻军成功占领
                campCtrlArr[campId].SetTroopsStatus(troopId , TroopStatus.FREE);
                Watcher.Dispatch(EventCmd.Occputated_Ntf, troopId, towerId , campId);
                return true;
            }
        }

        //上阵队伍到地图上 no use
        public void AddTroops(int camp , int troops)
        {
            if(campCtrlArr[camp] != null)
            {
                campCtrlArr[camp].AddTroops(troops);
            }
        }

        //从地图中下阵队伍
        public void DeleteTroops(int camp , int troops)
        {
            if (campCtrlArr[camp] != null)
            {
                campCtrlArr[camp].DeleteTroops(troops);
            }
        }

        //获取游戏状态
        public bool GetGameStatus()
        {
            return ifPause;
        }

        //选中节点玩家移动
        public bool SetTroopsMove(int troops , int target)
        {
             return campCtrlArr[mineCamp].SetTroopsMove(troops , target);
        }

        public int GetMineCamp()
        {
            return mineCamp;
        }
    }
}

