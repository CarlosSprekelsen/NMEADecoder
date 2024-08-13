using System;

namespace NMEADecoder
{
    public class NmeaData
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Speed { get; set; }
        public double Course { get; set; }
        public string Mode { get; set; }
        public int SatellitePRN { get; set; }
        public int SystemId { get; set; }
        public string Constellation { get; set; }
        public int Azimuth { get; set; }
        public int Elevation { get; set; }
        public int SNR { get; set; }
        public double HDOP { get; set; }
        public double VDOP { get; set; }
        public double PDOP { get; set; }

        public override string ToString()
        {
            return $"Timestamp: {Timestamp}, Latitude: {Latitude}, Longitude: {Longitude}, Altitude: {Altitude}, Speed: {Speed}, Course: {Course}, Mode: {Mode}, Satellite PRN: {SatellitePRN}, Azimuth: {Azimuth}, Elevation: {Elevation}, SNR: {SNR}, HDOP: {HDOP}, VDOP: {VDOP}, PDOP: {PDOP}";
        }
    }
}
