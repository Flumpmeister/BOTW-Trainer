using Microsoft.Win32;

namespace BotwTrainer
{
   public class DotNetInfo
   {
      public NetVersion GetFromRegistry()
      {
         var netVersion = new NetVersion();

         const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
         using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
         {
            if (ndpKey != null && ndpKey.GetValue("Release") != null)
            {
               netVersion.Details = ".NET Framework Version: " + CheckVersion((int)ndpKey.GetValue("Release"));
               netVersion.Version = CheckVersion((int)ndpKey.GetValue("Release"));
               netVersion.ReleaseKey = (int)ndpKey.GetValue("Release");
            }
            else
            {
               netVersion.Details = ".NET Framework Version 4.5 or later is not detected.";
               netVersion.Version = null;
               netVersion.ReleaseKey = 0;
            }
         }

         return netVersion;
      }

      // Checking the version using >= will enable forward compatibility.
      private static string CheckVersion(int releaseKey)
      {
         if (releaseKey >= 460798)
         {
            return "4.7+";
         }
         if (releaseKey >= 394802)
         {
            return "4.6.2";
         }
         if (releaseKey >= 394254)
         {
            return "4.6.1";
         }
         if (releaseKey >= 393295)
         {
            return "4.6";
         }
         if ((releaseKey >= 379893))
         {
            return "4.5.2";
         }
         if ((releaseKey >= 378675))
         {
            return "4.5.1";
         }
         if ((releaseKey >= 378389))
         {
            return "4.5";
         }
         // This code should never execute. A non-null release key should mean
         // that 4.5 or later is installed.
         return "No 4.5 or later version detected";
      }
   }

   public class NetVersion
   {
      public string Version { get; set; }

      public string Details { get; set; }

      public int ReleaseKey { get; set; }
   }
}
