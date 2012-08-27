using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace AnkiTools.SequenceGenerator.Model
{
    public sealed class AnkiImportFile
    {
        readonly List<AnkiField[]> _rows;
        string _tag;

        const string FIELD_SEPARATOR = "|";

        public AnkiImportFile()
        {
            _rows = new List<AnkiField[]>();
            _tag = string.Empty;
        }

        public void AddRow(params string[] row)
        {
            _rows.Add(row.Select(r => new AnkiField(r)).ToArray());
        }

        public void AddTag(string tag)
        {
            _tag = tag;
        }

        public void Save(string filepath)
        {
            using (StreamWriter importFile = new StreamWriter(filepath, false, GetAnkiFileEncoding()))
            {
                if (!string.IsNullOrWhiteSpace(_tag))
                {
                    importFile.WriteLine("tags:" + _tag);
                    importFile.WriteLine();
                }

                foreach (AnkiField[] row in _rows)
                {
                    importFile.WriteLine(FormatRow(row));
                }
            }
        }

        Encoding GetAnkiFileEncoding()
        {
            return Encoding.UTF8;
        }

        string FormatRow(AnkiField[] row)
        {
            return string.Join(FIELD_SEPARATOR, row.Select(f => f.Value));
        } 
    }
}