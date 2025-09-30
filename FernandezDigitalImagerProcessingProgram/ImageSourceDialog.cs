using System;
using System.Windows.Forms;

namespace FernandezDigitalImagerProcessingProgram
{
    public partial class ImageSourceDialog : Form
    {
        public enum SourceChoice
        {
            File,
            Camera,
            Cancel
        }

        public SourceChoice Choice { get; private set; } = SourceChoice.Cancel;

        public ImageSourceDialog()
        {
            InitializeComponent();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            Choice = SourceChoice.File;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            Choice = SourceChoice.Camera;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Choice = SourceChoice.Cancel;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}