using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.ServiceLayer.Models
{
    public class CreateNote
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
