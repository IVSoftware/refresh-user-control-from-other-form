Your question is **How To Refresh User Control From Another Form In WinForms Using C#**. The debugging details are missing in your question. Nevertheless, a general answer for one way of doing that is to have the requesting form fire an event when a refresh is needed. 

Here is a minimal example of a `MainForm` that features a [Refresh] button to test fire this event. The main form also creates the second form that hosts the `UserControl`.

[![screenshot][1]][1]

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            buttonRefresh.Click += onClickButtonRefresh;
            ucHost = new UserControlHostForm(this);
        }
        UserControlHostForm ucHost;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ucHost.StartPosition = FormStartPosition.Manual;
            ucHost.Location = new Point(Location.X + Width + 10, Location.Y);
            ucHost.Show();
        }
        private void onClickButtonRefresh(object? sender, EventArgs e)
        {
            RefreshNeeded?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler? RefreshNeeded;
    }

***
The second form subscribes to the main form event in its constructor method and calls `myUserControl.Refresh()` in response.

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

 ***
The custom `UserControl` implements the `Control.Refresh()` method in an app-specific manner, for example by calling load(). Here is a mock implementation that demonstrates that it's working.

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


  [1]: https://i.stack.imgur.com/FFEYt.png