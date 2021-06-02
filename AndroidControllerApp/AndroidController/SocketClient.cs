/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Documents;
using vJoyInterfaceWrap;

namespace AndroidController
{
    *//*public enum SocketState
    {
        EMPTY,
        PENDING,
        ACTIVE,
        DISCONNECTED
    }*//*
    public class SocketClient
    {
        private SocketServer server;
        private string name;
        private Socket workSocket;
        private const int BufferSize = 1024;
        private byte[] buffer;
        private StringBuilder sb;
        private VJoyDevice vJoyDevice;
        private SocketState socketState;
        private Queue<string> inputs;
        private bool paused;
        private bool removed;
        public SocketClient(SocketServer server, VJoyDevice vJoyDevice, Socket handler)
        {
            this.server = server;
            this.vJoyDevice = vJoyDevice;
            this.workSocket = handler;
            name = "<Unnamed Device>";
            buffer = new byte[BufferSize];
            sb = new StringBuilder();
            socketState = SocketState.EMPTY;
            inputs = new Queue<string>();
            paused = true;
            removed = false;
        }
        public void SetDevice(VJoyDevice vJoyDevice)
        {
            this.vJoyDevice = vJoyDevice;
        }
        public string GetName()
        {
            return name;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public uint GetDeviceId()
        {
            return vJoyDevice.GetId();
        }
        public string GetDeviceIdString()
        {
            return vJoyDevice.GetId() == 0 ? "": vJoyDevice.GetId().ToString();
        }
        public void SetDeviceId(uint id)
        {
            vJoyDevice.SetId(id);
        }
        public bool IsConnected()
        {
            return socketState == SocketState.ACTIVE;
        }
        public bool IsPaused()
        {
            return paused;
        }
        public void SetPaused(bool paused)
        {
            this.paused = paused;
        }
        public bool IsRemoved()
        {
            return removed;
        }
        public void SetRemoved(bool removed)
        {
            this.removed = removed;
        }
        public SocketState GetSocketState()
        {
            return socketState;
        }
        public void SetSocketState(SocketState socketState)
        {
            this.socketState = socketState;
        }
        public void Start()
        {
            //begin handshake
            SetSocketState(SocketState.PENDING);
            workSocket.BeginReceive(this.buffer, 0, BufferSize, 0,
                new AsyncCallback(ReadCallback), this);
        }
        public void Stop()
        {
            SetSocketState(SocketState.DISCONNECTED);
            SetRemoved(true);
            workSocket?.Close();
            
        }
        public void Disconnect()
        {
            server.Kill(this);
        }
        public void ReadCallback(IAsyncResult ar)
        {
            
            // Read data from the client socket.
            if (workSocket.Connected)
            {
                String content = String.Empty;
                int bytesRead = workSocket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    sb.Append(Encoding.ASCII.GetString(
                        buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read
                    // more data.  
                    content = sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        Console.WriteLine("Received: " + content);
                        Parse(content, "<EOF>");
                        ProcessContents();
                        sb = new StringBuilder();
                    }
                    workSocket.BeginReceive(buffer, 0, BufferSize, 0,
                                new AsyncCallback(ReadCallback), this);
                }
            }
            
        }
        private void Parse(string content, string delim)
        {
            int i = 0;
            while (i < content.Length - delim.Length + 1)
            {
                if (content.Substring(i, delim.Length) == delim)
                {
                    inputs.Enqueue(content.Substring(0, i));
                    content = content.Substring(i + delim.Length);
                    i = 0;
                }
                i++;
            }
        }
        private void ProcessContents()
        {
            while(inputs.Count > 0)
            {
                string input = inputs.Dequeue().Trim();
                
                string[] parts = input.Split();
                string tag = parts[0];
                
                switch (tag)
                {
                    case "CONNECT":
                        string name = input.Substring("CONNECT ".Length);
                        EndHandshake(name);
                        
                        break;
                    case "INPUT":
                        if (socketState == SocketState.ACTIVE && !paused && !removed)
                        {
                            vJoyDevice.ProcessInput(parts);
                        }
                        break;
                    case "DISCONNECT":
                        Disconnect();
                        break;
                }
            }
        }
        private void EndHandshake(string name)
        {
            Console.WriteLine("Ending Handshake with: " + name);
            SetName(name);
            byte[] byteData = Encoding.ASCII.GetBytes(name+"\r\n");
            workSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(HandshakeCallback), this);
        }
        private void HandshakeCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = workSocket.EndSend(ar);
                Console.WriteLine("Completed Handshake");
                
                SetSocketState(SocketState.ACTIVE);
                server.UpdateSockets();
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}*/