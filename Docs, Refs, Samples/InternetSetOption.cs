// copied from http://pinvoke.net/default.aspx/wininet.InternetSetOption

public struct INTERNET_PROXY_INFO
{
     public int dwAccessType;
     public IntPtr proxy;
     public IntPtr proxyBypass;
};

[DllImport("wininet.dll", SetLastError = true)]
     private static extern bool InternetSetOption(IntPtr hInternet,
     int dwOption,
     IntPtr lpBuffer,
     int lpdwBufferLength);

private void RefreshIESettings(string strProxy)
{
     const int INTERNET_OPTION_PROXY = 38;
     const int INTERNET_OPEN_NO_PROXY = 1;  
     const int INTERNET_OPEN_TYPE_PROXY = 3;  

     INTERNET_PROXY_INFO proxyInfo;

     // Filling in structure
     proxyInfo.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
     proxyInfo.proxy = Marshal.StringToHGlobalAnsi(strProxy);
     proxyInfo.proxyBypass = Marshal.StringToHGlobalAnsi("local");

     // Allocating memory
     IntPtr intptrStruct = Marshal.AllocCoTaskMem(Marshal.SizeOf(proxyInfo));

     // Converting structure to IntPtr
     Marshal.StructureToPtr(proxyInfo, intptrStruct, true);

     bool iReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, Marshal.SizeOf(proxyInfo));
}  