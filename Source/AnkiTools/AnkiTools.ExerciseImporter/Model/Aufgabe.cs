using System.Collections.Generic;
using System;
using System.IO;
namespace AnkiTools.ExerciseImporter.Model
{
    public sealed class Aufgabe
    {
        readonly string _filepath;
        string _aufgabenstellungNumber;
        string _seitennummer;
        string _aufgabennummer;
        readonly List<Loesung> _loesungen;

        public Aufgabe(string filepath)
        {
            _filepath = filepath;
            _loesungen = new List<Loesung>();

            Parse();
            ParseLoesungen();
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
            foreach (var loesung in _loesungen)
            {
                loesung.CopyTo(destinationDirectory);
            }
        }

        public void ForEachLoesung(Action<Loesung> loesungAction)
        {
            _loesungen.ForEach(loesungAction);
        }

        void Parse()
        {
            string[] components = _filepath.Split('_');
            _aufgabenstellungNumber = Path.GetFileName(components[0]);
            _seitennummer = components[1];
            _aufgabennummer = components[2];
        }

        void ParseLoesungen()
        {
            string[] loesungenFilenames = Directory.GetFiles(Path.GetDirectoryName(_filepath), string.Format("{0}_{1}_{2}_Loesung_*", _aufgabenstellungNumber, _seitennummer, _aufgabennummer));
            foreach (string loesungFilename in loesungenFilenames)
            {
                Loesung newLoesung = new Loesung(loesungFilename);
                _loesungen.Add(newLoesung);
            }
        }
    }
}