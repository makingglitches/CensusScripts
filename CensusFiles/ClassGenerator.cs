using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;
using System.IO;
using System.Data;
using System.Data.Common;

namespace CensusFiles
{

    // when these pieces of shit say they make happy stories for us to inspire us so they can fuck us up later
    // i think that was just that piece of zimmerman telling his ordinary batch of half truths and half lies
    // however in this case mostly a lie since, why would they emulate us at all if they thought their 
    // psychological mutilation made them so superior ? again :P
    public class ClassGenerator
    {

        public static void WriteClassBase(string ClassName, string filename, DbDataReader  dread)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Data.SqlClient;
            using System.Data.SqlTypes;
            using System.Data.Common;
            using System.Data;
            using DbfDataReader;

            namespace CensusFiles
            {
                public abstract class "+ClassName+@":IBaseReader
                {");

            string readmethod = "\r\npublic void Read(DbDataReader dread)\r\n{\r\n";

            for (int x = 0; x < dread.FieldCount; x++)
            {
                var type = dread.GetFieldType(x);
                var name = dread.GetName(x);

                var nullable = type.Name.StartsWith("Nullable");

                if (type.GenericTypeArguments.Length > 0)
                {
                    type = type.GenericTypeArguments.First();
                }

                var fulltype = (nullable ? "Nullable<" : string.Empty) +
                    type.Name + (nullable ? ">" : string.Empty);

                sb.AppendLine("     public " + fulltype + " " + name + "{get;set;}");

                readmethod += "this." + name + "=(" + fulltype + ") dread[\"" + name + "\"];\r\n";
            }

            readmethod += "\r\n}";

            sb.AppendLine(readmethod);

            sb.AppendLine(" }");
            sb.AppendLine("}");

            File.WriteAllText(filename, sb.ToString());


        }
    }
}
