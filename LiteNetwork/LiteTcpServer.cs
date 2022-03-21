using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace LiteNetwork
{

   public class Connection : TcpSession {

        IParser parser;
        public Connection(TcpServer server, IParser parser):base(server)
        {
            this.parser = parser;
        }
        public Connection(TcpServer server) : base(server) {
           
           
        }
        protected override void OnConnected()
        {
            Console.WriteLine($"Connection session with Id {Id} connected!");
        }
        protected override void OnDisconnected()
        {
            Console.WriteLine($"Connection session with Id {Id} disconnected!");
        }
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var package = parser.ReadPackageFromBuffer(buffer);
            ((LiteTcpServer)Server).DispatchPackage(this,package);
        }
        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Connection session caught an error with code {error}");
        }
        public void SendPackage(Package package) {
            Send(parser.WritePackageToBuffer(package));
        }
        public void SendPackageAsyn(Package package)
        {
            SendAsync(parser.WritePackageToBuffer(package));
        }

        public void Brocast(Package package) {
            Server.Multicast(parser.WritePackageToBuffer(package));
        }
    }
    public class LiteTcpServer : TcpServer
    {
        IParser parser;
        Dictionary<UInt32, Tuple<MethodInfo, Handler>> handleActions
            = new Dictionary<UInt32, Tuple<MethodInfo, Handler>>();

        public LiteTcpServer(IParser parser, IPAddress address, int port) : base(address, port)
        {
          
            this.parser = parser;
            var handleClasses = Assembly.GetEntryAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(Handler)));
            foreach (var cs in handleClasses)
            {
                var handler = Activator.CreateInstance(cs) as Handler;

                foreach (var meth in handler.GetType().GetMethods())
                {
                    var attrib = meth.GetCustomAttribute<PackageHandleAttribute>();
                    if (attrib == null) continue;
                    handleActions.Add(attrib.id, new Tuple<MethodInfo, Handler>(meth, handler));
                }
            }

            Console.WriteLine($"Server Create!!");
        }

        public void DispatchPackage(Connection connection, Package package) {
            if (handleActions.TryGetValue(package.id, out var item))
            {
                var (m, h) = item;
                m.Invoke(h, new object[] { connection, package });
            }

        }
        protected override TcpSession CreateSession() => new Connection(this,parser);

        protected override void OnStarting()
        {
            base.OnStarting();
            Console.WriteLine($"LiteTcpServer OnStarting......");
        }
        protected override void OnStarted()
        {
            base.OnStarted();
            Console.WriteLine($"LiteTcpServer OnStarted!!");
        }

        protected override void OnConnecting(TcpSession session)
        {
            base.OnConnecting(session);
            Console.WriteLine($"Connection:{session.Id} OnConnecting......");
        }
        protected override void OnConnected(TcpSession session)
        {
            base.OnConnected(session);
            Console.WriteLine($"Connection:{session.Id} OnConnected!!");
        }

        protected override void OnDisconnecting(TcpSession session)
        {
            base.OnDisconnecting(session);
            Console.WriteLine($"Connection:{session.Id} OnDisconnecting......");
        }

        protected override void OnDisconnected(TcpSession session)
        {
            base.OnDisconnected(session);
            Console.WriteLine($"Connection:{session.Id} OnDisconnected!!");
        }

        protected override void OnStopping()
        {
            base.OnStopping();
            Console.WriteLine($"LiteTcpServer OnStopping......");
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            Console.WriteLine($"LiteTcpServer OnStopped!!");
        }
        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP server caught an error with code {error}");
        }
    }
}
