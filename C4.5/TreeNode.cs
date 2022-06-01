using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Analysis;

namespace C4._5
{
    enum Type
    {
        Discrete, 
        Numeric
    }
    internal class TreeNode
    {
        public bool NodeIsLeaf { get; set; }
        public string BestAttribute { get; set; }
        public string PreviousNodeValue { get; set; }
        public string MajorityClass { get; set; }
        public List<TreeNode> Nodes { get; set; }

        public TreeNode()
        {
            NodeIsLeaf = false;
            MajorityClass = "xd";
            BestAttribute = "-";
            PreviousNodeValue = "-";
            Nodes = new List<TreeNode>();
        }
        public TreeNode(bool nodeIsLeaf, string majorityClass)
        {
            NodeIsLeaf = nodeIsLeaf;
            MajorityClass = majorityClass;
            BestAttribute = "-";
            PreviousNodeValue = "-";
            Nodes = new();
        }

        public TreeNode(bool nodeIsLeaf, string bestAttribute,string majorityClass)
        {
            NodeIsLeaf = nodeIsLeaf;
            MajorityClass = majorityClass;
            BestAttribute = bestAttribute;
            PreviousNodeValue = "-";
            Nodes = new();
        }
        public TreeNode(bool nodeIsLeaf, string bestAttribute, string value, string majorityClass)
        {
            NodeIsLeaf = nodeIsLeaf;
            MajorityClass = majorityClass;
            BestAttribute = bestAttribute;
            PreviousNodeValue = value;
            Nodes = new();
        }
    }
}
