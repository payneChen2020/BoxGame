using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constant;
using SingleCase;

namespace GameFSM
{
    public class CampControl
    {
        //��Ӫ���
        public int ID { get; private set; }



        //����״̬���б�
        public List<TroopsControl> troopCtrlArr = new List<TroopsControl>();

        private List<int> moveCmds = new List<int>();

        private bool ifPause = true;

        private bool ifNPC = true;


        public CampControl(int Id , bool ifnpc)
        {
            ID = Id;
            ifNPC = ifnpc;
            moveCmds = ConstData.Move_Command_Arr[Id];
            CreateTroops();

            ifPause = false;
        }

        public void Update()
        {

            //��ҿ��Ƶ�λ����Ҫ
            if (ifPause) return;

            foreach (TroopsControl tc in troopCtrlArr)
            {
                tc.Update();
            }

            if (ifNPC)
            {
                TroopsControl troops = GetFreeTroops();
                if (troops == null) return;

                if (moveCmds.Count > 0)
                {
                    int target = moveCmds[0];
                    troops.Move2Tower(target);
                    moveCmds.RemoveAt(0);
                }
            }

        }

        private void CreateTroops()
        {
            Debug.Log("CampCtrl CreateTroops");
            List<int> list = new List<int>();
            //NPC������һ��
            if (!ifNPC) {
                list = GameModel.Instance().generalOnList;
            } else
            {
                list = ConstData.Camp_Npc_Troops[ID - 2];
            }

            foreach (int tid in list)
            {
                if (tid == 0) continue;
                TroopsControl obj = new TroopsControl(ID , tid , ifNPC);
                troopCtrlArr.Add(obj);
            }
        }

        //����Ƿ��ж��鴦�ڿ���״̬
        private TroopsControl GetFreeTroops()
        {
            foreach(TroopsControl tc in troopCtrlArr)
            {
                if(tc.status == TroopStatus.FREE)
                {
                    return tc;
                }
            }
            return null;
        }

        //�ı䲿��״̬
        public bool TurnTroopStatus(int id, TroopStatus sta)
        {
            foreach(TroopsControl tp in troopCtrlArr)
            {
                if(tp.ID == id)
                {
                    tp.status = sta;
                    return true;
                }
            }
            return false;
        }

        public int GetTroopForces(int id)
        {
            foreach (TroopsControl tp in troopCtrlArr)
            {
                if (tp.ID == id)
                {
                    return tp.curForces;
                }
            }
            return -1;
        }

        //������������
        public void TroopsBeDestroyed(int tId)
        {
            foreach(TroopsControl tc in troopCtrlArr)
            {
                if(tId == tc.ID)
                {
                    tc.BeDestroyed();
                    return;
                }
            }
        }

        public void SetTroopsStatus(int tId , TroopStatus sta)
        {
            foreach (TroopsControl tc in troopCtrlArr)
            {
                if (tId == tc.ID)
                {
                    tc.status = sta;
                    return;
                }
            }
        }

        public void Resume()
        {
            ifPause = false;
        }

        public void Pause()
        {
            ifPause = true;
        }
        
        public void AddTroops(int troopid)
        {
            TroopsControl obj = new TroopsControl(ID, troopid , ifNPC);
            troopCtrlArr.Add(obj);
            Watcher.Dispatch(EventCmd.Map_Troops_Up, ID , troopid);
        }


        public void DeleteTroops(int troopid)
        {
            for(int ii = 0 ; ii < troopCtrlArr.Count;ii++ )
            {
                if(troopCtrlArr[ii].ID == troopid)
                {
                    troopCtrlArr.RemoveAt(ii);
                    Watcher.Dispatch(EventCmd.Map_Troops_Delete, ID , troopid);
                    break;
                }
            }
        }

        public bool SetTroopsMove(int troopid , int target)
        {
            foreach (TroopsControl tc in troopCtrlArr)
            {
                if (tc.ID == troopid)
                {
                    tc.Move2Tower(target);
                    return true;
                }
            }

            return false;
        }
    }
}

