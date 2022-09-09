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

        //��Ӫ�б�
        private Dictionary<int , CampControl> campCtrlArr = new Dictionary<int, CampControl>();
        private bool ifPause = false;
        //�ڵ�״̬���б�
        //��������һ��վλ
        private List<TowerControl> towerCtrlArr = new List<TowerControl>();
        private  readonly int mineCamp = 2;

        //��Ӫ״̬����ʼ��
        private void CreateCamps()
        {
            Debug.Log("GameCtrl CreateCamps");
            for(int ii = 0 ; ii < ConstData.Camp_ID_Arr.Length ; ii++)
            {
                //��Ĭ��һ����ҿ��Ƶ�λ
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



        // ��Ϸ״̬������
        public void Resume()
        {
            Debug.Log("GameCtrl Resume");
            ifPause = false;
        }
        // ��Ϸ״̬����ͣ
        public void Pause()
        {
            ifPause = true;
        }

        //�ı䲿��״̬
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

        //�����ݵ�
        public bool AttackTower(int campId , int troopId, int towerId)
        {
            int curForces = campCtrlArr[campId].GetTroopForces(troopId);

            int ret = towerCtrlArr[towerId - 1].BeAttacked(campId , curForces);

            if(ret <= 0)
            {
                //���������Ѻľ�
                //�޷��ҵ��ò�������
                campCtrlArr[campId].TroopsBeDestroyed(troopId);
                return false;
            }
            else
            {
                //����פ���ɹ�ռ��
                campCtrlArr[campId].SetTroopsStatus(troopId , TroopStatus.FREE);
                Watcher.Dispatch(EventCmd.Occputated_Ntf, troopId, towerId , campId);
                return true;
            }
        }

        //������鵽��ͼ�� no use
        public void AddTroops(int camp , int troops)
        {
            if(campCtrlArr[camp] != null)
            {
                campCtrlArr[camp].AddTroops(troops);
            }
        }

        //�ӵ�ͼ���������
        public void DeleteTroops(int camp , int troops)
        {
            if (campCtrlArr[camp] != null)
            {
                campCtrlArr[camp].DeleteTroops(troops);
            }
        }

        //��ȡ��Ϸ״̬
        public bool GetGameStatus()
        {
            return ifPause;
        }

        //ѡ�нڵ�����ƶ�
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

