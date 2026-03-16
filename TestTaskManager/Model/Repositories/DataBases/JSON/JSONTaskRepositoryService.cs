using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace TestTaskManager.Model.Repositories.JSON
{
    public class JSONService
    {
        private readonly JsonSerializerOptions _options = new  JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        private readonly string _pathInFile;

        public JSONService(string pathInFile)
        {
            _pathInFile = pathInFile;
        }

        public IEnumerable<T> Deserialize<T>()
        {
            try
            {
                using (FileStream fs = new FileStream(_pathInFile, FileMode.OpenOrCreate))
                {
                    return JsonSerializer.Deserialize<IEnumerable<T>>(fs, _options);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }


        public void Serialize<T>(T obj)
        {
            try
            {
                using (FileStream fs = new FileStream(_pathInFile, FileMode.Create))
                {
                    JsonSerializer.Serialize<T>(fs, obj, _options);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        }
    }
