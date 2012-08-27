using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiTools.SequenceGenerator.Model
{
    public sealed class Derivation
    {
        readonly string _title;
        readonly string _category;
        readonly string _subcategory;
        readonly string _source;
        readonly List<AnkiField> _derivationSteps;

        public Derivation(string title, string category, string subcategory, string source)
        {
            _title = title;
            _category = category;
            _subcategory = subcategory;
            _source = source;
            _derivationSteps = new List<AnkiField>();
        }

        public void AddStep(string newStep)
        {
            _derivationSteps.Add(new AnkiField(newStep));
        }

        public void CreateAllCards(string ankiImportFilepath)
        {
            ExportCards(ankiImportFilepath, ExportAllCards);
        }

        public void CreateClozeDeletedDerivationCards(string ankiImportFilepath)
        {
            ExportCards(ankiImportFilepath, ExportClozeDeletedCards);
        }

        public void CreateForwardCards(string ankiImportFilepath)
        {
            ExportCards(ankiImportFilepath, ExportForwardCards);
        }

        public void CreateBackwardCards(string ankiImportFilepath)
        {
            ExportCards(ankiImportFilepath, ExportBackwardCards);
        }

        void ExportAllCards(AnkiImportFile importFile)
        {
            ExportBackwardCards(importFile);
            ExportClozeDeletedCards(importFile);
            ExportForwardCards(importFile);
        }

        void ExportClozeDeletedCards(AnkiImportFile importFile)
        {
            foreach (AnkiField field in _derivationSteps)
            {
                importFile.AddRow(CreateClozeDeletedQuestionCard(field), CreateClozeDeletedAnswerCard(field), _category, _subcategory, _source);
            }
        }

        void ExportForwardCards(AnkiImportFile importFile)
        {
            int maxStepPosition = _derivationSteps.Count - 1;
            for (int currentStepPosition = 0; currentStepPosition < maxStepPosition; currentStepPosition++)
            {
                importFile.AddRow(CreateForwardCardQuestion(_derivationSteps[currentStepPosition]), CreateForwardCardAnswer(_derivationSteps[currentStepPosition + 1]), _category, _subcategory, _source);
            }
        }

        void ExportBackwardCards(AnkiImportFile importFile)
        {
            int minimumStepPosition = 1;
            int maxStepPosition = _derivationSteps.Count;
            for (int currentStepPositoin = minimumStepPosition; currentStepPositoin < maxStepPosition; currentStepPositoin++)
            {
                importFile.AddRow(CreateBackwardCardQuestion(_derivationSteps[currentStepPositoin]), CreateBackwardCardAnswer(_derivationSteps[currentStepPositoin - 1]), _category, _subcategory, _source);
            }
        }

        void ExportCards(string importFilepath, Action<AnkiImportFile> exportFunctionality)
        {
            AnkiImportFile importFile = new AnkiImportFile();
            exportFunctionality(importFile);
            importFile.Save(importFilepath);
        }

        string CreateStepsOutline()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("{0}:", _title));
            builder.AppendLine();
            return new AnkiField(builder.ToString(), false).Value;
        }

        string CreateForwardCardQuestion(AnkiField currentStep)
        {
            string outline = CreateStepsOutline();
            StringBuilder builder = new StringBuilder();
            builder.Append(outline);
            builder.AppendLine("Schritt nach:");
            builder.AppendLine();
            builder.AppendLine(currentStep.Value);
            return new AnkiField(builder.ToString(), false).Value;
        }

        string CreateForwardCardAnswer(AnkiField nextStep)
        {
            return nextStep.Value;
        }

        string CreateBackwardCardQuestion(AnkiField step)
        {
            string outline = CreateStepsOutline();
            StringBuilder builder = new StringBuilder();
            builder.Append(outline);
            builder.AppendLine("Schritt vor:");
            builder.AppendLine();
            builder.AppendLine(step.Value);
            return new AnkiField(builder.ToString(), false).Value;
        }

        string CreateBackwardCardAnswer(AnkiField step)
        {
            return step.Value;
        }

        string CreateClozeDeletedAnswerCard(AnkiField clozeDeletedField)
        {
            string outline = CreateStepsOutline();
            StringBuilder builder = new StringBuilder();
            builder.Append(outline);
            int ordinal = 1;
            foreach (AnkiField step in _derivationSteps)
            {
                if (step == clozeDeletedField)
                {
                    builder.AppendLine(string.Format("{0}. {1}", ordinal, step.MarkAsClozeDeletionAnswer().Value));
                }
                else
                {
                    builder.AppendLine(string.Format("{0}. {1}", ordinal, step.Value));
                }
                ordinal++;
            }
            return new AnkiField(builder.ToString(), false).Value;
        }

        string CreateClozeDeletedQuestionCard(AnkiField clozeDeletedField)
        {
            string outline = CreateStepsOutline();
            StringBuilder builder = new StringBuilder();
            builder.Append(outline);
            int ordinal = 1;
            foreach (AnkiField step in _derivationSteps)
            {
                if (step == clozeDeletedField)
                {
                    builder.AppendLine(string.Format("{0}. {1}", ordinal, step.ClozeDelete(step.Value).Value));
                }
                else
                {
                    builder.AppendLine(string.Format("{0}. {1}", ordinal, step.Value));
                }
                ordinal++;
            }
            return new AnkiField(builder.ToString(), false).Value;
        }
    }
}
