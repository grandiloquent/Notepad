
using System;

namespace  Shared
{
	 
	public static class Memory
	{
		public static int ReadMemoryInt(IntPtr hProcess,IntPtr address)
		{
			var buffer = new byte[4];
			int bytesRead=0;
			NativeMethods.ReadProcessMemory(hProcess, address, buffer,4, ref bytesRead);
			return BitConverter.ToInt32(buffer, 0);
			
			
		}
		public static int ScanSegments(int pid, byte[] pattern)
		{
			var hProcess = NativeMethods.OpenProcess(NativeMethods.PROCESS_QUERY_INFORMATION | NativeMethods.PROCESS_VM_READ, false, pid);
			
			var sysinfo = new NativeMethods. SYSTEM_INFO();
			NativeMethods.GetSystemInfo(ref sysinfo);
			var minAddress = sysinfo.lpMinimumApplicationAddress;
			var maxAddress = sysinfo.lpMaximumApplicationAddress;
			var min = (long)minAddress;
			var max = (long)maxAddress;
			// https://docs.microsoft.com/en-us/windows/desktop/api/winnt/ns-winnt-_memory_basic_information
			var memoryInfo = new NativeMethods.	MEMORY_BASIC_INFORMATION();
			
			int bytesRead = 0;
			while (min < max) {
				NativeMethods.VirtualQueryEx(hProcess, minAddress, out memoryInfo, 28);
				
				//https://docs.microsoft.com/zh-cn/windows/desktop/Memory/memory-protection-constants
				if (((memoryInfo.State & NativeMethods.MEM_COMMIT) != 0) && ((memoryInfo.Type & NativeMethods.MEM_PRIVATE) != 0) && ((memoryInfo.Protect & NativeMethods.PAGE_READWRITE) != 0)
				    && ((memoryInfo.Protect & NativeMethods.PAGE_GUARD) == 0)) {
					var buffer = new byte[memoryInfo.RegionSize.ToUInt32()];
					NativeMethods.ReadProcessMemory(hProcess, memoryInfo.BaseAddress, buffer,  (int)memoryInfo.RegionSize, ref bytesRead);
					
					for (int i = 0; i < memoryInfo.RegionSize.ToUInt32(); ++i) {
					
						if ((pattern[0] == buffer[i]) && ((i + pattern.Length) < memoryInfo.RegionSize.ToUInt32())) {
							bool bSkip = false;
							for (int j = 0; j < pattern.Length; j++) {
								if (pattern[j] != buffer[i + j]) {
									bSkip = true;
									break;
								}
							}
							if (!bSkip)
								return memoryInfo.BaseAddress.ToInt32() + i;
						}
					}
				}
				min += memoryInfo.RegionSize.ToUInt32();
				minAddress = new IntPtr(min);
			}
			return -1;
		
			
			
		}
	}
}
