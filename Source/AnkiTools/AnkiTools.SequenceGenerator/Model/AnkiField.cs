using System;

namespace AnkiTools.SequenceGenerator.Model
{
    public sealed class AnkiField
    {
        readonly string _value;

        public AnkiField(string value)
            : this(value, true)
        {
        }

        public AnkiField(string value, bool escapeHtml)
        {
            if (escapeHtml)
            {
                _value = CleanValue(value);
            }
            else
            {
                _value = value;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        public AnkiField ClozeDelete(string deletionPart)
        {
            string clozeDeletedValue = _value.Replace(deletionPart, @"<span style=""font-weight:600; color:#0000ff;"">[...]</span>");
            return new AnkiField(clozeDeletedValue, false);
        }

        public AnkiField MarkAsClozeDeletionAnswer()
        {
            return new AnkiField(string.Format("<span style=\"font-weight:600; color:#0000ff;\">{0}</span>", _value), false);
        }

        public AnkiField MarkAsOneUnit()
        {
            return new AnkiField(string.Format("\"{0}\"", _value), false);
        }

        string CleanValue(string value)
        {
            return value
                //.Replace("<", "&lt;")
                //.Replace(">", "&gt;")
                .Replace(Environment.NewLine, "<br>");
        }
    }
}