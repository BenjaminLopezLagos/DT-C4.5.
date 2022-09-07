using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Analysis;

namespace C4._5
{
    internal class DecisionTree
    {
        private DataFrame _trainingDataFrame;
        private TreeNode _dtree;
        private List<string> _classes;

        public DataFrame trainingDataFrame
        {
            get { return _trainingDataFrame; }
            set { _trainingDataFrame = value; }
        }

        public TreeNode dtree
        {
            get { return _dtree; }
            set { _dtree = value; }
        }

        public List<string> classes
        {
            get { return _classes; }
            set { _classes = value; }
        }

        public DecisionTree(string csvName)
        {
            var originalDF = DataFrame.LoadCsv(csvName);
            _classes = GetUniqueValuesFromColumn(originalDF, "Target");
            _trainingDataFrame = originalDF;
            _trainingDataFrame.Columns.Remove("id");
            _dtree = C45(_trainingDataFrame, "-",_classes);
        }

        public TreeNode C45(DataFrame DF, string value,List<string> ClassList)
        {
            var ValuesAndCounts = GetCountOfUniqueValues(DF, "Target");
            var keyOfMaxValue = ValuesAndCounts.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            if (DF.Rows.Count == 0)
            {
                return new TreeNode(true,"Fail");
            }
            var UniqueTargetVal = GetUniqueValuesFromColumn(DF, "Target");
            if (UniqueTargetVal.Count == 1)
            {
                return new TreeNode(true, FindMostInformativeAttribute(DF,ClassList), value, UniqueTargetVal[0]);
            }
            // It suppossedly means "when only the target column remains
            if (DF.Columns.Count == 1 && DF.Columns[0].Name == "Target")
            {
                return new TreeNode(true, FindMostInformativeAttribute(DF, ClassList), value, keyOfMaxValue);
            }

            var BestAttribute = FindMostInformativeAttribute(DF, ClassList);
            var node = new TreeNode(false, BestAttribute, keyOfMaxValue);
            var AttrValueList = GetUniqueValuesFromColumn(DF, BestAttribute);
            var dfSplit = DF;
            PrimitiveDataFrameColumn<bool> boolFilter;
            DataFrame dfTemp;
            foreach (var attr in AttrValueList)
            {
                boolFilter = dfSplit[BestAttribute].ElementwiseEquals(attr);
                dfTemp = dfSplit.Filter(boolFilter);
                dfTemp.Columns.Remove(dfTemp[BestAttribute]);
                node.previousNodeValue = value;
                node.nodes.Add(C45(dfTemp, attr,ClassList));
            }

            return node;
        }
        /*
        public string Predecir(TreeNode tree, DataFrame test)
        {
            if (tree.NodeIsLeaf)
            {
                return tree.MajorityClass;
            }
            else
            {
                // MyClass result = list.Find(x => x.GetId() == "xy");
                var val = GetUniqueValuesFromColumn(test, tree.BestAttribute)[0];
                var nextBranch = tree.Nodes.Find(x => x.PreviousNodeValue == val);
                return Predecir(nextBranch, test);
            }
        }
        */

        public string Predict(TreeNode tree, DataFrame test)
        {
            if (tree.leaf)
            {
                return tree.majorityClass;
            }
            else
            {
                // MyClass result = list.Find(x => x.GetId() == "xy");
                var val = GetUniqueValuesFromColumn(test, tree.bestAtt)[0];
                var nextBranch = tree.nodes.Find(x => x.previousNodeValue == val);
                return Predict(nextBranch, test);
            }
        }

        public List<string> GetUniqueValuesFromColumn(DataFrame df, string attributeName)
        {
            var dfff = df[attributeName];
            var uniqueValueList = new List<string>();
            for (int i = 0; i < dfff.Length; i++)
            {
                if (!uniqueValueList.Contains(dfff[i]))
                {
                    uniqueValueList.Add((string)dfff[i]);
                }
            }

            return uniqueValueList;
        }

        public Dictionary<string, double> GetCountOfUniqueValues(DataFrame DF, string attributeName)
        {
            List<string> uniqueValues = GetUniqueValuesFromColumn(DF, attributeName);
            Dictionary<string, double> countOfUniqueValues = new Dictionary<string, double>();
            foreach (var value in uniqueValues)
            {
                double count = 0;
                for (int i = 0; i < DF[attributeName].Length; i++)
                {
                    if (DF[attributeName][i].ToString() == value)
                    {
                        count++;
                    }
                }
                countOfUniqueValues.Add(value, count);
            }
            return countOfUniqueValues;
        }

        public double Entropy(DataFrame DF, List<string> ClassList)
        {
            double TotalRows = DF.Rows.Count;
            double Entropy = 0;
            foreach (var c in ClassList)
            {
                PrimitiveDataFrameColumn<bool> boolFilter = DF["Target"].ElementwiseEquals(c);
                DataFrame dfTemp = DF.Filter(boolFilter);
                double EntropyClass = 0;
                if (dfTemp.Rows.Count != 0)
                {
                    double TotalClassCount = dfTemp.Rows.Count;
                    EntropyClass = EntropyClass = -(TotalClassCount / TotalRows) * Math.Log2(TotalClassCount / TotalRows);
                }
                Entropy += EntropyClass;
            }

            return Entropy;
        }

        public double GetInformationGain(DataFrame DF, string attributeName, List<string> ClassList)
        {
            var AttributeValues = GetUniqueValuesFromColumn(DF, attributeName);
            double TotalRows = DF.Rows.Count;
            double AttrInfo = 0;
            foreach (var Value in AttributeValues)
            {
                PrimitiveDataFrameColumn<bool> boolFilter = DF[attributeName].ElementwiseEquals(Value);
                DataFrame dfTemp = DF.Filter(boolFilter);
                double FeatureValueCount = dfTemp.Rows.Count;
                double FeatureValueEntropy = Entropy(dfTemp, ClassList);
                double FeatureValueProb = FeatureValueCount / TotalRows;
                AttrInfo += FeatureValueProb * FeatureValueEntropy;
            }

            return Entropy(DF, ClassList) - AttrInfo;
        }

        public double GainRatio(DataFrame DF, string attributeName, List<string> ClassList)
        {
            double TotalRowCount = DF.Rows.Count;
            Dictionary<string, double> AttributeList = GetCountOfUniqueValues(DF, attributeName);
            double IG = GetInformationGain(DF, attributeName, ClassList);
            double SplitInfo = 0;
            foreach (KeyValuePair<string, double> kvp in AttributeList)
            {
                SplitInfo += -(kvp.Value / TotalRowCount) * Math.Log2(kvp.Value / TotalRowCount);
            }
            double GainRatio = IG / SplitInfo;

            return GainRatio;
        }

        public string FindMostInformativeAttribute(DataFrame DF, List<string> ClassList)
        {
            var AttributeList = new List<string>();
            var ColumnsList = DF.Columns.ToList();

            // excluyendo columna objetivo
            for (int i = 0; i < ColumnsList.Count - 1; i++)
            {
                AttributeList.Add(ColumnsList[i].Name);
            }

            double MaxGainRatio = -1;
            string? MaxInfoFeature = null;

            foreach (var attribute in AttributeList)
            {
                double AttriGainRatio = GainRatio(DF, attribute, ClassList);
                if (MaxGainRatio < AttriGainRatio)
                {
                    MaxGainRatio = AttriGainRatio;
                    MaxInfoFeature = attribute;
                }
            }

            return MaxInfoFeature;
        }
    }
}
