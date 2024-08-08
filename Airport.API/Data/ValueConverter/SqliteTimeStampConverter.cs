using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Airport.API.Data.ValueConverter
{
    class SqliteTimeStampConverter : ValueConverter<byte[], string>
    {
        public SqliteTimeStampConverter() : base(
            v => v == null ? null : ToDb(v),
            v => v == null ? null : FromDb(v)) { }
        static byte[] FromDb(string v) =>
            v.Select(c => (byte)c).ToArray(); // Encoding.ASCII.GetString(v)
        static string ToDb(byte[] v) =>
            new(v.Select(b => (char)b).ToArray()); // Encoding.ASCII.GetBytes(v))
    }
}
