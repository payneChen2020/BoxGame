using System;
using System.Xml;
using System.IO;

namespace SingleCase
{
    public class GameCache
    {
        private static GameCache instance = null;
        public GameCache() { }

        public static GameCache Instance()
        {
            if (instance == null)
            {
                instance = new GameCache();
            }
            return instance;
        }

        //读取
        public static string ReadXml(string type)
        {
            //doucument 文件、文档的意思
            XmlDocument xmlDoc = new XmlDocument(); //创建了一个xml的文件对象
                                                    //绝对路径:带有盘符信息的路径都是绝对路径
                                                    //xmlDoc.Load(@"D:\Users\Desktop\xmltest.xml");//根据给定的文件路径读取出xml文件中所有的信息，并存储在XmlDocument对象

            //相对路径:相对于项目的exe文件的路径

            xmlDoc.Load("UserData.xml");

            //在exe文件的下一级目录的写法
            //  xmlDoc.Load(@"Test/xmltest.xml");

            //上一级 ../
            // xmlDoc.Load(@"../../xmltest.xml");
            //node
            XmlNode root = xmlDoc.SelectSingleNode("root");//在文件中根据对应的节点名找到对应的节点
            XmlNodeList list = root.ChildNodes;//找到根节点下所有的子节点
            foreach (var item in list)
            {
                //XmlNode无法直接解析节点中的内容
                //Element:元素，元件
                XmlElement element = item as XmlElement;

                if (type == element.GetAttribute("type"))
                {
                    return element.GetAttribute("value");
                }
            }

            return "";
        }


        //创建xml文件
        public static void CreateXml()
        {
            XmlDocument xmlDoc = new XmlDocument();

            //creatElement 创建一个节点，参数是要创建的节点的节点名
            XmlNode root = xmlDoc.CreateElement("root");
            //AppendChild把参数的节点作为前面的节点的子节点
            xmlDoc.AppendChild(root);
            xmlDoc.Save("Userdata.xml");//只有执行过这句代码的时候，才会真正的在文件夹中创建xml文件并保存数据
        }

        //添加节点
        public static void AddNewNode(string type, string value)
        {
            //先打开要添加的xml文件
            XmlDocument xmlDoc = new XmlDocument();
            //找到根节点
            xmlDoc.Load("Userdata.xml");
            XmlNode root = xmlDoc.SelectSingleNode("root");
            //创建新的账号密码节点，并将传入的参数存进去
            XmlElement newElement = xmlDoc.CreateElement("gameCache");//创建一个新的节点，并命名为account
            newElement.SetAttribute("type", type);
            newElement.SetAttribute("value", value);
            root.AppendChild(newElement);
            //保存文件
            xmlDoc.Save("Userdata.xml");
        }
        //删除xml文件
        public static void DeleteXml(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine("文件删除成功！");
            }
            else
            {
                Console.WriteLine("文件不存在");
            }
        }

        //删除对应的节点
        public static void RemoveNode(string type)
        {
            //打开文件
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Userdata.xml");
            //从文件数据中找到对应的节点
            XmlNode root = xmlDoc.SelectSingleNode("root");
            XmlNodeList list = root.ChildNodes;
            foreach (XmlElement item in list)
            {
                if (item.GetAttribute("type") == type)
                {
                    //删除节点
                    //Removechile从该节点的子节点中移除参数传入的节点
                    root.RemoveChild(item);
                    break;
                }
            }
            //删除节点
            //保存
            xmlDoc.Save("Userdata.xml");

        }

        //修改节点值
        public static void ChangeNode(string type, string value)
        {
            //打开文件
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Userdata.xml");
            //从文件数据中找到对应的节点
            XmlNode root = xmlDoc.SelectSingleNode("root");
            XmlNodeList list = root.ChildNodes;
            bool isFound = false;
            foreach (XmlElement item in list)
            {
                if (item.GetAttribute("type") == type)
                {
                    //修改节点
                    //Removechile从该节点的子节点中移除参数传入的节点
                    item.SetAttribute("value", value);
                    isFound = true;
                    break;
                }
            }


            //保存
            xmlDoc.Save("Userdata.xml");

            if (!isFound)
            {
                AddNewNode(type, value);
            }
        }
    }
}
