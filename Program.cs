using System;
using System.Threading.Tasks;
using System.Windows.Forms; // Make sure to add this for GUI components

namespace NMEADecoder
{
    static class Program
    {
        [STAThread] // Required for Windows Forms applications
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if USE_FILE_READER
            // Use file reader
            string filePath = "sample_nmea_data.txt";
            IReader reader = new FilePortReader(filePath);
            var form = new NmeaGui(reader);
            reader.StartReading(); 
            form.FormClosing += (sender, e) => reader.StopReading();
            Application.Run(form);
#else
            // Use serial port reader
            Reader reader = new SerialPortReader("COM3", 9600); 
            var form = new NmeaGui(reader); // Pass reader to the form
            reader.StartReading(); // Start reading
            form.FormClosing += (sender, e) => reader.StopReading(); // Ensure the port is closed when the form closes
            Application.Run(form);
#endif
        }
    }
}
