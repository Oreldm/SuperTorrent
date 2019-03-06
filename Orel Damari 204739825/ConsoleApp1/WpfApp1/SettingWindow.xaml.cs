using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TorrentLibrary;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        ConfigData configData = new ConfigData();

        public SettingWindow(ConfigData configData)
        {
            InitializeComponent();
            this.configData = configData;
            LoadDataToForm();
        }

        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            ConfigData configData = new ConfigData();
            configData.Ip = Ip_TextBox.Text.ToString();
            configData.Port = Convert.ToInt32(Port_TextBox.Text.ToString());
            configData.Username = Username_TextBox.Text.ToString();
            configData.Password = Password_TextBox.Text.ToString();
            configData.UploadPath = UploadPath_TextBox.Text.ToString();
            configData.DownloadPath = DownloadPath_TextBox.Text.ToString();

            this.configData = configData;

            //File.WriteAllText(MainWindow.CONFIGURATION_PATH + MainWindow.CONFIGURATION_FILE_NAME, JsonConvert.SerializeObject(configData, Formatting.Indented));
            File.WriteAllText(MainWindow.CONFIGURATION_PATH + MainWindow.CONFIGURATION_FILE_NAME, configData.ToXML());


            MessageBox.Show("Config Saved");
            this.Close();

        }



        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadDataToForm()
        {
            try
            {
                Ip_TextBox.Text = configData.Ip.ToString();
                Port_TextBox.Text = configData.Port.ToString();
                Username_TextBox.Text = configData.Username.ToString();
                Password_TextBox.Text = configData.Password.ToString();
                UploadPath_TextBox.Text = configData.UploadPath.ToString();
                DownloadPath_TextBox.Text = configData.DownloadPath.ToString();
            }
            catch (Exception ex) { }
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindow.configData = this.configData;
            base.OnClosed(e);
        }
    }
}
