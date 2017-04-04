using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000137 RID: 311
	public class FileSelector
	{
		// Token: 0x06000C64 RID: 3172 RVA: 0x00043AA7 File Offset: 0x00041CA7
		public FileSelector(string selectionCriteria) : this(selectionCriteria, true)
		{
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00043AB1 File Offset: 0x00041CB1
		public FileSelector(string selectionCriteria, bool traverseDirectoryReparsePoints)
		{
			if (!string.IsNullOrEmpty(selectionCriteria))
			{
				this._Criterion = FileSelector._ParseCriterion(selectionCriteria);
			}
			this.TraverseReparsePoints = traverseDirectoryReparsePoints;
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00043AD4 File Offset: 0x00041CD4
		// (set) Token: 0x06000C67 RID: 3175 RVA: 0x00043AEB File Offset: 0x00041CEB
		public string SelectionCriteria
		{
			get
			{
				if (this._Criterion == null)
				{
					return null;
				}
				return this._Criterion.ToString();
			}
			set
			{
				if (value == null)
				{
					this._Criterion = null;
					return;
				}
				if (value.Trim() == "")
				{
					this._Criterion = null;
					return;
				}
				this._Criterion = FileSelector._ParseCriterion(value);
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00043B1E File Offset: 0x00041D1E
		// (set) Token: 0x06000C69 RID: 3177 RVA: 0x00043B26 File Offset: 0x00041D26
		public bool TraverseReparsePoints
		{
			get;
			set;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00043B30 File Offset: 0x00041D30
		private static string NormalizeCriteriaExpression(string source)
		{
			string[][] array = new string[][]
			{
				new string[]
				{
					"([^']*)\\(\\(([^']+)",
					"$1( ($2"
				},
				new string[]
				{
					"(.)\\)\\)",
					"$1) )"
				},
				new string[]
				{
					"\\((\\S)",
					"( $1"
				},
				new string[]
				{
					"(\\S)\\)",
					"$1 )"
				},
				new string[]
				{
					"^\\)",
					" )"
				},
				new string[]
				{
					"(\\S)\\(",
					"$1 ("
				},
				new string[]
				{
					"\\)(\\S)",
					") $1"
				},
				new string[]
				{
					"(=)('[^']*')",
					"$1 $2"
				},
				new string[]
				{
					"([^ !><])(>|<|!=|=)",
					"$1 $2"
				},
				new string[]
				{
					"(>|<|!=|=)([^ =])",
					"$1 $2"
				},
				new string[]
				{
					"/",
					"\\"
				}
			};
			string input = source;
			for (int i = 0; i < array.Length; i++)
			{
				string pattern = FileSelector.RegexAssertions.PrecededByEvenNumberOfSingleQuotes + array[i][0] + FileSelector.RegexAssertions.FollowedByEvenNumberOfSingleQuotesAndLineEnd;
				input = Regex.Replace(input, pattern, array[i][1]);
			}
			string pattern2 = "/" + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			input = Regex.Replace(input, pattern2, "\\");
			pattern2 = " " + FileSelector.RegexAssertions.FollowedByOddNumberOfSingleQuotesAndLineEnd;
			return Regex.Replace(input, pattern2, "\u0006");
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00043D1C File Offset: 0x00041F1C
		private static SelectionCriterion _ParseCriterion(string s)
		{
			if (s == null)
			{
				return null;
			}
			s = FileSelector.NormalizeCriteriaExpression(s);
			if (s.IndexOf(" ") == -1)
			{
				s = "name = " + s;
			}
			string[] array = s.Trim().Split(new char[]
			{
				' ',
				'\t'
			});
			if (array.Length < 3)
			{
				throw new ArgumentException(s);
			}
			SelectionCriterion selectionCriterion = null;
			Stack<FileSelector.ParseState> stack = new Stack<FileSelector.ParseState>();
			Stack<SelectionCriterion> stack2 = new Stack<SelectionCriterion>();
			stack.Push(FileSelector.ParseState.Start);
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i].ToLower();
				string key;
				if ((key = text) != null)
				{
					if (<PrivateImplementationDetails>{0FE034E3-13E4-4121-9910-ADD6363B8EF6}.$$method0x6000c55-1 == null)
					{
						<PrivateImplementationDetails>{0FE034E3-13E4-4121-9910-ADD6363B8EF6}.$$method0x6000c55-1 = new Dictionary<string, int>(16)
						{
							{
								"and",
								0
							},
							{
								"xor",
								1
							},
							{
								"or",
								2
							},
							{
								"(",
								3
							},
							{
								")",
								4
							},
							{
								"atime",
								5
							},
							{
								"ctime",
								6
							},
							{
								"mtime",
								7
							},
							{
								"length",
								8
							},
							{
								"size",
								9
							},
							{
								"filename",
								10
							},
							{
								"name",
								11
							},
							{
								"attrs",
								12
							},
							{
								"attributes",
								13
							},
							{
								"type",
								14
							},
							{
								"",
								15
							}
						};
					}
					int num;
					if (<PrivateImplementationDetails>{0FE034E3-13E4-4121-9910-ADD6363B8EF6}.$$method0x6000c55-1.TryGetValue(key, out num))
					{
						FileSelector.ParseState parseState;
						switch (num)
						{
						case 0:
						case 1:
						case 2:
						{
							parseState = stack.Peek();
							if (parseState != FileSelector.ParseState.CriterionDone)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							if (array.Length <= i + 3)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							LogicalConjunction conjunction = (LogicalConjunction)Enum.Parse(typeof(LogicalConjunction), array[i].ToUpper(), true);
							selectionCriterion = new CompoundCriterion
							{
								Left = selectionCriterion,
								Right = null,
								Conjunction = conjunction
							};
							stack.Push(parseState);
							stack.Push(FileSelector.ParseState.ConjunctionPending);
							stack2.Push(selectionCriterion);
							break;
						}
						case 3:
							parseState = stack.Peek();
							if (parseState != FileSelector.ParseState.Start && parseState != FileSelector.ParseState.ConjunctionPending && parseState != FileSelector.ParseState.OpenParen)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							if (array.Length <= i + 4)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							stack.Push(FileSelector.ParseState.OpenParen);
							break;
						case 4:
							parseState = stack.Pop();
							if (stack.Peek() != FileSelector.ParseState.OpenParen)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							stack.Pop();
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						case 5:
						case 6:
						case 7:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							DateTime dateTime;
							try
							{
								dateTime = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd-HH:mm:ss", null);
							}
							catch (FormatException)
							{
								try
								{
									dateTime = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd-HH:mm:ss", null);
								}
								catch (FormatException)
								{
									try
									{
										dateTime = DateTime.ParseExact(array[i + 2], "yyyy/MM/dd", null);
									}
									catch (FormatException)
									{
										try
										{
											dateTime = DateTime.ParseExact(array[i + 2], "MM/dd/yyyy", null);
										}
										catch (FormatException)
										{
											dateTime = DateTime.ParseExact(array[i + 2], "yyyy-MM-dd", null);
										}
									}
								}
							}
							dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime();
							selectionCriterion = new TimeCriterion
							{
								Which = (WhichTime)Enum.Parse(typeof(WhichTime), array[i], true),
								Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]),
								Time = dateTime
							};
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 8:
						case 9:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							string text2 = array[i + 2];
							long size;
							if (text2.ToUpper().EndsWith("K"))
							{
								size = long.Parse(text2.Substring(0, text2.Length - 1)) * 1024L;
							}
							else if (text2.ToUpper().EndsWith("KB"))
							{
								size = long.Parse(text2.Substring(0, text2.Length - 2)) * 1024L;
							}
							else if (text2.ToUpper().EndsWith("M"))
							{
								size = long.Parse(text2.Substring(0, text2.Length - 1)) * 1024L * 1024L;
							}
							else if (text2.ToUpper().EndsWith("MB"))
							{
								size = long.Parse(text2.Substring(0, text2.Length - 2)) * 1024L * 1024L;
							}
							else if (text2.ToUpper().EndsWith("G"))
							{
								size = long.Parse(text2.Substring(0, text2.Length - 1)) * 1024L * 1024L * 1024L;
							}
							else if (text2.ToUpper().EndsWith("GB"))
							{
								size = long.Parse(text2.Substring(0, text2.Length - 2)) * 1024L * 1024L * 1024L;
							}
							else
							{
								size = long.Parse(array[i + 2]);
							}
							selectionCriterion = new SizeCriterion
							{
								Size = size,
								Operator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1])
							};
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 10:
						case 11:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							ComparisonOperator comparisonOperator = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
							if (comparisonOperator != ComparisonOperator.NotEqualTo && comparisonOperator != ComparisonOperator.EqualTo)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							string text3 = array[i + 2];
							if (text3.StartsWith("'") && text3.EndsWith("'"))
							{
								text3 = text3.Substring(1, text3.Length - 2).Replace("\u0006", " ");
							}
							selectionCriterion = new NameCriterion
							{
								MatchingFileSpec = text3,
								Operator = comparisonOperator
							};
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 12:
						case 13:
						case 14:
						{
							if (array.Length <= i + 2)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							ComparisonOperator comparisonOperator2 = (ComparisonOperator)EnumUtil.Parse(typeof(ComparisonOperator), array[i + 1]);
							if (comparisonOperator2 != ComparisonOperator.NotEqualTo && comparisonOperator2 != ComparisonOperator.EqualTo)
							{
								throw new ArgumentException(string.Join(" ", array, i, array.Length - i));
							}
							selectionCriterion = ((text == "type") ? new TypeCriterion
							{
								AttributeString = array[i + 2],
								Operator = comparisonOperator2
							} : new AttributesCriterion
							{
								AttributeString = array[i + 2],
								Operator = comparisonOperator2
							});
							i += 2;
							stack.Push(FileSelector.ParseState.CriterionDone);
							break;
						}
						case 15:
							stack.Push(FileSelector.ParseState.Whitespace);
							break;
						default:
							goto IL_7AF;
						}
						parseState = stack.Peek();
						if (parseState == FileSelector.ParseState.CriterionDone)
						{
							stack.Pop();
							if (stack.Peek() == FileSelector.ParseState.ConjunctionPending)
							{
								while (stack.Peek() == FileSelector.ParseState.ConjunctionPending)
								{
									CompoundCriterion compoundCriterion = stack2.Pop() as CompoundCriterion;
									compoundCriterion.Right = selectionCriterion;
									selectionCriterion = compoundCriterion;
									stack.Pop();
									parseState = stack.Pop();
									if (parseState != FileSelector.ParseState.CriterionDone)
									{
										throw new ArgumentException("??");
									}
								}
							}
							else
							{
								stack.Push(FileSelector.ParseState.CriterionDone);
							}
						}
						if (parseState == FileSelector.ParseState.Whitespace)
						{
							stack.Pop();
						}
						i++;
						continue;
					}
				}
				IL_7AF:
				throw new ArgumentException("'" + array[i] + "'");
			}
			return selectionCriterion;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x000445B0 File Offset: 0x000427B0
		public override string ToString()
		{
			return "FileSelector(" + this._Criterion.ToString() + ")";
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x000445CC File Offset: 0x000427CC
		private bool Evaluate(string filename)
		{
			return this._Criterion.Evaluate(filename);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x000445E7 File Offset: 0x000427E7
		[Conditional("SelectorTrace")]
		private void SelectorTrace(string format, params object[] args)
		{
			if (this._Criterion != null && this._Criterion.Verbose)
			{
				Console.WriteLine(format, args);
			}
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00044605 File Offset: 0x00042805
		public ICollection<string> SelectFiles(string directory)
		{
			return this.SelectFiles(directory, false);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00044610 File Offset: 0x00042810
		public ReadOnlyCollection<string> SelectFiles(string directory, bool recurseDirectories)
		{
			if (this._Criterion == null)
			{
				throw new ArgumentException("SelectionCriteria has not been set");
			}
			List<string> list = new List<string>();
			try
			{
				if (Directory.Exists(directory))
				{
					string[] files = Directory.GetFiles(directory);
					string[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						if (this.Evaluate(text))
						{
							list.Add(text);
						}
					}
					if (recurseDirectories)
					{
						string[] directories = Directory.GetDirectories(directory);
						string[] array2 = directories;
						for (int j = 0; j < array2.Length; j++)
						{
							string text2 = array2[j];
							if (this.TraverseReparsePoints || (File.GetAttributes(text2) & FileAttributes.ReparsePoint) == (FileAttributes)0)
							{
								if (this.Evaluate(text2))
								{
									list.Add(text2);
								}
								list.AddRange(this.SelectFiles(text2, recurseDirectories));
							}
						}
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (IOException)
			{
			}
			return list.AsReadOnly();
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00044700 File Offset: 0x00042900
		private bool Evaluate(ZipEntry entry)
		{
			return this._Criterion.Evaluate(entry);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0004471C File Offset: 0x0004291C
		public ICollection<ZipEntry> SelectEntries(ZipFile zip)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			foreach (ZipEntry current in zip)
			{
				if (this.Evaluate(current))
				{
					list.Add(current);
				}
			}
			return list;
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00044784 File Offset: 0x00042984
		public ICollection<ZipEntry> SelectEntries(ZipFile zip, string directoryPathInArchive)
		{
			if (zip == null)
			{
				throw new ArgumentNullException("zip");
			}
			List<ZipEntry> list = new List<ZipEntry>();
			string text = (directoryPathInArchive == null) ? null : directoryPathInArchive.Replace("/", "\\");
			if (text != null)
			{
				while (text.EndsWith("\\"))
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			foreach (ZipEntry current in zip)
			{
				if ((directoryPathInArchive == null || Path.GetDirectoryName(current.FileName) == directoryPathInArchive || Path.GetDirectoryName(current.FileName) == text) && this.Evaluate(current))
				{
					list.Add(current);
				}
			}
			return list;
		}

		// Token: 0x0400067C RID: 1660
		internal SelectionCriterion _Criterion;

		// Token: 0x02000138 RID: 312
		private enum ParseState
		{
			// Token: 0x0400067F RID: 1663
			Start,
			// Token: 0x04000680 RID: 1664
			OpenParen,
			// Token: 0x04000681 RID: 1665
			CriterionDone,
			// Token: 0x04000682 RID: 1666
			ConjunctionPending,
			// Token: 0x04000683 RID: 1667
			Whitespace
		}

		// Token: 0x02000139 RID: 313
		private static class RegexAssertions
		{
			// Token: 0x04000684 RID: 1668
			public static readonly string PrecededByOddNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*'[^']*)";

			// Token: 0x04000685 RID: 1669
			public static readonly string FollowedByOddNumberOfSingleQuotesAndLineEnd = "(?=[^']*'(?:[^']*'[^']*')*[^']*$)";

			// Token: 0x04000686 RID: 1670
			public static readonly string PrecededByEvenNumberOfSingleQuotes = "(?<=(?:[^']*'[^']*')*[^']*)";

			// Token: 0x04000687 RID: 1671
			public static readonly string FollowedByEvenNumberOfSingleQuotesAndLineEnd = "(?=(?:[^']*'[^']*')*[^']*$)";
		}
	}
}
