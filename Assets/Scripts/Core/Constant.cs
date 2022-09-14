using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Constant
{
    public enum TowerColor
    {
        INVALID,
        GRAY,
        RED,
        BLUE,
        GREEN,
        YELLOW,
        PURPLE
    }

    //部队状态：休闲，移动，等待，交战，征兵休整
    public enum TroopStatus
    {
        REFORM,
        FREE,
        MOVE,
        WAIT,
        FIRE,
        RECRUIT,
    }

    //节点状态 无占领，被占领，被摧毁，（水攻、火攻、空城）
    public enum TowerStatus
    {
        NO_OCCUPUTATION,
        OCCUPUTATED,
        DESTROY
    }

    public class ConstData
    {
        public static Dictionary<int, List<int>> Move_Command_Arr = new Dictionary<int, List<int>>
        {
            { 2 , new List<int>{1 , 2, 3 , 5 , 4 , 6 , 27 ,26 , 7 } },
            { 3 , new  List<int>{12 , 13 , 11, 10 , 24 , 14 , 15 , 16 ,17 , 23 } },
            { 4 , new  List<int>{42 , 41 , 43, 39 , 37 , 40 , 36 , 44 ,45 } },
            { 5 , new  List<int>{50 , 49 , 51, 32 , 48 , 34 , 33 , 30 , 47 , 46 } },
        };

        public static int[] Camp_ID_Arr = new int[4] { 2 , 3 , 4 , 5 };
        public static List<List<int>> Camp_Npc_Troops = new List<List<int>>() {
            new List<int>(){ 16 },
            new List<int>(){ 17 },
            new List<int>(){ 18 },
            new List<int>(){ 19 },
        };

        //征兵刷新频率30s
        public static float ForcesUpdateFrame = 1f;
    }

    public class EventCmd
    {
        public static string Move_Cmd = "move_cmd";
        public static string Tower_Status_Cmd = "tower_status_cmd";
        public static string Game_Config_inited = "game_config_inited";
        public static string Map_Troops_Delete = "map_troops_delete";
        public static string Map_Troops_Up = "map_troops_up";
        public static string Occputated_Ntf = "ccputated_ntf";
        public static string Reform_Start = "reform_start";
        public static string Reform_Succ = "reform_succ";
    }
}
