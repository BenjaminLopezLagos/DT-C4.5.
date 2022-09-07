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
        private bool _leaf;
        private string _bestAtt;
        private string _previousNodeValue;
        private string _majorityClass;
        private List<TreeNode> _nodes;

        public bool leaf
        {
            get { return _leaf; }
            set { _leaf = value; }
        }

        public string bestAtt
        {
            get { return _bestAtt; }
            set { _bestAtt = value; }
        }

        public string previousNodeValue
        {
            get { return _previousNodeValue; }
            set { _previousNodeValue = value; }
        }

        public string majorityClass
        {
            get { return _majorityClass; }
            set { _majorityClass = value; }
        }

        public List<TreeNode> nodes
        {
            get { return _nodes; }
            set
            { _nodes = value; }
        }

        public TreeNode()
        {
            _leaf = false;
            _majorityClass = "xd";
            _bestAtt = "-";
            _previousNodeValue = "-";
            _nodes = new List<TreeNode>();
        }
        public TreeNode(bool nodeIsLeaf, string majorityClass)
        {
            _leaf = nodeIsLeaf;
            _majorityClass = majorityClass;
            _bestAtt = "-";
            _previousNodeValue = "-";
            _nodes = new();
        }

        public TreeNode(bool nodeIsLeaf, string bestAttribute,string majorityClass)
        {
            _leaf = nodeIsLeaf;
            _majorityClass = majorityClass;
            _bestAtt = bestAttribute;
            _previousNodeValue = "-";
            _nodes = new();
        }
        public TreeNode(bool nodeIsLeaf, string bestAttribute, string value, string majorityClass)
        {
            _leaf = nodeIsLeaf;
            _majorityClass = majorityClass;
            _bestAtt = bestAttribute;
            _previousNodeValue = value;
            _nodes = new();
        }
    }
}
