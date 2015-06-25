using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingFileSplitter;
using Xunit;

namespace BillingFileSplitter_Tests
{
    public class FileGroup_Tests
    {
        [Fact(DisplayName = "FileGroup Creates FileNames")]
        public void FileGroup_Creates_FileNames()
        {
            var fileGroup = new FileGroup(@"c:\test.txt");
            Assert.Equal(@"c:\test_NEWREN.txt", fileGroup.NewRenFileName);
            Assert.Equal(@"c:\test_CANREIN.txt", fileGroup.CanReinFileName);
            Assert.Equal(@"c:\test_ALLELSE.txt", fileGroup.AllElseFileName);
        }
    }
}
