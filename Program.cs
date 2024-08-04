using System;
using System.Threading.Tasks;

namespace NMEADecoder
{
    class Program
    {
#if USE_FILE_READER
        static async Task Main(string[] args)
        {
            string filePath = "sample_nmea_data.txt";
            var reader = new FilePortReader(filePath);
            await reader.ReadFileAsync();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
#else
        static void Main(string[] args)
        {
            var reader = new SerialPortReader("COM3", 4800);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            reader.Close();
        }
#endif
    }
}
