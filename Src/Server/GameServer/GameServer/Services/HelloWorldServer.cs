using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class HelloWorldServer:Singleton<HelloWorldServer>
    {
        public void Init()
        {

        }

        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);
        }

        private void OnFirstTestRequest(NetConnection<NetSession> sender, FirstTestRequest message)
        {
            Log.InfoFormat("xxxxx {0}   ssssss{1}",message.Helloworld.ToString(),sender.Session.User);
        }

        public void Stop()
        {

        }
    }
}
