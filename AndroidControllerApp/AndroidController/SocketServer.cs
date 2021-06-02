using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace AndroidController
{

    public enum Protocol
    {
        TCP,
        UDP
    }
    public class SocketState
    {
        public const int BufferSize = 1024;
        public enum Status
        {
            UNCONNECTED,
            PENDING,
            ACTIVE,
            PAUSED,
            DISCONNECTED
        }
        public string address = null;
        public Status status = Status.UNCONNECTED;
        public Socket workSocket = null;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
        public int id;
        public string name;
        public uint deviceId;
        public void SetDeviceId(uint id)
        {
            deviceId = id;
        }
        public void SetPending()
        {
            status = Status.PENDING;
        }
        public bool IsActive()
        {
            return status == Status.ACTIVE;
        }
        public void SetActive()
        {
            status = Status.ACTIVE;
        }
        public bool IsDisconnected()
        {
            return status == Status.DISCONNECTED;
        }
        public void SetDisconnected()
        {
            status = Status.DISCONNECTED;
        }
        public bool IsPaused()
        {
            return status == Status.PAUSED;
        }
        public void SetPaused()
        {
            status = Status.PAUSED;
        }
        public void SetPaused(bool IsPaused)
        {
            switch (status)
            {
                case Status.ACTIVE:
                    if (IsPaused)
                    {
                        status = Status.PAUSED;
                    }
                    break;
                case Status.PAUSED:
                    if (!IsPaused)
                    {
                        status = Status.ACTIVE;
                    }
                    break;
            }
        }
    }
    public class SocketServer
    {
        public bool IsActive;

        private ViewController viewController;
        private VJoyHandler vJoyHandler;

        private string address;
        private int port;

        private IPEndPoint localEndPoint;
        private ManualResetEvent allDone;
        private Socket listener;
        private Thread serverThread;
        private Protocol protocol;

        private List<SocketState> socketStates;

        public SocketServer(ViewController viewController, VJoyHandler vJoyHandler)
        {
            this.viewController = viewController;
            this.vJoyHandler = vJoyHandler;
            IsActive = false;
            allDone = new ManualResetEvent(false);
            socketStates = new List<SocketState>();
        }
        public bool Start(string address, int port)
        {
            switch (protocol)
            {
                case Protocol.TCP:
                    return StartTcpServer(address, port);
                    break;
            }
            return false;
        }
        private bool StartTcpServer(string address, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(address);
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            localEndPoint = new IPEndPoint(ipAddress, port);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(Constants.MaxControllers);

                serverThread = new Thread(() =>
                {
                    while (true)
                    {
                        allDone.Reset();
                        if (!IsActive)
                        {
                            break;
                        }
                        if (socketStates.Count < Constants.MaxControllers)
                        {
                            try
                            {
                                listener.BeginAccept(
                          new AsyncCallback(AcceptCallback),
                          listener);
                            }
                            catch (System.ObjectDisposedException)
                            {
                                Console.WriteLine("Server Closed");
                            }
                        }
                        allDone.WaitOne();
                    }
                });
                serverThread.Start();
                IsActive = true;
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Get the socket that handles the client request.  
            if (IsActive)
            {
                Socket listener = (Socket)ar.AsyncState;
                try
                {
                    Socket handler = listener.EndAccept(ar); // completes handshake and prepares communication
                    string clientAddress = (handler.RemoteEndPoint as IPEndPoint).Address.ToString();
                    Console.WriteLine("Found Connection From: " + clientAddress);
                    if (AvailableAddress(clientAddress))
                    {
                        SocketState socketState = new SocketState();
                        socketState.workSocket = handler;
                        socketState.address = clientAddress;
                        socketState.SetDeviceId(GetAvailableDeviceId());
                        socketState.SetPending();
                        socketStates.Add(socketState);

                        handler.BeginReceive(socketState.buffer, 0, SocketState.BufferSize, 0, new AsyncCallback(ReadCallback), socketState);
                        UpdateSockets();
                    }
                }
                catch (System.ObjectDisposedException)
                {

                }


            }
            //continue the main thread after socket is connected;
            allDone.Set();
        }
        private bool AvailableAddress(string address)
        {
            foreach (SocketState socketState in socketStates)
            {
                if (socketState.address.Equals(address))
                {
                    return false;
                }
            }
            return true;
        }
        private uint GetAvailableDeviceId()
        {
            uint[] usedIds = new uint[socketStates.Count];
            for (int i = 0; i < usedIds.Length; i++)
            {
                usedIds[i] = socketStates[i].deviceId;
            }
            foreach (uint id in vJoyHandler.deviceIds)
            {
                if (!usedIds.Contains(id))
                {
                    return id;
                }
            }
            return 0;
        }
        private void ReadCallback(IAsyncResult ar)
        {
            if (IsActive)
            {
                SocketState state = (SocketState)ar.AsyncState;
                Socket handler = state.workSocket;

                if (handler.Connected)
                {
                    String content = String.Empty;
                    int bytesRead = handler.EndReceive(ar);
                    if (bytesRead > 0)
                    {
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                        content = state.sb.ToString();
                        if (content.IndexOf("#") > -1)
                        {
                            Console.WriteLine("Received: " + content);
                            List<string> commands = ParseCommands(content, "#", state);
                            ProcessCommands(state, commands);
                            
                        }
                        handler.BeginReceive(state.buffer, 0, SocketState.BufferSize, 0,
                                    new AsyncCallback(ReadCallback), state);
                    }
                }
            }
        }
        private List<string> ParseCommands(string content, string delim, SocketState state)
        {
            List<string> commands = new List<string>();
            int i = 0;
            int j = 0;
            int n = content.Length;
            int dn = delim.Length;
            while(i < n-dn+1)
            {
                if (content.Substring(i, dn).Equals(delim))
                {
                    commands.Add(content.Substring(j, i-j));
                    i += dn;
                    j = i;
                }
                i++;
            }
            if(j < n)
            {
                state.sb = new StringBuilder();
                state.sb.Append(content.Substring(j, n - j));
            }
            return commands;

        }
        private void ProcessCommands(SocketState state, List<string> commands)
        {
            foreach(string command in commands)
            {
                string trimmed = command.Trim();
                string[] parts = trimmed.Split();
                if (parts.Length == 0)
                {
                    continue;
                }
                string tag = parts[0];
                switch (tag)
                {
                    case "CONNECT":
                        string name = trimmed.Substring("CONNECT ".Length);
                        EndHandshake(state, name);
                        break;
                    case "I":
                        /*string[] subarray = new string[parts.Length - 1];
                        Array.Copy(parts, 1, subarray, 0, parts.Length - 1);*/
                        Console.WriteLine("Running " + trimmed);
                        RunCommand(state, parts);
                        break;
                    case "DISCONNECT":
                        StopSocket(state);
                        break;
                }
            }
            vJoyHandler.Update(state.deviceId);
        }
        private void RunCommand(SocketState state, string[] parts)
        {
            if (state.IsActive())
            {
                vJoyHandler.RunCommand(state.deviceId, parts);
            }
        }
        private void EndHandshake(SocketState state, string name)
        {
            Console.WriteLine(name);
            state.name = name;
            byte[] byteData = Encoding.ASCII.GetBytes("CONNECTED" + "\r\n");
            state.workSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(EndHandshakeCallback), state);
        }
        private void EndHandshakeCallback(IAsyncResult ar)
        {
            try
            {
                SocketState state = (SocketState)ar.AsyncState;
                Socket handler = state.workSocket;
                if (handler.Connected)
                {
                    int bytesSent = handler.EndSend(ar);
                    state.SetPaused();
                    Console.WriteLine("Completed Handshake");
                    UpdateSockets();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void Stop()
        {
            allDone.Set();
            listener?.Close();
            StopSockets();
            UpdateSockets();
            IsActive = false;
        }
        public void StopSockets()
        {
            foreach (SocketState socketState in socketStates)
            {
                try
                {
                    StopSocket(socketState);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            
            socketStates.Clear();
        }
        public void StopSocket(SocketState socketState)
        {
            socketState.SetDisconnected();
            socketState.workSocket?.Close();

        }
        public void Kill(SocketState socketState)
        {
            StopSocket(socketState);
            for(int i = socketStates.Count - 1; i >= 0; i--)
            {
                if (socketState.Equals(socketStates[i]))
                {
                    socketStates.RemoveAt(i);
                    break;
                }
            }
        }
        public void ClearId(uint id)
        {
            foreach(SocketState socketState in socketStates)
            {
                if (socketState.deviceId == id)
                {
                    socketState.SetDeviceId(0);
                }
            }
        }
        public void UpdateSockets()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                viewController.SetSockets(socketStates);
            });
        }
    }
}