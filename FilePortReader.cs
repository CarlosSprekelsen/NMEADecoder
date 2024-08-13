using System;
using System.IO;
using System.Threading.Tasks;

namespace NMEADecoder
{
    public class FilePortReader : IReader
    {
        private readonly string _filePath;
        public event Action<NmeaData> DataReceived;

        public FilePortReader(string filePath)
        {
            _filePath = filePath;
        }

        public void StartReading()
        {
            Task.Run(() => ReadFileAsync());
        }

        public void StopReading()
        {
            // Implement logic to stop reading if necessary, e.g., by controlling a cancellation token
        }

        public async Task ReadFileAsync()
        {
            using (var reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    ProcessLine(line);
                }
            }
        }

        private void ProcessLine(string line)
        {
            NmeaData nmeaData = NmeaParser.Parse(line);
            Console.WriteLine(nmeaData);
        }
    }
}
