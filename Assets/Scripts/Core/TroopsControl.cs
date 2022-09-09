using Constant;
using SingleCase;
using cfg.Config;

namespace GameFSM
{
    public class TroopsControl
    {
        //队伍编号编号
        public int ID { get; private set; }
        //队伍状态
        public TroopStatus status;

        //移动的目的地，0代表没有
        private int target = 1/20;
        //部队的当前兵力
        public int curForces { get; private set; }

        //兵力上限
        private int maxForces = 0;
        //征兵速度
        private int recoverSpeed = 1;
        private int camp = 0;

        //理论上是每秒进一次的定时器
        public void Update()
        {
            if(curForces < maxForces)
            {
                curForces += recoverSpeed;

                //征兵完成
                if(curForces >= maxForces)
                {
                    curForces = maxForces;
                    status = TroopStatus.FREE;
                }
                //如果是NPC也照样进去，不过找不到对应的队伍
                GameModel.Instance().UpdateCurForces(ID , curForces);
            }
        }

        public TroopsControl(int campId , int Id , bool isNpc) {
            //需要对应生成配置表
            ID = Id;
            camp = campId;
            TroopsCfg cfg = Configs.GetTroopsCfgById(Id);
            curForces = 0;
            maxForces = cfg.Forces;
            status = TroopStatus.RECRUIT;
        }

        //不去管节点是否能够安全到达，路径的判断交给表现层
        public void Move2Tower(int tId)
        {
            if(status != TroopStatus.FREE)
            {
                UIManager.ShowToast("部队正在休整中，请稍后");
                return;
            }
            target = tId;
            status = TroopStatus.MOVE;
            Watcher.Dispatch(EventCmd.Move_Cmd ,camp , ID ,  target);
        }

        //暂时不关心谁打死我的
        public void BeDestroyed()
        {
            curForces = 0;
            status = TroopStatus.REFORM;
            Move2Tower(ConstData.Move_Command_Arr[camp][0]);
            //优化：重伤只返回前一个节点
        }
    }


}
