using System;

namespace Server
{
    class ServerStart
    {
        static void Main(string[] args)
        {
            ServerRoot.Instance.Init();

            while (true)
            {
                ServerRoot.Instance.Update();
            }
        }
    }
}
