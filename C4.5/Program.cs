using Microsoft.Data.Analysis;
using C4._5;
Console.WriteLine("Hello, World!");

// Los CSVs deben tener una id de nombre "id" xd, y una columna objetivo llamada "Target".
var dt = new DecisionTree("tennis.csv");

// Esto predice todo un set de prueba. NO BORRAR
var testDF = DataFrame.LoadCsv("test.csv");
PrimitiveDataFrameColumn<bool> boolFilter;
DataFrame dfTemp;
for(int i = 0; i < testDF.Rows.Count; i++)
{
    boolFilter = testDF["id"].ElementwiseEquals(i);
    dfTemp = testDF.Filter(boolFilter);
    dfTemp.Columns.Remove("id");
    Console.WriteLine(dt.Predict(dt.dtree, dfTemp));
}

Console.WriteLine(dt.trainingDataFrame["Outlook"].DataType);

//var dtest = DataFrame.LoadCsv("tennis_test.csv");
/*
Console.WriteLine(dt.Predecir(dt.DTree,dtest));
Console.WriteLine(dtest);
Console.WriteLine(dt.DataFrame);
*/
/*
Console.WriteLine(df);
var TN = new TreeNode();

Console.WriteLine(TN.GetUniqueValuesFromColumn(df, "Target"));
Console.WriteLine(TN.Entropy(df,TN.GetUniqueValuesFromColumn(df, "Target")));
for(int i = 0; i < df.Columns.Count-1; i++)
{
    Console.WriteLine($"el ig de {df.Columns[i].Name} es {TN.GetInformationGain(df, df.Columns[i].Name, classes)}");
}

var outlooks = TN.GetCountOfUniqueValues(df,"Outlook");
foreach(KeyValuePair<string, double> i in outlooks)
{
    Console.WriteLine($"{i.Key} : {i.Value}");
}

for (int i = 0; i < df.Columns.Count - 1; i++)
{
    Console.WriteLine($"el gr de {df.Columns[i].Name} es {TN.GainRatio(df, df.Columns[i].Name, classes)}");
}

var bestAtt = "Outlook";
var AttrValueList = TN.GetUniqueValuesFromColumn(df, bestAtt);
var dfSplit = df;
PrimitiveDataFrameColumn<bool> boolFilter;
DataFrame dfTemp;
foreach (var a in AttrValueList)
{
    boolFilter = dfSplit[bestAtt].ElementwiseEquals(a);
    dfTemp = dfSplit.Filter(boolFilter);
    Console.WriteLine("Después de filtrar");
    Console.WriteLine(dfTemp);
    dfTemp.Columns.Remove(dfTemp[bestAtt]);
    Console.WriteLine("Borrando columna");
    Console.WriteLine(dfTemp);
}

Console.WriteLine(TN.FindMostInformativeFeature(df, classes));
*/