using System;
using System.IO;
using System.Threading.Tasks;

namespace NMEADecoder
{
    public class FilePortReader
    {
        private readonly string _filePath;

        public FilePortReader(string filePath)
        {
            _filePath = filePath;
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
