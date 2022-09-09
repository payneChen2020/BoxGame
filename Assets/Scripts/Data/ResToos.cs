using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SingleCase
{
    public class ResTools
    {

        private static ResTools instance = null;
        public ResTools() { }

        public static ResTools Instance()
        {
            if (instance == null)
            {
                instance = new ResTools();
            }
            return instance;
        }

        //动态设置图片
        public static void SetImage(string path , Image img)
        {
            //image路径
            //string path = "Hero/2";
            //参数为资源路径和资源类型
            Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            //动态更换image
            img.sprite = sprite;
        }

        public static void SetSprite(string path, SpriteRenderer sp)
        {
            //image路径
            //string path = "Hero/2";
            //参数为资源路径和资源类型
            Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            //动态更换image
            sp.sprite = sprite;
        }
    }
}
