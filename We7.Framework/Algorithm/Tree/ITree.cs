using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework.Algorithm
{
    public interface ITreeStructureBuilder
    {
        ITreeStructure Root { get; }

        void BuildNode(ITreeStructure node, TreeArgument arg);

        void BuildStart();

        void BuildEnd();
    }

    public class TreeArgument
    {
        public int Deep { get; set; }

        public ITreeStructure Parent { get; set; }

        public ITreeStructure Root { get; set; }

        public bool IsLastChild { get; set; }

        public bool IsFirstChild { get; set; }

        public int SiblingIndex { get; set; }

        public Dictionary<string, object> Params { get; set; }
    }

    public interface ITreeStructure
    {
        string Name { get; set; }

        IList<ITreeStructure> Children { get; set; }
    }
}
