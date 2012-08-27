using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AnkiTools.ExerciseImporter.Model
{
    public sealed class Aufgabenstellung
    {
        readonly string _filepath;
        string _fragestellung;
        string _herkunft;
        List<Aufgabe> _aufgaben;
        string _aufgabenstellungsnr;

        public Aufgabenstellung(string filepath)
        {
            _filepath = filepath;
            _aufgaben = new List<Aufgabe>();

            Parse();
        }

        public string Fragestellung
        {
            get
            {
                return _fragestellung;
            }
        }

        public string Herkunft
        {
            get
            {
                return _herkunft;
            }
        }

        public void ForEachAufgabe(Action<Aufgabe> action)
        {
            _aufgaben.ForEach(action);
        }

        public void GatherAufgaben()
        {
            string directory = Path.GetDirectoryName(_filepath);
            string[] aufgabeFiles = Directory.GetFiles(directory, string.Format("{0}_*_Aufgabe.PNG", _aufgabenstellungsnr));
            foreach (string aufgabeFile in aufgabeFiles)
            {
                _aufgaben.Add(new Aufgabe(aufgabeFile));
            }
        }

        void Parse()
        {
            string[] lines = File.ReadAllLines(_filepath, Encoding.Default);
            _fragestellung = lines[0];
            _herkunft = lines[1];
            _aufgabenstellungsnr = Path.GetFileNameWithoutExtension(_filepath).Split('_')[1];
        }
    }
}