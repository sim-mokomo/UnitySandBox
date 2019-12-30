using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MokomoGames
{
    public class ClassBuilder
    {
        private string _className;
        private List<VariableBuilder> _variables = new List<VariableBuilder>();
        private List<ClassBuilder> _classBuilders = new List<ClassBuilder>();

        public ClassBuilder(string className)
        {
            _className = className;
        }
        
        public void AddClasses(ClassBuilder classBuilder)
        {
            _classBuilders.Add(classBuilder);
        }

        public void AddVariables(IReadOnlyList<VariableBuilder> variables)
        {
            foreach (var variable in variables)
            {
                _variables.Add(variable);
            }
        }

        public string Format(bool enableTab=false)
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTab(enableTab,$"public static class {_className}");
            builder.AppendLineWithTab(enableTab,"{");
            
            foreach (var classBuilder in _classBuilders)
            {
                builder.AppendLine(classBuilder.Format(enableTab: true));
            }
            
            foreach (var variable in _variables)
            {
                builder.AppendLine(variable.Format(enableTab: true));
            }
            
            builder.AppendLineWithTab(enableTab,"}");
            return builder.ToString();
        }
    }
}
