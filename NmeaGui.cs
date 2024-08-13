using System;
using System.Windows.Forms;
using NMEADecoder;
using System.IO.Ports;

public class NmeaGui : Form
{
    private TabControl tabControl = new TabControl();
    private TabPage dataTab = new TabPage("Data");
    private TabPage settingsTab = new TabPage("Settings");

    // Data tab components
    private Label lblTimestamp = new Label { Text = "Timestamp:" };
    private TextBox txtTimestamp = new TextBox();
    private Label lblLatitude = new Label { Text = "Latitude:" };
    private TextBox txtLatitude = new TextBox();
    private Label lblLongitude = new Label { Text = "Longitude:" };
    private TextBox txtLongitude = new TextBox();

    // Settings tab components
    private Label lblComPort = new Label { Text = "COM Port:" };
    private ComboBox comPortComboBox = new ComboBox { Dock = DockStyle.Top };
    private Label lblBaudRate = new Label { Text = "Baud Rate:" };
    private ComboBox baudRateComboBox = new ComboBox { Dock = DockStyle.Top };

    public NmeaGui(IReader reader)
    {
        InitializeComponents();
        reader.DataReceived += UpdateDisplay;
        PopulateSerialPortSettings();
    }

    private void InitializeComponents()
    {
        // Setup the main window
        this.Width = 800;
        this.Height = 500;
        this.Text = "NMEA Data Viewer";

        // Setup the tab control
        tabControl.Dock = DockStyle.Fill;
        this.Controls.Add(tabControl);

        // Setup data tab
        dataTab.Controls.Add(CreateDataPanel());
        
        // Setup settings tab
        settingsTab.Controls.Add(CreateSettingsPanel());

        // Add tabs to tab control
        tabControl.TabPages.Add(dataTab);
        tabControl.TabPages.Add(settingsTab);
    }

    private Panel CreateDataPanel()
    {
        Panel panel = new Panel { Dock = DockStyle.Fill };
        int yPos = 20;

        // Add Timestamp
        AddLabelTextBoxPair(panel, lblTimestamp, txtTimestamp, yPos);
        yPos += 30;

        // Add Latitude
        AddLabelTextBoxPair(panel, lblLatitude, txtLatitude, yPos);
        yPos += 30;

        // Add Longitude
        AddLabelTextBoxPair(panel, lblLongitude, txtLongitude, yPos);

        return panel;
    }

    private Panel CreateSettingsPanel()
    {
        Panel panel = new Panel { Dock = DockStyle.Fill };
        int yPos = 20;

        // Add COM Port Combo
        AddLabelControlPair(panel, lblComPort, comPortComboBox, yPos);
        yPos += 30;

        // Add Baud Rate Combo
        AddLabelControlPair(panel, lblBaudRate, baudRateComboBox, yPos);

        // Populate standard baud rates
        PopulateBaudRates();

        return panel;
    }

     private void PopulateSerialPortSettings()
    {
        // Populate COM port dropdown
        comPortComboBox.Items.Clear();
        string[] portNames = SerialPort.GetPortNames(); // Gets all available COM ports
        foreach (string portName in portNames)
        {
            comPortComboBox.Items.Add(portName);
        }
        if (comPortComboBox.Items.Count > 0) comPortComboBox.SelectedIndex = 0;

        // Optionally set a default or saved COM port
        // comPortComboBox.SelectedItem = "COM1"; // Set default or saved COM port if needed
    }

        private void PopulateBaudRates()
    {
        int[] baudRates = new int[] { 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 }; // Standard baud rates
        foreach (int rate in baudRates)
        {
            baudRateComboBox.Items.Add(rate);
        }
        baudRateComboBox.SelectedItem = 9600; // Set a common default baud rate
    }

    private void AddLabelTextBoxPair(Panel panel, Label label, TextBox textBox, int yPos)
    {
        label.Location = new Point(10, yPos);
        label.Size = new Size(100, 20);
        textBox.Location = new Point(120, yPos);
        textBox.Size = new Size(200, 20);
        panel.Controls.Add(label);
        panel.Controls.Add(textBox);
    }

    private void AddLabelControlPair(Panel panel, Label label, Control control, int yPos)
    {
        label.Location = new Point(10, yPos);
        label.Size = new Size(100, 20);
        control.Location = new Point(120, yPos);
        control.Size = new Size(200, 20);
        panel.Controls.Add(label);
        panel.Controls.Add(control);
    }

    public void UpdateDisplay(NmeaData data)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => UpdateDisplay(data)));
        }
        else
        {
            txtTimestamp.Text = data.Timestamp.ToString();
            txtLatitude.Text = data.Latitude.ToString();
            txtLongitude.Text = data.Longitude.ToString();
            // Add other fields as necessary
        }
    }
}
