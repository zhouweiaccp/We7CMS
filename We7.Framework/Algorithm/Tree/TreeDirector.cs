using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework.Algorithm.Tree
{
    public class TreeDirector
    {
        private ITreeStructureBuilder builder;

        public void ITreeStructureBuilder(ITreeStructureBuilder builder)
        {
            this.builder = builder;
        }

        public void Direct()
        {
            builder.BuildStart();
            Build(builder.Root, 0);
            builder.BuildEnd();
        }

        private void Build(ITreeStructure parent, int deep)
        {
            for(int i=0;i<parent.Children.Count;i++)
            {
                var arg=new TreeArgument{
                     Deep=++deep, 
                     IsFirstChild=i==0, 
                     IsLastChild=i==parent.Children.Count-1, 
                     Parent=parent, 
                     Root=builder.Root, 
                     SiblingIndex=i, 
                     Params=new Dictionary<string,object>{
                        {"prefix",""}
                     }
                };
                builder.BuildNode(parent.Children[i],arg);
            }
            foreach(ITreeStructure node in parent.Children)
            {
            }
        }
    }

    public class TreeListBuilder : ITreeStructureBuilder
    {
        private string prefix = "";
        private ITreeStructure parent;
        private List<ITreeStructure> trees = new List<ITreeStructure>();

        #region ITreeBuilder 成员

        public ITreeStructure Root
        {
            get { throw new NotImplementedException(); }
        }

        public void BuildNode(ITreeStructure node, TreeArgument arg)
        {
        }

        public void BuildStart()
        {
            throw new NotImplementedException();
        }

        public void BuildEnd()
        {
            throw new NotImplementedException();
        }

        public List<ITreeStructure> GetTreeList()
        {
            return trees;
        }

        #endregion
    }
}
