using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchmarkIt
{
    public class ResultColumn
    {
        public string Header { get; set; }
        public int Width { get; set; }

        public ResultColumn(string header, int width)
        {
            this.Header = header;
            this.Width = width;
        }
    }
}
