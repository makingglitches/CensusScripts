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


            // this was a change that piece of crap zimmerman added which we're pretty sure contains fucked up code
            // they misinterpreted just like EVERYTHING ELSE NORMAL PEOPLE DO.
            string connector =@"Driver ={ Microsoft dBASE Driver(*.dbf)} ; DriverID=277; Dbq = c:\testdbf";

            OdbcConnectionStringBuilder osc = new OdbcConnectionStringBuilder();
            osc.Dsn = "test";

            OdbcConnection ob = new OdbcConnection(osc.ConnectionString);
            
            ob.Open();
            DataTable tables =  ob.GetSchema("Tables");


        }
    }
}
