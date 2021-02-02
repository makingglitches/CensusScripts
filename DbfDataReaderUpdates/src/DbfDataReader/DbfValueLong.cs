using System.Globalization;
using System.IO;

namespace DbfDataReader
{
    public class DbfValueLong : DbfValue<long?>
    {
        private static readonly NumberFormatInfo _intNumberFormat = new NumberFormatInfo();

        public DbfValueLong(int length) : base(length)
        {
        }

        public override void Read(BinaryReader binaryReader)
        {
            if (binaryReader.PeekChar() == '\0')
            {
                binaryReader.ReadBytes(Length);
                Value = null;
            }
            else
            {
                var stringValue = new string(binaryReader.ReadChars(Length));

                if (long.TryParse(stringValue,
                    NumberStyles.Integer | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
                    _intNumberFormat, out var value))
                    Value = value;
                else
                    Value = null;
            }
        }
    }
}