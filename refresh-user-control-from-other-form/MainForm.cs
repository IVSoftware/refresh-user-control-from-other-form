namespace refresh_user_control_from_other_form
{
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
}