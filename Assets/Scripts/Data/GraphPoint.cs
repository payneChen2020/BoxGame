using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using cfg.Config;

namespace SingleCase
{
    [Serializable]
    public class GraphPoint
    {
        private static GraphPoint instance = null;

        struct disType
        {
            public int value;
            public bool visit;
            public List<int> path;
        };

        private int vexNum;
        private int edgesNum;
        private EdgesConfig edgesCfg;
        private int[,] arc = new int[76, 76];
        private readonly int INIT_MAX = 99;

        public GraphPoint() { }

        public static GraphPoint Instance()
        {
            if (instance == null)
            {
                instance = new GraphPoint();
            }
            return instance;
        }

        public void CreateGraph()
        {
            edgesCfg = Configs.GetGraphEdgesCfg();
            vexNum = 52;
            edgesNum = edgesCfg.DataList.Count;

            for (int ii = 0; ii < vexNum; ii++)
            {
                //if (!arc[ii]) this._arc[ii] = [];
                for (int jj = 0; jj < edgesNum; jj++)
                {
                    arc[ii, jj] = INIT_MAX;
                }
            }

            int count = 1;
            while (count < edgesNum + 1)
            {
               GraphEdgeCfg e = edgesCfg[count];
                count++;
                if (!CheckEdgeValid(e.StartIdx, e.EndIdx, e.Value))
                {
                    continue;
                }

                arc[e.StartIdx, e.EndIdx] = e.Value;
                /**无向图 */
                arc[e.EndIdx, e.StartIdx] = e.Value;

            }
        }


        //输入连线是否有效
        private bool CheckEdgeValid(int start, int end, int weight)
        {
            if (start < 0 || end < 0 || start > vexNum - 1 || end > vexNum - 1 || weight < 0)
            {
                return false;
            }

            return true;
        }

        /**采用dijkstra算法获取最短路径 */
        private List<int> GetDijkstraPath(int start, int end)
        {

            Dictionary<int, disType> dis = new Dictionary<int, disType> { };

            for (int ii = 0; ii < vexNum + 1; ii++)
            {

                disType disObj;

                if (dis.ContainsKey(ii))
                {
                    disObj = dis[ii];
                    disObj.value = arc[start, ii];
                    disObj.visit = false;
                    disObj.path = new List<int> { start, ii };
                    continue;
                }

                disObj.value = arc[start, ii];
                disObj.visit = false;
                disObj.path = new List<int> { start, ii };

                dis.Add(ii, disObj);

            }

            disType localDis = dis[start];

            localDis.value = 0;
            localDis.visit = true;

            dis[start] = localDis;

            int count = 0;

            List<int> paths = new List<int>();

            paths.Add(start);
            while (count < vexNum)
            {
                int tmp = 0;
                int min = INIT_MAX;

                for (int ii = 0; ii < vexNum; ii++)
                {
                    if (!dis[ii].visit && dis[ii].value < min)
                    {
                        min = dis[ii].value;
                        tmp = ii;
                    }
                }

                disType tmpDis = dis[tmp];
                tmpDis.visit = true;
                count++;
                dis[tmp] = tmpDis;

                for (int jj = 0; jj < vexNum; jj++)
                {
                    if (!dis[jj].visit && arc[tmp, jj] != INIT_MAX && (tmpDis.value + arc[tmp, jj]) < dis[jj].value)
                    {
                        disType jjDis = dis[jj];
                        jjDis.value = tmpDis.value + arc[tmp, jj];
                        //lpath.Add(jj);
                        jjDis.path = DeepCopyByBinary(tmpDis.path);
                        jjDis.path.Add(jj);
                        dis[jj] = jjDis;
                    }
                }
            }

            disType ret = dis[end];

            if (ret.value == INIT_MAX)
            {
                Debug.Log("无可用路径");
                return null;
            }

            return ret.path;
        }

        /**获取最短路径*/
        public List<int> SearchShortestPath(int start, int end)
        {
            List<int> ret = GetDijkstraPath(start, end);

            if (ret == null)
            {
                return new List<int> { };
            }

            return ret;
        }

        public static T DeepCopyByBinary<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
    }
}
