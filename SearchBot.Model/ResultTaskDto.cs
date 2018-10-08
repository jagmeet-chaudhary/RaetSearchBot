using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Model
{
    public class ResultTaskDto
    {
        public IList<TaskDto> Items { get; set; }
        public int Count { get; set; }

        //public string NextPageLink { get; set; }
    }
}
