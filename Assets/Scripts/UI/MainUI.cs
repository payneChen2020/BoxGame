using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SingleCase;
using GameFSM;
using cfg.Config;
using Constant;

public class MainUI : MonoBehaviour
{
    public Transform troopsHeadTmp;
    public GameObject pauseBtn;
    public Transform reportCtnt;
    public Transform reportTxtTmp;
    public Text pauseText;

    public Transform clickQuan;

    public GameObject DrawUI;
    public GameObject BagUI;
    public GameObject TroopsUI;

    private Transform troopsHeadSum;

    public int selectId { private set; get; } = 0;

    private List<Button> troopsHeadList = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        troopsHeadSum = transform.Find("troops");

        //脱离所有子节点
        troopsHeadSum.DetachChildren();
        reportCtnt.DetachChildren();

        Transform bottomSum = transform.Find("bottom");
        bottomSum.Find("heroes").GetComponent<Button>().onClick.AddListener(ToHeroList);
        bottomSum.Find("troops").GetComponent<Button>().onClick.AddListener(ToTroops);
        bottomSum.Find("draw").GetComponent<Button>().onClick.AddListener(ToDraw);
        pauseBtn.GetComponent<Button>().onClick.AddListener(OnPauseClick);


        Watcher.On<GraphPosConfig>(EventCmd.Game_Config_inited, InitShow); 
        Watcher.On(EventCmd.Map_Troops_Up, OnTroopsUp);
        Watcher.On<int , int>(EventCmd.Map_Troops_Delete, OnTroopsDelete);
        Watcher.On<int, int , int>(EventCmd.Occputated_Ntf, OnReport);
        bottomSum.Find("draw").GetComponent<Button>().onClick.AddListener(ToDraw);
        clickQuan.parent = null;
    }

    private void OnDestroy()
    {
        Watcher.Off<GraphPosConfig>(EventCmd.Game_Config_inited, InitShow);
        Watcher.Off(EventCmd.Map_Troops_Up, OnTroopsUp);
        Watcher.Off<int , int>(EventCmd.Map_Troops_Delete, OnTroopsDelete);
        Watcher.Off<int, int , int>(EventCmd.Occputated_Ntf, OnReport);
    }

    private void InitShow(GraphPosConfig  cfg = null)
    {
        clickQuan.parent = null;
        selectId = 0;
        //清除所有子节点重新来
        if (troopsHeadSum.childCount > 0)
        {
            for (int i = 0; i < troopsHeadSum.childCount; i++)
            {
                Destroy(troopsHeadSum.GetChild(i).gameObject);
                
            }
        }
        troopsHeadList.Clear();
        //troopsHeadTmp.GetComponent<Button>().onClick.AddListener(OnTroopsClick);
        List<int> generalOnList = GameModel.Instance().generalOnList;
        for (int ii = 0; ii < generalOnList.Count; ii++)
        {
            if (generalOnList[ii] == 0) continue;


            Transform obj = Instantiate(troopsHeadTmp);
            obj.parent = troopsHeadSum;
            obj.name = generalOnList[ii].ToString();

            ResTools.SetImage("HeadImg/" + generalOnList[ii], obj.GetComponent<Image>());
            troopsHeadList.Add(obj.GetComponent<Button>());


            if (selectId == 0)
            {
                selectId = generalOnList[ii];
                clickQuan.parent = obj;
                clickQuan.localPosition = new Vector3(0, 0, 0);
            }
        }

        foreach (Button btn in troopsHeadList)
        {
            int id = int.Parse(btn.transform.name);
            btn.onClick.AddListener(delegate ()
            {
                OnTroopsClick(id);
            });
        }
    }

    private void OnTroopsDelete(int camp , int troops)
    {
        InitShow();
    }

    private void OnTroopsUp()
    {
        InitShow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //选中部队
    private void OnTroopsClick(int id)
    {
        selectId = id;
        Debug.Log("选中部队" + id);
        Transform btn = troopsHeadSum.Find(id.ToString());
        clickQuan.parent = btn;
        clickQuan.localPosition = new Vector3(0, 0, 0);
    }

    //按下暂停键
    public void OnPauseClick()
    {
        Debug.Log("选中暂停");
        bool ifPause = Common.gameCtrl.GetGameStatus();
        if (ifPause)
        {
            Common.gameCtrl.Resume();
            pauseText.text = "暂停";
        }
        else
        {
            Common.gameCtrl.Pause();
            pauseText.text = "继续";
        }
    }

    //武将列表
    public void ToHeroList()
    {
        Debug.Log("点击将册");
        BagUI.SetActive(true);
    }

    //编队
    public void ToTroops()
    {
        Debug.Log("点击部队");
        TroopsUI.SetActive(true);
    }

    //招募武将
    public void ToDraw()
    {
        Debug.Log("点击招募");
        DrawUI.SetActive(true);
    }

    private void OnReport(int troopsId , int towerId , int camp)
    {
        GraphPosCfg tCfg = Configs.GetTowerCfgById(towerId);
        TroopsCfg trCfg = Configs.GetTroopsCfgById(troopsId);
        string desStr = trCfg.Name + "成功攻占" + tCfg.Name;
        Transform reportItem = Instantiate(reportTxtTmp);
        reportItem.GetComponent<Text>().text = desStr;
        reportItem.parent = reportCtnt;
        if (camp == (int)TowerColor.GRAY)
        {
            reportItem.GetComponent<Text>().color = Color.gray;
        }
        else if(camp == (int)TowerColor.GREEN)
        {
            reportItem.GetComponent<Text>().color = Color.green;
        }
        else if(camp == (int)TowerColor.BLUE)
        {
            reportItem.GetComponent<Text>().color = Color.blue;
        }
        else if(camp == (int)TowerColor.PURPLE)
        {
            reportItem.GetComponent<Text>().color = Color.HSVToRGB(138 , 0 , 226);
        }
        else if(camp == (int)TowerColor.RED)
        {
            reportItem.GetComponent<Text>().color = Color.red;
        }
        else if(camp == (int)TowerColor.YELLOW)
        {
            reportItem.GetComponent<Text>().color = Color.yellow;
        }
    }
}
