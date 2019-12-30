using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MokomoGames
{
    public class VariableBuilder
    {
        public string Name { get; private set; }
        public string DefaultValue { get; private set; }

        public VariableBuilder(string name, string defaultValue)
        {
            this.Name = name;
            this.DefaultValue = defaultValue;
        }

        public string Format(bool enableTab=false)
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTab(enableTab: enableTab,$"\tpublic static readonly string {Name} = \"{DefaultValue}\";");
            return builder.ToString();
        }
    }
}
