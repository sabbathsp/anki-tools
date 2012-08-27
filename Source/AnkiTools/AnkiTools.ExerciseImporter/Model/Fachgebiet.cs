using System.Collections.Generic;
using System.IO;
using AnkiImportApi.Core;
using AnkiImportApi.Core.Values;
using System;
namespace AnkiTools.ExerciseImporter.Model
{
    public sealed class Fachgebiet
    {
        readonly string _name;
        readonly string _sourceDirectory;
        readonly string _ankiDirectory;

        public Fachgebiet(string name, string sourceDirectory, string ankiDirectory)
        {
            _name = name;
            _sourceDirectory = sourceDirectory;
            _ankiDirectory = ankiDirectory;
        }

        public void Export(string csvFilepath)
        {
            var aufgabenstellungen = GatherAufgabenstellungen();
            CreateCsvFile(csvFilepath, aufgabenstellungen);
            CopyImagesToAnkiDirectory(aufgabenstellungen);
        }

        void CreateCsvFile(string csvFilepath, List<Aufgabenstellung> aufgabenstellungen)
        {
            ImportFile file = new ImportFile(csvFilepath);
            foreach (var aufgabenstellung in aufgabenstellungen)
            {
                aufgabenstellung.ForEachAufgabe(aufgabe =>
                    {
                        string aufgabeFilepath = Path.GetFileName(aufgabe.Filepath);
                        
                        
                        ImageValue img = new ImageValue(aufgabeFilepath);
                        IValue fragestellung = new PlainTextValue(string.Format("{0}{1}{1}", aufgabenstellung.Fragestellung, Environment.NewLine));
                        IValue problem = new HtmlValue(string.Format("{0}{1}", fragestellung.Value, img.Value));

                        IValue solution = null;
                        List<string> loesungImages = new List<string>();
                        aufgabe.ForEachLoesung(newLoesung =>
                            {
                                string loesungFilepath = Path.GetFileName(newLoesung.Filepath);
                                loesungImages.Add(new ImageValue(loesungFilepath).Value);
                            });
                        solution = new HtmlValue(string.Join(new PlainTextValue(Environment.NewLine).Value, loesungImages));
                        IValue fachgebiet = new PlainTextValue(_name);
                        IValue herkunft = new PlainTextValue(aufgabenstellung.Herkunft);
                        IValue seitenzahl = new PlainTextValue(aufgabe.Seitennummer);
                        IValue aufgabennnr = new PlainTextValue(aufgabe.Aufgabennummer);

                        file.AddField(problem, solution, fachgebiet, herkunft, seitenzahl, aufgabennnr);
                    });
            }
            file.Save();
        }

        void CopyImagesToAnkiDirectory(List<Aufgabenstellung> aufgabenstellungen)
        {
            foreach (var aufgabenstellung in aufgabenstellungen)
            {
                aufgabenstellung.ForEachAufgabe(aufgabe => aufgabe.CopyTo(_ankiDirectory));
            }
        }

        List<Aufgabenstellung> GatherAufgabenstellungen()
        {
            List<Aufgabenstellung> aufgabenstellungen = new List<Aufgabenstellung>();
            string[] files = Directory.GetFiles(_sourceDirectory, "Aufgabenstellung_*.txt");
            foreach (string file in files)
            {
                var aufgabenstellung = new Aufgabenstellung(file);
                aufgabenstellung.GatherAufgaben();
                aufgabenstellungen.Add(aufgabenstellung);
            }
            return aufgabenstellungen;
        }
    }
}