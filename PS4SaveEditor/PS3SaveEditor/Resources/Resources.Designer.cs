using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PS3SaveEditor.Resources
{
	// Token: 0x0200001F RID: 31
	[CompilerGenerated, DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		// Token: 0x06000138 RID: 312 RVA: 0x0000BDBC File Offset: 0x00009FBC
		internal Resources()
		{
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000BDC4 File Offset: 0x00009FC4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("PS3SaveEditor.Resources.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000BE03 File Offset: 0x0000A003
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000BE0A File Offset: 0x0000A00A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000BE14 File Offset: 0x0000A014
		internal static Bitmap bg_company
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("bg_company", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000BE3C File Offset: 0x0000A03C
		internal static Bitmap blue
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("blue", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000BE64 File Offset: 0x0000A064
		internal static string btnApply
		{
			get
			{
				return Resources.ResourceManager.GetString("btnApply", Resources.resourceCulture);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600013F RID: 319 RVA: 0x0000BE7A File Offset: 0x0000A07A
		internal static string btnApplyDownload
		{
			get
			{
				return Resources.ResourceManager.GetString("btnApplyDownload", Resources.resourceCulture);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000BE90 File Offset: 0x0000A090
		internal static string btnApplyPatch
		{
			get
			{
				return Resources.ResourceManager.GetString("btnApplyPatch", Resources.resourceCulture);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000BEA6 File Offset: 0x0000A0A6
		internal static string btnBackup
		{
			get
			{
				return Resources.ResourceManager.GetString("btnBackup", Resources.resourceCulture);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000BEBC File Offset: 0x0000A0BC
		internal static string btnBrowse
		{
			get
			{
				return Resources.ResourceManager.GetString("btnBrowse", Resources.resourceCulture);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000BED2 File Offset: 0x0000A0D2
		internal static string btnCancel
		{
			get
			{
				return Resources.ResourceManager.GetString("btnCancel", Resources.resourceCulture);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
		internal static string btnCancellation
		{
			get
			{
				return Resources.ResourceManager.GetString("btnCancellation", Resources.resourceCulture);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000BEFE File Offset: 0x0000A0FE
		internal static string btnClose
		{
			get
			{
				return Resources.ResourceManager.GetString("btnClose", Resources.resourceCulture);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000BF14 File Offset: 0x0000A114
		internal static string btnCompare
		{
			get
			{
				return Resources.ResourceManager.GetString("btnCompare", Resources.resourceCulture);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000BF2A File Offset: 0x0000A12A
		internal static string btnDeactivate
		{
			get
			{
				return Resources.ResourceManager.GetString("btnDeactivate", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000BF40 File Offset: 0x0000A140
		internal static string btnFind
		{
			get
			{
				return Resources.ResourceManager.GetString("btnFind", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000BF56 File Offset: 0x0000A156
		internal static string btnFindPrev
		{
			get
			{
				return Resources.ResourceManager.GetString("btnFindPrev", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000BF6C File Offset: 0x0000A16C
		internal static string btnHelp
		{
			get
			{
				return Resources.ResourceManager.GetString("btnHelp", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000BF82 File Offset: 0x0000A182
		internal static string btnHome
		{
			get
			{
				return Resources.ResourceManager.GetString("btnHome", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000BF98 File Offset: 0x0000A198
		internal static string btnManageProfiles
		{
			get
			{
				return Resources.ResourceManager.GetString("btnManageProfiles", Resources.resourceCulture);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000BFAE File Offset: 0x0000A1AE
		internal static string btnNew
		{
			get
			{
				return Resources.ResourceManager.GetString("btnNew", Resources.resourceCulture);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600014E RID: 334 RVA: 0x0000BFC4 File Offset: 0x0000A1C4
		internal static string btnOK
		{
			get
			{
				return Resources.ResourceManager.GetString("btnOK", Resources.resourceCulture);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600014F RID: 335 RVA: 0x0000BFDA File Offset: 0x0000A1DA
		internal static string btnOpenFolder
		{
			get
			{
				return Resources.ResourceManager.GetString("btnOpenFolder", Resources.resourceCulture);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000150 RID: 336 RVA: 0x0000BFF0 File Offset: 0x0000A1F0
		internal static string btnOptions
		{
			get
			{
				return Resources.ResourceManager.GetString("btnOptions", Resources.resourceCulture);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000C006 File Offset: 0x0000A206
		internal static string btnPage1
		{
			get
			{
				return Resources.ResourceManager.GetString("btnPage1", Resources.resourceCulture);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000C01C File Offset: 0x0000A21C
		internal static string btnPage2
		{
			get
			{
				return Resources.ResourceManager.GetString("btnPage2", Resources.resourceCulture);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000C032 File Offset: 0x0000A232
		internal static string btnPop
		{
			get
			{
				return Resources.ResourceManager.GetString("btnPop", Resources.resourceCulture);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000154 RID: 340 RVA: 0x0000C048 File Offset: 0x0000A248
		internal static string btnPush
		{
			get
			{
				return Resources.ResourceManager.GetString("btnPush", Resources.resourceCulture);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000C05E File Offset: 0x0000A25E
		internal static string btnRestore
		{
			get
			{
				return Resources.ResourceManager.GetString("btnRestore", Resources.resourceCulture);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000C074 File Offset: 0x0000A274
		internal static string btnRss
		{
			get
			{
				return Resources.ResourceManager.GetString("btnRss", Resources.resourceCulture);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000C08A File Offset: 0x0000A28A
		internal static string btnSave
		{
			get
			{
				return Resources.ResourceManager.GetString("btnSave", Resources.resourceCulture);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000158 RID: 344 RVA: 0x0000C0A0 File Offset: 0x0000A2A0
		internal static string btnSaveCodes
		{
			get
			{
				return Resources.ResourceManager.GetString("btnSaveCodes", Resources.resourceCulture);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000C0B6 File Offset: 0x0000A2B6
		internal static string btnSaves
		{
			get
			{
				return Resources.ResourceManager.GetString("btnSaves", Resources.resourceCulture);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600015A RID: 346 RVA: 0x0000C0CC File Offset: 0x0000A2CC
		internal static string btnStack
		{
			get
			{
				return Resources.ResourceManager.GetString("btnStack", Resources.resourceCulture);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000C0E2 File Offset: 0x0000A2E2
		internal static string btnUpdate
		{
			get
			{
				return Resources.ResourceManager.GetString("btnUpdate", Resources.resourceCulture);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000C0F8 File Offset: 0x0000A2F8
		internal static string btnUserAccount
		{
			get
			{
				return Resources.ResourceManager.GetString("btnUserAccount", Resources.resourceCulture);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000C10E File Offset: 0x0000A30E
		internal static string btnViewAllCheats
		{
			get
			{
				return Resources.ResourceManager.GetString("btnViewAllCheats", Resources.resourceCulture);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000C124 File Offset: 0x0000A324
		internal static Bitmap check
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("check", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000C14C File Offset: 0x0000A34C
		internal static string chkBackupSaves
		{
			get
			{
				return Resources.ResourceManager.GetString("chkBackupSaves", Resources.resourceCulture);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000C162 File Offset: 0x0000A362
		internal static string chkEnableRight
		{
			get
			{
				return Resources.ResourceManager.GetString("chkEnableRight", Resources.resourceCulture);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000C178 File Offset: 0x0000A378
		internal static string chkSyncScroll
		{
			get
			{
				return Resources.ResourceManager.GetString("chkSyncScroll", Resources.resourceCulture);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000C18E File Offset: 0x0000A38E
		internal static string colBytes
		{
			get
			{
				return Resources.ResourceManager.GetString("colBytes", Resources.resourceCulture);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000C1A4 File Offset: 0x0000A3A4
		internal static string colCheats
		{
			get
			{
				return Resources.ResourceManager.GetString("colCheats", Resources.resourceCulture);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000C1BA File Offset: 0x0000A3BA
		internal static string colComment
		{
			get
			{
				return Resources.ResourceManager.GetString("colComment", Resources.resourceCulture);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000C1D0 File Offset: 0x0000A3D0
		internal static string colDefault
		{
			get
			{
				return Resources.ResourceManager.GetString("colDefault", Resources.resourceCulture);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000C1E6 File Offset: 0x0000A3E6
		internal static string colDesc
		{
			get
			{
				return Resources.ResourceManager.GetString("colDesc", Resources.resourceCulture);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		internal static string colEndAddr
		{
			get
			{
				return Resources.ResourceManager.GetString("colEndAddr", Resources.resourceCulture);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000168 RID: 360 RVA: 0x0000C212 File Offset: 0x0000A412
		internal static string colGameCode
		{
			get
			{
				return Resources.ResourceManager.GetString("colGameCode", Resources.resourceCulture);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000C228 File Offset: 0x0000A428
		internal static string colGameName
		{
			get
			{
				return Resources.ResourceManager.GetString("colGameName", Resources.resourceCulture);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000C23E File Offset: 0x0000A43E
		internal static string colName
		{
			get
			{
				return Resources.ResourceManager.GetString("colName", Resources.resourceCulture);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000C254 File Offset: 0x0000A454
		internal static string colProfile
		{
			get
			{
				return Resources.ResourceManager.GetString("colProfile", Resources.resourceCulture);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000C26A File Offset: 0x0000A46A
		internal static string colProfileName
		{
			get
			{
				return Resources.ResourceManager.GetString("colProfileName", Resources.resourceCulture);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000C280 File Offset: 0x0000A480
		internal static string colSelect
		{
			get
			{
				return Resources.ResourceManager.GetString("colSelect", Resources.resourceCulture);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000C296 File Offset: 0x0000A496
		internal static string colStartAddr
		{
			get
			{
				return Resources.ResourceManager.GetString("colStartAddr", Resources.resourceCulture);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		internal static string colVersion
		{
			get
			{
				return Resources.ResourceManager.GetString("colVersion", Resources.resourceCulture);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
		internal static Bitmap company
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("company", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000171 RID: 369 RVA: 0x0000C2EC File Offset: 0x0000A4EC
		internal static string errAnotherInstance
		{
			get
			{
				return Resources.ResourceManager.GetString("errAnotherInstance", Resources.resourceCulture);
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000C302 File Offset: 0x0000A502
		internal static string errCheatExists
		{
			get
			{
				return Resources.ResourceManager.GetString("errCheatExists", Resources.resourceCulture);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000C318 File Offset: 0x0000A518
		internal static string errConnection
		{
			get
			{
				return Resources.ResourceManager.GetString("errConnection", Resources.resourceCulture);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000C32E File Offset: 0x0000A52E
		internal static string errContactSupport
		{
			get
			{
				return Resources.ResourceManager.GetString("errContactSupport", Resources.resourceCulture);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000C344 File Offset: 0x0000A544
		internal static string errDelete
		{
			get
			{
				return Resources.ResourceManager.GetString("errDelete", Resources.resourceCulture);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000C35A File Offset: 0x0000A55A
		internal static string errDuplicateProfile
		{
			get
			{
				return Resources.ResourceManager.GetString("errDuplicateProfile", Resources.resourceCulture);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000C370 File Offset: 0x0000A570
		internal static string errExtract
		{
			get
			{
				return Resources.ResourceManager.GetString("errExtract", Resources.resourceCulture);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000C386 File Offset: 0x0000A586
		internal static string errIncorrectValue
		{
			get
			{
				return Resources.ResourceManager.GetString("errIncorrectValue", Resources.resourceCulture);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000C39C File Offset: 0x0000A59C
		internal static string errInternal
		{
			get
			{
				return Resources.ResourceManager.GetString("errInternal", Resources.resourceCulture);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000C3B2 File Offset: 0x0000A5B2
		internal static string errInvalidAddress
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidAddress", Resources.resourceCulture);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		internal static string errInvalidCode
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidCode", Resources.resourceCulture);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000C3DE File Offset: 0x0000A5DE
		internal static string errInvalidDesc
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidDesc", Resources.resourceCulture);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		internal static string errInvalidFCode
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidFCode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000C40A File Offset: 0x0000A60A
		internal static string errInvalidHex
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidHex", Resources.resourceCulture);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000C420 File Offset: 0x0000A620
		internal static string errInvalidHexCode
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidHexCode", Resources.resourceCulture);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000C436 File Offset: 0x0000A636
		internal static string errInvalidResponse
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidResponse", Resources.resourceCulture);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000C44C File Offset: 0x0000A64C
		internal static string errInvalidSave
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidSave", Resources.resourceCulture);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000C462 File Offset: 0x0000A662
		internal static string errInvalidSerial
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidSerial", Resources.resourceCulture);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000C478 File Offset: 0x0000A678
		internal static string errInvalidUSB
		{
			get
			{
				return Resources.ResourceManager.GetString("errInvalidUSB", Resources.resourceCulture);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000184 RID: 388 RVA: 0x0000C48E File Offset: 0x0000A68E
		internal static string errMaxCodes
		{
			get
			{
				return Resources.ResourceManager.GetString("errMaxCodes", Resources.resourceCulture);
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
		internal static string errMaxProfiles
		{
			get
			{
				return Resources.ResourceManager.GetString("errMaxProfiles", Resources.resourceCulture);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000C4BA File Offset: 0x0000A6BA
		internal static string errNoBackup
		{
			get
			{
				return Resources.ResourceManager.GetString("errNoBackup", Resources.resourceCulture);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000C4D0 File Offset: 0x0000A6D0
		internal static string errNoDefault
		{
			get
			{
				return Resources.ResourceManager.GetString("errNoDefault", Resources.resourceCulture);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000C4E6 File Offset: 0x0000A6E6
		internal static string errNoFile
		{
			get
			{
				return Resources.ResourceManager.GetString("errNoFile", Resources.resourceCulture);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000C4FC File Offset: 0x0000A6FC
		internal static string errNoSavedata
		{
			get
			{
				return Resources.ResourceManager.GetString("errNoSavedata", Resources.resourceCulture);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000C512 File Offset: 0x0000A712
		internal static string errNotRegistered
		{
			get
			{
				return Resources.ResourceManager.GetString("errNotRegistered", Resources.resourceCulture);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000C528 File Offset: 0x0000A728
		internal static string errOffline
		{
			get
			{
				return Resources.ResourceManager.GetString("errOffline", Resources.resourceCulture);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000C53E File Offset: 0x0000A73E
		internal static string errOneCheat
		{
			get
			{
				return Resources.ResourceManager.GetString("errOneCheat", Resources.resourceCulture);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000C554 File Offset: 0x0000A754
		internal static string errProfileExist
		{
			get
			{
				return Resources.ResourceManager.GetString("errProfileExist", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0000C56A File Offset: 0x0000A76A
		internal static string errProfileLock
		{
			get
			{
				return Resources.ResourceManager.GetString("errProfileLock", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000C580 File Offset: 0x0000A780
		internal static string errPSNNameUsed
		{
			get
			{
				return Resources.ResourceManager.GetString("errPSNNameUsed", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000C596 File Offset: 0x0000A796
		internal static string errSaveData
		{
			get
			{
				return Resources.ResourceManager.GetString("errSaveData", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000C5AC File Offset: 0x0000A7AC
		internal static string errSerial
		{
			get
			{
				return Resources.ResourceManager.GetString("errSerial", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000C5C2 File Offset: 0x0000A7C2
		internal static string errServer
		{
			get
			{
				return Resources.ResourceManager.GetString("errServer", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000C5D8 File Offset: 0x0000A7D8
		internal static string errServerConnection
		{
			get
			{
				return Resources.ResourceManager.GetString("errServerConnection", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000C5EE File Offset: 0x0000A7EE
		internal static string errTooManyTimes
		{
			get
			{
				return Resources.ResourceManager.GetString("errTooManyTimes", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000C604 File Offset: 0x0000A804
		internal static string errUpgrade
		{
			get
			{
				return Resources.ResourceManager.GetString("errUpgrade", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000C61A File Offset: 0x0000A81A
		internal static string gamelistDownloaderMsg
		{
			get
			{
				return Resources.ResourceManager.GetString("gamelistDownloaderMsg", Resources.resourceCulture);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000C630 File Offset: 0x0000A830
		internal static string gamelistDownloaderTitle
		{
			get
			{
				return Resources.ResourceManager.GetString("gamelistDownloaderTitle", Resources.resourceCulture);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000C646 File Offset: 0x0000A846
		internal static string gbBackupLocation
		{
			get
			{
				return Resources.ResourceManager.GetString("gbBackupLocation", Resources.resourceCulture);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000C65C File Offset: 0x0000A85C
		internal static Bitmap home_gamelist_off
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("home_gamelist_off", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000C684 File Offset: 0x0000A884
		internal static Bitmap home_gamelist_on
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("home_gamelist_on", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000C6AC File Offset: 0x0000A8AC
		internal static Bitmap home_help_off
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("home_help_off", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000C6D4 File Offset: 0x0000A8D4
		internal static Bitmap home_help_on
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("home_help_on", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000C6FC File Offset: 0x0000A8FC
		internal static Bitmap home_settings_off
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("home_settings_off", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000C724 File Offset: 0x0000A924
		internal static Bitmap home_settings_on
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("home_settings_on", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000C74C File Offset: 0x0000A94C
		internal static string itmDec
		{
			get
			{
				return Resources.ResourceManager.GetString("itmDec", Resources.resourceCulture);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000C762 File Offset: 0x0000A962
		internal static string itmHex
		{
			get
			{
				return Resources.ResourceManager.GetString("itmHex", Resources.resourceCulture);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000C778 File Offset: 0x0000A978
		internal static string lblAvailableGames
		{
			get
			{
				return Resources.ResourceManager.GetString("lblAvailableGames", Resources.resourceCulture);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000C78E File Offset: 0x0000A98E
		internal static string lblCheats
		{
			get
			{
				return Resources.ResourceManager.GetString("lblCheats", Resources.resourceCulture);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000C7A4 File Offset: 0x0000A9A4
		internal static string lblCheckVersion
		{
			get
			{
				return Resources.ResourceManager.GetString("lblCheckVersion", Resources.resourceCulture);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000C7BA File Offset: 0x0000A9BA
		internal static string lblCodes
		{
			get
			{
				return Resources.ResourceManager.GetString("lblCodes", Resources.resourceCulture);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
		internal static string lblComment
		{
			get
			{
				return Resources.ResourceManager.GetString("lblComment", Resources.resourceCulture);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000C7E6 File Offset: 0x0000A9E6
		internal static string lblDeactivate
		{
			get
			{
				return Resources.ResourceManager.GetString("lblDeactivate", Resources.resourceCulture);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000C7FC File Offset: 0x0000A9FC
		internal static string lblDeleteProfile
		{
			get
			{
				return Resources.ResourceManager.GetString("lblDeleteProfile", Resources.resourceCulture);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000C812 File Offset: 0x0000AA12
		internal static string lblDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("lblDescription", Resources.resourceCulture);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000C828 File Offset: 0x0000AA28
		internal static string lblDownloadStatus
		{
			get
			{
				return Resources.ResourceManager.GetString("lblDownloadStatus", Resources.resourceCulture);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000C83E File Offset: 0x0000AA3E
		internal static string lblDrive
		{
			get
			{
				return Resources.ResourceManager.GetString("lblDrive", Resources.resourceCulture);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000C854 File Offset: 0x0000AA54
		internal static string lblEnterLoc
		{
			get
			{
				return Resources.ResourceManager.GetString("lblEnterLoc", Resources.resourceCulture);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000C86A File Offset: 0x0000AA6A
		internal static string lblEnterSerial
		{
			get
			{
				return Resources.ResourceManager.GetString("lblEnterSerial", Resources.resourceCulture);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000C880 File Offset: 0x0000AA80
		internal static string lblInstruction
		{
			get
			{
				return Resources.ResourceManager.GetString("lblInstruction", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000C896 File Offset: 0x0000AA96
		internal static string lblInstruction_2
		{
			get
			{
				return Resources.ResourceManager.GetString("lblInstruction_2", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000C8AC File Offset: 0x0000AAAC
		internal static string lblInstruction1
		{
			get
			{
				return Resources.ResourceManager.GetString("lblInstruction1", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000C8C2 File Offset: 0x0000AAC2
		internal static string lblInstruction1Red
		{
			get
			{
				return Resources.ResourceManager.GetString("lblInstruction1Red", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		internal static string lblInstruction2
		{
			get
			{
				return Resources.ResourceManager.GetString("lblInstruction2", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000C8EE File Offset: 0x0000AAEE
		internal static string lblInstruction3
		{
			get
			{
				return Resources.ResourceManager.GetString("lblInstruction3", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000C904 File Offset: 0x0000AB04
		internal static string lblInstructionPage2
		{
			get
			{
				return Resources.ResourceManager.GetString("lblInstructionPage2", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000C91A File Offset: 0x0000AB1A
		internal static string lblInstructionsPage1
		{
			get
			{
				return Resources.ResourceManager.GetString("lblInstructionsPage1", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000C930 File Offset: 0x0000AB30
		internal static string lblInstuctionPage3
		{
			get
			{
				return Resources.ResourceManager.GetString("lblInstuctionPage3", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000C946 File Offset: 0x0000AB46
		internal static string lblManageProfiles
		{
			get
			{
				return Resources.ResourceManager.GetString("lblManageProfiles", Resources.resourceCulture);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000C95C File Offset: 0x0000AB5C
		internal static string lblNoCheats
		{
			get
			{
				return Resources.ResourceManager.GetString("lblNoCheats", Resources.resourceCulture);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000C972 File Offset: 0x0000AB72
		internal static string lblNoSaves
		{
			get
			{
				return Resources.ResourceManager.GetString("lblNoSaves", Resources.resourceCulture);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000C988 File Offset: 0x0000AB88
		internal static string lblOffset
		{
			get
			{
				return Resources.ResourceManager.GetString("lblOffset", Resources.resourceCulture);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000C99E File Offset: 0x0000AB9E
		internal static string lblPage1
		{
			get
			{
				return Resources.ResourceManager.GetString("lblPage1", Resources.resourceCulture);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000C9B4 File Offset: 0x0000ABB4
		internal static string lblPage2
		{
			get
			{
				return Resources.ResourceManager.GetString("lblPage2", Resources.resourceCulture);
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000C9CA File Offset: 0x0000ABCA
		internal static string lblPage21
		{
			get
			{
				return Resources.ResourceManager.GetString("lblPage21", Resources.resourceCulture);
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000C9E0 File Offset: 0x0000ABE0
		internal static string lblProfile
		{
			get
			{
				return Resources.ResourceManager.GetString("lblProfile", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000C9F6 File Offset: 0x0000ABF6
		internal static string lblPSNAddTitle
		{
			get
			{
				return Resources.ResourceManager.GetString("lblPSNAddTitle", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000CA0C File Offset: 0x0000AC0C
		internal static string lblRenameProfile
		{
			get
			{
				return Resources.ResourceManager.GetString("lblRenameProfile", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000CA22 File Offset: 0x0000AC22
		internal static string lblRestoring
		{
			get
			{
				return Resources.ResourceManager.GetString("lblRestoring", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000CA38 File Offset: 0x0000AC38
		internal static string lblRSSSection
		{
			get
			{
				return Resources.ResourceManager.GetString("lblRSSSection", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000CA4E File Offset: 0x0000AC4E
		internal static string lblSelectDrive
		{
			get
			{
				return Resources.ResourceManager.GetString("lblSelectDrive", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000CA64 File Offset: 0x0000AC64
		internal static string lblSelectFolder
		{
			get
			{
				return Resources.ResourceManager.GetString("lblSelectFolder", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000CA7A File Offset: 0x0000AC7A
		internal static string lblSerialWait
		{
			get
			{
				return Resources.ResourceManager.GetString("lblSerialWait", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000CA90 File Offset: 0x0000AC90
		internal static string lblUnregistered
		{
			get
			{
				return Resources.ResourceManager.GetString("lblUnregistered", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000CAA6 File Offset: 0x0000ACA6
		internal static string lblUpgrade
		{
			get
			{
				return Resources.ResourceManager.GetString("lblUpgrade", Resources.resourceCulture);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000CABC File Offset: 0x0000ACBC
		internal static string lblUserAccount
		{
			get
			{
				return Resources.ResourceManager.GetString("lblUserAccount", Resources.resourceCulture);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000CAD2 File Offset: 0x0000ACD2
		internal static string lblUserName
		{
			get
			{
				return Resources.ResourceManager.GetString("lblUserName", Resources.resourceCulture);
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000CAE8 File Offset: 0x0000ACE8
		internal static Bitmap logo1
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("logo1", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000CB10 File Offset: 0x0000AD10
		internal static string mainTitle
		{
			get
			{
				return Resources.ResourceManager.GetString("mainTitle", Resources.resourceCulture);
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000CB26 File Offset: 0x0000AD26
		internal static string mnuAddCheatCode
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuAddCheatCode", Resources.resourceCulture);
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		internal static string mnuAdvanced
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuAdvanced", Resources.resourceCulture);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000CB52 File Offset: 0x0000AD52
		internal static string mnuDelete
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuDelete", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000CB68 File Offset: 0x0000AD68
		internal static string mnuDeleteCheatCode
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuDeleteCheatCode", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000CB7E File Offset: 0x0000AD7E
		internal static string mnuDeleteSave
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuDeleteSave", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000CB94 File Offset: 0x0000AD94
		internal static string mnuEditCheatCode
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuEditCheatCode", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000CBAA File Offset: 0x0000ADAA
		internal static string mnuExtractProfile
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuExtractProfile", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000CBC0 File Offset: 0x0000ADC0
		internal static string mnuRegisterPSN
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuRegisterPSN", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000CBD6 File Offset: 0x0000ADD6
		internal static string mnuRenameProfile
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuRenameProfile", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000CBEC File Offset: 0x0000ADEC
		internal static string mnuResign
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuResign", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000CC02 File Offset: 0x0000AE02
		internal static string mnuRestore
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuRestore", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000CC18 File Offset: 0x0000AE18
		internal static string mnuSimple
		{
			get
			{
				return Resources.ResourceManager.GetString("mnuSimple", Resources.resourceCulture);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000CC2E File Offset: 0x0000AE2E
		internal static string msgAdvModeFinish
		{
			get
			{
				return Resources.ResourceManager.GetString("msgAdvModeFinish", Resources.resourceCulture);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000CC44 File Offset: 0x0000AE44
		internal static string msgChooseCache
		{
			get
			{
				return Resources.ResourceManager.GetString("msgChooseCache", Resources.resourceCulture);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000CC5A File Offset: 0x0000AE5A
		internal static string msgConfirmBackup
		{
			get
			{
				return Resources.ResourceManager.GetString("msgConfirmBackup", Resources.resourceCulture);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000CC70 File Offset: 0x0000AE70
		internal static string msgConfirmCode
		{
			get
			{
				return Resources.ResourceManager.GetString("msgConfirmCode", Resources.resourceCulture);
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000CC86 File Offset: 0x0000AE86
		internal static string msgConfirmDeactivateAccount
		{
			get
			{
				return Resources.ResourceManager.GetString("msgConfirmDeactivateAccount", Resources.resourceCulture);
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000CC9C File Offset: 0x0000AE9C
		internal static string msgConfirmDelete
		{
			get
			{
				return Resources.ResourceManager.GetString("msgConfirmDelete", Resources.resourceCulture);
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000CCB2 File Offset: 0x0000AEB2
		internal static string msgConfirmDeleteSave
		{
			get
			{
				return Resources.ResourceManager.GetString("msgConfirmDeleteSave", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000CCC8 File Offset: 0x0000AEC8
		internal static string msgConfirmRestore
		{
			get
			{
				return Resources.ResourceManager.GetString("msgConfirmRestore", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000CCDE File Offset: 0x0000AEDE
		internal static string msgConnecting
		{
			get
			{
				return Resources.ResourceManager.GetString("msgConnecting", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000CCF4 File Offset: 0x0000AEF4
		internal static string msgDeactivate
		{
			get
			{
				return Resources.ResourceManager.GetString("msgDeactivate", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000CD0A File Offset: 0x0000AF0A
		internal static string msgDeactivated
		{
			get
			{
				return Resources.ResourceManager.GetString("msgDeactivated", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000CD20 File Offset: 0x0000AF20
		internal static string msgDownloadDec
		{
			get
			{
				return Resources.ResourceManager.GetString("msgDownloadDec", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000CD36 File Offset: 0x0000AF36
		internal static string msgDownloadEnc
		{
			get
			{
				return Resources.ResourceManager.GetString("msgDownloadEnc", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000CD4C File Offset: 0x0000AF4C
		internal static string msgDownloadingList
		{
			get
			{
				return Resources.ResourceManager.GetString("msgDownloadingList", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000CD62 File Offset: 0x0000AF62
		internal static string msgDownloadPatch
		{
			get
			{
				return Resources.ResourceManager.GetString("msgDownloadPatch", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000CD78 File Offset: 0x0000AF78
		internal static string msgError
		{
			get
			{
				return Resources.ResourceManager.GetString("msgError", Resources.resourceCulture);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000CD8E File Offset: 0x0000AF8E
		internal static string msgInfo
		{
			get
			{
				return Resources.ResourceManager.GetString("msgInfo", Resources.resourceCulture);
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000CDA4 File Offset: 0x0000AFA4
		internal static string msgMajor
		{
			get
			{
				return Resources.ResourceManager.GetString("msgMajor", Resources.resourceCulture);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000CDBA File Offset: 0x0000AFBA
		internal static string msgMaxCheats
		{
			get
			{
				return Resources.ResourceManager.GetString("msgMaxCheats", Resources.resourceCulture);
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000CDD0 File Offset: 0x0000AFD0
		internal static string msgMinor
		{
			get
			{
				return Resources.ResourceManager.GetString("msgMinor", Resources.resourceCulture);
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000CDE6 File Offset: 0x0000AFE6
		internal static string msgNewVersion
		{
			get
			{
				return Resources.ResourceManager.GetString("msgNewVersion", Resources.resourceCulture);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000CDFC File Offset: 0x0000AFFC
		internal static string msgNoCheats
		{
			get
			{
				return Resources.ResourceManager.GetString("msgNoCheats", Resources.resourceCulture);
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000CE12 File Offset: 0x0000B012
		internal static string msgNoPSNFolder
		{
			get
			{
				return Resources.ResourceManager.GetString("msgNoPSNFolder", Resources.resourceCulture);
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000CE28 File Offset: 0x0000B028
		internal static string msgNoupdate
		{
			get
			{
				return Resources.ResourceManager.GetString("msgNoupdate", Resources.resourceCulture);
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000CE3E File Offset: 0x0000B03E
		internal static string msgQuickModeFinish
		{
			get
			{
				return Resources.ResourceManager.GetString("msgQuickModeFinish", Resources.resourceCulture);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000CE54 File Offset: 0x0000B054
		internal static string msgRestored
		{
			get
			{
				return Resources.ResourceManager.GetString("msgRestored", Resources.resourceCulture);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000CE6A File Offset: 0x0000B06A
		internal static string msgSelectCheat
		{
			get
			{
				return Resources.ResourceManager.GetString("msgSelectCheat", Resources.resourceCulture);
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000CE80 File Offset: 0x0000B080
		internal static string msgUnsupported
		{
			get
			{
				return Resources.ResourceManager.GetString("msgUnsupported", Resources.resourceCulture);
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000CE96 File Offset: 0x0000B096
		internal static string msgUploadDec
		{
			get
			{
				return Resources.ResourceManager.GetString("msgUploadDec", Resources.resourceCulture);
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000CEAC File Offset: 0x0000B0AC
		internal static string msgUploadEnc
		{
			get
			{
				return Resources.ResourceManager.GetString("msgUploadEnc", Resources.resourceCulture);
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000CEC2 File Offset: 0x0000B0C2
		internal static string msgUploadPatch
		{
			get
			{
				return Resources.ResourceManager.GetString("msgUploadPatch", Resources.resourceCulture);
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000CED8 File Offset: 0x0000B0D8
		internal static string msgWait
		{
			get
			{
				return Resources.ResourceManager.GetString("msgWait", Resources.resourceCulture);
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000CEEE File Offset: 0x0000B0EE
		internal static string msgWaitSerial
		{
			get
			{
				return Resources.ResourceManager.GetString("msgWaitSerial", Resources.resourceCulture);
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000CF04 File Offset: 0x0000B104
		internal static Icon ps3se
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("ps3se", Resources.resourceCulture);
				return (Icon)@object;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000CF2C File Offset: 0x0000B12C
		internal static Bitmap red
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("red", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000CF54 File Offset: 0x0000B154
		internal static string rssTitle
		{
			get
			{
				return Resources.ResourceManager.GetString("rssTitle", Resources.resourceCulture);
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000CF6C File Offset: 0x0000B16C
		internal static Bitmap sel_drive
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("sel_drive", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000CF94 File Offset: 0x0000B194
		internal static Bitmap splash
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("splash", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000CFBC File Offset: 0x0000B1BC
		internal static string title
		{
			get
			{
				return Resources.ResourceManager.GetString("title", Resources.resourceCulture);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000CFD2 File Offset: 0x0000B1D2
		internal static string title0
		{
			get
			{
				return Resources.ResourceManager.GetString("title0", Resources.resourceCulture);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000CFE8 File Offset: 0x0000B1E8
		internal static string titleAdvDownloader
		{
			get
			{
				return Resources.ResourceManager.GetString("titleAdvDownloader", Resources.resourceCulture);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000CFFE File Offset: 0x0000B1FE
		internal static string titleAdvEdit
		{
			get
			{
				return Resources.ResourceManager.GetString("titleAdvEdit", Resources.resourceCulture);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000D014 File Offset: 0x0000B214
		internal static string titleBackupDetails
		{
			get
			{
				return Resources.ResourceManager.GetString("titleBackupDetails", Resources.resourceCulture);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000D02A File Offset: 0x0000B22A
		internal static string titleCancelAccount
		{
			get
			{
				return Resources.ResourceManager.GetString("titleCancelAccount", Resources.resourceCulture);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000D040 File Offset: 0x0000B240
		internal static string titleChooseBackup
		{
			get
			{
				return Resources.ResourceManager.GetString("titleChooseBackup", Resources.resourceCulture);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000D056 File Offset: 0x0000B256
		internal static string titleCodeEntry
		{
			get
			{
				return Resources.ResourceManager.GetString("titleCodeEntry", Resources.resourceCulture);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000D06C File Offset: 0x0000B26C
		internal static string titleDiffResults
		{
			get
			{
				return Resources.ResourceManager.GetString("titleDiffResults", Resources.resourceCulture);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000D082 File Offset: 0x0000B282
		internal static string titleGoto
		{
			get
			{
				return Resources.ResourceManager.GetString("titleGoto", Resources.resourceCulture);
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000D098 File Offset: 0x0000B298
		internal static string titleManageProfiles
		{
			get
			{
				return Resources.ResourceManager.GetString("titleManageProfiles", Resources.resourceCulture);
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000D0AE File Offset: 0x0000B2AE
		internal static string titlePSNAdd
		{
			get
			{
				return Resources.ResourceManager.GetString("titlePSNAdd", Resources.resourceCulture);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000D0C4 File Offset: 0x0000B2C4
		internal static string titleResign
		{
			get
			{
				return Resources.ResourceManager.GetString("titleResign", Resources.resourceCulture);
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000D0DA File Offset: 0x0000B2DA
		internal static string titleSerialEntry
		{
			get
			{
				return Resources.ResourceManager.GetString("titleSerialEntry", Resources.resourceCulture);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000D0F0 File Offset: 0x0000B2F0
		internal static string titleSimpleEdit
		{
			get
			{
				return Resources.ResourceManager.GetString("titleSimpleEdit", Resources.resourceCulture);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000D106 File Offset: 0x0000B306
		internal static string titleSimpleEditUploader
		{
			get
			{
				return Resources.ResourceManager.GetString("titleSimpleEditUploader", Resources.resourceCulture);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000D11C File Offset: 0x0000B31C
		internal static string titleTrial
		{
			get
			{
				return Resources.ResourceManager.GetString("titleTrial", Resources.resourceCulture);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000D132 File Offset: 0x0000B332
		internal static string titleUpgrade
		{
			get
			{
				return Resources.ResourceManager.GetString("titleUpgrade", Resources.resourceCulture);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000D148 File Offset: 0x0000B348
		internal static string titleUpgrader
		{
			get
			{
				return Resources.ResourceManager.GetString("titleUpgrader", Resources.resourceCulture);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000D160 File Offset: 0x0000B360
		internal static Bitmap uncheck
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("uncheck", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000D188 File Offset: 0x0000B388
		internal static string warnDeleteCache
		{
			get
			{
				return Resources.ResourceManager.GetString("warnDeleteCache", Resources.resourceCulture);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000D19E File Offset: 0x0000B39E
		internal static string warnOverwrite
		{
			get
			{
				return Resources.ResourceManager.GetString("warnOverwrite", Resources.resourceCulture);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000D1B4 File Offset: 0x0000B3B4
		internal static string warnOverwriteAdv
		{
			get
			{
				return Resources.ResourceManager.GetString("warnOverwriteAdv", Resources.resourceCulture);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000D1CA File Offset: 0x0000B3CA
		internal static string warnRestore
		{
			get
			{
				return Resources.ResourceManager.GetString("warnRestore", Resources.resourceCulture);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		internal static string warnTitle
		{
			get
			{
				return Resources.ResourceManager.GetString("warnTitle", Resources.resourceCulture);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		internal static Bitmap yellow
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("yellow", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x040000BB RID: 187
		private static ResourceManager resourceMan;

		// Token: 0x040000BC RID: 188
		private static CultureInfo resourceCulture;
	}
}
