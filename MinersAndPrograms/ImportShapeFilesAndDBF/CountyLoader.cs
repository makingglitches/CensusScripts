using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.IO.Compression;
using CensusFiles;
using System.Threading;

namespace ImportShapeFilesAndDBF
{
    public class CountyLoader
    {

        public string ZipDirectory { get; set; }
        public bool EmptyCountiesTable { get; set; }

        public string line;

        public CountyLoader(string zipdirectory, bool emptyCountiesTable = false, bool eventmode = false)
        {
            ZipDirectory = zipdirectory;
            EmptyCountiesTable = emptyCountiesTable;

            for (int x = 0; x < 50; x++)
            {
                line += " ";
            }

            EventMode = eventmode;
        }


        public bool EventMode { get; set; }


        public void LoadZips(string table = "Roads")
        {
            if (EventMode)
            {
                CountyRecord.OnParse += CountyRecord_OnParse;
                CountyRecord.OnFileLength += CountyRecord_OnFileLength;
            }



            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.IntegratedSecurity = true;
            scsb.DataSource = "localhost";
            scsb.InitialCatalog = "Geography";

            scon = new SqlConnection(scsb.ConnectionString);

            Console.WriteLine("Opening Sql Connection.");

            scon.Open();

            var zipfiles = Directory.GetFiles(ZipDirectory, "*.zip");
            var outputdir = ZipDirectory + "\\output\\";

            if (EmptyCountiesTable)
            {
                Console.WriteLine("Emptying Counties Table");
                SqlCommand trunccom = new SqlCommand("truncate table dbo.Counties", scon);
                trunccom.ExecuteNonQuery();
            }


            foreach (string filename in zipfiles)
            {
                Console.WriteLine("Processing Archive: " + filename);

                if (Directory.Exists(outputdir))
                {
                    Directory.Delete(outputdir, true);
                }

                Directory.CreateDirectory(outputdir);

                ZipFile.ExtractToDirectory(filename, outputdir);

                string[] files = Directory.GetFiles(outputdir, "*.dbf");

                insrec = CountyRecord.GetInsert(scon);

                x = Console.CursorLeft;
                y = Console.CursorTop;

                Console.Write(line);
                index = 1;

                if (!EventMode)
                {
                    List<CountyRecord> records = CountyRecord.ParseDBFFile(files[0], scon, true, false, false);
                    Console.WriteLine("Loaded " + records.Count.ToString() + " records.");

                    foreach (CountyRecord r in records)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.WriteLine("Processing Record " + index.ToString() + " of " + records.Count.ToString());

                        r.MapParameters(insrec);
                        insrec.ExecuteNonQuery();
                        index++;
                    }
                }
                else
                {
                    Task t = Task.Run(() =>
                    {
                        CountyRecord.ParseDBFFile(files[0], scon, true, false, true);
                    });

                    t.Wait();

                }

                Console.WriteLine();
            }
            scon.Close();

            Console.WriteLine("Closed SQL Connection");


        }

        private long index;
        private SqlCommand insrec;
        private SqlConnection scon;
        private long length;
        private int x;
        private int y;

        private void CountyRecord_OnFileLength(long obj)
        {
            length = obj;
        }

        /*
         * now you may be looking at this code and thinking
           'wtf is this guy doing ??'
            the answer is that the machine this was running on was competing with an ffmpeg process on limited ram
            which was causing sql server connections to fuck up.

            so you'll not the lines in the 'onparse' event handlers and hell also note the fact that instead of completely rewriting the code
            to just let the developer decide when to read and use a record, i decided to go another route because i'm lazy
            well thats pretty self explanatory
            but 'why the 2000 ms thread wait and why the scon.open()/close()'
            the answer requires a brief description of microsoft
            microsoft makes sure its product fail to stay profitable
            and .net while all nice and shiny and pretty is  DIRTY WHORE
            AND THAT DIRTY WHORE NEEDS SLAPPED ONCE AND AWHILE LIKE ALL DIRRTY WHORES DO !
            The hypocritical responses that likely will elicit amongst a multitude of dirty whores aside,
            MICROSOFT IS A DIRTY WHORE OR IT WOULD BE LINUX AS DEVELOPED BY A PRIVATE ORGANIZATION WITH A STAKE IN PERFORMANCE !!!
            That being said, processes don't play nice.
            Sql server's memory usage kept spiking as it struggled SOMEHOW to write records it was receiving to disk.
            This caused a 'memory pressure' exception saying basicallY 'fuck you you bastard !' to occur indicating that 
            the .net framework was defaulting to said peer pressure from its cousin fucker sql server overlord !
            and being not a moron i figured... oh... 2 seconds is a long time in the computing... so.. and also the sql connection objects in the past are also dirty masochistic motherfuckers that dont somehow spare the heap... so... i'll just let the GC finish and also the sql server instance to drop what it's buffer to disk.. and bob's your uncle.. somehow... at least for this large ass file it worked and my system played nice for itself so i could get some fucking work done on a limited goddamn budget !
            so.. THATS WHY ! and once again figured this deserved some fucking mention before one of said dirty fucked up whores runs off with my work or flat out deletes it because they like living in the same time period over and over when they and their ilk should JUST BE FUCKING SHOT IN THEIR DIRTY WHORE MOUTHS !!!!
            moment of truth will be when i check to see if the record counts are the same and then i will kiss uncle daddy sql servers ass as being one of the few fucking things microsoft got right !!
            seriously... ram having expanded so far and and a computer in my pocket am i the only one who wonders what kind of sloppy ass shit these people did to make budget computing require such personalized hacky attention ?
            oh btw the year is past 2030 you fucking assholes ! get your filthy baby raping fucking hands off my code !
            actually saved several goddamn percent in ram usage as well ! hurrah !
            now to open the only other fucking thing ms did right which was sql management studio and check !
            booya we're golden !
            now maybe just maybe these little boy raping pieces of shit will allow me to keep my fucking work so i can move on to what this damn thing was intended for
            instead of sitting and wondering what kind retard tree these mongrel monkey fuckers fell out of in between blacking out when 'it' their little
            retarded 'lets sell all our asses and make the world miserable till someone kills us or we look like our faces were dipped in acid' fucked up country
            wide celebration of their fucking daddies not loving them is over !
*/

        private void CountyRecord_OnParse(CountyRecord obj)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Processing Record " + index.ToString() + " of " + length.ToString());

            try
            {
                obj.MapParameters(insrec);
                insrec.ExecuteNonQuery();
            }
            catch
            {
                scon.Close();
                scon.Open();
                Thread.Sleep(2000);
                insrec.ExecuteNonQuery();

            }

            index++;
        }
    }
}
