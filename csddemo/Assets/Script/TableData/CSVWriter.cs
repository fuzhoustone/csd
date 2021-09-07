using System.IO;
using System.Text;

public static class CSVWriter
{
    public static void Write(CSVData data,
                             Stream writerStream,
                             string[] columnNames,
                             char seperator = ',')
    {
        Write(data, writerStream, columnNames, Encoding.UTF8, seperator);
    }

    public static void Write(CSVData data,
                             Stream writerStream,
                             string[] columnNames,
                             Encoding encoding,
                             char seperator = ',')
    {
        var writer = new StreamWriter(writerStream, encoding);
        WriteColumnNames(columnNames, seperator, writer);

        WriteContents(data, columnNames, seperator, writer);

        writer.Flush();
        writer.Close();
    }

    private static void WriteContents(CSVData data, string[] columnNames, char seperator, StreamWriter writer)
    {
        for (int rowIndex = 0; rowIndex < data.RowCount; ++rowIndex)
        {
            var row = data.GetRow(rowIndex);
            for (int columnIndex = 0; columnIndex < columnNames.Length; ++columnIndex)
            {
                var columnName = columnNames[columnIndex];
                var contents = row.SafeGetString(columnName);

                var doubleQuatation = contents.Contains(seperator.ToString()) || contents.Contains("\"");

                contents = contents.Replace("\"", "\"\"");
                if (doubleQuatation) contents = "\"" + contents + "\"";

                if (columnIndex < columnNames.Length - 1) writer.Write(contents + seperator);
                else writer.WriteLine(contents);
            }
        }
    }

    private static void WriteColumnNames(string[] columnNames, char seperator, StreamWriter writer)
    {
        for (int columnIndex = 0; columnIndex < columnNames.Length; ++columnIndex)
        {
            if (columnIndex < columnNames.Length-1) writer.Write(columnNames[columnIndex] + seperator);
            else writer.WriteLine(columnNames[columnIndex]);
        }
    }
}