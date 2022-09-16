using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CExportscope
{
    public class CSqlString
    {
        public int RevitID { get; set; }
        public string FamilyName { get; set; }
        public string FamilyAnnotation { get; set; }
        public int State { get; set; }
        public string ModelName { get; set; }
        public long Floor { get; set; }
        /// <summary>
        /// 是否有几何信息
        /// </summary>
        public bool IsExistGeometry { get; set; }
    }
}
