using LiteNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetorkDemo
{
    public class LoginHandler : Handler
    {
        public LoginHandler() 
        {

        }
        protected override void HandleUnKownPackage(object data, uint id)
        {

        }

        [PackageHandle(1)]
        public void OnLogin(Connection connection,Package package)
        {
            Console.WriteLine("Login Request");

            var pag = new LoginResponePackage() { msg = "登录成功" };
            //parser.WritePackageToStream(pag, connection.Writer);
            connection.SendPackageAsyn(pag);
        }
    }

    public class FightHandler : Handler
    {
        public FightHandler() 
        {

        }

        [PackageHandle(10)]
        public void OnFight(Connection connection, Package package)
        {
            Console.WriteLine("Fight Request");

            var pag = new FightResponePackage() { msg = "哈哈,开始战斗了!" };
            connection.SendPackageAsyn(pag);

        }
    }

}
