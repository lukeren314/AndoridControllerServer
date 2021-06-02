using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AndroidController
{
    public class Item
    {
        public Item(int i, SocketState state, ObservableCollection<string> ids)
        {
            this.State = state;
            this.Num = i;
            this.Name = state.name;
            this.DeviceIds = ids;
            this.DeviceId = state.deviceId == 0 ? "" : state.deviceId.ToString();
            switch (state.status)
            {
                case SocketState.Status.PENDING:
                    this.Status = "Connecting";
                    break;
                case SocketState.Status.PAUSED:
                    this.Status = "Paused";
                    break;
                case SocketState.Status.ACTIVE:
                    this.Status = "Active";
                    break;
                case SocketState.Status.DISCONNECTED:
                    this.Status = "Disconnected";
                    break;
                default:
                    this.Status = "";
                    break;
            }
            this.IsPaused = state.IsPaused();
            this.Remove = false;
        }
        public SocketState State { get; set; }
        public int Num { get; set; }
        public string Name { get; set; }
        public ObservableCollection<string> DeviceIds { get; set; }
        public string DeviceId { get; set; }
        public string Status { get; set; }
        public bool IsPaused { get; set; }
        public bool Remove { get; set; }
    }
    public class ViewController
    {
        private Window window;
        private ListBox devicesListBox;
        private Label vJoyStatusLabel;
        private Button installVJoyButton;
        private TextBox portTextBox;
        private TextBox ipAddressTextBox;
        private Button serverStatusButton;
        private Label serverStatusLabel;
        private DataGrid socketGrid;

        private ObservableCollection<Item> socketItems;
        public ObservableCollection<string> deviceIds { get; set; }
        public ViewController(Window window)
        {
            this.window = window;
            devicesListBox = (ListBox)window.FindName("DevicesListBox");
            vJoyStatusLabel = (Label)window.FindName("VJoyStatusLabel");
            installVJoyButton = (Button)window.FindName("InstallVJoyButton");
            portTextBox = (TextBox)window.FindName("PortTextBox");
            ipAddressTextBox = (TextBox)window.FindName("IpAddressTextBox");
            serverStatusButton = (Button)window.FindName("ServerStatusButton");
            serverStatusLabel = (Label)window.FindName("ServerStatusLabel");
            socketGrid = (DataGrid)window.FindName("SocketGrid");

            socketItems = new ObservableCollection<Item>();
        }
        public Item GetSelectedItem()
        {
            return (Item)socketGrid.SelectedItem;
        }
        public void SetAvailableDevices(List<uint> ids)
        {
            devicesListBox.Items.Clear();
            foreach (uint id in ids)
            {
                devicesListBox.Items.Add(id + "");
            }
        }
        public void SetEnabled(bool enabled)
        {
            vJoyStatusLabel.Content =  enabled ? "vJoy Installed" : "vJoy Not Installed";
        }
        public void SetInstallVJoyButton(bool enabled)
        {
            /*if (enabled)
            {
                installVJoyButton.IsEnabled = false;
            }*/
        }
        public string GetPortText()
        {
            return portTextBox.Text;
        }
        public void SetPortText(string port)
        {
            portTextBox.Text = port;
        }
        public string GetIpAddressText()
        {
            return ipAddressTextBox.Text;
        }
        public void SetIpAddressText(string content)
        {
            ipAddressTextBox.Text = content;

        }
        public void SetServerStatusButton(string content)
        {
            serverStatusButton.Content = content;
        }

        public void SetServerStatusLabel(string content)
        {
            serverStatusLabel.Content = content;
        }

        public void SetSocketGridColumns(List<uint> ids)
        {
            //DataGridComboBoxColumn deviceIdC = (DataGridComboBoxColumn)window.FindName("DeviceIdColumn");
            deviceIds = new ObservableCollection<string>();
            deviceIds.Add("");
            foreach (uint id in ids)
            {
                deviceIds.Add(id.ToString());
            }
            /*deviceIds.ItemsSource = deviceIds;
            */
            socketGrid.ItemsSource = socketItems;
        }
        public void SetSockets(List<SocketState> socketStates)
        {
            socketItems.Clear();
            for (int i = 0; i < socketStates.Count; i++)
            {
                socketItems.Add(new Item(i, socketStates[i], deviceIds));
            }
        }
        public void Refresh()
        {
            socketGrid.UpdateLayout();
        }

    }
    
}