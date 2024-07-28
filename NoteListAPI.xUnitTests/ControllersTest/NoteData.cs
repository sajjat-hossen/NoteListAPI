using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.xUnitTests.ControllersTest
{
    public static class NoteData
    {
        public static IEnumerable<object[]> CreateNoteData()
        {
            return new List<object[]>
            {
                new object[] { "Title 1", "Description 1" }
            };
        }
    }
}
