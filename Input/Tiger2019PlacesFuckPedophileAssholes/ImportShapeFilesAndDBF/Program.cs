using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;

namespace ImportShapeFilesAndDBF
{
    class Program
    {
        static void Main(string[] args)
        {

            // // apparently the msdos naming problem still exists in the dbf portion of the jet 4.0 driver.
            // // funny how everytime zimmerman and I discussed this he indicated a different fucking issue !

            // string dbffile = @"tl_2019_01_place.dbf";

            // string dblocation = //@"C:\Users\John\Documents\QrCode\Input\Tiger2019PlacesFuckPedophileAssholes\";
            //                     @"C:\testdbf";

            //     OleDbConnectionStringBuilder olec =
            //     new OleDbConnectionStringBuilder("Provider = Microsoft.Jet.OLEDB.4.0; Extended Properties = dBASE IV; User ID = Admin; Password =;");

            // //olec.Provider = "Microsoft.ACE.OLEDB.12.0";

            // olec.DataSource = dblocation;

            // OleDbConnection oc = new OleDbConnection(olec.ConnectionString);

            // oc.Open();

            // // ole provider information returns a list of crap similar to schema layout in sql server dbo, sys etc.

            //DataTable schemas = oc.GetSchema("Tables");

            // foreach ( DataRow dr in schemas.Rows)
            // {

            //     Console.WriteLine(dr["TABLE_NAME"]);

            // }


            // string sqlcomm = "select * from {0}";
            // string tablename = schemas.Rows[0]["TABLE_NAME"].ToString()+".dbf";

            // OleDbCommand comm = new OleDbCommand(string.Format(sqlcomm, tablename),oc);

            // var reader = comm.ExecuteReader();

            // while (reader.Read())
            // {
            //     var f = reader[0];
            // }

            // // they fucked with dataadapter and either introduced a bug or didnt fix it pretty sure because they're garbage as usual.
            // OleDbDataAdapter dat = 
            //     new OleDbDataAdapter(string.Format(sqlcomm,tablename), oc);

            // DataSet tabedata = new DataSet();
            // dat.Fill(tabedata);

            // oc.Close();


         // works with shortened non ntfs style filenames
         // update asshole indicated for trieber driver
         // kind of indicates they place bugs in shit to try to force people into certain formats of behavior
         // comp science would be a really convenient way of stripping people cognitively like all science and math
            OdbcConnectionStringBuilder osc = new OdbcConnectionStringBuilder();
            osc.Dsn = "fuckzim";

            // not supposed to create fucking odbc dsn;s ! 
            OdbcConnection ob = new OdbcConnection(osc.ConnectionString);
            
            ob.Open();

            // yup same stupid name format problem. wtf.
            DataTable tables =  ob.GetSchema("Tables");

            string tb = tables.Rows[0]["TABLE_NAME"].ToString();

            OdbcCommand comm = new OdbcCommand("select * from " + tb,ob);
            var reader = comm.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader[0]);
            }


            ob.Close();


        }
    }
}
