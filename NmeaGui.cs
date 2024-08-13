using System;
using System.Windows.Forms;
using NMEADecoder;

public class NmeaGui : Form
{
    private TabControl tabControl = new TabControl();
    private TabPage dataTab = new TabPage("Data");
    private TabPage settingsTab = new TabPage("Settings");
    private TextBox displayBox = new TextBox() { Multiline = true, Dock = DockStyle.Fill };
    private ComboBox comPortComboBox = new ComboBox() { Dock = DockStyle.Top };
    private ComboBox baudRateComboBox = new ComboBox() { Dock = DockStyle.Top };

    // Assuming reader is an interface type IReader that both FilePortReader and SerialPortReader implement
    public NmeaGui(IReader reader)
    {
        InitializeComponents();
        reader.DataReceived += UpdateDisplay; // Assuming UpdateDisplay can handle UI thread issues
    }


    public void UpdateDisplay(NmeaData data)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => UpdateDisplay(data))); // Safe call for cross-thread operation
        }
        else
        {
            displayBox.AppendText(data.ToString() + Environment.NewLine);
        }
    }

    private void InitializeComponents()
    {
        dataTab.Controls.Add(displayBox);
        settingsTab.Controls.Add(comPortComboBox);
        settingsTab.Controls.Add(baudRateComboBox);
        tabControl.TabPages.Add(dataTab);
        tabControl.TabPages.Add(settingsTab);
        tabControl.Dock = DockStyle.Fill;
        this.Controls.Add(tabControl);
        this.Width = 600;
        this.Height = 400;
    }
}

