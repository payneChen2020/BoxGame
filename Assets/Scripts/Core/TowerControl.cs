using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constant;
using cfg.Config;

namespace GameFSM
{
    public class TowerControl
    {
        //据点编号
        public int ID { get; private set; }
        public int belongs { get; private set; }

        public int defForces;

        public TowerStatus status {
            get;
            private set;
        }

        public void SwitchStatus(TowerStatus sta)
        {
            status = sta;

            //派发到表现层，节点表现更新
            Watcher.Dispatch(EventCmd.Tower_Status_Cmd, ID , status  , belongs);

        }

        //初始化守军级阵营数据
        public void InitData(GraphPosCfg cfg)
        {
            ID = cfg.Id;
            belongs = cfg.Camp;
            defForces = cfg.Forces;
            status = belongs == 1 ? TowerStatus.NO_OCCUPUTATION : TowerStatus.OCCUPUTATED;
        }

        //被攻击，返回一个攻击后剩余血量，代表结果
        public int BeAttacked(int camp , int forces)
        {

            if (camp == belongs) return forces;

            int ret = forces - defForces;
            defForces -= forces;

            if (defForces <= 0)
            {
                defForces = 0;
                belongs = camp;
                status = TowerStatus.OCCUPUTATED;
                Watcher.Dispatch(EventCmd.Tower_Status_Cmd, ID, status, belongs);
            }

            return ret;
        }
    }
}
