//
// minecraft_kurwa
// ZlomenyMesic, KryKom
//

using System.Windows.Forms;
using minecraft_kurwa.src.global;

namespace minecraft_kurwa.src.launcher {
    public partial class MCKurwaLauncher : Form {
        public MCKurwaLauncher() {
            InitializeComponent();
            LoadDefaultSettings();
        }

        private void StartEngineButtonClick(object sender, System.EventArgs e) {
            UpdateSettings();
            DialogResult = DialogResult.OK;
        }

        private void LoadDefaultSettings() {
            numericUpDown1.Value = Settings.WINDOW_WIDTH;
            numericUpDown2.Value = Settings.WINDOW_HEIGHT;
            numericUpDown3.Value = Settings.FIELD_OF_VIEW;
            numericUpDown4.Value = Settings.RENDER_DISTANCE;

            numericUpDown5.Value = Settings.SENSIBILITY;
            numericUpDown6.Value = Settings.MOVEMENT_SPEED;

            numericUpDown9.Value = Settings.WORLD_SIZE;
            numericUpDown8.Value = Settings.HEIGHT_LIMIT;
            numericUpDown7.Value = Settings.SEED;

            numericUpDown13.Value = Settings.MAIN_NOISE_SHARPNESS;
            numericUpDown12.Value = Settings.MAIN_NOISE_SCALE;
            numericUpDown11.Value = Settings.BIOME_SCALE;
            numericUpDown10.Value = Settings.SUBBIOME_SCALE;

            numericUpDown17.Value = Settings.WATER_LEVEL;
            numericUpDown16.Value = Settings.OCEAN_SCALE;
            numericUpDown15.Value = Settings.POND_DENSITY;
            numericUpDown14.Value = Settings.FREEZING_DISTANCE;
            numericUpDown18.Value = Settings.MAX_FREEZING_DISTANCE;
            numericUpDown19.Value = Settings.ICE_HOLES;

            numericUpDown21.Value = Settings.TREE_DENSITY;
            numericUpDown20.Value = Settings.TREE_EDGE_OFFSET;
        }

        private void UpdateSettings() {
            Settings.WINDOW_WIDTH = (int)numericUpDown1.Value;
            Settings.WINDOW_HEIGHT = (int)numericUpDown2.Value;
            Settings.FIELD_OF_VIEW = (int)numericUpDown3.Value;
            Settings.RENDER_DISTANCE = (int)numericUpDown4.Value;

            Settings.SENSIBILITY = (int)numericUpDown5.Value;
            Settings.MOVEMENT_SPEED = (int)numericUpDown6.Value;

            Settings.WORLD_SIZE = (int)numericUpDown9.Value;
            Settings.HEIGHT_LIMIT = (int)numericUpDown8.Value;
            Settings.SEED = (int)numericUpDown7.Value;

            Settings.MAIN_NOISE_SHARPNESS = (int)numericUpDown13.Value;
            Settings.MAIN_NOISE_SCALE = (int)numericUpDown12.Value;
            Settings.BIOME_SCALE = (int)numericUpDown11.Value;
            Settings.SUBBIOME_SCALE = (int)numericUpDown10.Value;

            Settings.WATER_LEVEL = (int)numericUpDown17.Value;
            Settings.OCEAN_SCALE = (int)numericUpDown16.Value;
            Settings.POND_DENSITY = (int)numericUpDown15.Value;
            Settings.FREEZING_DISTANCE = (int)numericUpDown14.Value;
            Settings.MAX_FREEZING_DISTANCE = (int)numericUpDown18.Value;
            Settings.ICE_HOLES = (int)numericUpDown19.Value;

            Settings.TREE_DENSITY = (int)numericUpDown21.Value;
            Settings.TREE_EDGE_OFFSET = (int)numericUpDown20.Value;
        }
    }
}
