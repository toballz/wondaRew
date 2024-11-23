using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using toolsHelper;

namespace wondaRew
{ 
    internal class Tool 
    {
        public class LadderClass
        {
            public required String Title;
            public required String Description;
            public String? Icon;
            public String Params = String.Empty;
            public LadderClass? Child;

        }
        public static string LadderName = "Mmalumba";
        public static string LadderDescription = "Welcome to Mmalumba world!!!!";
        public static List<LadderClass> LadderChildren = new List<LadderClass>{

                new LadderClass {
                    Title = "Hash",
                    Description ="Get Hash",
                    Icon = "asas",
                    Params = "hash",
                },

                new LadderClass {
                    Title = "Shred",
                    Description = "Shred this File",
                    Icon = "asas",
                    Params = "shred",
                },

                new LadderClass {
                    Title = "Encrypt",
                    Description = "Encrypt this File",
                    Icon = "asas",
                    Params = "encrypt",
                },

                new LadderClass {
                    Title = "Decrypt",
                    Description = "Decrypt this File",
                    Icon = "asas",
                    Params = "decrypt",
                },


            };

        public static string? FilePathToWorkOn;

        public static string encryption_outFileExtension = ".oxx";
        public static string encryption_pwdSalt_1 = "\u81BB\r\n\u057F%TR* 🈵Yꗞ𒆳\u0fcd꡵㔱䖙ꎘ🜑❧🡪𐃂🞳🡾🖕𐅜𒓺🤖🏽垓\\" +
            "/🎵🕒👢🈀𑢽🚆📭🀷𑍳🈵🏰🢩🙢🚻💵" +
            "▒ ░█▓ >▒sayg-4c557d229bfd80xA79b52e1c7b8d85859f8d4e0d5a9f3b955r\u003B" +
            "\"('№℉℗™®⁅⁆⌈⌋⌉⌊«⟦⟫⟬⟭⟧‰‱⁂※⁜⁛⁞⁙⁖⁚⁝…¡!⁈⁇±µ⁊⏕⏔⏖₫₩ϐϕͲϗϟϷϑϴᾮᾠᾨὢὕύΎΊὈᾚἦᾕὴεκλΞξΡρδ⫸⫷⫺⫻⫵⫴⫲⫱⫮⫯⫬⫫⫹ï»¿¯ª" +
            "¬&@\"^>-*'|)~\0+";

        public static byte[] encryption_pwdSalt_2 = { 0xA8, 0xAB ,0x8E ,0xC1 ,0x93 ,0x98 ,0x73 ,0x6A ,0x3B, 0x8E, 0x1A, 0x5F, 0xA7, 0x92, 0x7C, 0x4F, 0x68, 0x0D, 0x21, 0x8A, 0x7B,
            0xF9, 0x33, 0xE2, 0xD0, 0x11, 0x1C, 0xD8, 0x69, 0x02, 0x33, 0x34, 0x9F, 0x6B, 0xA4, 0x3D, 0x9A, 0x6E, 0x7A, 0x8C, 0x5D, 0x41, 0x71, 0xBF, 0x4A, 0x25, 0x87,
            0x9B,0x0D ,0xA4 ,0x21 ,0xA9 ,0xBD ,0xF9 ,0x33,0xA2, 0xD0 ,0x11 ,0xD8, 0x69, 2, 0x33, 0x34 };




        public static void RegisterContextMenuIfAdmin(string keyName, string menuText, string exePath, List<LadderClass> ladder)
        {
            //if administrator
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                RegistryKey baseLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey baseRoot = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
                //subcommander path
                string subMenuPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\";

                h.print("step 1");
                // Create registry key
                using (RegistryKey menuKey = baseRoot.CreateSubKey(@"*\shell\" + keyName))
                {
                    h.print("step 2");
                    // Add menu item
                    if (menuKey != null)
                    {
                        h.print("step 3");
                        string referenceTitles = "";//dnd
                        //create the reference
                        foreach (LadderClass item in ladder)
                        {
                            string keynameNtitle = keyName + "." + item.Title.ToLower();
                            referenceTitles += keynameNtitle + ";";
                            using (RegistryKey subMenuKey_child = baseLocalMachine.CreateSubKey(subMenuPath + keynameNtitle))
                            {
                                h.print("step 4");
                                if (subMenuKey_child != null)
                                {
                                    subMenuKey_child.SetValue(null, item.Description);
                                    RegistryKey ot = subMenuKey_child.CreateSubKey("command");
                                    ot.SetValue(null, "\"" + exePath + "\"  --file \"%1\" --command " + item.Params);
                                }
                            }
                        }
                        h.print("step 5");

                        //link reference too;
                        menuKey.SetValue("MUIVerb", menuText);
                        menuKey.SetValue("SubCommands", referenceTitles);
                        menuKey.SetValue("subcommander path", "HKEY_LOCAL_MACHINE\\" + subMenuPath);
                        menuKey.SetValue("Icon", exePath+",0");
                        menuKey.SetValue("Position", "Top");
                        return;



                    }
                }
                h.print("step 6");
            }


        }
        //  
        //
        //
    }
}

  