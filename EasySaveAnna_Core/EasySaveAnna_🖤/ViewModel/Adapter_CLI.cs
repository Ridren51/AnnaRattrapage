using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveAnna__.Model;

namespace EasySaveAnna_Core.ViewModel
{
    public class Adapter_CLI
    {
        private string _jobName = string.Empty;
        private string _pathSource = string.Empty;
        private string _pathTarget = string.Empty;
        private string _type = string.Empty;
        public string JobName { get { return _jobName; } set { _jobName = value; } }
        public string PathSource { get { return _pathSource; } set { _pathSource = value; } }
        public string PathTarget { get { return _pathTarget; } set { _pathTarget = value; } }
        public string Type { get { return _type; } set { _type = value; } }
        public Adapter_CLI()
        {

        }

    }
}
