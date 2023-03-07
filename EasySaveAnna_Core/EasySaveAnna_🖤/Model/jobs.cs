using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml;
using System.Diagnostics;
using EasySaveAnna_Core.ViewModel;
using System.Reflection;

namespace EasySaveAnna__.Model
{
    internal class jobs
    {
        public class job                                             // Structure d'un travail de sauvegarde | Structure of a backup job
        {
            internal string name { get; set; }
            internal string pathSource { get; set; }
            internal string pathTarget { get; set; }
            internal string type { get; set; }
        }

        public static List<job> listJobs = new List<job>();                           // Tableau avec tous les elements de sauvegarde | Table with all backup items

        public jobs(string name, string pathSource, string pathTarget, string type)
        {
            name = name;
            pathSource = pathSource;
            pathTarget = pathTarget;
            type = type;
        }

        public static void AddJob(string name, string pathSource, string pathTarget, string type)
        {
            job newJob = new job
            {
                name = name,
                pathSource = pathSource,
                pathTarget = pathTarget,
                type = type
            };

            listJobs.Add(newJob);
            addJobToXml(jb);

            //create log line each time a new job is create
            model.LogLine(jb.name, jb.pathSource, jb.pathTarget, "0", "0", "0");
        }

        public static void RemoveJob(string name)
        {
            job jobToRemove = listJobs.FirstOrDefault(job => job.name == name);

            if (jobToRemove != null)
            {
                listJobs.Remove(jobToRemove);
            }
        }



        static public void executeJob(job job)
        {
            var allDirectories = Directory.GetDirectories(job.pathSource, "*", SearchOption.AllDirectories);
            foreach (string dir in allDirectories)  // we create a folder in the target folder with respect to the list || on créé un dossier dans le dossier cible par rapport à la liste
            {
                string dirToCreate = dir.Replace(job.pathSource, job.pathTarget);
                if (job.type == "Full")
                {
                    Directory.CreateDirectory(dirToCreate);
                }
                else
                {
                    if (!Directory.Exists(dirToCreate)) Directory.CreateDirectory(dirToCreate);
                }
            }
            var allFiles = Directory.GetFiles(job.pathSource, "*", SearchOption.AllDirectories);




            foreach (string file in allFiles)  // for each file || pour chaque fichier
            {
                Stopwatch stopWatch = new Stopwatch();  // we start a stopwatch || on démarre un chronomètre
                Stopwatch stopWatch2 = new Stopwatch();
                stopWatch.Start();

                //if(SettingsViewModel.FileTypes == null || SettingsViewModel.FileTypes.Count == 0)
                //{
                //    Process process = new Process();
                //    process.StartInfo.FileName = "C:\\Users\\nathk\\source\\repos\\Projet-PSV2\\CryptoSoft\\CryptoSoft\\bin\\Release\\net6.0\\CryptoSoft.exe";
                //    try
                //    {
                //        foreach (string type in SettingsViewModel.FileTypes)
                //        {
                //            process.StartInfo.Arguments = type;
                //        }
                //    }
                //    catch { }

                //    process.Start();
                //}

                string fName = file.Replace(job.pathSource, job.pathTarget);  // we force the copy of the file in the target directory || on force la copie du fichier dans le répertoire cible
                if (!File.Exists(fName)) File.Copy(file, fName);


                stopWatch.Stop();                  // we stop the stopwatch || on arrête le chronomètre
                TimeSpan ts = stopWatch.Elapsed;   // Get the elapsed time as a TimeSpan value.

                stopWatch2.Start();
                //FileInfo fi = new FileInfo(fName);
                //if (fi.Extension == ".txt")
                //{
                Process pr = new Process();
                pr.StartInfo.FileName = @"..\..\..\..\CryptoSoft\test.exe";
                pr.StartInfo.Arguments = fName;
                pr.Start();
                //}

                stopWatch2.Stop();
                // we add the information in the logs
                // on ajoute les infos dans les logs
                FileInfo info = new FileInfo(Path.Combine(job.pathSource, fName));
                model.LogLine(job.name, Path.Combine(job.pathSource, fName), Path.Combine(job.pathTarget, fName), info.Length.ToString(), stopWatch.Elapsed.TotalSeconds.ToString(), stopWatch2.Elapsed.TotalSeconds.ToString());
            }



        }

        static public void deleteJob(int i)
        {
            deleteJobFromXML(listJobs[i].name);
            listJobs.RemoveAt(i);
        }



        static void addJobToXml(job jb)
        {

            string path = @"..\..\..\file\jobs.xml";

            if (!File.Exists(path))
            {
                XmlTextWriter doc = new XmlTextWriter(path, Encoding.UTF8);
                doc.Formatting = Formatting.Indented;

                doc.WriteStartDocument();

                doc.WriteStartElement("logs");

                doc.WriteEndElement();
                doc.Flush();
                doc.Close();
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNode jobs = xmlDoc.SelectSingleNode("jobs");
            XmlElement job = xmlDoc.CreateElement("job");
            jobs.AppendChild(job);

            XmlElement name = xmlDoc.CreateElement("name");
            XmlElement source = xmlDoc.CreateElement("source");
            XmlElement target = xmlDoc.CreateElement("target");
            XmlElement type = xmlDoc.CreateElement("type");

            name.InnerText = jb.name;
            source.InnerText = jb.pathSource;
            target.InnerText = jb.pathTarget;
            type.InnerText = jb.type;


            job.AppendChild(name);
            job.AppendChild(source);
            job.AppendChild(target);
            job.AppendChild(type);

            xmlDoc.Save(path);

        }
        public static void loadJobsFromXml()
        {
            List<job> jobsInXml = new List<job>();

            XmlDocument xmlDoc = new XmlDocument();
            string path = @"..\..\..\file\jobs.xml";
            xmlDoc.Load(path);

            XmlNodeList joblist = xmlDoc.GetElementsByTagName("job");

            for (int i = 0; i < joblist.Count; i++)
            {
                XmlNodeList child = joblist[i].ChildNodes;

                job job = new job();
                job.name = child[0].InnerText;
                job.pathSource = child[1].InnerText;
                job.pathTarget = child[2].InnerText;
                job.type = child[3].InnerText;

                jobsInXml.Add(job);

            }

            listJobs = jobsInXml;
        }

        public static void deleteJobFromXML(string name)
        {
            string path = @"..\..\..\file\jobs.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            var jobs = xmlDoc.SelectSingleNode("jobs");
            var job = jobs.SelectSingleNode("job[name='" + name + "']");
            jobs.RemoveChild(job);
            xmlDoc.Save(path);
        }

    }

}
