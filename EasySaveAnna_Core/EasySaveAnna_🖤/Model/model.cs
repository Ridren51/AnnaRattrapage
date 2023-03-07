using EasySave_2._0.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using EasySave_2._0.ViewModels;

namespace EasySave_2._0.Models
{
    class model : SettingsViewModel
    {
        //private static string logFormat = ViewModels.SettingsViewModel.logFormat;
        public static string language = "en";
        public static ResourceDictionary dictionary = new ResourceDictionary();

        public static void LogLine(string _name, string _source, string _target, string _size, string _transfertTime, string _cryptTime)
        {
            //SettingsViewModel svm = new SettingsViewModel();

            var creeFileLogXML = 0;
            
            if (logFormat == "JSON")
            {
                string path = @"..\..\..\file\logs_" + DateTime.Now.ToString("dd-MM-yyyy") + ".json";

                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "[]");
                }

                logs WriteLog = new logs(_name, _source, _target, _size, _transfertTime, _cryptTime);

                //read the json file
                //On recupère les données du Json
                string JsonLog = File.ReadAllText(path);

                //JsonToString put into the list 
                //Transforme le Json en string et met les données dans une list
                var LogList = JsonConvert.DeserializeObject<List<logs>>(JsonLog);

                //Add the data to the log list
                //Ajout les données dans la list des logs
                LogList.Add(WriteLog);

                //Convert the LogLine list into a json with indented format
                //Convertit notre liste en un json avec des indentations
                var StringToJson = JsonConvert.SerializeObject(LogList, Newtonsoft.Json.Formatting.Indented);

                //Write the data in the log file
                //Ecrit les données dans le fichier log
                File.WriteAllText(path, StringToJson);

            }
            else if (logFormat == "XML")
            {
                string path = @"..\..\..\file\logs_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xml";

                if (!File.Exists(path))
                {

                    XmlTextWriter xmlDoc = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                    xmlDoc.Formatting = System.Xml.Formatting.Indented;

                    xmlDoc.WriteStartDocument();

                    xmlDoc.WriteStartElement("logs");

                    xmlDoc.WriteEndElement();
                    xmlDoc.Flush();
                    xmlDoc.Close();

                    creeFileLogXML = 1;

                }

                List<logs> data = new List<logs>();

                if (creeFileLogXML != 1)
                {
                    //deserialize file
                    XmlSerializer Dserializer = new XmlSerializer(typeof(List<logs>));
                    StreamReader reader = new StreamReader(path);
                    data = (List<logs>)Dserializer.Deserialize(reader);
                    reader.Close();

                }

                logs WriteLog = new logs(_name, _source, _target, _size, _transfertTime, _cryptTime);

                data.Add(WriteLog);

                //create the serialiser to create the xml
                XmlSerializer serialiser = new XmlSerializer(typeof(List<logs>));

                // Create the TextWriter for the serialiser to use
                TextWriter filestream = new StreamWriter(path);

                //write to the file
                serialiser.Serialize(filestream, data);

                // Close the file
                filestream.Close();
            }
        }
    }
}
