using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Win32.SafeHandles;

namespace microServiceBus.BizTalkReceiveeAdapter.Helper.Tools
{
    public class RegistryHelper
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegOpenKeyEx")]
        static extern int RegOpenKeyEx(IntPtr hKey, string subKey, uint options, int sam, out IntPtr phkResult);

        [Flags]
        public enum eRegWow64Options : int
        {
            None = 0x0000,
            KEY_WOW64_64KEY = 0x0100,
            KEY_WOW64_32KEY = 0x0200,
            // Add here any others needed, from the table of the previous chapter
        }

        [Flags]
        public enum eRegistryRights : int
        {
            ReadKey = 131097,
            WriteKey = 131078,
        }

        public static RegistryKey OpenSubKey(RegistryKey pParentKey, string pSubKeyName,
                                             bool pWriteable,
                                             eRegWow64Options pOptions)
        {
            if (pParentKey == null || GetRegistryKeyHandle(pParentKey).Equals(System.IntPtr.Zero))
                throw new System.Exception("OpenSubKey: Parent key is not open");

            eRegistryRights Rights = eRegistryRights.ReadKey;
            if (pWriteable)
                Rights = eRegistryRights.WriteKey;

            System.IntPtr SubKeyHandle;
            System.Int32 Result = RegOpenKeyEx(GetRegistryKeyHandle(pParentKey), pSubKeyName, 0,
                                              (int)Rights | (int)pOptions, out SubKeyHandle);
            if (Result != 0)
            {
                System.ComponentModel.Win32Exception W32ex =
                    new System.ComponentModel.Win32Exception();
                throw new System.Exception("OpenSubKey: Exception encountered opening key",
                    W32ex);
            }

            return PointerToRegistryKey(SubKeyHandle, pWriteable, false);
        }

        private static System.IntPtr GetRegistryKeyHandle(RegistryKey pRegisteryKey)
        {
            Type Type = Type.GetType("Microsoft.Win32.RegistryKey");
            FieldInfo Info = Type.GetField("hkey", BindingFlags.NonPublic | BindingFlags.Instance);

            SafeHandle Handle = (SafeHandle)Info.GetValue(pRegisteryKey);
            IntPtr RealHandle = Handle.DangerousGetHandle();

            return Handle.DangerousGetHandle();
        }

        private static RegistryKey PointerToRegistryKey(IntPtr hKey, bool pWritable,
            bool pOwnsHandle)
        {
            // Create a SafeHandles.SafeRegistryHandle from this pointer - this is a private class
            BindingFlags privateConstructors = BindingFlags.Instance | BindingFlags.NonPublic;
            Type safeRegistryHandleType = typeof(
                SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType(
                "Microsoft.Win32.SafeHandles.SafeRegistryHandle");

            Type[] safeRegistryHandleConstructorTypes = new Type[] { typeof(System.IntPtr),
        typeof(System.Boolean) };
            ConstructorInfo safeRegistryHandleConstructor =
                safeRegistryHandleType.GetConstructor(privateConstructors,
                null, safeRegistryHandleConstructorTypes, null);
            Object safeHandle = safeRegistryHandleConstructor.Invoke(new Object[] { hKey,
        pOwnsHandle });

            // Create a new Registry key using the private constructor using the
            // safeHandle - this should then behave like 
            // a .NET natively opened handle and disposed of correctly
            Type registryKeyType = typeof(Microsoft.Win32.RegistryKey);
            Type[] registryKeyConstructorTypes = new Type[] { safeRegistryHandleType,
        typeof(Boolean) };
            ConstructorInfo registryKeyConstructor =
                registryKeyType.GetConstructor(privateConstructors, null,
                registryKeyConstructorTypes, null);
            RegistryKey result = (RegistryKey)registryKeyConstructor.Invoke(new Object[] {
        safeHandle, pWritable });
            return result;
        }
    }
}
