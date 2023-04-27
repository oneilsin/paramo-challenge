using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Sat.Recruitment.Api.Infrastructure.DataConfiguration
{
    public class FileService : IFileService
    {
        private readonly string readFileConnection;
        private readonly string writeFileConnection;
        /// <summary>
        /// The file path to read and write is required, you need to provide the 
        /// complete path + document name and extension.
        /// </summary>
        /// <param name="readFileConnection"></param>
        /// <param name="writeFileConnection"></param>
        public FileService(string readFileConnection, string writeFileConnection)
        {
            this.readFileConnection = readFileConnection;
            this.writeFileConnection = writeFileConnection;
        }

        public IEnumerable<T> ReadValuesFromFile<T>() where T : class, new()
        {
            using (var reader = new StreamReader(readFileConnection))
            {
                List<T> values = new List<T>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var obj = new T();

                    var props = typeof(T).GetProperties();
                    var valuesArr = line.Split(',');

                    for (int i = 0; i < props.Length; i++)
                    {
                        var propType = props[i].PropertyType;
                        if (propType == typeof(decimal))
                        {
                            props[i].SetValue(obj, decimal.Parse(valuesArr[i]));
                        }
                        else
                        {
                            props[i].SetValue(obj, valuesArr[i]);
                        }
                    }
                    values.Add(obj);
                }
                return values;
            }
        }


        public void WriteValuesToFile<T>(List<T> data) where T : class, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> lines = new List<string>(data.Count);
            foreach (T item in data)
            {
                List<string> values = new List<string>(properties.Length);
                foreach (PropertyInfo property in properties)
                {
                    values.Add(property.GetValue(item).ToString());
                }
                lines.Add(string.Join(",", values));
            }

            string text = string.Join(Environment.NewLine, lines);
            File.WriteAllText(writeFileConnection, text);
            File.WriteAllText(readFileConnection, text);
        }

    }
}
