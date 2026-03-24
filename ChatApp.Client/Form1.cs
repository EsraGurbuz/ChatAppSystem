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

            // Check these two lines carefully:
            pnlLogin.Enabled = true;  // This must be TRUE at start
            pnlChat.Enabled = false;  // This must be FALSE until connected
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _client = new TcpClient();

                // 1. Try to connect to the server
                await _client.ConnectAsync("127.0.0.1", 5000);
                _stream = _client.GetStream();

                // 2. If connection is successful, change the UI state
                pnlLogin.Enabled = false; // Locks the login part (Name and Connect button)
                pnlChat.Enabled = true;   // Unlocks the chat part (Display, Input and Send button)

                txtDisplay.AppendText("### Connected to server! ###" + Environment.NewLine);

                // 3. Start listening for messages in the background
                _ = ReceiveMessagesAsync();
            }
            catch (Exception ex)
            {
                // If connection fails, stay in the login state and show error
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

                    // Deserialization: Converting JSON back to object
                    var msg = JsonConvert.DeserializeObject<ChatMessage>(json);

                    // Polymorphism & UI Update
                    // We ensure the UI thread handles the update
                    Invoke(new Action(() => {
                        string formattedMessage = $"[{msg.Timestamp:HH:mm}] {msg.Sender}: {msg.Message}";
                        txtDisplay.AppendText(formattedMessage + Environment.NewLine);

                        // Auto-scroll to bottom
                        txtDisplay.SelectionStart = txtDisplay.Text.Length;
                        txtDisplay.ScrollToCaret();
                    }));
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => txtDisplay.AppendText("### Connection Lost ###" + Environment.NewLine)));
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            // Check if we are connected and inputs are not empty
            if (_stream == null || string.IsNullOrWhiteSpace(txtInput.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                return;
            }

            try
            {
                var msg = new ChatMessage(txtName.Text, txtInput.Text);
                string json = JsonConvert.SerializeObject(msg);
                byte[] data = Encoding.UTF8.GetBytes(json);

                await _stream.WriteAsync(data, 0, data.Length);

                // Clear input after sending
                txtInput.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Send error: {ex.Message}");
            }
        }

        private void pnlChat_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}