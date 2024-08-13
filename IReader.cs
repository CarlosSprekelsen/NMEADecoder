using System;

namespace NMEADecoder
{
    public interface IReader
    {
        event Action<NmeaData> DataReceived;

        void StartReading();
        void StopReading();
    }
}
