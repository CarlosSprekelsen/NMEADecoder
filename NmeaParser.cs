using System;
using System.Globalization;

namespace NMEADecoder
{
    public static class NmeaParser
    {
        public static CultureInfo NmeaCultureInfo = new CultureInfo("en-US");
        public static double MPHPerKnot = double.Parse("1.150779", NmeaCultureInfo);

        public static NmeaData Parse(string sentence)
        {
            if (string.IsNullOrWhiteSpace(sentence))
                throw new ArgumentException("NMEA sentence is null or empty.");

            if (!IsValid(sentence))
                return null;

            string[] parts = GetWords(sentence);

            switch (parts[0])
            {
                case "$GPGGA":
                    return ParseGga(parts);
                case "$GPRMC":
                    return ParseRmc(parts);
                case "$GPGSV":
                    return ParseGpgsv(parts);
                case "$GPGSA":
                    return ParseGpgsa(parts);
                default:
                    throw new NotSupportedException(
                        $"NMEA sentence type {parts[0]} is not supported."
                    );
            }
        }

        private static string[] GetWords(string sentence)
        {
            return sentence.Split(',');
        }

        private static bool IsValid(string sentence)
        {
            return sentence.Substring(sentence.IndexOf("*") + 1) == GetChecksum(sentence);
        }

        private static string GetChecksum(string sentence)
        {
            int checksum = 0;
            foreach (char character in sentence)
            {
                if (character == '$')
                {
                    // Ignore the dollar sign
                }
                else if (character == '*')
                {
                    // Stop processing before the asterisk
                    break;
                }
                else
                {
                    // Is this the first value for the checksum?
                    if (checksum == 0)
                    {
                        // Yes. Set the checksum to the value
                        checksum = Convert.ToByte(character);
                    }
                    else
                    {
                        // No. XOR the checksum with this character's value
                        checksum = checksum ^ Convert.ToByte(character);
                    }
                }
            }
            // Return the checksum formatted as a two-character hexadecimal
            return checksum.ToString("X2");
        }

        private static NmeaData ParseGga(string[] parts)
        {
            var data = new NmeaData
            {
                Timestamp = ParseTime(parts[1]),
                Latitude = ParseLatitude(parts[2], parts[3]),
                Longitude = ParseLongitude(parts[4], parts[5]),
                Altitude = double.Parse(parts[9], NmeaCultureInfo)
            };

            return data;
        }

        private static NmeaData ParseRmc(string[] parts)
        {
            var data = new NmeaData
            {
                Timestamp = ParseDateTime(parts[9], parts[1]),
                Latitude = ParseLatitude(parts[3], parts[4]),
                Longitude = ParseLongitude(parts[5], parts[6]),
                Speed = double.Parse(parts[7], NmeaCultureInfo) * MPHPerKnot,
                Course = double.Parse(parts[8], NmeaCultureInfo),
                Mode = parts[2]
            };

            return data;
        }

        private static NmeaData ParseGpgsv(string[] parts)
        {
            var data = new NmeaData();

            int pseudoRandomCode = 0;
            int azimuth = 0;
            int elevation = 0;
            int signalToNoiseRatio = 0;

            int count = 0;
            for (count = 1; count <= 4; count++)
            {
                if ((parts.Length - 1) >= (count * 4 + 3))
                {
                    if (
                        parts[count * 4] != ""
                        && parts[count * 4 + 1] != ""
                        && parts[count * 4 + 2] != ""
                        && parts[count * 4 + 3] != ""
                    )
                    {
                        pseudoRandomCode = Convert.ToInt32(parts[count * 4]);
                        elevation = Convert.ToInt32(parts[count * 4 + 1]);
                        azimuth = Convert.ToInt32(parts[count * 4 + 2]);
                        signalToNoiseRatio = Convert.ToInt32(parts[count * 4 + 3]);

                        data.SatellitePRN = pseudoRandomCode;
                        data.Azimuth = azimuth;
                        data.Elevation = elevation;
                        data.SNR = signalToNoiseRatio;
                    }
                }
            }

            return data;
        }

        private static NmeaData ParseGpgsa(string[] parts)
        {
            var data = new NmeaData();

            if (parts[15] != "")
            {
                data.PDOP = double.Parse(parts[15], NmeaCultureInfo);
            }
            if (parts[16] != "")
            {
                data.HDOP = double.Parse(parts[16], NmeaCultureInfo);
            }
            if (parts[17] != "")
            {
                data.VDOP = double.Parse(parts[17], NmeaCultureInfo);
            }

            return data;
        }

        private static DateTime ParseTime(string time)
        {
            if (time.Length < 6)
                throw new ArgumentException("Invalid time format in NMEA sentence.");

            int hour = int.Parse(time.Substring(0, 2));
            int minute = int.Parse(time.Substring(2, 2));
            int second = int.Parse(time.Substring(4, 2));

            return new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                DateTime.UtcNow.Day,
                hour,
                minute,
                second,
                DateTimeKind.Utc
            );
        }

        private static DateTime ParseDateTime(string date, string time)
        {
            if (date.Length != 6 || time.Length < 6)
                throw new ArgumentException("Invalid date or time format in NMEA sentence.");

            int day = int.Parse(date.Substring(0, 2));
            int month = int.Parse(date.Substring(2, 2));
            int year = int.Parse(date.Substring(4, 2)) + 2000;

            int hour = int.Parse(time.Substring(0, 2));
            int minute = int.Parse(time.Substring(2, 2));
            int second = int.Parse(time.Substring(4, 2));

            return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }

        private static double ParseLatitude(string latitude, string direction)
        {
            double lat =
                double.Parse(latitude.Substring(0, 2)) + double.Parse(latitude.Substring(2)) / 60;
            return direction == "S" ? -lat : lat;
        }

        private static double ParseLongitude(string longitude, string direction)
        {
            double lon =
                double.Parse(longitude.Substring(0, 3)) + double.Parse(longitude.Substring(3)) / 60;
            return direction == "W" ? -lon : lon;
        }
    }
}
