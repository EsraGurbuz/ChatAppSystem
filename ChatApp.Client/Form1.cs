using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatApp.Shared;
using Newtonsoft.Json;

namespace ChatApp.Client
{
    public partial class Form1 : Form
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _client = new TcpClient();
                // Connect asynchronously to not freeze the UI
                await _client.ConnectAsync("127.0.0.1", 5000);
                _stream = _client.GetStream();

                txtDisplay.AppendText("Connected to server!" + Environment.NewLine);

                // Start listening for incoming messages in background
                _ = ReceiveMessagesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var msg = JsonConvert.DeserializeObject<ChatMessage>(json);

                    // Update UI safely from a background task
                    Invoke(new Action(() => {
                        txtDisplay.AppendText($"[{msg.Timestamp:HH:mm}] {msg.Sender}: {msg.Message}{Environment.NewLine}");
                    }));
                }
            }
            catch { /* Handle disconnection */ }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (_stream == null) return;

            var msg = new ChatMessage(txtName.Text, txtInput.Text);
            string json = JsonConvert.SerializeObject(msg);
            byte[] data = Encoding.UTF8.GetBytes(json);

            await _stream.WriteAsync(data, 0, data.Length);
            txtInput.Clear();
        }
    }
}