using System;
using WixSharp;

namespace windatefrom_setup
{
    internal class Program
    {
        static void Main()
        {
            var project = new Project("numoeminformations",
                              new Dir(@"[ProgramFiles64Folder]\\numoeminformations",
                                  new DirFiles(@"*.*")
                        ),
                        new Dir(@"%ProgramMenu%",
                         new ExeFileShortcut("numoeminformations", "[ProgramFiles64Folder]\\numoeminformations\\numoeminformations.exe", "") { WorkingDirectory = "[INSTALLDIR]" }
                      )//,
                       //new Property("ALLUSERS","0")
            );

            project.GUID = new Guid("6CF4E806-2B0C-4846-89A5-848C998AAC26");
            project.Version = new Version("0.8.9");
            project.Platform = Platform.x64;
            project.SourceBaseDir = "F:\\source\\numoeminformations\\numoeminformations\\bin\\Release\\net8.0-windows10.0.22621.0";
            project.LicenceFile = "LICENSE.rtf";
            project.OutDir = "f:\\";
            project.ControlPanelInfo.Manufacturer = "Giulio Sorrentino";
            project.ControlPanelInfo.Name = "numerone's oem informations";
            project.ControlPanelInfo.HelpLink = "https://github.com/numerunix/numoeminformations/issues";
            project.Description = "Un modificatore di informazioni OEM per windows 10 e 11.";
            //            project.Properties.SetValue("ALLUSERS", 0);
            project.BuildMsi();
        }
    }
}