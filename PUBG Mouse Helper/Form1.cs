using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Drawing;
using LiteDB;
using static Pluto.Games.BF1.BF1;

namespace PUBG_Mouse_Helper
{
    public partial class Form1 : Form, IOkButtonPressedInSavePresetDialog, IOnHotkeyPressed
    {

        private class Preset
        {
            public Preset(bool isUserDefined, string presetName, int dx, int dy, int waitMs, int delayMs)
            {
                UserDefined = isUserDefined;
                PresetName = presetName;
                Dx = dx;
                Dy = dy;
                WaitMs = waitMs;
                DelayMs = delayMs;
            }

            public Preset()
            {
            }

            public string PresetName { get; set; }
            public bool UserDefined { get; set; }
            public int Dx { get; set; }
            public int Dy { get; set; }
            public int WaitMs { get; set; }
            public int DelayMs { get; set; }

            public bool IsEmpty() => PresetName == null;
        }

        private Preset _currentPreset;

        private Preset CurrentPreset
        {
            get { return this._currentPreset; }
            set
            {
                this._currentPreset = value;
                this.OnCurrentPresetChanged();
            }
        }

        private Poller poller;

        ToolStripMenuItem userPresetsMenuItem;

        private int presetSwitchHotkeyIndex = 0;

        public Form1()
        {
            InitializeComponent();

            trackBar1.Scroll += OnTrackBarScroll;
            trackBar2.Scroll += OnTrackBarScroll;
            trackBar3.Scroll += OnTrackBarScroll;
            trackBar4.Scroll += OnTrackBarScroll;
            poller = new Poller(this);
        }

        #region Interface methods

        public void OnOkButtonPressed(string presetName)
        {
            try
            {
                if (Savefilehandler.SavePresets(presetName, trackBar1.Value, trackBar2.Value, trackBar3.Value, trackBar4.Value))
                {
                    toolStripStatusLabel1.Text = $"Saved preset {presetName}";
                    toolStripMenuItemPresets.DropDownItems.Remove(userPresetsMenuItem);
                    LoadUserPresetsNames();
                }
                else
                    throw new Exception("Return value false");
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = $"Failed to save preset {presetName}";
            }
        }

        public void OnPresetSwitchHotkeyPressed()
        {
            List<ToolStripMenuItem> presetMenuItemsList = new List<ToolStripMenuItem>();

            foreach (ToolStripMenuItem presetsMenuItem in toolStripMenuItemPresets.DropDownItems)
            {
                if (!presetsMenuItem.HasDropDownItems)
                {
                    presetMenuItemsList.Add(presetsMenuItem);
                }
                else
                {
                    foreach (ToolStripMenuItem userPresetMenuItem in presetsMenuItem.DropDownItems)
                    {
                        presetMenuItemsList.Add(userPresetMenuItem);
                    }
                }
            }

            presetSwitchHotkeyIndex++;
            presetSwitchHotkeyIndex = presetSwitchHotkeyIndex % presetMenuItemsList.Count; //circle back to the range of available presets

            presetMenuItemsList[presetSwitchHotkeyIndex].PerformClick(); //call the corresponding method

            //show a message to user regarding the preset selected
            //new MessageToast($"{presetMenuItemsList[presetSwitchHotkeyIndex].Text}").Show();
        }

        public void OnRightArrowPressed()
        {
            if (trackBar1.Value == trackBar1.Maximum)
                return;
            trackBar1.Value++;
            this.CurrentPreset = new Preset();
            //new MessageToast($"dx = {trackBar1.Value}").Show(); //show a message to user regarding the new dx value
        }

        public void OnLeftArrowPressed()
        {
            if (trackBar1.Value == trackBar1.Minimum)
                return;
            trackBar1.Value--;
            this.CurrentPreset = new Preset();
            //new MessageToast($"dx = {trackBar1.Value}").Show(); //show a message to user regarding the new dx value
        }

        public void OnUpArrowPressed()
        {
            if (trackBar2.Value == trackBar2.Minimum)
                return;
            trackBar2.Value--;
            this.CurrentPreset = new Preset();
            //new MessageToast($"dy = {trackBar2.Value}").Show(); //show a message to user regarding the new dy value
        }

        public void OnDownArrowPressed()
        {
            if (trackBar2.Value == trackBar2.Maximum)
                return;
            trackBar2.Value++;
            this.CurrentPreset = new Preset();
            //new MessageToast($"dy = {trackBar2.Value}").Show(); //show a message to user regarding the new dy value
        }

        public void OnToggleRecoilCompensationHotkeyPressed()
        {
            //SpeechSynthesizer synth = new SpeechSynthesizer();
            toolStripMenuItemEnableAntiRecoil.PerformClick();
            string enabledOrDisabled = toolStripMenuItemEnableAntiRecoil.Checked ? "enable" : "disable";
            string enabledOrDisabledANTI = !toolStripMenuItemEnableAntiRecoil.Checked ? "enable" : "disable";
            //new MessageToast($"Recoil compensation {enabledOrDisabled}d.\nPress F7 to {enabledOrDisabledANTI}.", 50).Show();
            //new MessageToast($"Recoil compensation {enabledOrDisabled}d.\nPress F7 to {enabledOrDisabledANTI}.", 50).Show();
            //Console.Beep(400, 500);

            //if (toolStripMenuItemEnableAntiRecoil.Checked)
            //{
            //    synth.SpeakAsync(trackBar2.Location.ToString());
            //}
            //else
            //{
            //    synth.SpeakAsync("Off");
            //}

        }

        public void OnFastLootPressed() //[XButton1]
        {
            MouseHelperClass.FastLoot();
        }

        public void OnRapidfirePressed() //[XButton2]
        {
            MouseHelperClass.Rapidfire();
        }
        public void OnRapidfireHotkeyPressed()
        {

            //new MessageToast($"dy = {trackBar2.Value}").Show(); //show a message to user regarding the new dy value
        }

        #endregion Interface methods

        private void OnCurrentPresetChanged()
        {
            if (!this.CurrentPreset.IsEmpty()) //if preset is named
            {
                this.Text = HelperFunctions.GetApplicationName() + " [Preset:  " + this.CurrentPreset.PresetName + "]";
                if (!timer1.Enabled) //only allow deleting presets if timer is off i.e. monitoring is stopped
                {
                    if (this.CurrentPreset.UserDefined)
                    {
                        toolStripMenuItemDeletePreset.Enabled = true;
                    }
                    else
                    {
                        toolStripMenuItemDeletePreset.Enabled = false;
                    }
                }

                trackBar1.Value = this.CurrentPreset.Dx;
                trackBar2.Value = this.CurrentPreset.Dy;
                trackBar3.Value = this.CurrentPreset.WaitMs;
                trackBar4.Value = this.CurrentPreset.DelayMs;
            }
            else
            {
                this.Text = HelperFunctions.GetApplicationName();
                toolStripMenuItemDeletePreset.Enabled = false;
            }
        }

        private void OnTrackBarScroll(object sender, EventArgs e)
        {
            //show tooltip regarding the trackbar value
            TrackBar thisTrackBar = (TrackBar)sender;
            toolTip1.SetToolTip(thisTrackBar, thisTrackBar.Value + "");
            //empty out the current preset if any
            this.CurrentPreset = new Preset();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e) //Activate
        {
            toolStripMenuItemStop.Enabled = true;
            toolStripMenuItemSaveAsPreset.Enabled = false;
            toolStripMenuItemDeletePreset.Enabled = false;
            toolStripMenuItemActivate.Enabled = false;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = trackBar4.Value;
            poller.Poll(trackBar1.Value, trackBar2.Value, (uint)trackBar3.Value);
            toolStripStatusLabel1.Text = $"Monitoring Active : (dx={trackBar1.Value}, dy={trackBar2.Value}, WaitMs={trackBar3.Value}, DelayMs={trackBar4.Value})";
            notifyIcon1.Text = toolStripStatusLabel1.Text;
            //PLUTO FUNCS
            poller.Poll_ExtraFunctions();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            toolStripStatusLabel1.Text = "Ready";
            notifyIcon1.Text = HelperFunctions.GetApplicationName();
            toolStripMenuItemSaveAsPreset.Enabled = true;
            if (!CurrentPreset.IsEmpty() && CurrentPreset.UserDefined)
            {
                toolStripMenuItemDeletePreset.Enabled = true;
            }
            toolStripMenuItemActivate.Enabled = true;
            toolStripMenuItemStop.Enabled = false;
        }

        //WEAPON VARIABLE FUNCTIONS

        //WEAPON PRESETS
        private void toolStripMenuItem2_Click(object sender, EventArgs e) //0 Default
        {
            string presetName = ((ToolStripItem)sender).Text;
            this.CurrentPreset = new Preset(false, presetName, 0, 2, 2, 6);
            this.presetSwitchHotkeyIndex = 0;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) //AKM
        {
            string presetName = ((ToolStripItem)sender).Text;
            this.CurrentPreset = new Preset(false, presetName, 0, 9, 2, 6);
            this.presetSwitchHotkeyIndex = 1;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) //M4A1
        {
            string presetName = ((ToolStripItem)sender).Text;
            this.CurrentPreset = new Preset(false, presetName, 0, 7, 2, 6);
            this.presetSwitchHotkeyIndex = 2;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e) //SKS
        {
            string presetName = ((ToolStripItem)sender).Text;
            this.CurrentPreset = new Preset(false, presetName, 0, 10, 2, 6);
            this.presetSwitchHotkeyIndex = 3;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e) //AKM
        {
            string presetName = ((ToolStripItem)sender).Text;
            this.CurrentPreset = new Preset(false, presetName, 0, 9, 2, 6);
            this.presetSwitchHotkeyIndex = 4;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e) //MK14
        {
            string presetName = ((ToolStripItem)sender).Text;
            this.CurrentPreset = new Preset(false, presetName, 0, 8, 2, 6);
            this.presetSwitchHotkeyIndex = 5;
        }

        private void trayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void toolStripMenuItemSaveAsPreset_Click(object sender, EventArgs e)
        {
            new SavePresetAsDialog(this).Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = HelperFunctions.GetApplicationName();
            LoadUserPresetsNames();
            toolStripMenuItemPresets.DropDownItems[0].PerformClick(); //load the first preset by default
        }

        private void OnUserPresetClicked(object sender, EventArgs eventArgs)
        {
            try
            {
                string presetName = ((ToolStripItem)sender).Text;
                if (Savefilehandler.GetSavedPresetNamesList().Contains(presetName))
                {
                    List<int> presetValues = Savefilehandler.GetPresetValuesFromName(presetName);
                    if (presetValues.Count == 4)
                    {
                        this.CurrentPreset = new Preset(true, presetName, presetValues[0], presetValues[1], presetValues[2], presetValues[3]);
                        this.presetSwitchHotkeyIndex = 6 + Savefilehandler.GetSavedPresetNamesList().IndexOf(presetName); //6=number of default presets+1
                        toolStripStatusLabel1.Text = $"Loaded preset {this.CurrentPreset.PresetName}";
                    }
                    else
                    {
                        if (!Savefilehandler.DeletePreset(presetName))
                            throw new Exception($"Cannot delete preset {presetName}");
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = $"Preset {presetName} not found";
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private void toolStripMenuItemDeletePreset_Click(object sender, EventArgs e)
        {
            if (!this.CurrentPreset.IsEmpty() && this.CurrentPreset.UserDefined)
            {
                try
                {
                    if (!Savefilehandler.DeletePreset(this.CurrentPreset.PresetName))
                    {
                        throw new Exception($"Could not delete preset {this.CurrentPreset.PresetName}");
                    }
                    else
                    {
                        toolStripMenuItemDeletePreset.Enabled = false;
                        toolStripMenuItemPresets.DropDownItems.Remove(userPresetsMenuItem);
                        LoadUserPresetsNames();
                        toolStripStatusLabel1.Text = $"Deleted preset {this.CurrentPreset.PresetName}";
                    }
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel1.Text = ex.Message;
                }
            }
        }

        private void LoadUserPresetsNames()
        {
            try
            {
                List<string> savedPresetNames = Savefilehandler.GetSavedPresetNamesList();
                if (savedPresetNames.Count > 0)
                {
                    userPresetsMenuItem = new ToolStripMenuItem("User Presets");
                    toolStripMenuItemPresets.DropDownItems.Add(userPresetsMenuItem);

                    foreach (var presetName in savedPresetNames)
                    {
                        ToolStripItem newPreset = new ToolStripMenuItem(presetName);
                        newPreset.Click += OnUserPresetClicked;
                        userPresetsMenuItem.DropDownItems.Add(newPreset);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Exception in loading presets!");
            }
        }

        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Designed By: Nirinium \nNTG:Ridgefire (c) 2018", HelperFunctions.GetApplicationName(), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripMenuItemInstructions_Click(object sender, EventArgs e)
        {
            string instructionMsg = @"
[TIPS]
Run as Administrator!
IMPORTANT -> You must click Activate in menu to enable!

[CONTROLS]
CHANGE_PRESET -> ENTER
RECOIL_CORRECTION_PARAMETERS -> ARROW KEYS
TOGGLE_ON_OFF -> F7
RAPIDFIRE (EXPERIMENTAL) > XButton2 (Mouse)
AUTO_LOOT > XButton1 (Mouse)";

            MessageBox.Show(instructionMsg, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (timer1.Enabled == true)
            {
                //eat up shortcut keys when timer is running i.e. monitoring is active lest there be a conflict
                switch (keyData)
                {
                    case Keys.F7:
                        return false;
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolStripMenuItemEnableAntiRecoil_Click(object sender, EventArgs e)
        {
            this.poller.PerformRecoilCompensation = toolStripMenuItemEnableAntiRecoil.Checked;
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Pluto.User.UserPrefs userPrefs = new Pluto.User.UserPrefs();
            //
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            // Open database (or create if not exits)
            using (var db = new LiteDatabase(@"BF1.db"))
            {
                // Get weapon collection
                var Guns = db.GetCollection<Weapons>("Guns");

                // Create your new customer instance
                var newWep = new Weapons
                {
                    Name = textBox1.Text,
                    //Guns = new string[] { "SMG", "SMG" },
                    RecoilX = new string[] { txtRecoilX.Text },
                    RecoilY = new string[] { txtRecoilY.Text },
                    GunTypes = comboBox1.SelectedItem.ToString(),
                    IsActive = true
                };

                // Insert new customer document (Id will be auto-incremented)
                Guns.Insert(newWep);

                // Update a document inside a collection
                newWep.Name = textBox1.Text;

                Guns.Update(newWep);

                // Index document using a document property
                Guns.EnsureIndex(x => x.Name);

                // Use Linq to query documents
                var results = Guns.Find(x => x.Name.StartsWith("Jo"));
            }
        }

        public void AdvancedRecoilDBDriver()
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }
    }
}