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

        //���������ӽڵ�
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
        //��������ӽڵ�������
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

    //ѡ�в���
    private void OnTroopsClick(int id)
    {
        selectId = id;
        Debug.Log("ѡ�в���" + id);
        Transform btn = troopsHeadSum.Find(id.ToString());
        clickQuan.parent = btn;
        clickQuan.localPosition = new Vector3(0, 0, 0);
    }

    //������ͣ��
    public void OnPauseClick()
    {
        Debug.Log("ѡ����ͣ");
        bool ifPause = Common.gameCtrl.GetGameStatus();
        if (ifPause)
        {
            Common.gameCtrl.Resume();
            pauseText.text = "��ͣ";
        }
        else
        {
            Common.gameCtrl.Pause();
            pauseText.text = "����";
        }
    }

    //�佫�б�
    public void ToHeroList()
    {
        Debug.Log("�������");
        BagUI.SetActive(true);
    }

    //���
    public void ToTroops()
    {
        Debug.Log("�������");
        TroopsUI.SetActive(true);
    }

    //��ļ�佫
    public void ToDraw()
    {
        Debug.Log("�����ļ");
        DrawUI.SetActive(true);
    }

    private void OnReport(int troopsId , int towerId , int camp)
    {
        GraphPosCfg tCfg = Configs.GetTowerCfgById(towerId);
        TroopsCfg trCfg = Configs.GetTroopsCfgById(troopsId);
        string desStr = trCfg.Name + "�ɹ���ռ" + tCfg.Name;
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
