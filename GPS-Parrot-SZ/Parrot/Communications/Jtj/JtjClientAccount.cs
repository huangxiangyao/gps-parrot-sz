using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Parrot
{
    /// <summary>
    /// 交通局客户端帐户资料。
    /// </summary>
    public class JtjClientAccount
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="address"></param>
        /// <param name="listeningPort"></param>
        /// <param name="omcId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public JtjClientAccount(IPAddress address, int listeningPort, int omcId, string username, string password)
            : this(address, listeningPort, omcId, username, password, listeningPort) { }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="address"></param>
        /// <param name="listeningPort"></param>
        /// <param name="omcId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public JtjClientAccount(IPAddress address, int listeningPort, int omcId, string username, string password, int localPort)
        {
            this.ServerIp = address;
            this.ListeningPort = listeningPort;
            this.ClientId = omcId;
            this.Username = username;
            this.Password = password;
            this.LocalPort = localPort;
        }

        /// <summary>
        /// 交通局监控平台的IP地址。
        /// </summary>
        public IPAddress ServerIp { get; private set; }
        /// <summary>
        /// 交通局监控平台的侦听端口。
        /// </summary>
        public int ListeningPort { get; private set; }
        /// <summary>
        /// 企业监控平台代码：即OMC代码。由政府职能部门指定机构统一分配。
        /// </summary>
        public int ClientId { get; private set; }
        /// <summary>
        /// 用于登录交通局监控平台的用户名。
        /// </summary>
        public string Username { get; private set; }
        /// <summary>
        /// 用于登录交通局监控平台的用户密码。
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 本地（客户端）端口。可选，默认与<see cref="ListeningPort"/>相同。
        /// </summary>
        /// <remarks>一般是由本地网络管理员根据防火墙设置选择一个可用的TCP端口。</remarks>
        public int LocalPort { get; private set; }
    }
}
