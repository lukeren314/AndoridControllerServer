using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace AndroidController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int DefaultPort = 11000;

        private VJoyHandler vJoyHandler;
        private ViewController viewController;
        private SocketServer socketServer;

        private string address;
        private int port;
        public MainWindow()
        {
            InitializeComponent();

            vJoyHandler = new VJoyHandler();
            List<uint> ids = vJoyHandler.deviceIds;

            string hostname = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostname);
            address = ipHostInfo.AddressList[ipHostInfo.AddressList.Length - 1].ToString(); //get the default ip
            port = DefaultPort;

            viewController = new ViewController(this);
            viewController.SetAvailableDevices(ids);
            viewController.SetEnabled(vJoyHandler.IsEnabled);
            viewController.SetInstallVJoyButton(vJoyHandler.IsEnabled);
            viewController.SetIpAddressText(address);
            viewController.SetPortText(port.ToString());
            viewController.SetSocketGridColumns(ids);

            socketServer = new SocketServer(viewController, vJoyHandler);
        }
        private void ServerStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (!vJoyHandler.IsEnabled)
            {
                MessageBox.Show("vJoy must be installed in order to start the server.");
                return;
            }
            if (socketServer.IsActive)
            {
                viewController.SetServerStatusButton("Start Server");
                viewController.SetServerStatusLabel("Server Inactive");
                socketServer.Stop();
            }
            else
            {
                string ipAddress = viewController.GetIpAddressText();
                string portString = viewController.GetPortText();

                if (ipAddress.Length > 0 && IsValidPort(portString))
                {
                    bool success = socketServer.Start(viewController.GetIpAddressText(), int.Parse(portString));
                    if (success)
                    {
                        viewController.SetServerStatusButton("Stop Server");
                        viewController.SetServerStatusLabel("Server Active");
                    }
                    else
                    {
                        MessageBox.Show("Failed to start server. (Tip) Duplicate servers not allowed on same port");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid IP Address/Port.");
                }
            }

        }
        private bool IsValidPort(string portString)
        {
            bool isNumber = int.TryParse(portString, out port);
            return isNumber;
        }/*
        <DataGridComboBoxColumn x:Name="DeviceIdColumn" SelectedItemBinding="{Binding DeviceId}" Header="Device ID">
                    
                </DataGridComboBoxColumn>
        <DataGridComboBoxColumn.EditingElementStyle>
                        <Style TargetType = "{x:Type ComboBox}" >
                            < EventSetter Event="SelectionChanged" Handler="DeviceIdColumn_Changed" />
                        </Style>
                    </DataGridComboBoxColumn.EditingElementStyle>*/
        private void DeviceIdColumn_Changed(object sender, RoutedEventArgs e)
        {
            Item item = viewController.GetSelectedItem();
            if (item != null)
            {
                string newIdText = (sender as ComboBox).SelectedValue.ToString();
                uint targetId = DeviceIdStringToUInt(newIdText);
                socketServer.ClearId(targetId);
                item.State.SetDeviceId(targetId);
                socketServer.UpdateSockets();
            }
        }
        private uint DeviceIdStringToUInt(string id)
        {
            return id == "" ? 0 : (uint)int.Parse(id);
        }
        private void IsPaused_Checked(object sender, RoutedEventArgs e)
        {
            IsPaused_Changed(sender);
        }
        private void IsPaused_Unchecked(object sender, RoutedEventArgs e)
        {
            IsPaused_Changed(sender);
        }
        private void IsPaused_Changed(object sender)
        {
            CheckBox check = (CheckBox)sender;
            Item item = viewController.GetSelectedItem();
            if (item != null && check != null)
            {
                item.State.SetPaused((bool)check.IsChecked);
                socketServer.UpdateSockets();
            }

        }
        private void RemoveDevice_Clicked(object sender, RoutedEventArgs e)
        {
            //kill uinque id
            Item item = viewController.GetSelectedItem();
            if (item != null && !item.State.IsDisconnected())
            {
                socketServer.Kill(item.State);
            }
            socketServer.UpdateSockets();
        }

        
        private void Application_Exit(object sender, CancelEventArgs e)
        {
            socketServer.Stop();
            vJoyHandler.Close();
        }

        private void InstallVJoyButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("vJoySetup.exe");
        }
    }
}
