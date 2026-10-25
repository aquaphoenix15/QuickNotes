namespace QuickNotes
{
    public partial class Form1 : Form
    {
        private string currentFilePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        // Check if the user is closing and prompt them to confirm if they want to exit the application.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(string.IsNullOrEmpty(writeBox.Text))
            {
                return;
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show("Are you sure you want to quit? Any unsaved changes will be lost.", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    e.Cancel = true; // Cancel the closing event
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Check if the user pressed Ctrl + S
            if (keyData == (Keys.Control | Keys.S))
            {
                _SaveFileDialog();
                return true;
            }

            // Check if the user pressed Ctrl + O
            else if (keyData == (Keys.Control | Keys.O))
            {
                _OpenFileDialog();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void _SaveFileDialog()
        {
            // If the file is already saved, just write over it.
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                System.IO.File.WriteAllText(currentFilePath, writeBox.Text);
                MessageBox.Show("Changes saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Otherwise, get the user to pick a file name and location to save the file.
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.Title = "Save Your File As";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    currentFilePath = filePath;
                    System.IO.File.WriteAllText(filePath, writeBox.Text);

                    MessageBox.Show($"File {System.IO.Path.GetFileName(filePath)} saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    // Set the name to match the open file.
                    this.Text = $"QuickNotes - {System.IO.Path.GetFileName(currentFilePath)}";
                }
            }
        }

        private void _OpenFileDialog()
        {
            // Ask the user to select a file to open.
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Open a File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = openFileDialog.FileName;


                    writeBox.Text = System.IO.File.ReadAllText(currentFilePath);

                    this.Text = $"QuickNotes - {System.IO.Path.GetFileName(currentFilePath)}";
                }

            }
        }
    }
}
