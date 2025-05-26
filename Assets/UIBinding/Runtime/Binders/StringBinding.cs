using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DataBinding
{
    [Serializable]
    public class StringBinding
    {
        [SerializeField] private string bindingTypeName; 
        
        public string formattedString;
        public BindingField[] parameters;

        private object[] _values;

        public string GetFormattedString(object obj, Type bindingType)
        {
            if (_values == null || _values.Length != parameters.Length)
            {
                _values = new object[parameters.Length];
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                _values[i] = parameters[i].GetBindingVariable(obj, bindingType).stringValue;
            }
            return string.Format(formattedString, _values);
        }
        


        public void OnValidate()
        {
            if (parameters.Length != CountParameters(formattedString))
            {
                Array.Resize(ref parameters, CountParameters(formattedString));
            }
        }
        
        private int CountParameters(string formatString)
        {
            const string pattern = @"(?<!\{)(?>\{\{)*\{\d(.*?)";
            var matches = Regex.Matches(formatString, pattern);
            var uniqueMatchCount = matches.OfType<Match>().Select(m => m.Value).Distinct().Count();
            var parameterMatchCount = (uniqueMatchCount == 0) ? 0 : matches.OfType<Match>().Select(m => m.Value).Distinct().Select(m => int.Parse(m.Replace("{", string.Empty))).Max() + 1;
            return parameterMatchCount;
        }
    }
}
