using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class FlagImage
    {
        public string Tag { get; set; }
        public string FileName { get; set; }
        public string Bitmap { get; set; }
        public string Size { get; set; }

        public FlagImage(string tag, string filename, string bitmap, string size)
        {
            Tag = tag;
            FileName = filename;
            Bitmap = bitmap;
            Size = size;
        }
    }
}
