
# NMEA Decoder

This project is a simple NMEA decoder implemented in C#. It reads NMEA sentences from a serial port or a file, parses them, and converts the data into a structured format.

## Features

- Supports parsing of various NMEA sentence types, including `$GPGGA`, `$GPRMC`, `$GPGSV`, and `$GPGSA`.
- Allows reading NMEA sentences from a serial port for real-time data processing.
- Provides a file reader for testing and development without requiring actual NMEA hardware.

## Getting Started

### Prerequisites

- .NET SDK 8.0 or later
- A GitHub account for repository management
- Visual Studio Code or another suitable IDE

### Installation

1. **Clone the repository**:
   ```sh
   git clone https://github.com/yourusername/your-repository.git
   cd your-repository
   ```

### Configuration

To switch between reading from a serial port and a file, use the `global.json` file and define the compilation symbol `USE_FILE_READER` for file reading.

## Usage

### Running the Application

1. **Run with serial port** (default):
   ```sh
   dotnet run
   ```

2. **Run with file reader**:
   Update the `.csproj` file to define the `USE_FILE_READER` symbol, then run the application:
   ```sh
   dotnet run
   ```

### Sample NMEA Data

For testing with the file reader, create a file named `sample_nmea_data.txt` with sample NMEA sentences:
```txt
$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47
$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A
$GPGSV,3,1,12,04,77,068,46,05,05,033,,07,11,097,42,08,17,196,45*75
$GPGSA,A,3,04,05,,07,08,,09,10,,12,,14,3.8,1.2,2.9*3D
```

## Project Structure

```
NMEADecoder/
├── .vscode/
│   ├── launch.json
│   ├── tasks.json
├── NMEADecoder.csproj
├── Program.cs
├── NmeaData.cs
├── NmeaParser.cs
├── SerialPortReader.cs
├── FilePortReader.cs
├── sample_nmea_data.txt
├── global.json
└── README.md
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.