using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECSCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Component : Attribute
    {
        public string Type { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
