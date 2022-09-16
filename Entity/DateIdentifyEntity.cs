using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cost.Entity
{

    [Serializable]
    public class DateIdentifyEntity : INotifyPropertyChanged
    {
        private string name;               //名字
        private List<Element> eleList;     //收集到的数据
        private string startDate;         //开始日期
        private string endDate;           //终止日期

        public DateIdentifyEntity() { }

        public DateIdentifyEntity(string name, List<Element> eleList, string startDate, string endDate)
        {
            this.name = name;
            this.eleList = eleList;
            this.startDate = startDate;
            this.endDate = endDate;
        }


        public string Name { get { return name; } set { if (name != value) { name = value; OnPropertyChanged("Name"); } } }
        public List<Element> EleList { get { return eleList; } set { if (eleList != value) { eleList = value; OnPropertyChanged("EleList"); } } }

        public string StartDate { get { return startDate; } set { if (startDate != value) { startDate = value; OnPropertyChanged("StartDate"); } } }

        public string EndDate { get { return endDate; } set { if (endDate != value) { endDate = value; OnPropertyChanged("EndDate"); } } }


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
