using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Cost.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CExportscope
{
    public enum State
    {
        Initiate,  //启动
        UpdateToInitiate, //更新开始
        UpdateToComplete,//更新结束
        Delete,//删除
        Other//其他
    }
    public partial class FormRecordData : System.Windows.Forms.Form
    {
        ExternalCommandData commandData = null;
        Document doc = null;
        ExternalEventFc eventFc = null;
        RvtDateEx cmd = null;
        public event EventHandler<EventArgs> ConnectDBEvent;
        public event EventHandler<EventArgs> InitiateDBEvent;
        public event EventHandler<EventArgs> UpdateDBEvent;
        public event EventHandler<EventArgs> DeleteAllDataDBEvent;
        public string IP { get; set; }
        public short Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public State DBstate { get; set; }
        public string Modelname { get; set; }
        public FormRecordData()
        {
            InitializeComponent();
            DBstate = State.Initiate;
        }
        public FormRecordData(ExternalEventFc _eventFc, ExternalCommandData _commandData, RvtDateEx _cmd)
        {
            InitializeComponent();
            this.commandData = _commandData;
            this.doc = _commandData.Application.ActiveUIDocument.Document;
            this.eventFc = _eventFc;
            this.cmd = _cmd;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //SetValue();
            //if(radioButtonInitiate.Checked)
            //{
            //    DBstate=State.Initiate;
            //}
            //else if(radioButtonUpdateToInitiate.Checked)
            //{
            //    DBstate = State.Update;
            //}
            //ConnectDBEvent(sender, e);
            //Close();
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            SetValue();
            DeleteAllDataDBEvent(this, e);
            Close();
        }

        private void btnDeleteAllData_Click(object sender, EventArgs e)
        {
            DBstate = State.Delete;
            SetValue();
            UpdateDBEvent(this, e);
            Close();
        }

        private void SetValue()
        {
            IP = txtIP.Text;
            Port = short.Parse(txtPort.Text);
            UserName = txtName.Text;
            Password = txtPassword.Text;
            Modelname = txtModelName.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DBstate = State.Other;
            Close();
        }
    }
}
