using System;
using System.Net;
using System.Net.Security;
using System.Text;

namespace PS3SaveEditor
{
	// Token: 0x0200010E RID: 270
	internal class WebClientEx : WebClient
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x0003F6B4 File Offset: 0x0003D8B4
		protected override WebRequest GetWebRequest(Uri address)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)base.GetWebRequest(address);
			httpWebRequest.UserAgent = Util.GetUserAgent();
			httpWebRequest.PreAuthenticate = true;
			string value = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
			httpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
			httpWebRequest.Headers.Add("Authorization", value);
			return httpWebRequest;
		}
	}
}
