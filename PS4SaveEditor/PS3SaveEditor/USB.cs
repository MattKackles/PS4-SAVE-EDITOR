using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

namespace PS3SaveEditor
{
	// Token: 0x02000029 RID: 41
	public class USB
	{
		// Token: 0x0600029F RID: 671 RVA: 0x0000FC14 File Offset: 0x0000DE14
		public static List<USB.USBDevice> GetConnectedDevices()
		{
			List<USB.USBDevice> list = new List<USB.USBDevice>();
			foreach (USB.USBController current in USB.GetHostControllers())
			{
				USB.ListHub(current.GetRootHub(), list);
			}
			return list;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000FC6C File Offset: 0x0000DE6C
		private static void ListHub(USB.USBHub Hub, List<USB.USBDevice> DevList)
		{
			foreach (USB.USBPort current in Hub.GetPorts())
			{
				if (current.IsHub)
				{
					USB.ListHub(current.GetHub(), DevList);
				}
				else if (current.IsDeviceConnected)
				{
					DevList.Add(current.GetDevice());
				}
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000FCDC File Offset: 0x0000DEDC
		public static USB.USBDevice FindDeviceByDriverKeyName(string DriverKeyName)
		{
			USB.USBDevice uSBDevice = null;
			foreach (USB.USBController current in USB.GetHostControllers())
			{
				USB.SearchHubDriverKeyName(current.GetRootHub(), ref uSBDevice, DriverKeyName);
				if (uSBDevice != null)
				{
					break;
				}
			}
			return uSBDevice;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000FD38 File Offset: 0x0000DF38
		private static void SearchHubDriverKeyName(USB.USBHub Hub, ref USB.USBDevice FoundDevice, string DriverKeyName)
		{
			foreach (USB.USBPort current in Hub.GetPorts())
			{
				if (current.IsHub)
				{
					USB.SearchHubDriverKeyName(current.GetHub(), ref FoundDevice, DriverKeyName);
				}
				else if (current.IsDeviceConnected)
				{
					USB.USBDevice device = current.GetDevice();
					if (device.DeviceDriverKey == DriverKeyName)
					{
						FoundDevice = device;
						break;
					}
				}
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000FDB8 File Offset: 0x0000DFB8
		public static USB.USBDevice FindDeviceByInstanceID(string InstanceID)
		{
			USB.USBDevice uSBDevice = null;
			foreach (USB.USBController current in USB.GetHostControllers())
			{
				USB.SearchHubInstanceID(current.GetRootHub(), ref uSBDevice, InstanceID);
				if (uSBDevice != null)
				{
					break;
				}
			}
			return uSBDevice;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000FE14 File Offset: 0x0000E014
		private static void SearchHubInstanceID(USB.USBHub Hub, ref USB.USBDevice FoundDevice, string InstanceID)
		{
			foreach (USB.USBPort current in Hub.GetPorts())
			{
				if (current.IsHub)
				{
					USB.SearchHubInstanceID(current.GetHub(), ref FoundDevice, InstanceID);
				}
				else if (current.IsDeviceConnected)
				{
					USB.USBDevice device = current.GetDevice();
					if (device.InstanceID == InstanceID)
					{
						FoundDevice = device;
						break;
					}
				}
			}
		}

		// Token: 0x060002A5 RID: 677
		[DllImport("setupapi.dll")]
		private static extern int CM_Get_Parent(out IntPtr pdnDevInst, int dnDevInst, int ulFlags);

		// Token: 0x060002A6 RID: 678
		[DllImport("setupapi.dll", CharSet = CharSet.Auto)]
		private static extern int CM_Get_Device_ID(IntPtr dnDevInst, IntPtr Buffer, int BufferLen, int ulFlags);

		// Token: 0x060002A7 RID: 679 RVA: 0x0000FE94 File Offset: 0x0000E094
		public static USB.USBDevice FindDriveLetter(string DriveLetter)
		{
			USB.USBDevice result = null;
			string text = "";
			int deviceNumber = USB.GetDeviceNumber("\\\\.\\" + DriveLetter.TrimEnd(new char[]
			{
				'\\'
			}));
			if (deviceNumber < 0)
			{
				return result;
			}
			Guid guid = new Guid("53f56307-b6bf-11d0-94f2-00a0c91efb8b");
			IntPtr deviceInfoSet = USB.SetupDiGetClassDevs(ref guid, 0, IntPtr.Zero, 18);
			if (deviceInfoSet.ToInt32() != -1)
			{
				int num = 0;
				USB.SP_DEVINFO_DATA sP_DEVINFO_DATA;
				int num3;
				while (true)
				{
					USB.SP_DEVICE_INTERFACE_DATA sP_DEVICE_INTERFACE_DATA = default(USB.SP_DEVICE_INTERFACE_DATA);
					sP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(sP_DEVICE_INTERFACE_DATA);
					bool flag = USB.SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref guid, num, ref sP_DEVICE_INTERFACE_DATA);
					if (flag)
					{
						sP_DEVINFO_DATA = default(USB.SP_DEVINFO_DATA);
						sP_DEVINFO_DATA.cbSize = Marshal.SizeOf(sP_DEVINFO_DATA);
						USB.SP_DEVICE_INTERFACE_DETAIL_DATA sP_DEVICE_INTERFACE_DETAIL_DATA = default(USB.SP_DEVICE_INTERFACE_DETAIL_DATA);
						sP_DEVICE_INTERFACE_DETAIL_DATA.cbSize = ((IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);
						int num2 = 0;
						num3 = 2048;
						if (USB.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref sP_DEVICE_INTERFACE_DATA, ref sP_DEVICE_INTERFACE_DETAIL_DATA, num3, ref num2, ref sP_DEVINFO_DATA) && USB.GetDeviceNumber(sP_DEVICE_INTERFACE_DETAIL_DATA.DevicePath) == deviceNumber)
						{
							break;
						}
					}
					num++;
					if (!flag)
					{
						goto IL_14B;
					}
				}
				IntPtr dnDevInst;
				USB.CM_Get_Parent(out dnDevInst, sP_DEVINFO_DATA.DevInst, 0);
				IntPtr intPtr = Marshal.AllocHGlobal(num3);
				USB.CM_Get_Device_ID(dnDevInst, intPtr, num3, 0);
				text = Marshal.PtrToStringAuto(intPtr);
				Marshal.FreeHGlobal(intPtr);
				IL_14B:
				USB.SetupDiDestroyDeviceInfoList(deviceInfoSet);
			}
			if (text.StartsWith("USB\\"))
			{
				result = USB.FindDeviceByInstanceID(text);
			}
			return result;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0001000C File Offset: 0x0000E20C
		private static int GetDeviceNumber(string DevicePath)
		{
			int result = -1;
			IntPtr intPtr = USB.CreateFile(DevicePath.TrimEnd(new char[]
			{
				'\\'
			}), 0, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
			if (intPtr.ToInt32() != -1)
			{
				int num = Marshal.SizeOf(default(USB.STORAGE_DEVICE_NUMBER));
				IntPtr intPtr2 = Marshal.AllocHGlobal(num);
				int num2;
				if (USB.DeviceIoControl(intPtr, 2953344, IntPtr.Zero, 0, intPtr2, num, out num2, IntPtr.Zero))
				{
					USB.STORAGE_DEVICE_NUMBER sTORAGE_DEVICE_NUMBER = (USB.STORAGE_DEVICE_NUMBER)Marshal.PtrToStructure(intPtr2, typeof(USB.STORAGE_DEVICE_NUMBER));
					result = (sTORAGE_DEVICE_NUMBER.DeviceType << 8) + sTORAGE_DEVICE_NUMBER.DeviceNumber;
				}
				Marshal.FreeHGlobal(intPtr2);
				USB.CloseHandle(intPtr);
			}
			return result;
		}

		// Token: 0x060002A9 RID: 681
		[DllImport("setupapi.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, int Enumerator, IntPtr hwndParent, int Flags);

		// Token: 0x060002AA RID: 682
		[DllImport("setupapi.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SetupDiGetClassDevs(int ClassGuid, string Enumerator, IntPtr hwndParent, int Flags);

		// Token: 0x060002AB RID: 683
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, int MemberIndex, ref USB.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

		// Token: 0x060002AC RID: 684
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref USB.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, ref USB.SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, ref USB.SP_DEVINFO_DATA DeviceInfoData);

		// Token: 0x060002AD RID: 685
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref USB.SP_DEVINFO_DATA DeviceInfoData, int iProperty, ref int PropertyRegDataType, IntPtr PropertyBuffer, int PropertyBufferSize, ref int RequiredSize);

		// Token: 0x060002AE RID: 686
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, int MemberIndex, ref USB.SP_DEVINFO_DATA DeviceInfoData);

		// Token: 0x060002AF RID: 687
		[DllImport("setupapi.dll", SetLastError = true)]
		private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		// Token: 0x060002B0 RID: 688
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet, ref USB.SP_DEVINFO_DATA DeviceInfoData, StringBuilder DeviceInstanceId, int DeviceInstanceIdSize, out int RequiredSize);

		// Token: 0x060002B1 RID: 689
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool DeviceIoControl(IntPtr hDevice, int dwIoControlCode, IntPtr lpInBuffer, int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);

		// Token: 0x060002B2 RID: 690
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x060002B3 RID: 691
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x060002B4 RID: 692 RVA: 0x000100C4 File Offset: 0x0000E2C4
		public static ReadOnlyCollection<USB.USBController> GetHostControllers()
		{
			List<USB.USBController> list = new List<USB.USBController>();
			Guid guid = new Guid("3abf6f2d-71c4-462a-8a92-1e6861e6af27");
			IntPtr deviceInfoSet = USB.SetupDiGetClassDevs(ref guid, 0, IntPtr.Zero, 18);
			if (deviceInfoSet.ToInt32() != -1)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(2048);
				int num = 0;
				bool flag;
				do
				{
					USB.USBController uSBController = new USB.USBController();
					uSBController.ControllerIndex = num;
					USB.SP_DEVICE_INTERFACE_DATA sP_DEVICE_INTERFACE_DATA = default(USB.SP_DEVICE_INTERFACE_DATA);
					sP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(sP_DEVICE_INTERFACE_DATA);
					flag = USB.SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref guid, num, ref sP_DEVICE_INTERFACE_DATA);
					if (flag)
					{
						USB.SP_DEVINFO_DATA sP_DEVINFO_DATA = default(USB.SP_DEVINFO_DATA);
						sP_DEVINFO_DATA.cbSize = Marshal.SizeOf(sP_DEVINFO_DATA);
						USB.SP_DEVICE_INTERFACE_DETAIL_DATA sP_DEVICE_INTERFACE_DETAIL_DATA = default(USB.SP_DEVICE_INTERFACE_DETAIL_DATA);
						sP_DEVICE_INTERFACE_DETAIL_DATA.cbSize = ((IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);
						int num2 = 0;
						int deviceInterfaceDetailDataSize = 2048;
						if (USB.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref sP_DEVICE_INTERFACE_DATA, ref sP_DEVICE_INTERFACE_DETAIL_DATA, deviceInterfaceDetailDataSize, ref num2, ref sP_DEVINFO_DATA))
						{
							uSBController.ControllerDevicePath = sP_DEVICE_INTERFACE_DETAIL_DATA.DevicePath;
							int num3 = 0;
							int num4 = 1;
							if (USB.SetupDiGetDeviceRegistryProperty(deviceInfoSet, ref sP_DEVINFO_DATA, 0, ref num4, intPtr, 2048, ref num3))
							{
								uSBController.ControllerDeviceDesc = Marshal.PtrToStringAuto(intPtr);
							}
							if (USB.SetupDiGetDeviceRegistryProperty(deviceInfoSet, ref sP_DEVINFO_DATA, 9, ref num4, intPtr, 2048, ref num3))
							{
								uSBController.ControllerDriverKeyName = Marshal.PtrToStringAuto(intPtr);
							}
						}
						list.Add(uSBController);
					}
					num++;
				}
				while (flag);
				Marshal.FreeHGlobal(intPtr);
				USB.SetupDiDestroyDeviceInfoList(deviceInfoSet);
			}
			return new ReadOnlyCollection<USB.USBController>(list);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00010234 File Offset: 0x0000E434
		private static string GetDescriptionByKeyName(string DriverKeyName)
		{
			string result = "";
			string enumerator = "USB";
			IntPtr deviceInfoSet = USB.SetupDiGetClassDevs(0, enumerator, IntPtr.Zero, 6);
			if (deviceInfoSet.ToInt32() != -1)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(2048);
				int num = 0;
				USB.SP_DEVINFO_DATA sP_DEVINFO_DATA;
				int num2;
				int num3;
				while (true)
				{
					sP_DEVINFO_DATA = default(USB.SP_DEVINFO_DATA);
					sP_DEVINFO_DATA.cbSize = Marshal.SizeOf(sP_DEVINFO_DATA);
					bool flag = USB.SetupDiEnumDeviceInfo(deviceInfoSet, num, ref sP_DEVINFO_DATA);
					if (flag)
					{
						num2 = 0;
						num3 = 1;
						string a = "";
						if (USB.SetupDiGetDeviceRegistryProperty(deviceInfoSet, ref sP_DEVINFO_DATA, 9, ref num3, intPtr, 2048, ref num2))
						{
							a = Marshal.PtrToStringAuto(intPtr);
						}
						if (a == DriverKeyName)
						{
							break;
						}
					}
					num++;
					if (!flag)
					{
						goto IL_C0;
					}
				}
				if (USB.SetupDiGetDeviceRegistryProperty(deviceInfoSet, ref sP_DEVINFO_DATA, 0, ref num3, intPtr, 2048, ref num2))
				{
					result = Marshal.PtrToStringAuto(intPtr);
				}
				IL_C0:
				Marshal.FreeHGlobal(intPtr);
				USB.SetupDiDestroyDeviceInfoList(deviceInfoSet);
			}
			return result;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00010310 File Offset: 0x0000E510
		private static string GetInstanceIDByKeyName(string DriverKeyName)
		{
			string result = "";
			string enumerator = "USB";
			IntPtr deviceInfoSet = USB.SetupDiGetClassDevs(0, enumerator, IntPtr.Zero, 6);
			if (deviceInfoSet.ToInt32() != -1)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(2048);
				int num = 0;
				USB.SP_DEVINFO_DATA sP_DEVINFO_DATA;
				int num2;
				while (true)
				{
					sP_DEVINFO_DATA = default(USB.SP_DEVINFO_DATA);
					sP_DEVINFO_DATA.cbSize = Marshal.SizeOf(sP_DEVINFO_DATA);
					bool flag = USB.SetupDiEnumDeviceInfo(deviceInfoSet, num, ref sP_DEVINFO_DATA);
					if (flag)
					{
						num2 = 0;
						int num3 = 1;
						string a = "";
						if (USB.SetupDiGetDeviceRegistryProperty(deviceInfoSet, ref sP_DEVINFO_DATA, 9, ref num3, intPtr, 2048, ref num2))
						{
							a = Marshal.PtrToStringAuto(intPtr);
						}
						if (a == DriverKeyName)
						{
							break;
						}
					}
					num++;
					if (!flag)
					{
						goto IL_CB;
					}
				}
				int num4 = 2048;
				StringBuilder stringBuilder = new StringBuilder(num4);
				USB.SetupDiGetDeviceInstanceId(deviceInfoSet, ref sP_DEVINFO_DATA, stringBuilder, num4, out num2);
				result = stringBuilder.ToString();
				IL_CB:
				Marshal.FreeHGlobal(intPtr);
				USB.SetupDiDestroyDeviceInfoList(deviceInfoSet);
			}
			return result;
		}

		// Token: 0x040000FB RID: 251
		private const int IOCTL_STORAGE_GET_DEVICE_NUMBER = 2953344;

		// Token: 0x040000FC RID: 252
		private const string GUID_DEVINTERFACE_DISK = "53f56307-b6bf-11d0-94f2-00a0c91efb8b";

		// Token: 0x040000FD RID: 253
		private const int GENERIC_WRITE = 1073741824;

		// Token: 0x040000FE RID: 254
		private const int FILE_SHARE_READ = 1;

		// Token: 0x040000FF RID: 255
		private const int FILE_SHARE_WRITE = 2;

		// Token: 0x04000100 RID: 256
		private const int OPEN_EXISTING = 3;

		// Token: 0x04000101 RID: 257
		private const int INVALID_HANDLE_VALUE = -1;

		// Token: 0x04000102 RID: 258
		private const int IOCTL_GET_HCD_DRIVERKEY_NAME = 2229284;

		// Token: 0x04000103 RID: 259
		private const int IOCTL_USB_GET_ROOT_HUB_NAME = 2229256;

		// Token: 0x04000104 RID: 260
		private const int IOCTL_USB_GET_NODE_INFORMATION = 2229256;

		// Token: 0x04000105 RID: 261
		private const int IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX = 2229320;

		// Token: 0x04000106 RID: 262
		private const int IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION = 2229264;

		// Token: 0x04000107 RID: 263
		private const int IOCTL_USB_GET_NODE_CONNECTION_NAME = 2229268;

		// Token: 0x04000108 RID: 264
		private const int IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME = 2229280;

		// Token: 0x04000109 RID: 265
		private const int USB_DEVICE_DESCRIPTOR_TYPE = 1;

		// Token: 0x0400010A RID: 266
		private const int USB_STRING_DESCRIPTOR_TYPE = 3;

		// Token: 0x0400010B RID: 267
		private const int BUFFER_SIZE = 2048;

		// Token: 0x0400010C RID: 268
		private const int MAXIMUM_USB_STRING_LENGTH = 255;

		// Token: 0x0400010D RID: 269
		private const string GUID_DEVINTERFACE_HUBCONTROLLER = "3abf6f2d-71c4-462a-8a92-1e6861e6af27";

		// Token: 0x0400010E RID: 270
		private const string REGSTR_KEY_USB = "USB";

		// Token: 0x0400010F RID: 271
		private const int DIGCF_PRESENT = 2;

		// Token: 0x04000110 RID: 272
		private const int DIGCF_ALLCLASSES = 4;

		// Token: 0x04000111 RID: 273
		private const int DIGCF_DEVICEINTERFACE = 16;

		// Token: 0x04000112 RID: 274
		private const int SPDRP_DRIVER = 9;

		// Token: 0x04000113 RID: 275
		private const int SPDRP_DEVICEDESC = 0;

		// Token: 0x04000114 RID: 276
		private const int REG_SZ = 1;

		// Token: 0x0200002A RID: 42
		private struct STORAGE_DEVICE_NUMBER
		{
			// Token: 0x04000115 RID: 277
			public int DeviceType;

			// Token: 0x04000116 RID: 278
			public int DeviceNumber;

			// Token: 0x04000117 RID: 279
			public int PartitionNumber;
		}

		// Token: 0x0200002B RID: 43
		private enum USB_HUB_NODE
		{
			// Token: 0x04000119 RID: 281
			UsbHub,
			// Token: 0x0400011A RID: 282
			UsbMIParent
		}

		// Token: 0x0200002C RID: 44
		private enum USB_CONNECTION_STATUS
		{
			// Token: 0x0400011C RID: 284
			NoDeviceConnected,
			// Token: 0x0400011D RID: 285
			DeviceConnected,
			// Token: 0x0400011E RID: 286
			DeviceFailedEnumeration,
			// Token: 0x0400011F RID: 287
			DeviceGeneralFailure,
			// Token: 0x04000120 RID: 288
			DeviceCausedOvercurrent,
			// Token: 0x04000121 RID: 289
			DeviceNotEnoughPower,
			// Token: 0x04000122 RID: 290
			DeviceNotEnoughBandwidth,
			// Token: 0x04000123 RID: 291
			DeviceHubNestedTooDeeply,
			// Token: 0x04000124 RID: 292
			DeviceInLegacyHub
		}

		// Token: 0x0200002D RID: 45
		private enum USB_DEVICE_SPEED : byte
		{
			// Token: 0x04000126 RID: 294
			UsbLowSpeed,
			// Token: 0x04000127 RID: 295
			UsbFullSpeed,
			// Token: 0x04000128 RID: 296
			UsbHighSpeed
		}

		// Token: 0x0200002E RID: 46
		private struct SP_DEVINFO_DATA
		{
			// Token: 0x04000129 RID: 297
			public int cbSize;

			// Token: 0x0400012A RID: 298
			public Guid ClassGuid;

			// Token: 0x0400012B RID: 299
			public int DevInst;

			// Token: 0x0400012C RID: 300
			public IntPtr Reserved;
		}

		// Token: 0x0200002F RID: 47
		private struct SP_DEVICE_INTERFACE_DATA
		{
			// Token: 0x0400012D RID: 301
			public int cbSize;

			// Token: 0x0400012E RID: 302
			public Guid InterfaceClassGuid;

			// Token: 0x0400012F RID: 303
			public int Flags;

			// Token: 0x04000130 RID: 304
			public IntPtr Reserved;
		}

		// Token: 0x02000030 RID: 48
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			// Token: 0x04000131 RID: 305
			public int cbSize;

			// Token: 0x04000132 RID: 306
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
			public string DevicePath;
		}

		// Token: 0x02000031 RID: 49
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct USB_HCD_DRIVERKEY_NAME
		{
			// Token: 0x04000133 RID: 307
			public int ActualLength;

			// Token: 0x04000134 RID: 308
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
			public string DriverKeyName;
		}

		// Token: 0x02000032 RID: 50
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct USB_ROOT_HUB_NAME
		{
			// Token: 0x04000135 RID: 309
			public int ActualLength;

			// Token: 0x04000136 RID: 310
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
			public string RootHubName;
		}

		// Token: 0x02000033 RID: 51
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct USB_HUB_DESCRIPTOR
		{
			// Token: 0x04000137 RID: 311
			public byte bDescriptorLength;

			// Token: 0x04000138 RID: 312
			public byte bDescriptorType;

			// Token: 0x04000139 RID: 313
			public byte bNumberOfPorts;

			// Token: 0x0400013A RID: 314
			public short wHubCharacteristics;

			// Token: 0x0400013B RID: 315
			public byte bPowerOnToPowerGood;

			// Token: 0x0400013C RID: 316
			public byte bHubControlCurrent;

			// Token: 0x0400013D RID: 317
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] bRemoveAndPowerMask;
		}

		// Token: 0x02000034 RID: 52
		private struct USB_HUB_INFORMATION
		{
			// Token: 0x0400013E RID: 318
			public USB.USB_HUB_DESCRIPTOR HubDescriptor;

			// Token: 0x0400013F RID: 319
			public byte HubIsBusPowered;
		}

		// Token: 0x02000035 RID: 53
		private struct USB_NODE_INFORMATION
		{
			// Token: 0x04000140 RID: 320
			public int NodeType;

			// Token: 0x04000141 RID: 321
			public USB.USB_HUB_INFORMATION HubInformation;
		}

		// Token: 0x02000036 RID: 54
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct USB_NODE_CONNECTION_INFORMATION_EX
		{
			// Token: 0x04000142 RID: 322
			public int ConnectionIndex;

			// Token: 0x04000143 RID: 323
			public USB.USB_DEVICE_DESCRIPTOR DeviceDescriptor;

			// Token: 0x04000144 RID: 324
			public byte CurrentConfigurationValue;

			// Token: 0x04000145 RID: 325
			public byte Speed;

			// Token: 0x04000146 RID: 326
			public byte DeviceIsHub;

			// Token: 0x04000147 RID: 327
			public short DeviceAddress;

			// Token: 0x04000148 RID: 328
			public int NumberOfOpenPipes;

			// Token: 0x04000149 RID: 329
			public int ConnectionStatus;
		}

		// Token: 0x02000037 RID: 55
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct USB_DEVICE_DESCRIPTOR
		{
			// Token: 0x0400014A RID: 330
			public byte bLength;

			// Token: 0x0400014B RID: 331
			public byte bDescriptorType;

			// Token: 0x0400014C RID: 332
			public short bcdUSB;

			// Token: 0x0400014D RID: 333
			public byte bDeviceClass;

			// Token: 0x0400014E RID: 334
			public byte bDeviceSubClass;

			// Token: 0x0400014F RID: 335
			public byte bDeviceProtocol;

			// Token: 0x04000150 RID: 336
			public byte bMaxPacketSize0;

			// Token: 0x04000151 RID: 337
			public short idVendor;

			// Token: 0x04000152 RID: 338
			public short idProduct;

			// Token: 0x04000153 RID: 339
			public short bcdDevice;

			// Token: 0x04000154 RID: 340
			public byte iManufacturer;

			// Token: 0x04000155 RID: 341
			public byte iProduct;

			// Token: 0x04000156 RID: 342
			public byte iSerialNumber;

			// Token: 0x04000157 RID: 343
			public byte bNumConfigurations;
		}

		// Token: 0x02000038 RID: 56
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct USB_STRING_DESCRIPTOR
		{
			// Token: 0x04000158 RID: 344
			public byte bLength;

			// Token: 0x04000159 RID: 345
			public byte bDescriptorType;

			// Token: 0x0400015A RID: 346
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
			public string bString;
		}

		// Token: 0x02000039 RID: 57
		private struct USB_SETUP_PACKET
		{
			// Token: 0x0400015B RID: 347
			public byte bmRequest;

			// Token: 0x0400015C RID: 348
			public byte bRequest;

			// Token: 0x0400015D RID: 349
			public short wValue;

			// Token: 0x0400015E RID: 350
			public short wIndex;

			// Token: 0x0400015F RID: 351
			public short wLength;
		}

		// Token: 0x0200003A RID: 58
		private struct USB_DESCRIPTOR_REQUEST
		{
			// Token: 0x04000160 RID: 352
			public int ConnectionIndex;

			// Token: 0x04000161 RID: 353
			public USB.USB_SETUP_PACKET SetupPacket;
		}

		// Token: 0x0200003B RID: 59
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct USB_NODE_CONNECTION_NAME
		{
			// Token: 0x04000162 RID: 354
			public int ConnectionIndex;

			// Token: 0x04000163 RID: 355
			public int ActualLength;

			// Token: 0x04000164 RID: 356
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
			public string NodeName;
		}

		// Token: 0x0200003C RID: 60
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct USB_NODE_CONNECTION_DRIVERKEY_NAME
		{
			// Token: 0x04000165 RID: 357
			public int ConnectionIndex;

			// Token: 0x04000166 RID: 358
			public int ActualLength;

			// Token: 0x04000167 RID: 359
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
			public string DriverKeyName;
		}

		// Token: 0x0200003D RID: 61
		public class USBController
		{
			// Token: 0x060002B8 RID: 696 RVA: 0x000103FE File Offset: 0x0000E5FE
			public USBController()
			{
				this.ControllerIndex = 0;
				this.ControllerDevicePath = "";
				this.ControllerDeviceDesc = "";
				this.ControllerDriverKeyName = "";
			}

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x060002B9 RID: 697 RVA: 0x0001042E File Offset: 0x0000E62E
			public int Index
			{
				get
				{
					return this.ControllerIndex;
				}
			}

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x060002BA RID: 698 RVA: 0x00010436 File Offset: 0x0000E636
			public string DevicePath
			{
				get
				{
					return this.ControllerDevicePath;
				}
			}

			// Token: 0x1700015F RID: 351
			// (get) Token: 0x060002BB RID: 699 RVA: 0x0001043E File Offset: 0x0000E63E
			public string DriverKeyName
			{
				get
				{
					return this.ControllerDriverKeyName;
				}
			}

			// Token: 0x17000160 RID: 352
			// (get) Token: 0x060002BC RID: 700 RVA: 0x00010446 File Offset: 0x0000E646
			public string Name
			{
				get
				{
					return this.ControllerDeviceDesc;
				}
			}

			// Token: 0x060002BD RID: 701 RVA: 0x00010450 File Offset: 0x0000E650
			public USB.USBHub GetRootHub()
			{
				USB.USBHub uSBHub = new USB.USBHub();
				uSBHub.HubIsRootHub = true;
				uSBHub.HubDeviceDesc = "Root Hub";
				IntPtr intPtr = USB.CreateFile(this.ControllerDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
				if (intPtr.ToInt32() != -1)
				{
					int num = Marshal.SizeOf(default(USB.USB_ROOT_HUB_NAME));
					IntPtr intPtr2 = Marshal.AllocHGlobal(num);
					int num2;
					if (USB.DeviceIoControl(intPtr, 2229256, intPtr2, num, intPtr2, num, out num2, IntPtr.Zero))
					{
						uSBHub.HubDevicePath = "\\\\.\\" + ((USB.USB_ROOT_HUB_NAME)Marshal.PtrToStructure(intPtr2, typeof(USB.USB_ROOT_HUB_NAME))).RootHubName;
					}
					IntPtr intPtr3 = USB.CreateFile(uSBHub.HubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
					if (intPtr3.ToInt32() != -1)
					{
						USB.USB_NODE_INFORMATION uSB_NODE_INFORMATION = default(USB.USB_NODE_INFORMATION);
						uSB_NODE_INFORMATION.NodeType = 0;
						num = Marshal.SizeOf(uSB_NODE_INFORMATION);
						IntPtr intPtr4 = Marshal.AllocHGlobal(num);
						Marshal.StructureToPtr(uSB_NODE_INFORMATION, intPtr4, true);
						if (USB.DeviceIoControl(intPtr3, 2229256, intPtr4, num, intPtr4, num, out num2, IntPtr.Zero))
						{
							uSB_NODE_INFORMATION = (USB.USB_NODE_INFORMATION)Marshal.PtrToStructure(intPtr4, typeof(USB.USB_NODE_INFORMATION));
							uSBHub.HubIsBusPowered = Convert.ToBoolean(uSB_NODE_INFORMATION.HubInformation.HubIsBusPowered);
							uSBHub.HubPortCount = (int)uSB_NODE_INFORMATION.HubInformation.HubDescriptor.bNumberOfPorts;
						}
						Marshal.FreeHGlobal(intPtr4);
						USB.CloseHandle(intPtr3);
					}
					Marshal.FreeHGlobal(intPtr2);
					USB.CloseHandle(intPtr);
				}
				return uSBHub;
			}

			// Token: 0x04000168 RID: 360
			internal int ControllerIndex;

			// Token: 0x04000169 RID: 361
			internal string ControllerDriverKeyName;

			// Token: 0x0400016A RID: 362
			internal string ControllerDevicePath;

			// Token: 0x0400016B RID: 363
			internal string ControllerDeviceDesc;
		}

		// Token: 0x0200003E RID: 62
		public class USBHub
		{
			// Token: 0x060002BE RID: 702 RVA: 0x000105EC File Offset: 0x0000E7EC
			public USBHub()
			{
				this.HubPortCount = 0;
				this.HubDevicePath = "";
				this.HubDeviceDesc = "";
				this.HubDriverKey = "";
				this.HubIsBusPowered = false;
				this.HubIsRootHub = false;
				this.HubManufacturer = "";
				this.HubProduct = "";
				this.HubSerialNumber = "";
				this.HubInstanceID = "";
			}

			// Token: 0x17000161 RID: 353
			// (get) Token: 0x060002BF RID: 703 RVA: 0x00010661 File Offset: 0x0000E861
			public int PortCount
			{
				get
				{
					return this.HubPortCount;
				}
			}

			// Token: 0x17000162 RID: 354
			// (get) Token: 0x060002C0 RID: 704 RVA: 0x00010669 File Offset: 0x0000E869
			public string DevicePath
			{
				get
				{
					return this.HubDevicePath;
				}
			}

			// Token: 0x17000163 RID: 355
			// (get) Token: 0x060002C1 RID: 705 RVA: 0x00010671 File Offset: 0x0000E871
			public string DriverKey
			{
				get
				{
					return this.HubDriverKey;
				}
			}

			// Token: 0x17000164 RID: 356
			// (get) Token: 0x060002C2 RID: 706 RVA: 0x00010679 File Offset: 0x0000E879
			public string Name
			{
				get
				{
					return this.HubDeviceDesc;
				}
			}

			// Token: 0x17000165 RID: 357
			// (get) Token: 0x060002C3 RID: 707 RVA: 0x00010681 File Offset: 0x0000E881
			public string InstanceID
			{
				get
				{
					return this.HubInstanceID;
				}
			}

			// Token: 0x17000166 RID: 358
			// (get) Token: 0x060002C4 RID: 708 RVA: 0x00010689 File Offset: 0x0000E889
			public bool IsBusPowered
			{
				get
				{
					return this.HubIsBusPowered;
				}
			}

			// Token: 0x17000167 RID: 359
			// (get) Token: 0x060002C5 RID: 709 RVA: 0x00010691 File Offset: 0x0000E891
			public bool IsRootHub
			{
				get
				{
					return this.HubIsRootHub;
				}
			}

			// Token: 0x17000168 RID: 360
			// (get) Token: 0x060002C6 RID: 710 RVA: 0x00010699 File Offset: 0x0000E899
			public string Manufacturer
			{
				get
				{
					return this.HubManufacturer;
				}
			}

			// Token: 0x17000169 RID: 361
			// (get) Token: 0x060002C7 RID: 711 RVA: 0x000106A1 File Offset: 0x0000E8A1
			public string Product
			{
				get
				{
					return this.HubProduct;
				}
			}

			// Token: 0x1700016A RID: 362
			// (get) Token: 0x060002C8 RID: 712 RVA: 0x000106A9 File Offset: 0x0000E8A9
			public string SerialNumber
			{
				get
				{
					return this.HubSerialNumber;
				}
			}

			// Token: 0x060002C9 RID: 713 RVA: 0x000106B4 File Offset: 0x0000E8B4
			public ReadOnlyCollection<USB.USBPort> GetPorts()
			{
				List<USB.USBPort> list = new List<USB.USBPort>();
				IntPtr intPtr = USB.CreateFile(this.HubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
				if (intPtr.ToInt32() != -1)
				{
					int num = Marshal.SizeOf(typeof(USB.USB_NODE_CONNECTION_INFORMATION_EX));
					IntPtr intPtr2 = Marshal.AllocHGlobal(num);
					for (int i = 1; i <= this.HubPortCount; i++)
					{
						Marshal.StructureToPtr(new USB.USB_NODE_CONNECTION_INFORMATION_EX
						{
							ConnectionIndex = i
						}, intPtr2, true);
						int num2;
						if (USB.DeviceIoControl(intPtr, 2229320, intPtr2, num, intPtr2, num, out num2, IntPtr.Zero))
						{
							USB.USB_NODE_CONNECTION_INFORMATION_EX uSB_NODE_CONNECTION_INFORMATION_EX = (USB.USB_NODE_CONNECTION_INFORMATION_EX)Marshal.PtrToStructure(intPtr2, typeof(USB.USB_NODE_CONNECTION_INFORMATION_EX));
							USB.USBPort uSBPort = new USB.USBPort();
							uSBPort.PortPortNumber = i;
							uSBPort.PortHubDevicePath = this.HubDevicePath;
							USB.USB_CONNECTION_STATUS connectionStatus = (USB.USB_CONNECTION_STATUS)uSB_NODE_CONNECTION_INFORMATION_EX.ConnectionStatus;
							uSBPort.PortStatus = connectionStatus.ToString();
							USB.USB_DEVICE_SPEED speed = (USB.USB_DEVICE_SPEED)uSB_NODE_CONNECTION_INFORMATION_EX.Speed;
							uSBPort.PortSpeed = speed.ToString();
							uSBPort.PortIsDeviceConnected = (uSB_NODE_CONNECTION_INFORMATION_EX.ConnectionStatus == 1);
							uSBPort.PortIsHub = Convert.ToBoolean(uSB_NODE_CONNECTION_INFORMATION_EX.DeviceIsHub);
							uSBPort.PortDeviceDescriptor = uSB_NODE_CONNECTION_INFORMATION_EX.DeviceDescriptor;
							list.Add(uSBPort);
						}
					}
					Marshal.FreeHGlobal(intPtr2);
					USB.CloseHandle(intPtr);
				}
				return new ReadOnlyCollection<USB.USBPort>(list);
			}

			// Token: 0x0400016C RID: 364
			internal int HubPortCount;

			// Token: 0x0400016D RID: 365
			internal string HubDriverKey;

			// Token: 0x0400016E RID: 366
			internal string HubDevicePath;

			// Token: 0x0400016F RID: 367
			internal string HubDeviceDesc;

			// Token: 0x04000170 RID: 368
			internal string HubManufacturer;

			// Token: 0x04000171 RID: 369
			internal string HubProduct;

			// Token: 0x04000172 RID: 370
			internal string HubSerialNumber;

			// Token: 0x04000173 RID: 371
			internal string HubInstanceID;

			// Token: 0x04000174 RID: 372
			internal bool HubIsBusPowered;

			// Token: 0x04000175 RID: 373
			internal bool HubIsRootHub;
		}

		// Token: 0x0200003F RID: 63
		public class USBPort
		{
			// Token: 0x060002CA RID: 714 RVA: 0x00010817 File Offset: 0x0000EA17
			public USBPort()
			{
				this.PortPortNumber = 0;
				this.PortStatus = "";
				this.PortHubDevicePath = "";
				this.PortSpeed = "";
				this.PortIsHub = false;
				this.PortIsDeviceConnected = false;
			}

			// Token: 0x1700016B RID: 363
			// (get) Token: 0x060002CB RID: 715 RVA: 0x00010855 File Offset: 0x0000EA55
			public int PortNumber
			{
				get
				{
					return this.PortPortNumber;
				}
			}

			// Token: 0x1700016C RID: 364
			// (get) Token: 0x060002CC RID: 716 RVA: 0x0001085D File Offset: 0x0000EA5D
			public string HubDevicePath
			{
				get
				{
					return this.PortHubDevicePath;
				}
			}

			// Token: 0x1700016D RID: 365
			// (get) Token: 0x060002CD RID: 717 RVA: 0x00010865 File Offset: 0x0000EA65
			public string Status
			{
				get
				{
					return this.PortStatus;
				}
			}

			// Token: 0x1700016E RID: 366
			// (get) Token: 0x060002CE RID: 718 RVA: 0x0001086D File Offset: 0x0000EA6D
			public string Speed
			{
				get
				{
					return this.PortSpeed;
				}
			}

			// Token: 0x1700016F RID: 367
			// (get) Token: 0x060002CF RID: 719 RVA: 0x00010875 File Offset: 0x0000EA75
			public bool IsHub
			{
				get
				{
					return this.PortIsHub;
				}
			}

			// Token: 0x17000170 RID: 368
			// (get) Token: 0x060002D0 RID: 720 RVA: 0x0001087D File Offset: 0x0000EA7D
			public bool IsDeviceConnected
			{
				get
				{
					return this.PortIsDeviceConnected;
				}
			}

			// Token: 0x060002D1 RID: 721 RVA: 0x00010888 File Offset: 0x0000EA88
			public USB.USBDevice GetDevice()
			{
				if (!this.PortIsDeviceConnected)
				{
					return null;
				}
				USB.USBDevice uSBDevice = new USB.USBDevice();
				uSBDevice.DevicePortNumber = this.PortPortNumber;
				uSBDevice.DeviceHubDevicePath = this.PortHubDevicePath;
				uSBDevice.DeviceDescriptor = this.PortDeviceDescriptor;
				IntPtr intPtr = USB.CreateFile(this.PortHubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
				if (intPtr.ToInt32() != -1)
				{
					int num = 2048;
					string s = new string('\0', 2048 / Marshal.SystemDefaultCharSize);
					int num2;
					if (this.PortDeviceDescriptor.iManufacturer > 0)
					{
						USB.USB_DESCRIPTOR_REQUEST uSB_DESCRIPTOR_REQUEST = default(USB.USB_DESCRIPTOR_REQUEST);
						uSB_DESCRIPTOR_REQUEST.ConnectionIndex = this.PortPortNumber;
						uSB_DESCRIPTOR_REQUEST.SetupPacket.wValue = (short)(768 + (int)this.PortDeviceDescriptor.iManufacturer);
						uSB_DESCRIPTOR_REQUEST.SetupPacket.wLength = (short)(num - Marshal.SizeOf(uSB_DESCRIPTOR_REQUEST));
						uSB_DESCRIPTOR_REQUEST.SetupPacket.wIndex = 1033;
						IntPtr intPtr2 = Marshal.StringToHGlobalAuto(s);
						Marshal.StructureToPtr(uSB_DESCRIPTOR_REQUEST, intPtr2, true);
						if (USB.DeviceIoControl(intPtr, 2229264, intPtr2, num, intPtr2, num, out num2, IntPtr.Zero))
						{
							IntPtr ptr = new IntPtr(intPtr2.ToInt32() + Marshal.SizeOf(uSB_DESCRIPTOR_REQUEST));
							uSBDevice.DeviceManufacturer = ((USB.USB_STRING_DESCRIPTOR)Marshal.PtrToStructure(ptr, typeof(USB.USB_STRING_DESCRIPTOR))).bString;
						}
						Marshal.FreeHGlobal(intPtr2);
					}
					if (this.PortDeviceDescriptor.iProduct > 0)
					{
						USB.USB_DESCRIPTOR_REQUEST uSB_DESCRIPTOR_REQUEST2 = default(USB.USB_DESCRIPTOR_REQUEST);
						uSB_DESCRIPTOR_REQUEST2.ConnectionIndex = this.PortPortNumber;
						uSB_DESCRIPTOR_REQUEST2.SetupPacket.wValue = (short)(768 + (int)this.PortDeviceDescriptor.iProduct);
						uSB_DESCRIPTOR_REQUEST2.SetupPacket.wLength = (short)(num - Marshal.SizeOf(uSB_DESCRIPTOR_REQUEST2));
						uSB_DESCRIPTOR_REQUEST2.SetupPacket.wIndex = 1033;
						IntPtr intPtr3 = Marshal.StringToHGlobalAuto(s);
						Marshal.StructureToPtr(uSB_DESCRIPTOR_REQUEST2, intPtr3, true);
						if (USB.DeviceIoControl(intPtr, 2229264, intPtr3, num, intPtr3, num, out num2, IntPtr.Zero))
						{
							IntPtr ptr2 = new IntPtr(intPtr3.ToInt32() + Marshal.SizeOf(uSB_DESCRIPTOR_REQUEST2));
							uSBDevice.DeviceProduct = ((USB.USB_STRING_DESCRIPTOR)Marshal.PtrToStructure(ptr2, typeof(USB.USB_STRING_DESCRIPTOR))).bString;
						}
						Marshal.FreeHGlobal(intPtr3);
					}
					if (this.PortDeviceDescriptor.iSerialNumber > 0)
					{
						USB.USB_DESCRIPTOR_REQUEST uSB_DESCRIPTOR_REQUEST3 = default(USB.USB_DESCRIPTOR_REQUEST);
						uSB_DESCRIPTOR_REQUEST3.ConnectionIndex = this.PortPortNumber;
						uSB_DESCRIPTOR_REQUEST3.SetupPacket.wValue = (short)(768 + (int)this.PortDeviceDescriptor.iSerialNumber);
						uSB_DESCRIPTOR_REQUEST3.SetupPacket.wLength = (short)(num - Marshal.SizeOf(uSB_DESCRIPTOR_REQUEST3));
						uSB_DESCRIPTOR_REQUEST3.SetupPacket.wIndex = 1033;
						IntPtr intPtr4 = Marshal.StringToHGlobalAuto(s);
						Marshal.StructureToPtr(uSB_DESCRIPTOR_REQUEST3, intPtr4, true);
						if (USB.DeviceIoControl(intPtr, 2229264, intPtr4, num, intPtr4, num, out num2, IntPtr.Zero))
						{
							IntPtr ptr3 = new IntPtr(intPtr4.ToInt32() + Marshal.SizeOf(uSB_DESCRIPTOR_REQUEST3));
							uSBDevice.DeviceSerialNumber = ((USB.USB_STRING_DESCRIPTOR)Marshal.PtrToStructure(ptr3, typeof(USB.USB_STRING_DESCRIPTOR))).bString;
						}
						Marshal.FreeHGlobal(intPtr4);
					}
					USB.USB_NODE_CONNECTION_DRIVERKEY_NAME uSB_NODE_CONNECTION_DRIVERKEY_NAME = default(USB.USB_NODE_CONNECTION_DRIVERKEY_NAME);
					uSB_NODE_CONNECTION_DRIVERKEY_NAME.ConnectionIndex = this.PortPortNumber;
					num = Marshal.SizeOf(uSB_NODE_CONNECTION_DRIVERKEY_NAME);
					IntPtr intPtr5 = Marshal.AllocHGlobal(num);
					Marshal.StructureToPtr(uSB_NODE_CONNECTION_DRIVERKEY_NAME, intPtr5, true);
					if (USB.DeviceIoControl(intPtr, 2229280, intPtr5, num, intPtr5, num, out num2, IntPtr.Zero))
					{
						uSBDevice.DeviceDriverKey = ((USB.USB_NODE_CONNECTION_DRIVERKEY_NAME)Marshal.PtrToStructure(intPtr5, typeof(USB.USB_NODE_CONNECTION_DRIVERKEY_NAME))).DriverKeyName;
						uSBDevice.DeviceName = USB.GetDescriptionByKeyName(uSBDevice.DeviceDriverKey);
						uSBDevice.DeviceInstanceID = USB.GetInstanceIDByKeyName(uSBDevice.DeviceDriverKey);
					}
					Marshal.FreeHGlobal(intPtr5);
					USB.CloseHandle(intPtr);
				}
				return uSBDevice;
			}

			// Token: 0x060002D2 RID: 722 RVA: 0x00010C74 File Offset: 0x0000EE74
			public USB.USBHub GetHub()
			{
				if (!this.PortIsHub)
				{
					return null;
				}
				USB.USBHub uSBHub = new USB.USBHub();
				uSBHub.HubIsRootHub = false;
				uSBHub.HubDeviceDesc = "External Hub";
				IntPtr intPtr = USB.CreateFile(this.PortHubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
				if (intPtr.ToInt32() != -1)
				{
					USB.USB_NODE_CONNECTION_NAME uSB_NODE_CONNECTION_NAME = default(USB.USB_NODE_CONNECTION_NAME);
					uSB_NODE_CONNECTION_NAME.ConnectionIndex = this.PortPortNumber;
					int num = Marshal.SizeOf(uSB_NODE_CONNECTION_NAME);
					IntPtr intPtr2 = Marshal.AllocHGlobal(num);
					Marshal.StructureToPtr(uSB_NODE_CONNECTION_NAME, intPtr2, true);
					int num2;
					if (USB.DeviceIoControl(intPtr, 2229268, intPtr2, num, intPtr2, num, out num2, IntPtr.Zero))
					{
						uSBHub.HubDevicePath = "\\\\.\\" + ((USB.USB_NODE_CONNECTION_NAME)Marshal.PtrToStructure(intPtr2, typeof(USB.USB_NODE_CONNECTION_NAME))).NodeName;
					}
					IntPtr intPtr3 = USB.CreateFile(uSBHub.HubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
					if (intPtr3.ToInt32() != -1)
					{
						USB.USB_NODE_INFORMATION uSB_NODE_INFORMATION = default(USB.USB_NODE_INFORMATION);
						uSB_NODE_INFORMATION.NodeType = 0;
						num = Marshal.SizeOf(uSB_NODE_INFORMATION);
						IntPtr intPtr4 = Marshal.AllocHGlobal(num);
						Marshal.StructureToPtr(uSB_NODE_INFORMATION, intPtr4, true);
						if (USB.DeviceIoControl(intPtr3, 2229256, intPtr4, num, intPtr4, num, out num2, IntPtr.Zero))
						{
							uSB_NODE_INFORMATION = (USB.USB_NODE_INFORMATION)Marshal.PtrToStructure(intPtr4, typeof(USB.USB_NODE_INFORMATION));
							uSBHub.HubIsBusPowered = Convert.ToBoolean(uSB_NODE_INFORMATION.HubInformation.HubIsBusPowered);
							uSBHub.HubPortCount = (int)uSB_NODE_INFORMATION.HubInformation.HubDescriptor.bNumberOfPorts;
						}
						Marshal.FreeHGlobal(intPtr4);
						USB.CloseHandle(intPtr3);
					}
					USB.USBDevice device = this.GetDevice();
					uSBHub.HubInstanceID = device.DeviceInstanceID;
					uSBHub.HubManufacturer = device.Manufacturer;
					uSBHub.HubProduct = device.Product;
					uSBHub.HubSerialNumber = device.SerialNumber;
					uSBHub.HubDriverKey = device.DriverKey;
					Marshal.FreeHGlobal(intPtr2);
					USB.CloseHandle(intPtr);
				}
				return uSBHub;
			}

			// Token: 0x04000176 RID: 374
			internal int PortPortNumber;

			// Token: 0x04000177 RID: 375
			internal string PortStatus;

			// Token: 0x04000178 RID: 376
			internal string PortHubDevicePath;

			// Token: 0x04000179 RID: 377
			internal string PortSpeed;

			// Token: 0x0400017A RID: 378
			internal bool PortIsHub;

			// Token: 0x0400017B RID: 379
			internal bool PortIsDeviceConnected;

			// Token: 0x0400017C RID: 380
			internal USB.USB_DEVICE_DESCRIPTOR PortDeviceDescriptor;
		}

		// Token: 0x02000040 RID: 64
		public class USBDevice
		{
			// Token: 0x060002D3 RID: 723 RVA: 0x00010E80 File Offset: 0x0000F080
			public USBDevice()
			{
				this.DevicePortNumber = 0;
				this.DeviceHubDevicePath = "";
				this.DeviceDriverKey = "";
				this.DeviceManufacturer = "";
				this.DeviceProduct = "Unknown USB Device";
				this.DeviceSerialNumber = "";
				this.DeviceName = "";
				this.DeviceInstanceID = "";
			}

			// Token: 0x17000171 RID: 369
			// (get) Token: 0x060002D4 RID: 724 RVA: 0x00010EE7 File Offset: 0x0000F0E7
			public int PortNumber
			{
				get
				{
					return this.DevicePortNumber;
				}
			}

			// Token: 0x17000172 RID: 370
			// (get) Token: 0x060002D5 RID: 725 RVA: 0x00010EEF File Offset: 0x0000F0EF
			public string HubDevicePath
			{
				get
				{
					return this.DeviceHubDevicePath;
				}
			}

			// Token: 0x17000173 RID: 371
			// (get) Token: 0x060002D6 RID: 726 RVA: 0x00010EF7 File Offset: 0x0000F0F7
			public string DriverKey
			{
				get
				{
					return this.DeviceDriverKey;
				}
			}

			// Token: 0x17000174 RID: 372
			// (get) Token: 0x060002D7 RID: 727 RVA: 0x00010EFF File Offset: 0x0000F0FF
			public string InstanceID
			{
				get
				{
					return this.DeviceInstanceID;
				}
			}

			// Token: 0x17000175 RID: 373
			// (get) Token: 0x060002D8 RID: 728 RVA: 0x00010F07 File Offset: 0x0000F107
			public string Name
			{
				get
				{
					return this.DeviceName;
				}
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x060002D9 RID: 729 RVA: 0x00010F0F File Offset: 0x0000F10F
			public string Manufacturer
			{
				get
				{
					return this.DeviceManufacturer;
				}
			}

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x060002DA RID: 730 RVA: 0x00010F17 File Offset: 0x0000F117
			public string Product
			{
				get
				{
					return this.DeviceProduct;
				}
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x060002DB RID: 731 RVA: 0x00010F1F File Offset: 0x0000F11F
			public string SerialNumber
			{
				get
				{
					return this.DeviceSerialNumber;
				}
			}

			// Token: 0x0400017D RID: 381
			internal int DevicePortNumber;

			// Token: 0x0400017E RID: 382
			internal string DeviceDriverKey;

			// Token: 0x0400017F RID: 383
			internal string DeviceHubDevicePath;

			// Token: 0x04000180 RID: 384
			internal string DeviceInstanceID;

			// Token: 0x04000181 RID: 385
			internal string DeviceName;

			// Token: 0x04000182 RID: 386
			internal string DeviceManufacturer;

			// Token: 0x04000183 RID: 387
			internal string DeviceProduct;

			// Token: 0x04000184 RID: 388
			internal string DeviceSerialNumber;

			// Token: 0x04000185 RID: 389
			internal USB.USB_DEVICE_DESCRIPTOR DeviceDescriptor;
		}
	}
}
