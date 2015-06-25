using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingFileSplitter
{
    public class FileGroup
    {
        public List<string> NewRenRecords = new List<string>();
        public List<string> CanReinRecords = new List<string>();
        public List<string> AllElseRecords = new List<string>();

        public string NewRenFileName;
        public string CanReinFileName;
        public string AllElseFileName;

        public FileGroup(string rootFileName, bool deleteFilesUponInitialization = true)
        {
            NewRenFileName = AppendString(rootFileName, "_NEWREN");
            CanReinFileName = AppendString(rootFileName, "_CANREIN");
            AllElseFileName = AppendString(rootFileName, "_ALLELSE");

            if (!deleteFilesUponInitialization)
                return;

            if (File.Exists(NewRenFileName))
                File.Delete(NewRenFileName);

            if (File.Exists(CanReinFileName))
                File.Delete(CanReinFileName);

            if (File.Exists(AllElseFileName))
                File.Delete(AllElseFileName);
        }

        public string AppendString(string filename, string pieceToAppend)
        {
            var fileNameOnly = Path.GetFileNameWithoutExtension(filename);
            var fileNameAppended = fileNameOnly + pieceToAppend;
            return filename.Replace(fileNameOnly, fileNameAppended);
        }

        public void SaveAllFiles()
        {
            if (NewRenRecords.Any())
                File.WriteAllLines(NewRenFileName, NewRenRecords);

            if (CanReinRecords.Any())
                File.WriteAllLines(CanReinFileName, CanReinRecords);

            if (AllElseRecords.Any())
                File.WriteAllLines(AllElseFileName, AllElseRecords);
        }
    }
}
