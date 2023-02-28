﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
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
            RegistryKey key;

            try
            {
                key=hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", true);
                if (txtManufacturer.Text != null)
                    key.SetValue("Manufacturer", txtManufacturer.Text);
                if (txtModel.Text != null)
                    key.SetValue("Model", txtModel.Text);
                if (txtSupportHours.Text != null)
                    key.SetValue("SupportHours", txtSupportHours.Text);
                if (txtSupportUrl.Text != null)
                    key.SetValue("SupportURL", txtSupportUrl.Text);
                if (txtSupportPhone.Text != null)
                    key.SetValue("SupportPhone", txtSupportPhone.Text);
                if (strlogo != null)
                    key.SetValue("Logo", strlogo);
            }
            catch (Exception ex)
            {
                txtRisultato.Content = ex.Message;
                txtRisultato.Foreground=Brushes.Red;
                return;
            }
            txtRisultato.Content = "Open My Computer Properties";
            txtRisultato.Foreground = Brushes.Green;
        }

        private void OnCancellaLogo_Clicked(object sender, RoutedEventArgs e)
        {
            strlogo = "";
            imgLogo.Source=new BitmapImage();
        }
    }
}
