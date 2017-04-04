using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using PS3SaveEditor;

namespace Rss
{
	// Token: 0x0200007C RID: 124
	[Serializable]
	public class RssFeed
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x00022EDD File Offset: 0x000210DD
		public RssFeed()
		{
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00022F1C File Offset: 0x0002111C
		public RssFeed(Encoding encoding)
		{
			this.encoding = encoding;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00022F6D File Offset: 0x0002116D
		public override string ToString()
		{
			return this.url;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00022F75 File Offset: 0x00021175
		public RssChannelCollection Channels
		{
			get
			{
				return this.channels;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00022F7D File Offset: 0x0002117D
		public RssModuleCollection Modules
		{
			get
			{
				return this.modules;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x00022F85 File Offset: 0x00021185
		public ExceptionCollection Exceptions
		{
			get
			{
				if (this.exceptions != null)
				{
					return this.exceptions;
				}
				return new ExceptionCollection();
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00022F9B File Offset: 0x0002119B
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x00022FA3 File Offset: 0x000211A3
		public RssVersion Version
		{
			get
			{
				return this.rssVersion;
			}
			set
			{
				this.rssVersion = value;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00022FAC File Offset: 0x000211AC
		public string ETag
		{
			get
			{
				return this.etag;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x00022FB4 File Offset: 0x000211B4
		public DateTime LastModified
		{
			get
			{
				return this.lastModified;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00022FBC File Offset: 0x000211BC
		public bool Cached
		{
			get
			{
				return this.cached;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00022FC4 File Offset: 0x000211C4
		public string Url
		{
			get
			{
				return this.url;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00022FCC File Offset: 0x000211CC
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x00022FD4 File Offset: 0x000211D4
		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				this.encoding = value;
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00022FDD File Offset: 0x000211DD
		public static RssFeed Read(string url)
		{
			return RssFeed.read(url, null, null);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00022FE7 File Offset: 0x000211E7
		public static RssFeed Read(HttpWebRequest Request)
		{
			return RssFeed.read(Request.RequestUri.ToString(), Request, null);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00022FFB File Offset: 0x000211FB
		public static RssFeed Read(RssFeed oldFeed)
		{
			return RssFeed.read(oldFeed.url, null, oldFeed);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0002300A File Offset: 0x0002120A
		public static RssFeed Read(HttpWebRequest Request, RssFeed oldFeed)
		{
			return RssFeed.read(oldFeed.url, Request, oldFeed);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0002301C File Offset: 0x0002121C
		private static RssFeed read(string url, HttpWebRequest request, RssFeed oldFeed)
		{
			RssFeed rssFeed = new RssFeed();
			Stream stream = null;
			Uri uri = new Uri(url);
			rssFeed.url = url;
			string scheme;
			if ((scheme = uri.Scheme) != null)
			{
				if (!(scheme == "file"))
				{
					if (scheme == "https" || scheme == "http")
					{
						if (request == null)
						{
							request = (HttpWebRequest)WebRequest.Create(uri);
						}
						request.Credentials = Util.GetNetworkCredential();
						string value = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
						request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
						request.Headers.Add("Authorization", value);
						request.UserAgent = Util.GetUserAgent();
						request.PreAuthenticate = true;
						if (oldFeed != null)
						{
							request.IfModifiedSince = oldFeed.LastModified;
							request.Headers.Add("If-None-Match", oldFeed.ETag);
						}
						try
						{
							HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
							rssFeed.lastModified = httpWebResponse.LastModified;
							rssFeed.etag = httpWebResponse.Headers["ETag"];
							try
							{
								if (httpWebResponse.ContentEncoding != "")
								{
									rssFeed.encoding = Encoding.GetEncoding(httpWebResponse.ContentEncoding);
								}
							}
							catch
							{
							}
							stream = httpWebResponse.GetResponseStream();
						}
						catch (WebException ex)
						{
							if (oldFeed != null)
							{
								oldFeed.cached = true;
								return oldFeed;
							}
							throw ex;
						}
					}
				}
				else
				{
					rssFeed.lastModified = File.GetLastWriteTime(url);
					if (oldFeed != null && rssFeed.LastModified == oldFeed.LastModified)
					{
						oldFeed.cached = true;
						return oldFeed;
					}
					stream = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				}
			}
			if (stream != null)
			{
				RssReader rssReader = null;
				try
				{
					rssReader = new RssReader(stream);
					RssElement rssElement;
					do
					{
						rssElement = rssReader.Read();
						if (rssElement is RssChannel)
						{
							rssFeed.Channels.Add((RssChannel)rssElement);
						}
					}
					while (rssElement != null);
					rssFeed.rssVersion = rssReader.Version;
					return rssFeed;
				}
				finally
				{
					rssFeed.exceptions = rssReader.Exceptions;
					rssReader.Close();
				}
			}
			throw new ApplicationException("Not a valid Url");
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0002325C File Offset: 0x0002145C
		public void Write(Stream stream)
		{
			RssWriter writer;
			if (this.encoding == null)
			{
				writer = new RssWriter(stream);
			}
			else
			{
				writer = new RssWriter(stream, this.encoding);
			}
			this.write(writer);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00023290 File Offset: 0x00021490
		public void Write(string fileName)
		{
			RssWriter writer = new RssWriter(fileName);
			this.write(writer);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000232AC File Offset: 0x000214AC
		private void write(RssWriter writer)
		{
			try
			{
				if (this.channels.Count == 0)
				{
					throw new InvalidOperationException("Feed must contain at least one channel.");
				}
				writer.Version = this.rssVersion;
				writer.Modules = this.modules;
				foreach (RssChannel rssChannel in this.channels)
				{
					if (rssChannel.Items.Count == 0)
					{
						throw new InvalidOperationException("Channel must contain at least one item.");
					}
					writer.Write(rssChannel);
				}
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
				}
			}
		}

		// Token: 0x040002E0 RID: 736
		private RssChannelCollection channels = new RssChannelCollection();

		// Token: 0x040002E1 RID: 737
		private RssModuleCollection modules = new RssModuleCollection();

		// Token: 0x040002E2 RID: 738
		private ExceptionCollection exceptions;

		// Token: 0x040002E3 RID: 739
		private DateTime lastModified = RssDefault.DateTime;

		// Token: 0x040002E4 RID: 740
		private RssVersion rssVersion;

		// Token: 0x040002E5 RID: 741
		private bool cached;

		// Token: 0x040002E6 RID: 742
		private string etag = "";

		// Token: 0x040002E7 RID: 743
		private string url = "";

		// Token: 0x040002E8 RID: 744
		private Encoding encoding;
	}
}
