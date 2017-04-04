using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Rss
{
	// Token: 0x0200008B RID: 139
	public class RssReader
	{
		// Token: 0x06000695 RID: 1685 RVA: 0x00023FB6 File Offset: 0x000221B6
		private void InitReader()
		{
			this.reader.WhitespaceHandling = WhitespaceHandling.None;
			this.reader.XmlResolver = null;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00023FD0 File Offset: 0x000221D0
		public RssReader(string url)
		{
			try
			{
				this.reader = new XmlTextReader(url);
				this.InitReader();
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Unable to retrieve file containing the RSS data.", innerException);
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00024038 File Offset: 0x00022238
		public RssReader(TextReader textReader)
		{
			try
			{
				this.reader = new XmlTextReader(textReader);
				this.InitReader();
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Unable to retrieve file containing the RSS data.", innerException);
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000240A0 File Offset: 0x000222A0
		public RssReader(Stream stream)
		{
			try
			{
				this.reader = new XmlTextReader(stream);
				this.InitReader();
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Unable to retrieve file containing the RSS data.", innerException);
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00024108 File Offset: 0x00022308
		public RssElement Read()
		{
			bool flag = false;
			bool flag2 = true;
			RssElement result = null;
			int num = -1;
			int num2 = -1;
			if (this.reader == null)
			{
				throw new InvalidOperationException("RssReader has been closed, and can not be read.");
			}
			do
			{
				flag2 = true;
				try
				{
					flag = this.reader.Read();
				}
				catch (EndOfStreamException innerException)
				{
					throw new EndOfStreamException("Unable to read an RssElement. Reached the end of the stream.", innerException);
				}
				catch (XmlException exception)
				{
					if ((num != -1 || num2 != -1) && this.reader.LineNumber == num && this.reader.LinePosition == num2)
					{
						throw this.exceptions.LastException;
					}
					num = this.reader.LineNumber;
					num2 = this.reader.LinePosition;
					this.exceptions.Add(exception);
				}
				if (flag)
				{
					string text = this.reader.Name.ToLower();
					XmlNodeType nodeType = this.reader.NodeType;
					switch (nodeType)
					{
					case XmlNodeType.Element:
						if (!this.reader.IsEmptyElement)
						{
							this.elementText = new StringBuilder();
							string key;
							switch (key = text)
							{
							case "item":
								if (!this.wroteChannel)
								{
									this.wroteChannel = true;
									result = this.channel;
									flag = false;
								}
								this.item = new RssItem();
								this.channel.Items.Add(this.item);
								break;
							case "source":
								this.source = new RssSource();
								this.item.Source = this.source;
								for (int i = 0; i < this.reader.AttributeCount; i++)
								{
									this.reader.MoveToAttribute(i);
									string a;
									if ((a = this.reader.Name.ToLower()) != null && a == "url")
									{
										try
										{
											this.source.Url = new Uri(this.reader.Value);
										}
										catch (Exception exception2)
										{
											this.exceptions.Add(exception2);
										}
									}
								}
								break;
							case "enclosure":
								this.enclosure = new RssEnclosure();
								this.item.Enclosure = this.enclosure;
								for (int j = 0; j < this.reader.AttributeCount; j++)
								{
									this.reader.MoveToAttribute(j);
									string a2;
									if ((a2 = this.reader.Name.ToLower()) != null)
									{
										if (!(a2 == "url"))
										{
											if (!(a2 == "length"))
											{
												if (!(a2 == "type"))
												{
													goto IL_3BA;
												}
												goto IL_3A4;
											}
										}
										else
										{
											try
											{
												this.enclosure.Url = new Uri(this.reader.Value);
												goto IL_3BA;
											}
											catch (Exception exception3)
											{
												this.exceptions.Add(exception3);
												goto IL_3BA;
											}
										}
										try
										{
											this.enclosure.Length = int.Parse(this.reader.Value);
											goto IL_3BA;
										}
										catch (Exception exception4)
										{
											this.exceptions.Add(exception4);
											goto IL_3BA;
										}
										IL_3A4:
										this.enclosure.Type = this.reader.Value;
									}
									IL_3BA:;
								}
								break;
							case "guid":
								this.guid = new RssGuid();
								this.item.Guid = this.guid;
								for (int k = 0; k < this.reader.AttributeCount; k++)
								{
									this.reader.MoveToAttribute(k);
									string a3;
									if ((a3 = this.reader.Name.ToLower()) != null && a3 == "ispermalink")
									{
										try
										{
											this.guid.PermaLink = bool.Parse(this.reader.Value);
										}
										catch (Exception exception5)
										{
											this.exceptions.Add(exception5);
										}
									}
								}
								break;
							case "category":
								this.category = new RssCategory();
								if ((string)this.xmlNodeStack.Peek() == "channel")
								{
									this.channel.Categories.Add(this.category);
								}
								else
								{
									this.item.Categories.Add(this.category);
								}
								for (int l = 0; l < this.reader.AttributeCount; l++)
								{
									this.reader.MoveToAttribute(l);
									string a4;
									if ((a4 = this.reader.Name.ToLower()) != null && (a4 == "url" || a4 == "domain"))
									{
										this.category.Domain = this.reader.Value;
									}
								}
								break;
							case "channel":
								this.channel = new RssChannel();
								this.textInput = null;
								this.image = null;
								this.cloud = null;
								this.source = null;
								this.enclosure = null;
								this.category = null;
								this.item = null;
								break;
							case "image":
								this.image = new RssImage();
								this.channel.Image = this.image;
								break;
							case "textinput":
								this.textInput = new RssTextInput();
								this.channel.TextInput = this.textInput;
								break;
							case "cloud":
								flag2 = false;
								this.cloud = new RssCloud();
								this.channel.Cloud = this.cloud;
								for (int m = 0; m < this.reader.AttributeCount; m++)
								{
									this.reader.MoveToAttribute(m);
									string a5;
									if ((a5 = this.reader.Name.ToLower()) != null)
									{
										if (!(a5 == "domain"))
										{
											if (!(a5 == "port"))
											{
												if (!(a5 == "path"))
												{
													if (a5 == "registerprocedure")
													{
														this.cloud.RegisterProcedure = this.reader.Value;
														goto IL_759;
													}
													if (!(a5 == "protocol"))
													{
														goto IL_759;
													}
													string a6;
													if ((a6 = this.reader.Value.ToLower()) != null)
													{
														if (a6 == "xml-rpc")
														{
															this.cloud.Protocol = RssCloudProtocol.XmlRpc;
															goto IL_759;
														}
														if (a6 == "soap")
														{
															this.cloud.Protocol = RssCloudProtocol.Soap;
															goto IL_759;
														}
														if (a6 == "http-post")
														{
															this.cloud.Protocol = RssCloudProtocol.HttpPost;
															goto IL_759;
														}
													}
													this.cloud.Protocol = RssCloudProtocol.Empty;
													goto IL_759;
												}
											}
											else
											{
												try
												{
													this.cloud.Port = (int)ushort.Parse(this.reader.Value);
													goto IL_759;
												}
												catch (Exception exception6)
												{
													this.exceptions.Add(exception6);
													goto IL_759;
												}
											}
											this.cloud.Path = this.reader.Value;
										}
										else
										{
											this.cloud.Domain = this.reader.Value;
										}
									}
									IL_759:;
								}
								break;
							case "rss":
								for (int n = 0; n < this.reader.AttributeCount; n++)
								{
									this.reader.MoveToAttribute(n);
									if (this.reader.Name.ToLower() == "version")
									{
										string value;
										if ((value = this.reader.Value) != null)
										{
											if (value == "0.91")
											{
												this.rssVersion = RssVersion.RSS091;
												goto IL_805;
											}
											if (value == "0.92")
											{
												this.rssVersion = RssVersion.RSS092;
												goto IL_805;
											}
											if (value == "2.0")
											{
												this.rssVersion = RssVersion.RSS20;
												goto IL_805;
											}
										}
										this.rssVersion = RssVersion.NotSupported;
									}
									IL_805:;
								}
								break;
							case "rdf":
								for (int num4 = 0; num4 < this.reader.AttributeCount; num4++)
								{
									this.reader.MoveToAttribute(num4);
									if (this.reader.Name.ToLower() == "version")
									{
										string value2;
										if ((value2 = this.reader.Value) != null)
										{
											if (value2 == "0.90")
											{
												this.rssVersion = RssVersion.RSS090;
												goto IL_897;
											}
											if (value2 == "1.0")
											{
												this.rssVersion = RssVersion.RSS10;
												goto IL_897;
											}
										}
										this.rssVersion = RssVersion.NotSupported;
									}
									IL_897:;
								}
								break;
							}
							if (flag2)
							{
								this.xmlNodeStack.Push(text);
							}
						}
						break;
					case XmlNodeType.Attribute:
						break;
					case XmlNodeType.Text:
						this.elementText.Append(this.reader.Value);
						break;
					case XmlNodeType.CDATA:
						this.elementText.Append(this.reader.Value);
						break;
					default:
						if (nodeType == XmlNodeType.EndElement)
						{
							if (this.xmlNodeStack.Count != 1)
							{
								string text2 = (string)this.xmlNodeStack.Pop();
								string text3 = (string)this.xmlNodeStack.Peek();
								string key2;
								switch (key2 = text2)
								{
								case "item":
									result = this.item;
									flag = false;
									break;
								case "source":
									this.source.Name = this.elementText.ToString();
									result = this.source;
									flag = false;
									break;
								case "enclosure":
									result = this.enclosure;
									flag = false;
									break;
								case "guid":
									this.guid.Name = this.elementText.ToString();
									result = this.guid;
									flag = false;
									break;
								case "category":
									this.category.Name = this.elementText.ToString();
									result = this.category;
									flag = false;
									break;
								case "channel":
									if (this.wroteChannel)
									{
										this.wroteChannel = false;
									}
									else
									{
										this.wroteChannel = true;
										result = this.channel;
										flag = false;
									}
									break;
								case "textinput":
									result = this.textInput;
									flag = false;
									break;
								case "image":
									result = this.image;
									flag = false;
									break;
								case "cloud":
									result = this.cloud;
									flag = false;
									break;
								}
								string a7;
								if ((a7 = text3) != null)
								{
									if (!(a7 == "item"))
									{
										if (!(a7 == "channel"))
										{
											if (a7 == "image")
											{
												goto IL_F78;
											}
											if (a7 == "textinput")
											{
												goto IL_10F1;
											}
											if (a7 == "skipdays")
											{
												goto IL_11BE;
											}
											if (!(a7 == "skiphours"))
											{
												break;
											}
											if (text2 == "hour")
											{
												this.channel.SkipHours[(int)byte.Parse(this.elementText.ToString().ToLower())] = true;
												break;
											}
											break;
										}
									}
									else
									{
										string a8;
										if ((a8 = text2) == null)
										{
											break;
										}
										if (a8 == "title")
										{
											this.item.Title = this.elementText.ToString();
											break;
										}
										if (!(a8 == "link"))
										{
											if (a8 == "description")
											{
												this.item.Description = this.elementText.ToString();
												break;
											}
											if (a8 == "author")
											{
												this.item.Author = this.elementText.ToString();
												break;
											}
											if (a8 == "comments")
											{
												this.item.Comments = this.elementText.ToString();
												break;
											}
											if (!(a8 == "pubdate"))
											{
												break;
											}
											try
											{
												this.item.PubDate = DateTime.Parse(this.elementText.ToString());
												break;
											}
											catch (Exception exception7)
											{
												try
												{
													string text4 = this.elementText.ToString();
													text4 = text4.Substring(0, text4.Length - 5);
													text4 += "GMT";
													this.item.PubDate = DateTime.Parse(text4);
												}
												catch
												{
													this.exceptions.Add(exception7);
												}
												break;
											}
										}
										else
										{
											if (this.elementText.Length > 0)
											{
												this.item.Link = new Uri(this.elementText.ToString());
												break;
											}
											this.item.Link = null;
											break;
										}
									}
									string key3;
									if ((key3 = text2) == null)
									{
										break;
									}
									if (<PrivateImplementationDetails>{0FE034E3-13E4-4121-9910-ADD6363B8EF6}.$$method0x600068e-3 == null)
									{
										<PrivateImplementationDetails>{0FE034E3-13E4-4121-9910-ADD6363B8EF6}.$$method0x600068e-3 = new Dictionary<string, int>(13)
										{
											{
												"title",
												0
											},
											{
												"link",
												1
											},
											{
												"description",
												2
											},
											{
												"language",
												3
											},
											{
												"copyright",
												4
											},
											{
												"managingeditor",
												5
											},
											{
												"webmaster",
												6
											},
											{
												"rating",
												7
											},
											{
												"pubdate",
												8
											},
											{
												"lastbuilddate",
												9
											},
											{
												"generator",
												10
											},
											{
												"docs",
												11
											},
											{
												"ttl",
												12
											}
										};
									}
									int num6;
									if (<PrivateImplementationDetails>{0FE034E3-13E4-4121-9910-ADD6363B8EF6}.$$method0x600068e-3.TryGetValue(key3, out num6))
									{
										switch (num6)
										{
										case 0:
											this.channel.Title = this.elementText.ToString();
											goto IL_136F;
										case 1:
											try
											{
												this.channel.Link = new Uri(this.elementText.ToString());
												goto IL_136F;
											}
											catch (Exception exception8)
											{
												this.exceptions.Add(exception8);
												goto IL_136F;
											}
											break;
										case 2:
											break;
										case 3:
											this.channel.Language = this.elementText.ToString();
											goto IL_136F;
										case 4:
											this.channel.Copyright = this.elementText.ToString();
											goto IL_136F;
										case 5:
											this.channel.ManagingEditor = this.elementText.ToString();
											goto IL_136F;
										case 6:
											this.channel.WebMaster = this.elementText.ToString();
											goto IL_136F;
										case 7:
											this.channel.Rating = this.elementText.ToString();
											goto IL_136F;
										case 8:
											try
											{
												this.channel.PubDate = DateTime.Parse(this.elementText.ToString());
												goto IL_136F;
											}
											catch (Exception exception9)
											{
												this.exceptions.Add(exception9);
												goto IL_136F;
											}
											goto Block_80;
										case 9:
											goto IL_ED8;
										case 10:
											goto IL_F0D;
										case 11:
											this.channel.Docs = this.elementText.ToString();
											goto IL_136F;
										case 12:
											try
											{
												this.channel.TimeToLive = int.Parse(this.elementText.ToString());
												goto IL_136F;
											}
											catch (Exception exception10)
											{
												this.exceptions.Add(exception10);
												goto IL_136F;
											}
											goto IL_F78;
										default:
											goto IL_136F;
										}
										this.channel.Description = this.elementText.ToString();
										break;
										Block_80:
										try
										{
											IL_ED8:
											this.channel.LastBuildDate = DateTime.Parse(this.elementText.ToString());
											break;
										}
										catch (Exception exception11)
										{
											this.exceptions.Add(exception11);
											break;
										}
										IL_F0D:
										this.channel.Generator = this.elementText.ToString();
										break;
									}
									break;
									IL_F78:
									string a9;
									if ((a9 = text2) != null)
									{
										if (!(a9 == "url"))
										{
											if (!(a9 == "title"))
											{
												if (!(a9 == "link"))
												{
													if (!(a9 == "description"))
													{
														if (!(a9 == "width"))
														{
															if (!(a9 == "height"))
															{
																break;
															}
														}
														else
														{
															try
															{
																this.image.Width = (int)byte.Parse(this.elementText.ToString());
																break;
															}
															catch (Exception exception12)
															{
																this.exceptions.Add(exception12);
																break;
															}
														}
														try
														{
															this.image.Height = (int)byte.Parse(this.elementText.ToString());
															break;
														}
														catch (Exception exception13)
														{
															this.exceptions.Add(exception13);
															break;
														}
														goto IL_10F1;
													}
												}
												else
												{
													try
													{
														this.image.Link = new Uri(this.elementText.ToString());
														break;
													}
													catch (Exception exception14)
													{
														this.exceptions.Add(exception14);
														break;
													}
												}
												this.image.Description = this.elementText.ToString();
												break;
											}
										}
										else
										{
											try
											{
												this.image.Url = new Uri(this.elementText.ToString());
												break;
											}
											catch (Exception exception15)
											{
												this.exceptions.Add(exception15);
												break;
											}
										}
										this.image.Title = this.elementText.ToString();
										break;
									}
									break;
									IL_10F1:
									string a10;
									if ((a10 = text2) == null)
									{
										break;
									}
									if (a10 == "title")
									{
										this.textInput.Title = this.elementText.ToString();
										break;
									}
									if (a10 == "description")
									{
										this.textInput.Description = this.elementText.ToString();
										break;
									}
									if (a10 == "name")
									{
										this.textInput.Name = this.elementText.ToString();
										break;
									}
									if (!(a10 == "link"))
									{
										break;
									}
									try
									{
										this.textInput.Link = new Uri(this.elementText.ToString());
										break;
									}
									catch (Exception exception16)
									{
										this.exceptions.Add(exception16);
										break;
									}
									IL_11BE:
									string key4;
									if (text2 == "day" && (key4 = this.elementText.ToString().ToLower()) != null)
									{
										if (<PrivateImplementationDetails>{0FE034E3-13E4-4121-9910-ADD6363B8EF6}.$$method0x600068e-4 == null)
										{
											<PrivateImplementationDetails>{0FE034E3-13E4-4121-9910-ADD6363B8EF6}.$$method0x600068e-4 = new Dictionary<string, int>(7)
											{
												{
													"monday",
													0
												},
												{
													"tuesday",
													1
												},
												{
													"wednesday",
													2
												},
												{
													"thursday",
													3
												},
												{
													"friday",
													4
												},
												{
													"saturday",
													5
												},
												{
													"sunday",
													6
												}
											};
										}
										int num7;
										if (<PrivateImplementationDetails>{0FE034E3-13E4-4121-9910-ADD6363B8EF6}.$$method0x600068e-4.TryGetValue(key4, out num7))
										{
											switch (num7)
											{
											case 0:
												this.channel.SkipDays[0] = true;
												break;
											case 1:
												this.channel.SkipDays[1] = true;
												break;
											case 2:
												this.channel.SkipDays[2] = true;
												break;
											case 3:
												this.channel.SkipDays[3] = true;
												break;
											case 4:
												this.channel.SkipDays[4] = true;
												break;
											case 5:
												this.channel.SkipDays[5] = true;
												break;
											case 6:
												this.channel.SkipDays[6] = true;
												break;
											}
										}
									}
								}
							}
						}
						break;
					}
				}
				IL_136F:;
			}
			while (flag);
			return result;
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x00025568 File Offset: 0x00023768
		public ExceptionCollection Exceptions
		{
			get
			{
				return this.exceptions;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00025570 File Offset: 0x00023770
		public RssVersion Version
		{
			get
			{
				return this.rssVersion;
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00025578 File Offset: 0x00023778
		public void Close()
		{
			this.textInput = null;
			this.image = null;
			this.cloud = null;
			this.channel = null;
			this.source = null;
			this.enclosure = null;
			this.category = null;
			this.item = null;
			if (this.reader != null)
			{
				this.reader.Close();
				this.reader = null;
			}
			this.elementText = null;
			this.xmlNodeStack = null;
		}

		// Token: 0x04000303 RID: 771
		private Stack xmlNodeStack = new Stack();

		// Token: 0x04000304 RID: 772
		private StringBuilder elementText = new StringBuilder();

		// Token: 0x04000305 RID: 773
		private XmlTextReader reader;

		// Token: 0x04000306 RID: 774
		private bool wroteChannel;

		// Token: 0x04000307 RID: 775
		private RssVersion rssVersion;

		// Token: 0x04000308 RID: 776
		private ExceptionCollection exceptions = new ExceptionCollection();

		// Token: 0x04000309 RID: 777
		private RssTextInput textInput;

		// Token: 0x0400030A RID: 778
		private RssImage image;

		// Token: 0x0400030B RID: 779
		private RssCloud cloud;

		// Token: 0x0400030C RID: 780
		private RssChannel channel;

		// Token: 0x0400030D RID: 781
		private RssSource source;

		// Token: 0x0400030E RID: 782
		private RssEnclosure enclosure;

		// Token: 0x0400030F RID: 783
		private RssGuid guid;

		// Token: 0x04000310 RID: 784
		private RssCategory category;

		// Token: 0x04000311 RID: 785
		private RssItem item;
	}
}
