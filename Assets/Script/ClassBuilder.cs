﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MokomoGames
{
    public class ClassBuilder
    {
        public string ClassName;
        private List<VariableBuilder> _variables = new List<VariableBuilder>();
        private List<ClassBuilder> _classBuilders = new List<ClassBuilder>();

        public ClassBuilder(string className)
        {
            ClassName = className;
        }
        
        public void AddClass(ClassBuilder classBuilder)
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

        public string Format(int tabNum)
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTab(tabNum,$"public static class {ClassName}");
            builder.AppendLineWithTab(tabNum,"{");
            
            foreach (var classBuilder in _classBuilders)
            {
                builder.AppendLine(classBuilder.Format(tabNum + 1));
            }
            
            foreach (var variable in _variables)
            {
                builder.AppendLine(variable.Format(tabNum + 1));
            }
            
            builder.AppendLineWithTab(tabNum,"}");
            return builder.ToString();
        }
    }
}
