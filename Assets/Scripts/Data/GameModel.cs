using System.Collections.Generic;
using System.IO;
using System;
using Constant;
using cfg.Config;
using GameFSM;

namespace SingleCase
{
    public struct BuildStruct
    {
        public int id;
        public int lvl;
    }

    public struct TroopStruct
    {
        public int id;
        public int lvl;
        public TroopStatus status;
        public int curForces;
        public int maxForces;
    }

    //玩家数据
    public class GameModel
    {


        private static GameModel instance = null;
        public GameModel() { }

        public static GameModel Instance()
        {
            if (instance == null)
            {
                instance = new GameModel();
            }
            return instance;
        }
        //请自觉保证只有四个元素
        public List<int> generalOnList = new List<int>(4) {0 , 0 , 0 , 0};
        public List<int> generalCurForces = new List<int>(4) { 0, 0, 0, 0 };
        public List<int> generalList = new List<int>() { };
        public List<BuildStruct> buildList = new List<BuildStruct>();
        public List<TroopStruct> troopsList = new List<TroopStruct>();

        public void InitMinePlayerData()
        {
            if (!File.Exists("UserData.xml"))
            {
                GameCache.CreateXml();
                return;
            }

            //
            string[] list = GameCache.ReadXml("generalList").Split("|");
            if(list.Length > 0)
            {
                //int[] intList = Array.ConvertAll(list, int.Parse);
                for(int ii = 0 ; ii < list.Length; ii++)
                {
                    if (list[ii] == "") continue;
                    generalList.Add(int.Parse(list[ii]));
                }
            }

            string[]  listForces = GameCache.ReadXml("generalCurForces").Split("|");
            if (listForces.Length > 0)
            {
                //int[] intList = Array.ConvertAll(list, int.Parse);
                for (int ii = 0; ii < listForces.Length; ii++)
                {
                    if (listForces[ii] == "") continue;
                    //字符串数组第一个元素一定是空字符串
                    generalCurForces[ii - 1] = int.Parse(listForces[ii]);
                }
            }

            string[] listOn = GameCache.ReadXml("generalOnList").Split("|");
            if (listOn.Length > 0)
            {
                //int[] intList = Array.ConvertAll(list, int.Parse);
                for (int ii = 0; ii < listOn.Length; ii++)
                {
                    if (listOn[ii] == "") continue;
                    //字符串数组第一个元素一定是空字符串
                    generalOnList[ii -1] = int.Parse(listOn[ii]);
                }
            }
        }

        //保存数据
        public void SaveMinePlayerData()
        {
            string cStr = "";
            foreach (int id in generalList)
            {
                cStr = string.Join("|" , cStr ,  id);

            }
            GameCache.ChangeNode("generalList", cStr);

            cStr = "";
            foreach (int id in generalOnList)
            {
                cStr = string.Join("|", cStr, id);

            }
            GameCache.ChangeNode("generalOnList", cStr);

            cStr = "";
            foreach (int id in generalCurForces)
            {
                cStr = string.Join("|", cStr, id);

            }
            GameCache.ChangeNode("generalCurForces", cStr);
        }
        
        //检查上阵状态
        public bool CheckTroopOn(int id)
        {
            foreach(int gid in generalOnList)
            {
                if(gid == id)
                {
                    return true;
                }
            }
            return false;
        }

        //上阵
        public bool SetTroopsOn(int id)
        {
            for(int ii = 0; ii < generalOnList.Count; ii++)
            {
                if(generalOnList[ii] == 0)
                {
                    generalOnList[ii] = id;
                    Watcher.Dispatch(EventCmd.Map_Troops_Up);
                    SaveMinePlayerData();
                    return true;
                }
            }

            return false;
        }

        //取消上阵
        public bool SetTroopsOff(int id)
        {
            for (int ii = 0; ii < generalOnList.Count; ii++)
            {
                if (generalOnList[ii] == id)
                {
                    generalOnList[ii] = 0;
                    SaveMinePlayerData();
                    generalCurForces[ii] = 0;
                    Common.gameCtrl.DeleteTroops(Common.gameCtrl.GetMineCamp() , id);
                    return true;
                }
            }

            return false;
        }

        //征兵更新
        public void UpdateCurForces(int id , int forces)
        {
            int index = -1;
            for(int ii = 0 ; ii < generalOnList.Count ; ii++)
            {
                if(generalOnList[ii] == id)
                {
                    index = ii;
                    break;
                }
            }

            if(index >= 0)
            {
                generalCurForces[index] = forces;
            }
        }
    }
}
