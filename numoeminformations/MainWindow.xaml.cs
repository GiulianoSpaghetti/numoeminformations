using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace numoeminformations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string strlogo;
        public MainWindow()
        {
            InitializeComponent();
            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", false))
            {
                txtManufacturer.Text = key.GetValue("Manufacturer") as string;    
                txtModel.Text = key.GetValue("Model") as string;
                txtSupportHours.Text = key.GetValue("SupportHours") as string;
                txtSupportUrl.Text = key.GetValue("SupportURL") as string;
                txtSupportPhone.Text = key.GetValue("SupportPhone") as string;
                strlogo = key.GetValue("Logo") as string;
                if (File.Exists(strlogo))
                    imgLogo.Source = new BitmapImage(new Uri(strlogo));

            }
        }

        private void OnSelectImage_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                strlogo = op.FileName;
                imgLogo.Source = new BitmapImage(new Uri(strlogo));
            }
        }

        private void OnSalva_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key=null;

            try
            {
                key=hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", true);
                if (txtManufacturer.Text != null)
                    key.SetValue("Manufacturer", txtManufacturer.Text);
                else
                    key.DeleteValue("Manufacturer", false);
                if (txtModel.Text != null)
                    key.SetValue("Model", txtModel.Text);
                else
                    key.DeleteValue("Model", false);
                if (txtSupportHours.Text != null)
                    key.SetValue("SupportHours", txtSupportHours.Text);
                else
                    key.DeleteValue("SupportHours", false);
                if (txtSupportUrl.Text != null)
                    key.SetValue("SupportURL", txtSupportUrl.Text);
                else
                    key.DeleteValue("SupportUrl", false);
                if (txtSupportPhone.Text != null)
                    key.SetValue("SupportPhone", txtSupportPhone.Text);
                else
                    key.DeleteValue("SupportPhone", false);
                if (strlogo != null)
                    key.SetValue("Logo", strlogo);
                else
                    key.DeleteValue("Logo", false);

            }
            catch (Exception ex)
            {
                if (IsRunAsAdmin())
                {
                    txtRisultato.Content = ex.Message;
                    txtRisultato.Foreground = Brushes.Red;
                    Pulisci(hklm, key);
                    return;
                } else
                {
                    try
                    {
                        AdminRelauncher();
                    } catch (Exception exe)
                    {
                        txtRisultato.Content = exe.Message;
                        txtRisultato.Foreground = Brushes.Red;
                        Pulisci(hklm, key);
                        return;

                    }
                }
            }
            txtRisultato.Content = "Open System Informations under Settings";
            txtRisultato.Foreground = Brushes.Green;
            Pulisci(hklm, key);

        }
        private void Pulisci(RegistryKey hklm, RegistryKey key)
        {
            if (key != null)
                key.Close();
            hklm.Close();

        }
        private void OnCancellaLogo_Clicked(object sender, RoutedEventArgs e)
        {
            strlogo = "";
            imgLogo.Source=new BitmapImage();
        }
        private void AdminRelauncher()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase.Replace(".dll", ".exe");
                proc.Verb = "runas";
                new ToastContentBuilder().AddArgument("Required admin launch").AddText("This app will be relaunched with administrative priviledges").AddAudio(new Uri("ms-winsoundevent:Notification.Reminder")).Show();
                Process.Start(proc);
                Application.Current.Shutdown();
            }
        }

        private bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
