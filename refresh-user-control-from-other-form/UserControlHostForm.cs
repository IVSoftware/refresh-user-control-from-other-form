using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace refresh_user_control_from_other_form
{
    public partial class UserControlHostForm : Form
    {
        public UserControlHostForm(Form owner)
        {
            Owner = owner;
            InitializeComponent();

            // If the UserControl hasn't already been
            // added in the Designer, add it here.
            myUserControl = new MyUserControl
            {
                BackColor = Color.LightBlue,
                Dock = DockStyle.Fill,
            };
            Controls.Add(myUserControl);

            // Subscribe to the MainForm event
            ((MainForm)Owner).RefreshNeeded += onRefreshNeeded;
        }
        MyUserControl myUserControl;
        private void onRefreshNeeded(object? sender, EventArgs e)
        {
            myUserControl.Refresh();
            Visible = true;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason.Equals(CloseReason.UserClosing))
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
    class MyUserControl : UserControl
    {
        public MyUserControl()
        {
            Controls.Add(label);
        }
        public new void Refresh()
        {
            base.Refresh();
            load();
        }
        Label label = new Label { Location = new Point(10, 100), AutoSize = true,  };
        int debugCount = 0;
        private void load()
        {
            label.Text =  $"Count = {++debugCount}: Your custom SQL load code goes here";
        }
    }
}
