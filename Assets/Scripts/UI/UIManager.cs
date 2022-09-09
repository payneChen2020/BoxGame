using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SingleCase
{
    class UIManager
    {
        public static Transform root { set; get; }

        public static void ShowToast(string s)
        {
            GameObject pre = (GameObject)Resources.Load("Prefabs/Toast");
            //实例化到场景中
            GameObject instancess = UnityEngine.Object.Instantiate(pre) as GameObject;
            //将物体绑定到父物体下面
            instancess.transform.parent = root;
            //给物体赋值坐标
            instancess.transform.localPosition = new Vector3(0, 1f, 0f);
            //给物体大小赋值
            instancess.transform.localScale = new Vector3(1, 1, 1);

            instancess.GetComponent<ToastUI>().Show(s);
        }
    }
}
