using cfg.Config;
using SimpleJSON;
using System.IO;
using UnityEngine;

namespace SingleCase
{
    public class Configs
    {
        private static Configs instance = null;
        private static GraphPosConfig TowerPosCfg;
        private static EdgesConfig EdgeCfgs;
        private static TroopsConfig TroopsCfgs;
        public Configs() { }

        public static Configs Instance()
        {
            if (instance == null)
            {
                instance = new Configs();
            }
            return instance;
        }

        public void InitTowerPosArr()
        {

        }

        private static JSONNode LoadByteBuf(string file)
        {
            return JSON.Parse(File.ReadAllText(Application.dataPath + "/Scripts/Configs/" + file + ".json", System.Text.Encoding.UTF8));
        }

        //加载配置表
        public static void LoadConfigs()
        {
            var tables = new cfg.Tables(LoadByteBuf);
            TowerPosCfg = tables.GraphPosConfig;
            EdgeCfgs = tables.EdgesConfig;
            TroopsCfgs = tables.TroopsConfig;

            Debug.Log("== load succ==");
        }

        public static EdgesConfig GetGraphEdgesCfg()
        {
            return EdgeCfgs;
        }

        public static GraphPosConfig GetTowerCfgs()
        {
            return TowerPosCfg;
        }

        //获取单个武将配置
        public static TroopsCfg GetTroopsCfgById(int id)
        {
            foreach(TroopsCfg cfg in TroopsCfgs.DataList)
            {
                if (id == cfg.Id)
                {
                    return cfg;
                }
            }
            return null;
        }

        //获取单个地图节点配置
        public static GraphPosCfg GetTowerCfgById(int id)
        {
            foreach (GraphPosCfg cfg in TowerPosCfg.DataList)
            {
                if (id == cfg.Id)
                {
                    return cfg;
                }
            }
            return null;
        }
    }
}
