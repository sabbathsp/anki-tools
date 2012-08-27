using System.IO;

namespace AnkiTools.ExerciseImporter.Model
{
    public sealed class Loesung
    {
        readonly string _filepath;
        string _aufgabenstellungNumber;
        string _seitennummer;
        string _aufgabennummer;

        public Loesung(string filepath)
        {
            _filepath = filepath;
            Parse();
        }

        public string Filepath
        {
            get
            {
                return _filepath;
            }
        }

        public string AufgabenstellungNumber
        {
            get
            {
                return _aufgabenstellungNumber;
            }
        }

        public string Seitennummer
        {
            get
            {
                return _seitennummer;
            }
        }

        public string Aufgabennummer
        {
            get
            {
                return _aufgabennummer;
            }
        }

        public void CopyTo(string destinationDirectory)
        {
            File.Copy(_filepath, Path.Combine(destinationDirectory, Path.GetFileName(_filepath)));
        }

        void Parse()
        {
            string[] components = _filepath.Split('_');
            _aufgabenstellungNumber = components[0];
            _seitennummer = components[1];
            _aufgabennummer = components[2];
        }
    }
}