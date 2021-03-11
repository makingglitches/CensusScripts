using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using CensusFiles.Loaders;
using System.Data.SqlClient;
using System.Xml;

namespace CensusFiles.Utilities
{

    // for some reason the gov packages species archives different than literally EVERY fucking thing they design.
    public class SpeciesRepackager
    {

        private class speciespiece
        {
            public string ArchiveName { get; set; }
            public string CommonName { get; set; }
            public string ScientificName { get; set; }
            public string DownloadGuid { get; set; }
            public string ContentKey { get; set; } 
            public bool DescriptorXMLMatches { get; set; }
        }


        public static void Repackage(string inputzipdir, string outputzipdir, string connectionstring)
        {
            SqlConnection scon = new SqlConnection(connectionstring);
            scon.Open();

            SqlCommand getspecies = new SqlCommand("select s.ArchiveName,s.CommonName,s.ScientificName,s.DownloadGuid from dbo.species s", scon);

            var dr = getspecies.ExecuteReader();

            List<speciespiece> species = new List<speciespiece>();
            
            while ( dr.Read())
            {
                speciespiece p = new speciespiece()
                {
                    ArchiveName = dr["ArchiveName"].ToString(),
                    CommonName = dr["CommonName"].ToString(),
                    DownloadGuid = dr["DownloadGuid"].ToString(),
                    ScientificName = dr["ScientificName"].ToString()
                };

                species.Add(p);
            }

            List<speciespiece> matchedPieces = new List<speciespiece>();

            dr.Close();
            scon.Close();

            // thats a new attempt for them to try to curb my progress heh.
            // or perhaps not so much now.


            inputzipdir = inputzipdir.Trim().EndsWith("\\") ?
                inputzipdir.Trim().Substring(0, inputzipdir.Trim().Length - 1) : inputzipdir.Trim();

            outputzipdir = outputzipdir.Trim().EndsWith("\\") ?
               outputzipdir.Trim().Substring(0, outputzipdir.Trim().Length - 1) : outputzipdir.Trim();

            string[] files = Directory.GetFiles(inputzipdir, "*.zip");


            Console.WriteLine("Discovered " + files.Length.ToString() + " zips");

            Dictionary<string, string> filekey = new Dictionary<string, string>();

            foreach (string file in files)
            {
                Console.WriteLine("Processing " + Path.GetFileName(file));

                ZipArchive za = ZipFile.OpenRead(file);

                speciespiece match = null;

                foreach ( speciespiece p  in species)
                {
                    if (p.ArchiveName==Path.GetFileName(file))
                    {
                        match = p;
                        break;
                    }
                }


                // better got that species database populated !
                if (match == null)
                {
                    Console.WriteLine(Path.GetFileName(file) + " has no matches in dbo.Species !");
                    continue;
                }

                // reduce the number of items to scan.
                species.Remove(match);
                matchedPieces.Add(match);
                

                foreach (ZipArchiveEntry ze in za.Entries)
                {

                    if (ze.Name.ToLower().EndsWith(".xml"))
                    {
                        // process xml descriptor file to double check crap.

                        ze.ExtractToFile("desc.xml");
   
                        XmlDocument x = new XmlDocument();

                        x.Load("desc.xml");

                        var titlenode = x.SelectSingleNode("/metadata/idinfo/citation/citeinfo/title");

                        string speciesinfo = titlenode.InnerText;
                        string commonname = speciesinfo.Substring(0, speciesinfo.IndexOf("(")).Trim().ToLower();
                        string latinname = speciesinfo.Substring(speciesinfo.IndexOf("(") + 1,
                            speciesinfo.IndexOf(")") - speciesinfo.IndexOf("(") - 1).Trim().ToLower();
                        string contentdesc = ze.Name.Split(new char[] { '_' })[0].Trim();

                        // todo: insert compare code here once sql records are added.
                        match.ContentKey = contentdesc;

                        match.DescriptorXMLMatches = match.CommonName.Trim().ToLower() == commonname &&
                                                     match.ScientificName.Trim().ToLower() == latinname;

                    }
                    if (ze.Name.ToLower().EndsWith(".zip"))
                    {
                        // earlier tests indicate all the species subcontents files have a prefix which is unqiue.
                        // this could be used as a primary key.
                        filekey.Add(Path.GetFileName(file), ze.Name.Split(new char[] { '_' })[0]);

                        string tempdir = outputzipdir + "\\temp";
                        string contentdir = tempdir + "\\contents";
                        string repacked = outputzipdir + "\\" + Path.GetFileName(file);

                        if (Directory.Exists(tempdir))
                        {
                            Directory.Delete(tempdir, true);
                        }

                        Directory.CreateDirectory(tempdir);
                        Directory.CreateDirectory(contentdir);

                        ze.ExtractToFile(tempdir + "\\temp.zip");

                        ZipFile.ExtractToDirectory(tempdir + "\\temp.zip", contentdir);

                        if (File.Exists(repacked))
                        {
                            File.Delete(repacked);
                        }

                        // so you know what my magnum opus was ?
                        // I got a fuck ton of pedophiles stuck who were trying to heartlessly steal
                        // my creations and work and pass it off as their own 
                        // as these completely immoral garbage tend to with everything because they're souless
                        // meanwhile the garbage that keeps trying to entrap me and delete decades of life is old
                        // fat and in their own kind of prison, and they'll die there.
                        // and was this at an acceptable cost ? no not really. 
                        // these weird little dont give a damn about anyone or anything garbage fucks 
                        // are a disease and that disease was/is spreading because their parents are the same.
                        // in a way discovering my own parents were garbage that were pretending otherwise
                        // was a tad better for me morally. but still not ideal because the world doesnt need
                        // to be filled with thrillkilling thrillraping garbage that wants everyone to suffer
                        // or just doesnt have ANY standards or values whatsoever.
                        // we've had quite enough of those little whores.

                        ZipFile.CreateFromDirectory(contentdir,
                                                   repacked);

                    }
                }



            }

            var s = File.Create(outputzipdir + "\\keys.txt");

            StreamWriter sw = new StreamWriter(s);

            foreach (KeyValuePair<string, string> kvp in filekey)
            {
                sw.WriteLine(kvp.Key + "\t" + kvp.Value);
            }

            sw.Flush();
            s.Flush();
            s.Close();

            // how many times this long time period has passed, no idea, but everyone is freaked out and scared
            // when i end up back somewhere else, who knows if its an act or theyre being awoken from 
            // a very long slumber if that is even the case but they certainly all seem MORE miserable
            // and less alive everytime i see any of them before i even comment on these things.
            // shame i have had to continually reproduce this/
            // noone really explained any of the fucked up rules that allow me to avoid them playing
            // child molester drinking games with one another to do literally anything so i'm kind of stuck it would seem
            Console.WriteLine("wrote keys file.");

        }
    }
}
