using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Configuration;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using System.Drawing.Drawing2D;


namespace MediaPortal.Plugins
{
    public partial class BackdropMenuEditorConfig : Form
    {
        string path;
        List<string> ids = new List<string>();
        List<menuItem> AvailmenuItems = new List<menuItem>();
        List<menuItem> menuItems1 = new List<menuItem>();
        List<menuItem> menuItems2 = new List<menuItem>();
        List<menuItem> menuItems3 = new List<menuItem>();
        List<menuItem> menuItems4 = new List<menuItem>();
        List<menuItem> menuItems5 = new List<menuItem>();
        List<menuItem> menuItems6 = new List<menuItem>();
        List<bgItem> bgItems = new List<bgItem>();        
        StringBuilder rawXML = new StringBuilder();
        StringBuilder rawXMLBg = new StringBuilder();
        StringBuilder rawXMLDefineTags = new StringBuilder();        
        bool configloaded = false;
        int updateIndex = 999;
        string latestPictures = null;
        string latestMusic = null;
        string latestMovingPictures = null;
        string latestTVSeries = null;
        string latestTVRecordings = null;
        string useMenu1 = null;
        string useMenu2 = null;
        string useMenu3 = null;
        string useMenu4 = null;
        string useMenu5 = null;
        string useMenu6 = null;
        int menuCount = 0;
        string doCenterMenu = null;
        string doHideClock = null;


        public BackdropMenuEditorConfig()
        {            
            InitializeComponent();
        }

        public bool readMPConfigFile(MediaPortal.Profile.Settings xmlreader)
        {
            string skinName = xmlreader.GetValueAsString("skin", "name", "");
            if (skinName != null)
            {
                if (skinName.Equals("Backdrop"))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckSkinName()
        {
            try
            {
                bool doEnable = false;
                using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Configuration.Config.Dir.Config, "MediaPortal.xml")))
                {
                    doEnable = readMPConfigFile(xmlreader);
                }
                return doEnable;
            }
            catch
            {
                return false;
            }

        }

        private void BackdropMenuEditorConfig_FormClosing(object sender, FormClosedEventArgs e)
        {
            if (!DesignMode)
            {
                DialogResult result = MessageBox.Show("Do you want to save your changes?", "Save Changes?", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    //do nothing
                }

                if (result == DialogResult.Yes)
                {
                    DoSave();
                }
                this.Close();                
            }
        }

        private void BackdropMenuEditorConfig_Load(object sender, EventArgs e)
        {                      
            
            //Config.GetFile(Config.Dir.Config, "BackdropMenuEditor.xml")
            path = Config.GetFolder(Config.Dir.Skin) + @"\Backdrop";
            //path = sMediaPortalDir + "\\skin\\BackdropMenuEditor";
            if (System.IO.Directory.Exists(path))
            {
                if (loadIDs(true) == true)
                {
                    avail_list.Enabled = true;
                    //Enable whatever needs to be enabled...
                }
            }

            if (loadMenuIDs(true) == true)
            {
                used_list.Enabled = true;
                //Enable whatever needs to be enabled...
            }
            
            if (checkBox1.Checked)
            {
                useMenu2 = "True";
                //tabControl12.TabPages[2].Enabled = true;
            }
            else
            {
                useMenu2 = "False";
                //useMenu3 = "False";
                //tabControl12.TabPages[2].Enabled = false;
                //checkBox3.Checked = false;
            }
            if (checkBox2.Checked)
            {
                useMenu1 = "True";
                //tabControl12.TabPages[1].Enabled = true;
            }
            else
            {
                useMenu1 = "False";
                //useMenu2 = "False";
                //tabControl12.TabPages[1].Enabled = false;
                //checkBox1.Checked = false;
            }
            if (checkBox3.Checked)
            {
                useMenu3 = "True";
                //tabControl12.TabPages[3].Enabled = true;
            }
            else
            {
                useMenu3 = "False";
                //useMenu4 = "False";
                //tabControl12.TabPages[3].Enabled = false;
                //checkBox4.Checked = false;
            }
            if (checkBox4.Checked)
            {
                useMenu4 = "True";
                //tabControl12.TabPages[4].Enabled = true;
            }
            else
            {
                useMenu4 = "False";
                //useMenu5 = "False";
                //tabControl12.TabPages[4].Enabled = false;
                //checkBox5.Checked = false;
            }
            if (checkBox5.Checked)
            {
                useMenu5 = "True";
                //tabControl12.TabPages[5].Enabled = true;
            }
            else
            {
                useMenu5 = "False";
                //useMenu6 = "False";
                //tabControl12.TabPages[5].Enabled = false;
                //checkBox6.Checked = false;
            }
            tabControl2.TabPages[1].Enabled = false;
            
            
        }

        private void SetMenuCount()
        {
            menuCount = 0;
            if (checkBox1.Checked)
            {
                menuCount++;
            }
            if (checkBox2.Checked)
            {
                menuCount++;
            }
            if (checkBox3.Checked)
            {
                menuCount++;
            }
            if (checkBox4.Checked)
            {
                menuCount++;
            }
            if (checkBox5.Checked)
            {
                menuCount++;
            }         
            if (checkBox6.Checked)
            {
                menuCount++;
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool b = true;
            if (new_windowid.Text != "" && new_name.Text != "")
            {
                if (comboBox5.SelectedItem.Equals("Multiple Images"))
                {
                    if (getFilenameOnly(new_imagepath.Text).IndexOf(".") > 0)
                    {
                        MessageBox.Show("'Backdrop Path' must be the name of a path and not contain any dots (.)!");
                        b = false;
                    }
                }
                else if (comboBox5.SelectedItem.Equals("Single Image"))
                {
                    if (getFilenameOnly(new_imagepath.Text).IndexOf(".") <= 0)
                    {
                        MessageBox.Show("'Image Name' must be the name of a picture somewhere in the Animations directory and must contain a file extensiono (like .jpg).");
                        b = false;
                    }
                }
                else if (comboBox16.SelectedItem.Equals("Fanart Plugin"))
                {
                    MessageBox.Show("'Backdrop Name' must have a value (like Random Movies).");
                    b = false;
                }
                if (b == true)
                {
                    //toolStripStatusLabel1.Text = avail_list.SelectedItem.ToString() + " added to menu";
                    menuItem item = new menuItem();
                    item.name = new_name.Text;
                    item.hyperlink = new_windowid.Text;
                    item.useLatestMedia = comboBox17.SelectedItem.ToString();
                    if (comboBox5.SelectedItem.Equals("Fanart Plugin"))
                        item.imagePath = comboBox16.SelectedItem.ToString();
                    else
                        item.imagePath = new_imagepath.Text;
                    if (new_windowid.Text.Equals("96742"))
                    {
                        if (comboBox18.SelectedItem != null)
                        {
                            item.mainCategory = comboBox18.SelectedItem.ToString();
                        }
                        else
                        {
                            item.mainCategory = "";
                        }
                        if (comboBox19.SelectedItem != null)
                        {
                            item.subCategory = comboBox19.SelectedItem.ToString();
                        }
                        else
                        {
                            item.subCategory = "";
                        }
                    }
                    if (new_windowid.Text.Equals("4755"))
                    {
                        if (comboBox21.SelectedItem != null)
                        {
                            item.mainCategory = comboBox21.SelectedItem.ToString();
                        }
                        else
                        {
                            item.mainCategory = "";
                        }
                        if (comboBox20.SelectedItem != null)
                        {
                            item.subCategory = comboBox20.SelectedItem.ToString();
                        }
                        else
                        {
                            item.subCategory = "";
                        }
                    }
                    if (new_windowid.Text.Equals("9811"))
                    {
                        if (comboBox23.SelectedItem != null)
                        {
                            item.mainCategory = comboBox23.SelectedItem.ToString();
                        }
                        else
                        {
                            item.mainCategory = "";
                        }
                    }
                    if (new_windowid.Text.Equals("504") || new_windowid.Text.Equals("504"))
                    {
                        if (comboBox22.SelectedItem != null)
                        {
                            item.mainCategory = comboBox22.SelectedItem.ToString();
                        }
                        else
                        {
                            item.mainCategory = "";
                        }
                    }
                    if (comboBox1.SelectedItem.Equals("1"))
                    {
                        if (menuItems1.Count < 15)
                        {
                            if (updateIndex == 999)
                            {
                                menuItems1.Add(item);
                                used_list.Items.Add(item.name);
                            }
                            else
                            {
                                used_list.Items.RemoveAt(updateIndex);
                                menuItems1.RemoveAt(updateIndex);
                                menuItems1.Insert(updateIndex, item);
                                used_list.Items.Insert(updateIndex, item.name);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Only fifteen submenu items are supported per module!");
                        }
                    }
                    else if (comboBox1.SelectedItem.Equals("2"))
                    {
                        if (menuItems1.Count < 15)
                        {
                            if (updateIndex == 999)
                            {
                                menuItems2.Add(item);
                                listBox1.Items.Add(item.name);
                            }
                            else
                            {
                                listBox1.Items.RemoveAt(updateIndex);
                                menuItems2.RemoveAt(updateIndex);
                                menuItems2.Insert(updateIndex, item);
                                listBox1.Items.Insert(updateIndex, item.name);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Only fifteen submenu items are supported per module!");
                        }
                    }
                    else if (comboBox1.SelectedItem.Equals("3"))
                    {
                        if (menuItems1.Count < 15)
                        {
                            if (updateIndex == 999)
                            {
                                menuItems3.Add(item);
                                listBox2.Items.Add(item.name);
                            }
                            else
                            {
                                listBox2.Items.RemoveAt(updateIndex);
                                menuItems3.RemoveAt(updateIndex);
                                menuItems3.Insert(updateIndex, item);
                                listBox2.Items.Insert(updateIndex, item.name);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Only fifteen submenu items are supported per module!");
                        }
                    }
                    else if (comboBox1.SelectedItem.Equals("4"))
                    {
                        if (menuItems1.Count < 15)
                        {
                            if (updateIndex == 999)
                            {
                                menuItems4.Add(item);
                                listBox3.Items.Add(item.name);
                            }
                            else
                            {
                                listBox3.Items.RemoveAt(updateIndex);
                                menuItems4.RemoveAt(updateIndex);
                                menuItems4.Insert(updateIndex, item);
                                listBox3.Items.Insert(updateIndex, item.name);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Only fifteen submenu items are supported per module!");
                        }
                    }
                    else if (comboBox1.SelectedItem.Equals("5"))
                    {
                        if (menuItems1.Count < 15)
                        {
                            if (updateIndex == 999)
                            {
                                menuItems5.Add(item);
                                listBox4.Items.Add(item.name);
                            }
                            else
                            {
                                listBox4.Items.RemoveAt(updateIndex);
                                menuItems5.RemoveAt(updateIndex);
                                menuItems5.Insert(updateIndex, item);
                                listBox4.Items.Insert(updateIndex, item.name);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Only fifteen submenu items are supported per module!");
                        }
                    }
                    else
                    {
                        if (menuItems1.Count < 15)
                        {
                            if (updateIndex == 999)
                            {
                                menuItems6.Add(item);
                                listBox5.Items.Add(item.name);
                            }
                            else
                            {
                                listBox5.Items.RemoveAt(updateIndex);
                                menuItems6.RemoveAt(updateIndex);
                                menuItems6.Insert(updateIndex, item);
                                listBox5.Items.Insert(updateIndex, item.name);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Only fifteen submenu items are supported per module!");
                        }
                    }

                    // Clear items
                    new_name.Text = "";
                    new_imagepath.Text = "";
                    new_windowid.Text = "";
                    comboBox16.SelectedIndex = -1;
                    //comboBox5.SelectedIndex = -1;
                    if (used_list.Items.Count > 0)
                        avail_list.SelectedIndex = -1;
                    updateIndex = 999;
                    comboBox1.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("All fields must be complete.", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


        private void btnRemove_Click(object sender, EventArgs e)
        {
            removeToolStripMenuItem_Click1(sender, e);
        }

        // Move selected menu item down one position in list. 
        private void btMoveDown_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at bottom
            if (used_list.SelectedItem != null && used_list.SelectedIndex < used_list.Items.Count - 1)
            {
                int index = used_list.SelectedIndex;
                Object listItem = used_list.SelectedItem;
                menuItem mnuItem = menuItems1[index];
                used_list.Items.RemoveAt(index);
                menuItems1.RemoveAt(index);
                if (index < used_list.Items.Count)
                {
                    used_list.Items.Insert(index + 1, listItem);
                    menuItems1.Insert(index + 1, mnuItem);
                }
                else
                {
                    used_list.Items.Add(listItem);
                    menuItems1.Add(mnuItem);
                }
                used_list.SelectedIndex = index + 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        // Move selected menu item up one position in list. 
        private void btMoveUp_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at top
            if (used_list.SelectedItem != null && used_list.SelectedIndex > 0)
            {
                int index = used_list.SelectedIndex;
                Object listItem = used_list.SelectedItem;
                menuItem mnuItem = menuItems1[index];
                used_list.Items.RemoveAt(index);
                menuItems1.RemoveAt(index);
                used_list.Items.Insert(index - 1, listItem);
                menuItems1.Insert(index - 1, mnuItem);
                used_list.SelectedIndex = index - 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }

        }



        private void removeToolStripMenuItem_Click1(object sender, EventArgs e)
        {
            if (used_list.SelectedItem != null)
            {
                menuItems1.RemoveAt(used_list.SelectedIndex);
                used_list.Items.Remove(used_list.SelectedItem);
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
            else
                MessageBox.Show("No item is selected for removal", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void removeToolStripMenuItem_Click2(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                menuItems2.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.Remove(listBox1.SelectedItem);
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
            else
                MessageBox.Show("No item is selected for removal", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void removeToolStripMenuItem_Click3(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                menuItems3.RemoveAt(listBox2.SelectedIndex);
                listBox2.Items.Remove(listBox2.SelectedItem);
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
            else
                MessageBox.Show("No item is selected for removal", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void removeToolStripMenuItem_Click4(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                menuItems4.RemoveAt(listBox3.SelectedIndex);
                listBox3.Items.Remove(listBox3.SelectedItem);
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
            else
                MessageBox.Show("No item is selected for removal", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void removeToolStripMenuItem_Click5(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem != null)
            {
                menuItems5.RemoveAt(listBox4.SelectedIndex);
                listBox4.Items.Remove(listBox4.SelectedItem);
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
            else
                MessageBox.Show("No item is selected for removal", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void removeToolStripMenuItem_Click6(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem != null)
            {
                menuItems6.RemoveAt(listBox5.SelectedIndex);
                listBox5.Items.Remove(listBox5.SelectedItem);
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
            else
                MessageBox.Show("No item is selected for removal", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void DoSave()
        {
            if (checkFields() == true)
            {

                int mycount = 0;                

                using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "BackdropMenuEditor.xml")))
                {
                    if (configloaded)
                    {
                        DateTime thisDay = DateTime.Now;
                        string fBasicHomeOrg = Config.GetFile(Configuration.Config.Dir.Config, "BackdropMenuEditor.xml");
                        string fBasicHomeNew = fBasicHomeOrg + "_" + thisDay.ToString("yyyy_M_d_hh_mm_ss") + ".xml";
                        File.Copy(fBasicHomeOrg, fBasicHomeNew);
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText1");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText1ImagePath");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText1Multiimage");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu1");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText2");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText2ImagePath");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText2Multiimage");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu2");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText3");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText3ImagePath");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText3Multiimage");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu3");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText4");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText4ImagePath");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText4Multiimage");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu4");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText5");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText5ImagePath");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText5Multiimage");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu5");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText6");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText6ImagePath");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuText6Multiimage");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu6");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "menuDefaultMenu");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "doCenterMenu");                        
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "IdleTimeBasic");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "IdleTimeOther");
                        xmlwriter.RemoveEntry("FanartHandler", "useMenu1");
                        xmlwriter.RemoveEntry("FanartHandler", "useMenu2");
                        xmlwriter.RemoveEntry("FanartHandler", "useMenu3");
                        xmlwriter.RemoveEntry("FanartHandler", "useMenu4");
                        xmlwriter.RemoveEntry("FanartHandler", "useMenu5");
                        xmlwriter.RemoveEntry("FanartHandler", "useMenu6");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu1");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu2");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu3");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu4");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu5");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "useMenu6");
                        xmlwriter.RemoveEntry("BackdropMenuEditor", "doHideClock");
                        

                        int a = 0;
                        do
                        {
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemNameA" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemBgA" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemHyperlinkA" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemUseLatestMediaA" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemMainCategoryA" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemSubCategoryA" + a);
                            a++;
                        } while (a < 25);
                        a = 0;
                        do
                        {
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemNameB" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemBgB" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemHyperlinkB" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemUseLatestMediaB" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemMainCategoryB" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemSubCategoryB" + a);
                            a++;
                        } while (a < 25);
                        a = 0;
                        do
                        {
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemNameC" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemBgC" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemHyperlinkC" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemUseLatestMediaC" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemMainCategoryC" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemSubCategoryC" + a);
                            a++;
                        } while (a < 25);
                        a = 0;
                        do
                        {
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemNameD" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemBgD" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemHyperlinkD" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemUseLatestMediaD" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemMainCategoryD" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemSubCategoryD" + a);
                            a++;
                        } while (a < 25);
                        a = 0;
                        do
                        {
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemNameE" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemBgE" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemHyperlinkE" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemUseLatestMediaE" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemMainCategoryE" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemSubCategoryE" + a);
                            a++;
                        } while (a < 25);
                        a = 0;
                        do
                        {
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemNameF" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemBgF" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemHyperlinkF" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemUseLatestMediaF" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemMainCategoryF" + a);
                            xmlwriter.RemoveEntry("BackdropMenuEditor", "menuItemSubCategoryF" + a);
                            a++;
                        } while (a < 25);

                    }
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText1Name", textBox1.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText1Multiimage", comboBox11.SelectedItem);
                    xmlwriter.SetValue("BackdropMenuEditor", "useMenu1", checkBox2.Checked ? true : false);
                    if (comboBox11.SelectedItem != null && comboBox11.SelectedItem.Equals("Fanart Plugin"))
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText1ImagePath", comboBox3.SelectedItem);
                    else
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText1ImagePath", textBox15.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText2Name", textBox2.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText2Multiimage", comboBox10.SelectedItem);
                    xmlwriter.SetValue("BackdropMenuEditor", "useMenu2", checkBox1.Checked ? true : false);
                    if (comboBox10.SelectedItem != null && comboBox10.SelectedItem.Equals("Fanart Plugin"))
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText2ImagePath", comboBox4.SelectedItem);
                    else
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText2ImagePath", textBox16.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText3Name", textBox4.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText3Multiimage", comboBox9.SelectedItem);
                    xmlwriter.SetValue("BackdropMenuEditor", "useMenu3", checkBox3.Checked ? true : false);
                    if (comboBox9.SelectedItem != null && comboBox9.SelectedItem.Equals("Fanart Plugin"))
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText3ImagePath", comboBox12.SelectedItem);
                    else
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText3ImagePath", textBox17.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText4Name", textBox6.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText4Multiimage", comboBox8.SelectedItem);
                    xmlwriter.SetValue("BackdropMenuEditor", "useMenu4", checkBox4.Checked ? true : false);
                    if (comboBox8.SelectedItem != null && comboBox8.SelectedItem.Equals("Fanart Plugin"))
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText4ImagePath", comboBox13.SelectedItem);
                    else
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText4ImagePath", textBox18.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText5Name", textBox8.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText5Multiimage", comboBox7.SelectedItem);
                    xmlwriter.SetValue("BackdropMenuEditor", "useMenu5", checkBox5.Checked ? true : false);
                    if (comboBox7.SelectedItem != null && comboBox7.SelectedItem.Equals("Fanart Plugin"))
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText5ImagePath", comboBox14.SelectedItem);
                    else
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText5ImagePath", textBox19.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText6Name", textBox7.Text);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuText6Multiimage", comboBox6.SelectedItem);
                    xmlwriter.SetValue("BackdropMenuEditor", "useMenu6", checkBox6.Checked ? true : false);
                    if (comboBox6.SelectedItem != null && comboBox6.SelectedItem.Equals("Fanart Plugin"))
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText6ImagePath", comboBox15.SelectedItem);
                    else
                        xmlwriter.SetValue("BackdropMenuEditor", "menuText6ImagePath", textBox3.Text);

                    xmlwriter.SetValue("BackdropMenuEditor", "doHideClock", checkBox8.Checked ? true : false);
                    xmlwriter.SetValue("BackdropMenuEditor", "doCenterMenu", checkBox7.Checked ? true : false);
                    xmlwriter.SetValue("BackdropMenuEditor", "menuDefaultMenu", comboBox2.SelectedItem);
                    xmlwriter.SetValue("BackdropMenuEditor", "IdleTimeOther", IdleTimeOther.SelectedItem);
                    xmlwriter.SetValue("BackdropMenuEditor", "IdleTimeBasic", IdleTimeBasic.SelectedItem);      


                    foreach (menuItem item in menuItems1)
                    {
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemNameA" + mycount, item.name);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemBgA" + mycount, item.imagePath);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemHyperlinkA" + mycount, item.hyperlink);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemUseLatestMediaA" + mycount, item.useLatestMedia);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemMainCategoryA" + mycount, item.mainCategory);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemSubCategoryA" + mycount, item.subCategory);

                        mycount++;
                    }
                    mycount = 0;
                    foreach (menuItem item in menuItems2)
                    {
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemNameB" + mycount, item.name);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemBgB" + mycount, item.imagePath);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemHyperlinkB" + mycount, item.hyperlink);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemUseLatestMediaB" + mycount, item.useLatestMedia);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemMainCategoryB" + mycount, item.mainCategory);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemSubCategoryB" + mycount, item.subCategory);

                        mycount++;
                    }
                    mycount = 0;
                    foreach (menuItem item in menuItems3)
                    {
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemNameC" + mycount, item.name);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemBgC" + mycount, item.imagePath);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemHyperlinkC" + mycount, item.hyperlink);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemUseLatestMediaC" + mycount, item.useLatestMedia);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemMainCategoryC" + mycount, item.mainCategory);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemSubCategoryC" + mycount, item.subCategory);
                        mycount++;
                    }
                    mycount = 0;
                    foreach (menuItem item in menuItems4)
                    {
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemNameD" + mycount, item.name);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemBgD" + mycount, item.imagePath);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemHyperlinkD" + mycount, item.hyperlink);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemUseLatestMediaD" + mycount, item.useLatestMedia);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemMainCategoryD" + mycount, item.mainCategory);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemSubCategoryD" + mycount, item.subCategory);
                        mycount++;
                    }
                    mycount = 0;
                    foreach (menuItem item in menuItems5)
                    {
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemNameE" + mycount, item.name);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemBgE" + mycount, item.imagePath);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemHyperlinkE" + mycount, item.hyperlink);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemUseLatestMediaE" + mycount, item.useLatestMedia);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemMainCategoryE" + mycount, item.mainCategory);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemSubCategoryE" + mycount, item.subCategory);
                        mycount++;
                    }
                    mycount = 0;
                    foreach (menuItem item in menuItems6)
                    {
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemNameF" + mycount, item.name);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemBgF" + mycount, item.imagePath);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemHyperlinkF" + mycount, item.hyperlink);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemUseLatestMediaF" + mycount, item.useLatestMedia);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemMainCategoryF" + mycount, item.mainCategory);
                        xmlwriter.SetValue("BackdropMenuEditor", "menuItemSubCategoryF" + mycount, item.subCategory);
                        mycount++;
                    }
                }
                MessageBox.Show("Done saving your menu configuration (old is kept as backup). Settings is stored in memory. Make sure to press Ok when exiting MP Configuration. Pressing Cancel when exiting MP Configuration will result in these setting NOT being saved!");
                this.Close();
            }
            else
            {
                MessageBox.Show("ERROR!! Nothing was saved because of missing values!");
            }
        }

        // Save settings to file
        private void btSave_Click(object sender, EventArgs e)
        {
            
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string getFolderName(string s)
        {
            string ss = s.ToLower();
            string sOut = "pluginsbg";
            if (ss.IndexOf("emulator") >= 0)
                sOut = "emulator";
            else if (ss.IndexOf("programs") >= 0)
                sOut = "games";
            else if (ss.IndexOf("lastfm") >= 0)
                sOut = "lastfm";
            else if (ss.IndexOf("lyrics") >= 0)
                sOut = "lyrics";
            else if (ss.IndexOf("mediaslayer") >= 0)
                sOut = "movies";
            else if (ss.IndexOf("films") >= 0)
                sOut = "movies";
            else if (ss.IndexOf("movingpictures") >= 0)
                sOut = "movies";
            else if (ss.IndexOf("video") >= 0)
                sOut = "movies";
            else if (ss.IndexOf("dvd") >= 0)
                sOut = "movies";
            else if (ss.IndexOf("youtube") >= 0)
                sOut = "movies";
            else if (ss.IndexOf("globalsearch") >= 0)
                sOut = "music";
            else if (ss.IndexOf("music") >= 0)
                sOut = "music";
            else if (ss.IndexOf("mvids") >= 0)
                sOut = "music";
            else if (ss.IndexOf("pics") >= 0)
                sOut = "pictures";
            else if (ss.IndexOf("radio") >= 0)
                sOut = "radio";
            else if (ss.IndexOf("settings") >= 0)
                sOut = "settings";
            else if (ss.IndexOf("mynumberplace") >= 0)
                sOut = "suduko";
            else if (ss.IndexOf("tetris") >= 0)
                sOut = "tetris";
            else if (ss.IndexOf("guide") >= 0)
                sOut = "tvguide";
            else if (ss.IndexOf("4tr") >= 0)
                sOut = "tv";
            else if (ss.IndexOf("tvseries") >= 0)
                sOut = "tvseries";
            else if (ss.IndexOf("tv") >= 0)
                sOut = "tv";
            else if (ss.IndexOf("dripper") >= 0)
                sOut = "tv";
            else if (ss.IndexOf("weather") >= 0)
                sOut = "weatherbg";
            else if (ss.IndexOf("map") >= 0)
                sOut = "worldmap";
            
            return sOut;
        }

        private bool loadIDs(bool onLoad)
        {
            comboBox1.Enabled = true;
            comboBox1.Items.Clear();
            comboBox1.Items.Add("1");
            comboBox1.Items.Add("2");
            comboBox1.Items.Add("3");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("5");
            comboBox1.Items.Add("6");            
            
            comboBox5.Enabled = true;
            comboBox5.Items.Clear();
            comboBox5.Items.Add("Single Image");
            comboBox5.Items.Add("Multiple Images");
            comboBox5.Items.Add("Fanart Plugin");

            comboBox17.Enabled = true;
            comboBox17.Items.Clear();
            comboBox17.Items.Add("No");
            comboBox17.Items.Add("Moving Pictures");
            comboBox17.Items.Add("Music");
            comboBox17.Items.Add("Pictures");            
            comboBox17.Items.Add("TV Recordings");
            comboBox17.Items.Add("TVSeries");            

            comboBox6.Enabled = true;
            comboBox6.Items.Clear();
            comboBox6.Items.Add("Single Image");
            comboBox6.Items.Add("Multiple Images");
            comboBox6.Items.Add("Fanart Plugin");

            comboBox7.Enabled = true;
            comboBox7.Items.Clear();
            comboBox7.Items.Add("Single Image");
            comboBox7.Items.Add("Multiple Images");
            comboBox7.Items.Add("Fanart Plugin");

            comboBox8.Enabled = true;
            comboBox8.Items.Clear();
            comboBox8.Items.Add("Single Image");
            comboBox8.Items.Add("Multiple Images");
            comboBox8.Items.Add("Fanart Plugin");

            comboBox9.Enabled = true;
            comboBox9.Items.Clear();
            comboBox9.Items.Add("Single Image");
            comboBox9.Items.Add("Multiple Images");
            comboBox9.Items.Add("Fanart Plugin");

            comboBox10.Enabled = true;
            comboBox10.Items.Clear();
            comboBox10.Items.Add("Single Image");
            comboBox10.Items.Add("Multiple Images");
            comboBox10.Items.Add("Fanart Plugin");

            comboBox11.Enabled = true;
            comboBox11.Items.Clear();
            comboBox11.Items.Add("Single Image");
            comboBox11.Items.Add("Multiple Images");
            comboBox11.Items.Add("Fanart Plugin");

            comboBox3.Enabled = true;
            comboBox3.Items.Clear();
            comboBox3.Items.Add("Random Games");
            comboBox3.Items.Add("Random Movies");
            comboBox3.Items.Add("Random Movies Scraper");
            comboBox3.Items.Add("Random MovingPictures");
            comboBox3.Items.Add("Random Music");
            comboBox3.Items.Add("Random Music Scraper");
            comboBox3.Items.Add("Random Pictures");
            comboBox3.Items.Add("Random Plugins");
            comboBox3.Items.Add("Random TV");
            comboBox3.Items.Add("Random TVSeries");

            comboBox4.Enabled = true;
            comboBox4.Items.Clear();
            comboBox4.Items.Add("Random Games");
            comboBox4.Items.Add("Random Movies");
            comboBox4.Items.Add("Random Movies Scraper");
            comboBox4.Items.Add("Random MovingPictures");
            comboBox4.Items.Add("Random Music");
            comboBox4.Items.Add("Random Music Scraper");
            comboBox4.Items.Add("Random Pictures");
            comboBox4.Items.Add("Random Plugins");
            comboBox4.Items.Add("Random TV");
            comboBox4.Items.Add("Random TVSeries");

            comboBox12.Enabled = true;
            comboBox12.Items.Clear();
            comboBox12.Items.Add("Random Games");
            comboBox12.Items.Add("Random Movies");
            comboBox12.Items.Add("Random Movies Scraper");
            comboBox12.Items.Add("Random MovingPictures");
            comboBox12.Items.Add("Random Music");
            comboBox12.Items.Add("Random Music Scraper");
            comboBox12.Items.Add("Random Pictures");
            comboBox12.Items.Add("Random Plugins");
            comboBox12.Items.Add("Random TV");
            comboBox12.Items.Add("Random TVSeries");

            comboBox13.Enabled = true;
            comboBox13.Items.Clear();
            comboBox13.Items.Add("Random Games");
            comboBox13.Items.Add("Random Movies");
            comboBox13.Items.Add("Random Movies Scraper");
            comboBox13.Items.Add("Random MovingPictures");
            comboBox13.Items.Add("Random Music");
            comboBox13.Items.Add("Random Music Scraper");
            comboBox13.Items.Add("Random Pictures");
            comboBox13.Items.Add("Random Plugins");
            comboBox13.Items.Add("Random TV");
            comboBox13.Items.Add("Random TVSeries");

            comboBox14.Enabled = true;
            comboBox14.Items.Clear();
            comboBox14.Items.Add("Random Games");
            comboBox14.Items.Add("Random Movies");
            comboBox14.Items.Add("Random Movies Scraper");
            comboBox14.Items.Add("Random MovingPictures");
            comboBox14.Items.Add("Random Music");
            comboBox14.Items.Add("Random Music Scraper");
            comboBox14.Items.Add("Random Pictures");
            comboBox14.Items.Add("Random Plugins");
            comboBox14.Items.Add("Random TV");
            comboBox14.Items.Add("Random TVSeries");

            comboBox15.Enabled = true;
            comboBox15.Items.Clear();
            comboBox15.Items.Add("Random Games");
            comboBox15.Items.Add("Random Movies");
            comboBox15.Items.Add("Random Movies Scraper");
            comboBox15.Items.Add("Random MovingPictures");
            comboBox15.Items.Add("Random Music");
            comboBox15.Items.Add("Random Music Scraper");
            comboBox15.Items.Add("Random Pictures");
            comboBox15.Items.Add("Random Plugins");
            comboBox15.Items.Add("Random TV");
            comboBox15.Items.Add("Random TVSeries");

            comboBox16.Enabled = true;
            comboBox16.Items.Clear();
            comboBox16.Items.Add("Random Games");
            comboBox16.Items.Add("Random Movies");
            comboBox16.Items.Add("Random Movies Scraper");
            comboBox16.Items.Add("Random MovingPictures");
            comboBox16.Items.Add("Random Music");
            comboBox16.Items.Add("Random Music Scraper");
            comboBox16.Items.Add("Random Pictures");
            comboBox16.Items.Add("Random Plugins");
            comboBox16.Items.Add("Random TV");
            comboBox16.Items.Add("Random TVSeries");
            
            avail_list.Enabled = true;
            avail_list.Items.Clear();

            menuItem avail_item;
            XPathDocument myXPathDocument;

            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] rgFiles = di.GetFiles("*.xml");
            foreach (FileInfo fi in rgFiles)
            {
                string s = fi.Name;
                bool b = false;
                
                if (s.StartsWith("common") == true)
                    b = true;
                else if (s.StartsWith("Common") == true)
                    b = true;
                else if (s.StartsWith("dialog") == true)
                    b = true;
                else if (s.StartsWith("Dialog") == true)
                    b = true;
                else if (s.StartsWith("references") == true)
                    b = true;
                else if (s.StartsWith("fonts") == true)
                    b = true;
                else if (s.StartsWith("time") == true)
                    b = true;
                else if (s.StartsWith("splashscreen") == true)
                    b = true;
                else if (s.StartsWith("topbar") == true)
                    b = true;
                else if (s.StartsWith("wizard") == true)
                    b = true;
                else if (s.StartsWith("BackdropMenuEditor_selector") == true)                    
                    b = false;
                else if (s.StartsWith("BackdropMenuEditor_currentvideo") == true)
                    b = false;
                else if (s.StartsWith("BackdropMenuEditor") == true)
                    b = true;
                else if (s.StartsWith("BackdropMenuEditor") == true)
                    b = true;
                else if (s.StartsWith(".watched") == true)
                    b = true;
                
                if (b == false)
                {
                    myXPathDocument = new XPathDocument(fi.FullName);
                    XPathNavigator myXPathNavigator = myXPathDocument.CreateNavigator();
                    XPathNodeIterator myXPathNodeIterator  = myXPathNavigator.Select("/window/id");
                    
                    if (myXPathNodeIterator.Count > 0)
                    {
                        myXPathNodeIterator.MoveNext();    
                        avail_item = new menuItem();
                        avail_item.name = s;                        
                        avail_item.hyperlink = myXPathNodeIterator.Current.Value;
                        avail_item.imagePath = getFolderName(s);
                        avail_list.Items.Add(avail_item.name);
                        AvailmenuItems.Add(avail_item);
                    }
                }               
            }

            avail_item = new menuItem();
            avail_item.name = "Multishortcut";
            avail_item.hyperlink = "";
            avail_item.imagePath = "games";
            avail_list.Items.Add(avail_item.name);
            AvailmenuItems.Add(avail_item);     

            avail_item = new menuItem();
            avail_item.name = "Exit";
            avail_item.hyperlink = "Exit";
            avail_item.imagePath = "shutdownbg";
            avail_list.Items.Add(avail_item.name);
            AvailmenuItems.Add(avail_item);           

            avail_item = new menuItem();
            avail_item.name = "Restart";
            avail_item.hyperlink = "Restart";
            avail_item.imagePath = "shutdownbg";
            avail_list.Items.Add(avail_item.name);
            AvailmenuItems.Add(avail_item);

            avail_item = new menuItem();
            avail_item.name = "Shutdown";
            avail_item.hyperlink = "Shutdown";
            avail_item.imagePath = "shutdownbg";
            avail_list.Items.Add(avail_item.name);
            AvailmenuItems.Add(avail_item);

            comboBox2.Items.Add("Menu 1");
            comboBox2.Items.Add("Menu 2");
            comboBox2.Items.Add("Menu 3");
            comboBox2.Items.Add("Menu 4");
            comboBox2.Items.Add("Menu 5");
            comboBox2.Items.Add("Menu 6");

            IdleTimeOther.Items.Add("0");
            IdleTimeOther.Items.Add("1");
            IdleTimeOther.Items.Add("2");
            IdleTimeOther.Items.Add("3");
            IdleTimeOther.Items.Add("4");
            IdleTimeOther.Items.Add("5");
            IdleTimeOther.Items.Add("6");
            IdleTimeOther.Items.Add("7");
            IdleTimeOther.Items.Add("8");
            IdleTimeOther.Items.Add("9");
            IdleTimeOther.Items.Add("10");

            IdleTimeBasic.Items.Add("0");
            IdleTimeBasic.Items.Add("1");
            IdleTimeBasic.Items.Add("2");
            IdleTimeBasic.Items.Add("3");
            IdleTimeBasic.Items.Add("4");
            IdleTimeBasic.Items.Add("5");
            IdleTimeBasic.Items.Add("6");
            IdleTimeBasic.Items.Add("7");
            IdleTimeBasic.Items.Add("8");
            IdleTimeBasic.Items.Add("9");
            IdleTimeBasic.Items.Add("10");

            return true;

        }

        public void readFHConfigFile(MediaPortal.Profile.Settings xmlreader)
        {
            latestPictures = xmlreader.GetValueAsString("LatestMediaHandler", "latestPictures", String.Empty);
            latestMusic = xmlreader.GetValueAsString("LatestMediaHandler", "latestMusic", String.Empty);
            latestMovingPictures = xmlreader.GetValueAsString("LatestMediaHandler", "latestMovingPictures", String.Empty);
            latestTVSeries = xmlreader.GetValueAsString("LatestMediaHandler", "latestTVSeries", String.Empty);
            latestTVRecordings = xmlreader.GetValueAsString("LatestMediaHandler", "latestTVRecordings", String.Empty);
        }
        public void readConfigFile(MediaPortal.Profile.Settings xmlreader)
        {
            textBox1.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText1Name", "");
            //new_prefix.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText1Pefix", "");
            textBox15.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText1ImagePath", "");
            string sImgType = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText1Multiimage", "");
            useMenu1 = xmlreader.GetValueAsString("BackdropMenuEditor", "useMenu1", "");
            if (useMenu1 != null && useMenu1.Length > 0)
            {
                if (useMenu1.Equals("True", StringComparison.CurrentCulture))
                    checkBox2.Checked = true;
                else
                    checkBox2.Checked = false;
            }
            else
            {
                useMenu1 = "True";
                checkBox2.Checked = true;
            }

            if (textBox15.Text.Length < 1)
                textBox15.Text = "pictures";
            if (sImgType != null && sImgType.Length > 0)
            {
                if (sImgType.Equals("Yes"))
                {
                    comboBox11.SelectedItem = "Multiple Images";
                    button5.Visible = true;
                }
                else if (sImgType.Equals("No"))
                {
                    comboBox11.SelectedItem = "Single Image";
                    button5.Visible = true;
                }
                else if (textBox15.Text.StartsWith("Random"))
                {
                    comboBox11.SelectedItem = "Fanart Plugin";
                    comboBox3.SelectedItem = textBox15.Text;
                    textBox15.Text = "";
                    button5.Visible = false;
                }
                else if (getFilenameOnly(textBox15.Text).IndexOf(".") <= 0)
                {
                    comboBox11.SelectedItem = "Multiple Images";
                    button5.Visible = true;
                }
                else
                {
                    comboBox11.SelectedItem = "Single Image";
                    button5.Visible = true;
                }
            }
            else
            {
                if (textBox15.Text.StartsWith("Random"))
                {
                    comboBox11.SelectedItem = "Fanart Plugin";
                    comboBox3.SelectedItem = textBox15.Text;
                    textBox15.Text = "";
                    button5.Visible = false;
                }
                else if (getFilenameOnly(textBox15.Text).IndexOf(".") <= 0)
                {
                    comboBox11.SelectedItem = "Multiple Images";
                    button5.Visible = true;
                }
                else
                {
                    comboBox11.SelectedItem = "Single Image";
                    button5.Visible = true;
                }
            }
            textBox2.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText2Name", "");
            textBox16.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText2ImagePath", "");
            sImgType = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText2Multiimage", "");
            useMenu2 = xmlreader.GetValueAsString("BackdropMenuEditor", "useMenu2", "");
            if (useMenu2 != null && useMenu2.Length > 0)
            {
                if (useMenu2.Equals("True", StringComparison.CurrentCulture))
                    checkBox1.Checked = true;
                else
                    checkBox1.Checked = false;
            }
            else
            {
                useMenu2 = "True";
                checkBox1.Checked = true;
            }
            if (textBox16.Text.Length < 1)
                textBox16.Text = "music";
            if (sImgType != null && sImgType.Length > 0)
            {
                if (sImgType.Equals("Yes"))
                {
                    button9.Visible = true;
                    comboBox10.SelectedItem = "Multiple Images";
                }
                else if (sImgType.Equals("No"))
                {
                    comboBox10.SelectedItem = "Single Image";
                    button9.Visible = true;
                }
                else if (textBox16.Text.StartsWith("Random"))
                {
                    comboBox10.SelectedItem = "Fanart Plugin";
                    comboBox4.SelectedItem = textBox16.Text;
                    textBox16.Text = "";
                    button9.Visible = false;
                }
                else if (getFilenameOnly(textBox16.Text).IndexOf(".") <= 0)
                {
                    comboBox10.SelectedItem = "Multiple Images";
                    button9.Visible = true;
                }
                else
                {
                    comboBox10.SelectedItem = "Single Image";
                    button9.Visible = true;
                }
            }
            else
            {
                if (textBox16.Text.StartsWith("Random"))
                {
                    comboBox10.SelectedItem = "Fanart Plugin";
                    comboBox4.SelectedItem = textBox16.Text;
                    textBox16.Text = "";
                    button9.Visible = false;
                }
                else if (getFilenameOnly(textBox16.Text).IndexOf(".") <= 0)
                {
                    comboBox10.SelectedItem = "Multiple Images";
                    button9.Visible = true;
                }
                else
                {
                    comboBox10.SelectedItem = "Single Image";
                    button9.Visible = true;
                }
            }
            textBox4.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText3Name", "");
            textBox17.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText3ImagePath", "");
            sImgType = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText3Multiimage", "");
            useMenu3 = xmlreader.GetValueAsString("BackdropMenuEditor", "useMenu3", "");
            if (useMenu3 != null && useMenu3.Length > 0)
            {
                if (useMenu3.Equals("True", StringComparison.CurrentCulture))
                    checkBox3.Checked = true;
                else
                    checkBox3.Checked = false;
            }
            else
            {
                useMenu3 = "True";
                checkBox3.Checked = true;
            }
            if (textBox17.Text.Length < 1)
                textBox17.Text = "movies";
            if (sImgType != null && sImgType.Length > 0)
            {
                if (sImgType.Equals("Yes"))
                {
                    comboBox9.SelectedItem = "Multiple Images";
                    button13.Visible = true;
                }
                else if (sImgType.Equals("No"))
                {
                    comboBox9.SelectedItem = "Single Image";
                    button13.Visible = true;
                }
                else if (textBox17.Text.StartsWith("Random"))
                {
                    comboBox9.SelectedItem = "Fanart Plugin";
                    comboBox12.SelectedItem = textBox17.Text;
                    textBox17.Text = "";
                    button13.Visible = false;
                }
                else if (getFilenameOnly(textBox17.Text).IndexOf(".") <= 0)
                {
                    comboBox9.SelectedItem = "Multiple Images";
                    button13.Visible = true;
                }
                else
                {
                    comboBox9.SelectedItem = "Single Image";
                    button13.Visible = true;
                }
            }
            else
            {
                if (textBox17.Text.StartsWith("Random"))
                {
                    comboBox9.SelectedItem = "Fanart Plugin";
                    comboBox12.SelectedItem = textBox17.Text;
                    textBox17.Text = "";
                    button13.Visible = false;
                }
                else if (getFilenameOnly(textBox17.Text).IndexOf(".") <= 0)
                {
                    comboBox9.SelectedItem = "Multiple Images";
                    button13.Visible = true;
                }
                else
                {
                    comboBox9.SelectedItem = "Single Image";
                    button13.Visible = true;
                }
            }
            textBox6.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText4Name", "");
            textBox18.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText4ImagePath", "");
            sImgType = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText4Multiimage", "");
            useMenu4 = xmlreader.GetValueAsString("BackdropMenuEditor", "useMenu4", "");
            if (useMenu4 != null && useMenu4.Length > 0)
            {
                if (useMenu4.Equals("True", StringComparison.CurrentCulture))
                    checkBox4.Checked = true;
                else
                    checkBox4.Checked = false;
            }
            else
            {
                useMenu4 = "True";
                checkBox4.Checked = true;
            }
            if (textBox18.Text.Length < 1)
                textBox18.Text = "games";
            if (sImgType != null && sImgType.Length > 0)
            {
                if (sImgType.Equals("Yes"))
                {
                    comboBox8.SelectedItem = "Multiple Images";
                    button18.Visible = true;
                }
                else if (sImgType.Equals("No"))
                {
                    comboBox8.SelectedItem = "Single Image";
                    button18.Visible = true;
                }
                else if (textBox18.Text.StartsWith("Random"))
                {
                    comboBox8.SelectedItem = "Fanart Plugin";
                    comboBox13.SelectedItem = textBox18.Text;
                    textBox18.Text = "";
                    button18.Visible = false;
                }
                else if (getFilenameOnly(textBox18.Text).IndexOf(".") <= 0)
                {
                    comboBox8.SelectedItem = "Multiple Images";
                    button18.Visible = true;
                }
                else
                {
                    comboBox8.SelectedItem = "Single Image";
                    button18.Visible = true;
                }
            }
            else
            {
                if (textBox18.Text.StartsWith("Random"))
                {
                    comboBox8.SelectedItem = "Fanart Plugin";
                    comboBox13.SelectedItem = textBox18.Text;
                    textBox18.Text = "";
                    button18.Visible = false;
                }
                else if (getFilenameOnly(textBox18.Text).IndexOf(".") <= 0)
                {
                    comboBox8.SelectedItem = "Multiple Images";
                    button18.Visible = true;
                }
                else
                {
                    comboBox8.SelectedItem = "Single Image";
                    button18.Visible = true;
                }
            }
            textBox8.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText5Name", "");
            textBox19.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText5ImagePath", "");
            sImgType = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText5Multiimage", "");
            useMenu5 = xmlreader.GetValueAsString("BackdropMenuEditor", "useMenu5", "");
            if (useMenu5 != null && useMenu5.Length > 0)
            {
                if (useMenu5.Equals("True", StringComparison.CurrentCulture))
                    checkBox5.Checked = true;
                else
                    checkBox5.Checked = false;
            }
            else
            {
                useMenu5 = "True";
                checkBox5.Checked = true;
            }
            if (textBox19.Text.Length < 1)
                textBox19.Text = "pluginsbg";
            if (sImgType != null && sImgType.Length > 0)
            {
                if (sImgType.Equals("Yes"))
                {
                    comboBox7.SelectedItem = "Multiple Images";
                    button19.Visible = true;
                }
                else if (sImgType.Equals("No"))
                {
                    comboBox7.SelectedItem = "Single Image";
                    button19.Visible = true;
                }
                else if (textBox19.Text.StartsWith("Random"))
                {
                    comboBox7.SelectedItem = "Fanart Plugin";
                    comboBox14.SelectedItem = textBox19.Text;
                    textBox19.Text = "";
                    button19.Visible = false;
                }
                else if (getFilenameOnly(textBox19.Text).IndexOf(".") <= 0)
                {
                    comboBox7.SelectedItem = "Multiple Images";
                    button19.Visible = true;
                }
                else
                {
                    comboBox7.SelectedItem = "Single Image";
                    button19.Visible = true;
                }
            }
            else
            {
                if (textBox19.Text.StartsWith("Random"))
                {
                    comboBox7.SelectedItem = "Fanart Plugin";
                    comboBox14.SelectedItem = textBox19.Text;
                    textBox19.Text = "";
                    button19.Visible = false;
                }
                else if (getFilenameOnly(textBox19.Text).IndexOf(".") <= 0)
                {
                    comboBox7.SelectedItem = "Multiple Images";
                    button19.Visible = true;
                }
                else
                {
                    comboBox7.SelectedItem = "Single Image";
                    button19.Visible = true;
                }
            }
            textBox7.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText6Name", "");
            textBox3.Text = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText6ImagePath", "");
            sImgType = xmlreader.GetValueAsString("BackdropMenuEditor", "menuText6Multiimage", "");
            useMenu6 = xmlreader.GetValueAsString("BackdropMenuEditor", "useMenu6", "");
            if (useMenu6 != null && useMenu6.Length > 0)
            {
                if (useMenu6.Equals("True", StringComparison.CurrentCulture))
                    checkBox6.Checked = true;
                else
                    checkBox6.Checked = false;
            }
            else
            {
                useMenu6 = "True";
                checkBox6.Checked = true;
            }
            if (textBox7.Text.Length < 1)
                textBox7.Text = "Plugins";
            if (textBox3.Text.Length < 1)
                textBox3.Text = "pluginsbg";
            
            if (sImgType != null && sImgType.Length > 0)
            {
                if (sImgType.Equals("Yes"))
                {
                    comboBox6.SelectedItem = "Multiple Images";
                    button23.Visible = true;
                }
                else if (sImgType.Equals("No"))
                {
                    comboBox6.SelectedItem = "Single Image";
                    button23.Visible = true;
                }
                else if (textBox3.Text.StartsWith("Random"))
                {
                    comboBox6.SelectedItem = "Fanart Plugin";
                    comboBox15.SelectedItem = textBox3.Text;
                    textBox3.Text = "";
                    button23.Visible = false;
                }
                else if (getFilenameOnly(textBox3.Text).IndexOf(".") <= 0)
                {
                    comboBox6.SelectedItem = "Multiple Images";
                    button23.Visible = true;
                }
                else
                {
                    comboBox6.SelectedItem = "Single Image";
                    button23.Visible = true;
                }
            }
            else
            {
                if (textBox3.Text.StartsWith("Random"))
                {
                    comboBox6.SelectedItem = "Fanart Plugin";
                    comboBox15.SelectedItem = textBox3.Text;
                    textBox3.Text = "";
                    button23.Visible = false;
                }
                else if (getFilenameOnly(textBox3.Text).IndexOf(".") <= 0)
                {
                    comboBox6.SelectedItem = "Multiple Images";
                    button23.Visible = true;
                }
                else
                {
                    comboBox6.SelectedItem = "Single Image";
                    button23.Visible = true;
                }
            }

            doHideClock = xmlreader.GetValueAsString("BackdropMenuEditor", "doHideClock", "");
            if (doHideClock != null && doHideClock.Length > 0)
            {
                if (doHideClock.Equals("True", StringComparison.CurrentCulture))
                    checkBox8.Checked = true;
                else
                    checkBox8.Checked = false;
            }
            else
            {
                doHideClock = "False";
                checkBox8.Checked = false;
            }

            doCenterMenu = xmlreader.GetValueAsString("BackdropMenuEditor", "doCenterMenu", "");
            if (doCenterMenu != null && doCenterMenu.Length > 0)
            {
                if (doCenterMenu.Equals("True", StringComparison.CurrentCulture))
                    checkBox7.Checked = true;
                else
                    checkBox7.Checked = false;
            }
            else
            {
                doCenterMenu = "False";
                checkBox7.Checked = false;
            }

            comboBox2.SelectedItem = xmlreader.GetValueAsString("BackdropMenuEditor", "menuDefaultMenu", "");
            if (comboBox2.SelectedIndex < 0)
                comboBox2.SelectedItem = "Menu 3";

            IdleTimeOther.SelectedItem = xmlreader.GetValueAsString("BackdropMenuEditor", "IdleTimeOther", "");
            if (IdleTimeOther.SelectedIndex < 0)
                IdleTimeOther.SelectedItem = "3";

            IdleTimeBasic.SelectedItem = xmlreader.GetValueAsString("BackdropMenuEditor", "IdleTimeBasic", "");
            if (IdleTimeBasic.SelectedIndex < 0)
                IdleTimeBasic.SelectedItem = "7";
          
            int a = 0;

            do
            {
                //Read menu
                if (xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameA" + a, "") != "")
                {
                    menuItem used_item = new menuItem();
                    used_item.name = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameA" + a, "");
                    used_item.imagePath = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemBgA" + a, "");
                    used_item.hyperlink = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemHyperlinkA" + a, "");
                    used_item.useLatestMedia = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemUseLatestMediaA" + a, "");
                    used_item.mainCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemMainCategoryA" + a, "");
                    used_item.subCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemSubCategoryA" + a, "");
                    menuItems1.Add(used_item);
                    used_list.Items.Add(used_item.name);
                    configloaded = true;
                }
                a++;
            } while (a < 25);

            a = 0;
            do
            {
                //Read menu
                if (xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameB" + a, "") != "")
                {
                    menuItem used_item = new menuItem();
                    used_item.name = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameB" + a, "");
                    used_item.imagePath = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemBgB" + a, "");
                    used_item.hyperlink = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemHyperlinkB" + a, "");
                    used_item.useLatestMedia = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemUseLatestMediaB" + a, "");
                    used_item.mainCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemMainCategoryB" + a, "");
                    used_item.subCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemSubCategoryB" + a, "");
                    menuItems2.Add(used_item);
                    listBox1.Items.Add(used_item.name);
                    configloaded = true;
                }
                a++;
            } while (a < 25);

            a = 0;
            do
            {
                //Read menu
                if (xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameC" + a, "") != "")
                {
                    menuItem used_item = new menuItem();
                    used_item.name = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameC" + a, "");
                    used_item.imagePath = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemBgC" + a, "");
                    used_item.hyperlink = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemHyperlinkC" + a, "");
                    used_item.useLatestMedia = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemUseLatestMediaC" + a, "");
                    used_item.mainCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemMainCategoryC" + a, "");
                    used_item.subCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemSubCategoryC" + a, "");
                    menuItems3.Add(used_item);
                    listBox2.Items.Add(used_item.name);
                    configloaded = true;
                }
                a++;
            } while (a < 25);

            a = 0;
            do
            {
                //Read menu
                if (xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameD" + a, "") != "")
                {
                    menuItem used_item = new menuItem();
                    used_item.name = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameD" + a, "");
                    used_item.imagePath = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemBgD" + a, "");
                    used_item.hyperlink = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemHyperlinkD" + a, "");
                    used_item.useLatestMedia = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemUseLatestMediaD" + a, "");
                    used_item.mainCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemMainCategoryD" + a, "");
                    used_item.subCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemSubCategoryD" + a, "");
                    menuItems4.Add(used_item);
                    listBox3.Items.Add(used_item.name);
                    configloaded = true;
                }
                a++;
            } while (a < 25);

            a = 0;
            do
            {
                //Read menu
                if (xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameE" + a, "") != "")
                {
                    menuItem used_item = new menuItem();
                    used_item.name = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameE" + a, "");
                    used_item.imagePath = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemBgE" + a, "");
                    used_item.hyperlink = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemHyperlinkE" + a, "");
                    used_item.useLatestMedia = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemUseLatestMediaE" + a, "");
                    used_item.mainCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemMainCategoryE" + a, "");
                    used_item.subCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemSubCategoryE" + a, "");
                    menuItems5.Add(used_item);
                    listBox4.Items.Add(used_item.name);
                    configloaded = true;
                }
                a++;
            } while (a < 25);

            a = 0;
            do
            {
                //Read menu
                if (xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameF" + a, "") != "")
                {
                    menuItem used_item = new menuItem();
                    used_item.name = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemNameF" + a, "");
                    used_item.imagePath = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemBgF" + a, "");
                    used_item.hyperlink = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemHyperlinkF" + a, "");
                    used_item.useLatestMedia = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemUseLatestMediaF" + a, "");
                    used_item.mainCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemMainCategoryF" + a, "");
                    used_item.subCategory = xmlreader.GetValueAsString("BackdropMenuEditor", "menuItemSubCategoryF" + a, "");
                    menuItems6.Add(used_item);
                    listBox5.Items.Add(used_item.name);
                    configloaded = true;
                }
                a++;
            } while (a < 25);
        }

        public bool loadMenuIDs(bool onLoad)
        {

            menuItems1.Clear();
            menuItems2.Clear();
            menuItems3.Clear();
            menuItems4.Clear();
            menuItems5.Clear();
            menuItems6.Clear();
            used_list.Enabled = true;
            listBox1.Enabled = true;
            listBox2.Enabled = true;
            listBox3.Enabled = true;
            listBox4.Enabled = true;
            listBox5.Enabled = true;
            used_list.Items.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();            
            textBox1.Clear();
            //new_prefix.Clear();
            textBox2.Clear();;
            //textBox3.Clear();
            textBox4.Clear();
            //textBox5.Clear();
            textBox6.Clear();
            //textBox7.Clear();
            textBox8.Clear();
            //textBox9.Clear();

            if (File.Exists(Config.GetFile(Configuration.Config.Dir.Config, "BackdropMenuEditor.xml")) == false)
            {
                File.Copy(path + @"\BackdropMenuEditor.xml", Config.GetFolder(Config.Dir.Config) + @"\BackdropMenuEditor.xml");                                   
            }
        


            using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Configuration.Config.Dir.Config, "BackdropMenuEditor.xml")))
            {
                readConfigFile(xmlreader);
            }

            using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Configuration.Config.Dir.Config, "LatestMediaHandler.xml")))
            {
                readFHConfigFile(xmlreader);
            } 

            //return true;
            if (used_list.Items.Count > 0)
            {
                return true;
            }
            else
            {
                // Dont need to complain when first loading the app as its possible that the skin isnt installed
                if (!onLoad)
                    MessageBox.Show("Error reading directory.");
                return false;

            }

        }



        private void button4_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at top
            if (listBox1.SelectedItem != null && listBox1.SelectedIndex > 0)
            {
                int index = listBox1.SelectedIndex;
                Object listItem = listBox1.SelectedItem;
                menuItem mnuItem = menuItems2[index];
                listBox1.Items.RemoveAt(index);
                menuItems2.RemoveAt(index);
                listBox1.Items.Insert(index - 1, listItem);
                menuItems2.Insert(index - 1, mnuItem);
                listBox1.SelectedIndex = index - 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at top
            if (listBox2.SelectedItem != null && listBox2.SelectedIndex > 0)
            {
                int index = listBox2.SelectedIndex;
                Object listItem = listBox2.SelectedItem;
                menuItem mnuItem = menuItems3[index];
                listBox2.Items.RemoveAt(index);
                menuItems3.RemoveAt(index);
                listBox2.Items.Insert(index - 1, listItem);
                menuItems3.Insert(index - 1, mnuItem);
                listBox2.SelectedIndex = index - 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at top
            if (listBox3.SelectedItem != null && listBox3.SelectedIndex > 0)
            {
                int index = listBox3.SelectedIndex;
                Object listItem = listBox3.SelectedItem;
                menuItem mnuItem = menuItems4[index];
                listBox3.Items.RemoveAt(index);
                menuItems4.RemoveAt(index);
                listBox3.Items.Insert(index - 1, listItem);
                menuItems4.Insert(index - 1, mnuItem);
                listBox3.SelectedIndex = index - 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at top
            if (listBox4.SelectedItem != null && listBox4.SelectedIndex > 0)
            {
                int index = listBox4.SelectedIndex;
                Object listItem = listBox4.SelectedItem;
                menuItem mnuItem = menuItems5[index];
                listBox4.Items.RemoveAt(index);
                menuItems5.RemoveAt(index);
                listBox4.Items.Insert(index - 1, listItem);
                menuItems5.Insert(index - 1, mnuItem);
                listBox4.SelectedIndex = index - 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at bottom
            if (listBox1.SelectedItem != null && listBox1.SelectedIndex < listBox1.Items.Count - 1)
            {
                int index = listBox1.SelectedIndex;
                Object listItem = listBox1.SelectedItem;
                menuItem mnuItem = menuItems2[index];
                listBox1.Items.RemoveAt(index);
                menuItems2.RemoveAt(index);
                if (index < listBox1.Items.Count)
                {
                    listBox1.Items.Insert(index + 1, listItem);
                    menuItems2.Insert(index + 1, mnuItem);
                }
                else
                {
                    listBox1.Items.Add(listItem);
                    menuItems2.Add(mnuItem);
                }
                listBox1.SelectedIndex = index + 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at bottom
            if (listBox2.SelectedItem != null && listBox2.SelectedIndex < listBox2.Items.Count - 1)
            {
                int index = listBox2.SelectedIndex;
                Object listItem = listBox2.SelectedItem;
                menuItem mnuItem = menuItems3[index];
                listBox2.Items.RemoveAt(index);
                menuItems3.RemoveAt(index);
                if (index < listBox2.Items.Count)
                {
                    listBox2.Items.Insert(index + 1, listItem);
                    menuItems3.Insert(index + 1, mnuItem);
                }
                else
                {
                    listBox2.Items.Add(listItem);
                    menuItems3.Add(mnuItem);
                }
                listBox2.SelectedIndex = index + 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at bottom
            if (listBox3.SelectedItem != null && listBox3.SelectedIndex < listBox3.Items.Count - 1)
            {
                int index = listBox3.SelectedIndex;
                Object listItem = listBox3.SelectedItem;
                menuItem mnuItem = menuItems4[index];
                listBox3.Items.RemoveAt(index);
                menuItems4.RemoveAt(index);
                if (index < listBox3.Items.Count)
                {
                    listBox3.Items.Insert(index + 1, listItem);
                    menuItems4.Insert(index + 1, mnuItem);
                }
                else
                {
                    listBox3.Items.Add(listItem);
                    menuItems4.Add(mnuItem);
                }
                listBox3.SelectedIndex = index + 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at bottom
            if (listBox4.SelectedItem != null && listBox4.SelectedIndex < listBox4.Items.Count - 1)
            {
                int index = listBox4.SelectedIndex;
                Object listItem = listBox4.SelectedItem;
                menuItem mnuItem = menuItems5[index];
                listBox4.Items.RemoveAt(index);
                menuItems5.RemoveAt(index);
                if (index < listBox4.Items.Count)
                {
                    listBox4.Items.Insert(index + 1, listItem);
                    menuItems5.Insert(index + 1, mnuItem);
                }
                else
                {
                    listBox4.Items.Add(listItem);
                    menuItems5.Add(mnuItem);
                }
                listBox4.SelectedIndex = index + 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            removeToolStripMenuItem_Click2(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            removeToolStripMenuItem_Click3(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            removeToolStripMenuItem_Click4(sender, e);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            removeToolStripMenuItem_Click5(sender, e);
        }


        private void HandleParameters()
        {
            try
            {
                //movingpictures
                if (new_windowid.Text.Equals("96742"))
                {
                    tabControl2.TabPages[1].Enabled = true;
                    comboBox21.SelectedIndex = -1;
                    comboBox20.SelectedIndex = -1;
                    comboBox22.SelectedIndex = -1;
                    comboBox23.SelectedIndex = -1;
                    label17.Visible = true;
                    label18.Visible = true;
                    comboBox18.Visible = true;
                    comboBox19.Visible = true;
                    comboBox21.Visible = false;
                    comboBox20.Visible = false;
                    label19.Visible = false;
                    label26.Visible = false;
                    label34.Visible = false;
                    label35.Visible = false;
                    comboBox22.Visible = false;
                    comboBox23.Visible = false;
                    tabControl2.SelectedTab = tabControl2.TabPages[1];
                    new_name.Focus();
                    comboBox18.Items.Clear();
                    ArrayList al = new ArrayList();
                    al.Add("");
                    try
                    {
                        string filename = Path.Combine(Path.Combine(Config.GetFolder(Configuration.Config.Dir.Plugins), @"windows"), "MovingPictures.dll");
                        if (IsAssemblyAvailable("MovingPictures", new Version(1, 1, 0, 0), filename))
                        {
                            al = MovingPictureHandler.GetRootNodes();
                        }
                    }
                    catch { }                       
                    foreach (string s in al)
                    {
                        comboBox18.Items.Add(s);
                    }
                    if (al != null)
                    {
                        al.Clear();
                    }
                    al = null;
                }
                //onlinevideos
                else if (new_windowid.Text.Equals("4755"))
                {
                    tabControl2.TabPages[1].Enabled = true;
                    comboBox18.SelectedIndex = -1;
                    comboBox19.SelectedIndex = -1;
                    comboBox22.SelectedIndex = -1;
                    comboBox23.SelectedIndex = -1;
                    comboBox21.Visible = true;
                    comboBox20.Visible = true;
                    label19.Visible = true;
                    label26.Visible = true;
                    comboBox18.Visible = false;
                    comboBox19.Visible = false;
                    label17.Visible = false;
                    label18.Visible = false;
                    label34.Visible = false;
                    label35.Visible = false;
                    comboBox22.Visible = false;
                    comboBox23.Visible = false;
                    tabControl2.SelectedTab = tabControl2.TabPages[1];
                    new_name.Focus();
                    comboBox21.Items.Clear();
                    ArrayList al = new ArrayList();
                    al.Add("");
                    try
                    {
                        string filename = Path.Combine(Path.Combine(Config.GetFolder(Configuration.Config.Dir.Plugins), @"windows\OnlineVideos"), "OnlineVideos.dll");
                        if (IsAssemblyAvailable("OnlineVideos", new Version(0, 27, 0, 0), filename))
                        {
                            al = OnlineVideoHandler.GetSites();
                        }
                    }
                    catch { }
                    foreach (string s in al)
                    {
                        comboBox21.Items.Add(s);
                    }
                    if (al != null)
                    {
                        al.Clear();
                    }
                    al = null;
                }
                //tvseries
                else if (new_windowid.Text.Equals("9811"))
                {
                    tabControl2.TabPages[1].Enabled = true;
                    comboBox18.SelectedIndex = -1;
                    comboBox19.SelectedIndex = -1;
                    comboBox20.SelectedIndex = -1;
                    comboBox21.SelectedIndex = -1;
                    comboBox22.SelectedIndex = -1;
                    comboBox21.Visible = false;
                    comboBox20.Visible = false;
                    label19.Visible = false;
                    label26.Visible = false;
                    comboBox18.Visible = false;
                    comboBox19.Visible = false;
                    label17.Visible = false;
                    label18.Visible = false;
                    label34.Visible = false;
                    label35.Visible = true;
                    comboBox22.Visible = false;
                    comboBox23.Visible = true;
                    tabControl2.SelectedTab = tabControl2.TabPages[1];
                    new_name.Focus();
                    comboBox23.Items.Clear();

                    ArrayList al = new ArrayList();
                    al.Add("");
                    try
                    {
                        string filename = Path.Combine(Path.Combine(Config.GetFolder(Configuration.Config.Dir.Plugins), @"windows"), "MP-TVSeries.dll");
                        if (IsAssemblyAvailable("MP-TVSeries", new Version(2, 6, 5, 1265), filename))
                        {
                            al = TVSeriesHandler.GetViews();
                        }
                    }
                    catch { }

                    foreach (string s in al)
                    {
                        comboBox23.Items.Add(s);
                    }
                    if (al != null)
                    {
                        al.Clear();
                    }
                    al = null;
                }
                //music
                else if (new_windowid.Text.Equals("504") || new_windowid.Text.Equals("504"))
                {
                    tabControl2.TabPages[1].Enabled = true;
                    comboBox18.SelectedIndex = -1;
                    comboBox19.SelectedIndex = -1;
                    comboBox20.SelectedIndex = -1;
                    comboBox21.SelectedIndex = -1;
                    comboBox23.SelectedIndex = -1;
                    comboBox21.Visible = false;
                    comboBox20.Visible = false;
                    label19.Visible = false;
                    label26.Visible = false;
                    comboBox18.Visible = false;
                    comboBox19.Visible = false;
                    label17.Visible = false;
                    label18.Visible = false;
                    label34.Visible = true;
                    label35.Visible = false;
                    comboBox22.Visible = true;
                    comboBox23.Visible = false;
                    tabControl2.SelectedTab = tabControl2.TabPages[1];
                    new_name.Focus();
                    comboBox22.Items.Clear();
                    ArrayList al = null;
                    try
                    {
                        al = MusicHandler.GetViews();
                    }
                    catch { }
                    foreach (string s in al)
                    {
                        comboBox22.Items.Add(s);
                    }
                    if (al != null)
                    {
                        al.Clear();
                    }
                    al = null;
                }
                else
                {
                    comboBox18.SelectedIndex = -1;
                    comboBox19.SelectedIndex = -1;
                    comboBox21.SelectedIndex = -1;
                    comboBox20.SelectedIndex = -1;
                    comboBox22.SelectedIndex = -1;
                    comboBox23.SelectedIndex = -1;
                    tabControl2.TabPages[1].Enabled = false;
                }
            }
            catch
            { }
        }

        //Thank you to the author of StreamedMP for this method
        public static bool IsAssemblyAvailable(string name, Version ver, string filename)
        {
            bool result = false;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly a in assemblies)
            {
                try
                {
                    if (a.GetName().Name == name)
                    {
                        if (a.GetName().Version >= ver)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch
                {
                    result = false;
                }
            }

            if (!result)
            {
                try
                {
                    Assembly assembly = null;
                    if (string.IsNullOrEmpty(filename))
                    {
                        assembly = Assembly.ReflectionOnlyLoad(name);
                        if (assembly.GetName().Version >= ver) result = true;
                    }
                    else
                    {
                        assembly = Assembly.ReflectionOnlyLoadFrom(filename);
                        if (assembly.GetName().Version >= ver) result = true;
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }


        private void HandleMovingPictureSubCategories()
        {
            comboBox19.Items.Clear();
            if (comboBox18.SelectedItem != null)
            {
                ArrayList al = null;
                try
                {
                    al = MovingPictureHandler.GetSubNodes(comboBox18.SelectedItem.ToString());
                }
                catch { }
                foreach (string s in al)
                {
                    comboBox19.Items.Add(s);
                }
                if (al != null)
                {
                    al.Clear();
                }
                al = null;
            }
        }

        private void avail_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateIndex = 999;
            used_list.SelectedIndex = -1;
            listBox1.SelectedIndex = -1;
            listBox2.SelectedIndex = -1;
            listBox3.SelectedIndex = -1;
            listBox4.SelectedIndex = -1;
            comboBox1.Enabled = true;
            if (avail_list.SelectedIndex >= 0)
            {
                new_windowid.Text = "" + AvailmenuItems[avail_list.SelectedIndex].hyperlink;
                new_name.Text = "" + AvailmenuItems[avail_list.SelectedIndex].name;
                new_imagepath.Text = "" + AvailmenuItems[avail_list.SelectedIndex].imagePath;
                toolStripStatusLabel1.Text = "Window ID: " + AvailmenuItems[avail_list.SelectedIndex].hyperlink;
                used_list.SelectedIndex = -1;
                if (AvailmenuItems[avail_list.SelectedIndex].name.Equals("Multishortcut"))
                {
                    new_windowid.Enabled = true;
                }
                try
                {
                    HandleParameters();
                }
                catch
                { }
            }
        }

        private void writeMenuPart2(StreamWriter sw, int itemNo, List<menuItem> itemIn)
        {            
            int iCount = 0;
            string visibleTag = "";
            string xValue = "";
            string yValue = "";
            iCount = itemIn.Count;
            if (itemNo == 1)
            {
                //iCount = menuItems1.Count;
                xValue = (GetXStartValue()-5).ToString();
            }
            else if (itemNo == 2)
            {
                //iCount = menuItems2.Count;
                xValue = (GetXStartValue()+315).ToString();
            }
            else if (itemNo == 3)
            {
                //iCount = menuItems3.Count;
                xValue = (GetXStartValue() + 636).ToString(); 
            }
            else if (itemNo == 4)
            {
                //iCount = menuItems4.Count;
                xValue = (GetXStartValue() + 957).ToString(); //"957";
            }
            else if (itemNo == 5)
            {
                //iCount = menuItems5.Count;
                xValue = (GetXStartValue() + 1278).ToString(); //"1278";
            }
            else if (itemNo == 6)
            {
                //iCount = menuItems6.Count;
                xValue = (GetXStartValue() + 1599).ToString(); //"1599";
            }

            if (iCount == 1)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 2)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 3)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 4)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 5)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 6)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 7)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)|Control.HasFocus(770" + itemNo + "9)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 8)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)|Control.HasFocus(770" + itemNo + "9)|Control.HasFocus(770" + itemNo + "10)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 9)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)|Control.HasFocus(770" + itemNo + "9)|Control.HasFocus(770" + itemNo + "10)|Control.HasFocus(770" + itemNo + "11)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 10)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)|Control.HasFocus(770" + itemNo + "9)|Control.HasFocus(770" + itemNo + "10)|Control.HasFocus(770" + itemNo + "11)|Control.HasFocus(770" + itemNo + "12)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 11)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)|Control.HasFocus(770" + itemNo + "9)|Control.HasFocus(770" + itemNo + "10)|Control.HasFocus(770" + itemNo + "11)|Control.HasFocus(770" + itemNo + "12)|Control.HasFocus(770" + itemNo + "13)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 12)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)|Control.HasFocus(770" + itemNo + "9)|Control.HasFocus(770" + itemNo + "10)|Control.HasFocus(770" + itemNo + "11)|Control.HasFocus(770" + itemNo + "12)|Control.HasFocus(770" + itemNo + "13)|Control.HasFocus(770" + itemNo + "14)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 13)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)|Control.HasFocus(770" + itemNo + "9)|Control.HasFocus(770" + itemNo + "10)|Control.HasFocus(770" + itemNo + "11)|Control.HasFocus(770" + itemNo + "12)|Control.HasFocus(770" + itemNo + "13)|Control.HasFocus(770" + itemNo + "14)|Control.HasFocus(770" + itemNo + "15)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 14)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)|Control.HasFocus(770" + itemNo + "9)|Control.HasFocus(770" + itemNo + "10)|Control.HasFocus(770" + itemNo + "11)|Control.HasFocus(770" + itemNo + "12)|Control.HasFocus(770" + itemNo + "13)|Control.HasFocus(770" + itemNo + "14)|Control.HasFocus(770" + itemNo + "15)|Control.HasFocus(770" + itemNo + "16)]+!Control.isvisible(93761234)</visible>";
            else if (iCount == 15)
                visibleTag = "  <visible allowhiddenfocus=\"true\">[Control.HasFocus(770" + itemNo + "2)|Control.HasFocus(770" + itemNo + "3)|Control.HasFocus(770" + itemNo + "4)|Control.HasFocus(770" + itemNo + "5)|Control.HasFocus(770" + itemNo + "6)|Control.HasFocus(770" + itemNo + "7)|Control.HasFocus(770" + itemNo + "8)|Control.HasFocus(770" + itemNo + "9)|Control.HasFocus(770" + itemNo + "10)|Control.HasFocus(770" + itemNo + "11)|Control.HasFocus(770" + itemNo + "12)|Control.HasFocus(770" + itemNo + "13)|Control.HasFocus(770" + itemNo + "14)|Control.HasFocus(770" + itemNo + "15)|Control.HasFocus(770" + itemNo + "16)|Control.HasFocus(770" + itemNo + "16)]+!Control.isvisible(93761234)</visible>";

            int z = 0;

            int iHeight = ((iCount - 1) * 33) + 20 + 51;
            if (iHeight < 236)
                iHeight = 236;
            int iPosY = 977 - iHeight;


            rawXML.AppendLine("<control>");
            rawXML.AppendLine("  <description>Menu " + itemNo + " Submenu Background Top</description>");
            rawXML.AppendLine("  <type>image</type>");
            rawXML.AppendLine("  <id>0</id>");
            rawXML.AppendLine("  <posX>" + xValue + "</posX>");
            rawXML.AppendLine("  <posY>" + iPosY + "</posY>");
            rawXML.AppendLine("  <width>333</width>");
            rawXML.AppendLine("  <height>" + iHeight + "</height>");
            rawXML.AppendLine("  <shouldCache>true</shouldCache>");
            rawXML.AppendLine("  <texture>menu_selected_bg_top.png</texture>");
            rawXML.AppendLine(visibleTag.Replace("<visible allowhiddenfocus=\"true\">", "<visible allowhiddenfocus=\"true\">control.isvisible(93761236)+"));
            rawXML.AppendLine("  <animation effect=\"zoom\" start=\"90,10\" end=\"100,100\" acceleration=\"-1.3\" center=\"" + (Int32.Parse(xValue) + 166) + "," + Int32.Parse((iPosY + iHeight).ToString()) + "\" time=\"320\" reversible=\"true\">visible</animation>  ");
            rawXML.AppendLine("  <animation effect=\"fade\" start=\"0\" end =\"100\" time=\"120\" acceleration=\"-1.2\" reversible=\"true\">hidden</animation>");
            rawXML.AppendLine("  <animation effect=\"zoom\" start=\"100,100\" end=\"100,10\" center=\"" + (Int32.Parse(xValue) + 166) + "," + Int32.Parse((iPosY + iHeight).ToString()) + "\" time=\"120\" acceleration=\"-1.2\" reversible=\"true\">hidden</animation>  ");
            rawXML.AppendLine("</control>");
            rawXML.AppendLine(" ");

            rawXML.AppendLine("<control>");
            rawXML.AppendLine("  <description>Menu " + itemNo + " Selected - Nevigating from left</description>");
            rawXML.AppendLine("  <type>image</type>");
            rawXML.AppendLine("  <id>0</id>");
            rawXML.AppendLine("  <posX>" + xValue + "</posX>");
            rawXML.AppendLine("  <posY>982</posY>");
            rawXML.AppendLine("  <width>333</width>");
            rawXML.AppendLine("  <height>52</height>");
            rawXML.AppendLine("  <shouldCache>true</shouldCache>");
            rawXML.AppendLine("  <texture>menu.selected.png</texture>");
            rawXML.AppendLine(visibleTag.Replace("<visible allowhiddenfocus=\"true\">", "<visible allowhiddenfocus=\"true\">Control.isvisible(93761254)+"));            
            rawXML.AppendLine("  <animation effect=\"slide\" start=\"-333,0\" end=\"0,0\" time=\"200\" acceleration=\"-0.9\" reversible=\"false\">visible</animation>");
            rawXML.AppendLine("</control>");
            rawXML.AppendLine(" ");
            
            rawXML.AppendLine("<control>");
            rawXML.AppendLine("  <description>Menu " + itemNo + " Selected - Nevigating from right</description>");
            rawXML.AppendLine("  <type>image</type>");
            rawXML.AppendLine("  <id>0</id>");
            rawXML.AppendLine("  <posX>" + xValue + "</posX>");
            rawXML.AppendLine("  <posY>982</posY>");
            rawXML.AppendLine("  <width>333</width>");
            rawXML.AppendLine("  <height>52</height>");
            rawXML.AppendLine("  <shouldCache>true</shouldCache>");
            rawXML.AppendLine("  <texture>menu.selected.png</texture>");
            rawXML.AppendLine(visibleTag.Replace("</visible>", "+!Control.isvisible(93761254)</visible>"));
            rawXML.AppendLine("  <animation effect=\"slide\" start=\"333,0\" end=\"0,0\" time=\"200\" acceleration=\"-0.9\" reversible=\"false\">visible</animation>");
            rawXML.AppendLine("</control>");
            rawXML.AppendLine(" ");

            foreach (menuItem item in itemIn)
            {
                getSubMenyPart(sw, itemNo, z, xValue, yValue, visibleTag, item, iCount, itemIn.Count);
                z++;
            }
            
        }





        private string GetParameterValue(string s)
        {
            if (s.IndexOf("<") > 0 && s.IndexOf(">") > 0)
            {
                s = s.Substring(s.IndexOf("<")+1, (s.IndexOf(">") - s.IndexOf("<"))-1);
            }
            return s;
        }

        private void getSubMenyPart(StreamWriter sw, int itemNo, int z, string xValue, string yValue, string visibleTag, menuItem item, int iCount, int numItems)
        {
            int iHeight = ((iCount - 1) * 33) + 20 + 51;
            if (iHeight < 236)
                iHeight = 236;
            yValue = Convert.ToString(((977 - iHeight) + 20) + (z * 33));
            bool isLast = false;
            if (numItems == (z + 1))
                isLast = true;

            if (z == 0)
            {
                rawXML.AppendLine("<control>");
                rawXML.AppendLine("  <description>Menu " + itemNo + " Submenu</description>");
                rawXML.AppendLine("  <type>button</type>");
                rawXML.AppendLine("  <id>770" + itemNo + "2</id>");
                rawXML.AppendLine("  <posX>" + xValue + "</posX>");
                rawXML.AppendLine("  <posY>966</posY>");
                rawXML.AppendLine("  <width>321</width>");
                rawXML.AppendLine("  <height>76</height>");
                rawXML.AppendLine("  <textureFocus>hiddenmenu_button_fo.png</textureFocus>");
                rawXML.AppendLine("  <textureNoFocus>-</textureNoFocus>");
                rawXML.AppendLine("  <textalign>center</textalign>");
                rawXML.AppendLine("  <label>" + formatXMLChar(item.name) + "</label>");
                rawXML.AppendLine("  <textcolor>00ffffff</textcolor>");
                rawXML.AppendLine("  <colordiffuse>ffffffff</colordiffuse>");
                rawXML.AppendLine("  <disabledcolor>00ffffff</disabledcolor>");
                rawXML.AppendLine("  <textcolorNoFocus>00ffffff</textcolorNoFocus>");
                rawXML.AppendLine("  <font>font10</font>");
                if (item.hyperlink.Equals("Shutdown"))
                {
                    rawXML.AppendLine("  <action>99</action>");
                }
                else if (item.hyperlink.Equals("Exit"))
                {
                    rawXML.AppendLine("  <action>97</action>");
                }
                else if (item.hyperlink.Equals("Restart"))
                {
                    rawXML.AppendLine("  <action>98</action>");
                }
                else if (item.hyperlink.Equals("602") || item.hyperlink.Equals("2005"))
                {
                    rawXML.AppendLine("  <action>18</action>");
                }
                else
                {
                    rawXML.AppendLine("  <hyperlink>" + item.hyperlink + "</hyperlink>");
                }
                rawXML.AppendLine("  <hover>-</hover>");
                if (itemNo == 1)
                    rawXML.AppendLine("  <onleft>770"+menuCount+"2</onleft>");
                else
                    rawXML.AppendLine("  <onleft>770" + (itemNo - 1) + "2</onleft>");
                if (itemNo == menuCount)
                    rawXML.AppendLine("  <onright>77012</onright>");
                else
                    rawXML.AppendLine("  <onright>770" + (itemNo + 1) + "2</onright>");
                rawXML.AppendLine("  <onup>770" + itemNo + "" + (iCount + 2) + "</onup>");
                rawXML.AppendLine("  <ondown>770" + itemNo + "" + (iCount + 2) + "</ondown>");
                rawXML.AppendLine("</control>");
                rawXML.AppendLine(" ");
            }

            rawXML.AppendLine("<control>");
            rawXML.AppendLine("  <description>Menu " + itemNo + " Submenu Button Selected - Nevigating from left</description>");
            rawXML.AppendLine("  <type>image</type>");
            rawXML.AppendLine("  <id>0</id>");
            rawXML.AppendLine("  <posX>" + (Int32.Parse(xValue) + 20) + "</posX>");
            rawXML.AppendLine("  <posY>" + (Int32.Parse(yValue)+9) + "</posY>");
            rawXML.AppendLine("  <width>291</width>");
            rawXML.AppendLine("  <height>34</height>");
            rawXML.AppendLine("  <shouldCache>true</shouldCache>");
            if (item.useLatestMedia.Equals("Moving Pictures") && latestMovingPictures != null && latestMovingPictures.Equals("True"))
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo_right.png</texture>");
            }
            else if (item.useLatestMedia.Equals("TVSeries") && latestTVSeries != null && latestTVSeries.Equals("True"))
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo_right.png</texture>");
            }
            else if (item.useLatestMedia.Equals("Music") && latestMusic != null && latestMusic.Equals("True"))
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo_right.png</texture>");
            }
            else if (item.useLatestMedia.Equals("TV Recordings") && latestTVRecordings != null && latestTVRecordings.Equals("True"))
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo_right.png</texture>");
            }
            else
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo.png</texture>");
            }
            rawXML.AppendLine("  <visible>control.isvisible(93761236)+Control.HasFocus(770" + itemNo + "" + (z + 3) + ")+Control.isvisible(93761264)+!Control.isvisible(93761234)</visible>");
            rawXML.AppendLine("  <animation effect=\"slide\" start=\"0,-34\" end=\"0,0\" time=\"300\" acceleration=\"-1.4\" reversible=\"false\">visible</animation>");
            rawXML.AppendLine("</control>");
            rawXML.AppendLine(" ");

            rawXML.AppendLine("<control>");
            rawXML.AppendLine("  <description>Menu " + itemNo + " Submenu Button Selected - Nevigating from right</description>");
            rawXML.AppendLine("  <type>image</type>");
            rawXML.AppendLine("  <id>0</id>");
            rawXML.AppendLine("  <posX>" + (Int32.Parse(xValue) + 20) + "</posX>");
            rawXML.AppendLine("  <posY>" + (Int32.Parse(yValue) + 9) + "</posY>");
            rawXML.AppendLine("  <width>291</width>");
            rawXML.AppendLine("  <height>34</height>");
            rawXML.AppendLine("  <shouldCache>true</shouldCache>");
            if (item.useLatestMedia.Equals("Moving Pictures") && latestMovingPictures != null && latestMovingPictures.Equals("True"))
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo_right.png</texture>");
            }
            else if (item.useLatestMedia.Equals("TVSeries") && latestTVSeries != null && latestTVSeries.Equals("True"))
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo_right.png</texture>");
            }
            else if (item.useLatestMedia.Equals("Music") && latestMusic != null && latestMusic.Equals("True"))
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo_right.png</texture>");
            }
            else if (item.useLatestMedia.Equals("TV Recordings") && latestTVRecordings != null && latestTVRecordings.Equals("True"))
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo_right.png</texture>");
            }
            else
            {
                rawXML.AppendLine("  <texture>hiddenmenu_button_fo.png</texture>");
            }
            rawXML.AppendLine("  <visible>control.isvisible(93761236)+Control.HasFocus(770" + itemNo + "" + (z + 3) + ")+!Control.isvisible(93761264)+!Control.isvisible(93761234)</visible>");
            rawXML.AppendLine("  <animation effect=\"slide\" start=\"0,34\" end=\"0,0\" time=\"300\" acceleration=\"-1.4\" reversible=\"false\">visible</animation>");
            rawXML.AppendLine("</control>");
            rawXML.AppendLine(" ");

            rawXML.AppendLine("<control>");
            rawXML.AppendLine("  <description>Menu " + itemNo + " Submenu Button " + z + "</description>");
            rawXML.AppendLine("  <type>button</type>");
            rawXML.AppendLine("  <id>770" + itemNo + "" + (z + 3) + "</id>");
            rawXML.AppendLine("  <posX>" + (Int32.Parse(xValue) + 20) + "</posX>");
            rawXML.AppendLine("  <posY>" + (Int32.Parse(yValue) + 9) + "</posY>");
            rawXML.AppendLine("  <width>291</width>");
            rawXML.AppendLine("  <height>34</height>");
            rawXML.AppendLine("  <label>" + formatXMLChar(item.name) + "</label>");
            rawXML.AppendLine("  <textcolor>FFFFFFFF</textcolor>");
            rawXML.AppendLine("  <font>font12</font>");
            rawXML.AppendLine("  <disabledcolor>FF7d7d7d</disabledcolor>				");
            rawXML.AppendLine("  <textalign>center</textalign>");
            if (item.hyperlink.Equals("Shutdown"))
            {
                rawXML.AppendLine("  <action>99</action>");
            }
            else if (item.hyperlink.Equals("Exit"))
            {
                rawXML.AppendLine("  <action>97</action>");
            }
            else if (item.hyperlink.Equals("Restart"))
            {
                rawXML.AppendLine("  <action>98</action>");
            }
            else if (item.hyperlink.Equals("602") || item.hyperlink.Equals("2005"))
            {
                rawXML.AppendLine("  <action>18</action>");
            }
            else
            {
                rawXML.AppendLine("  <hyperlink>" + item.hyperlink + "</hyperlink>");
            }
            if (item.hyperlink.Equals("96742"))
            {
                string key = "";
                if (item.subCategory != null && item.subCategory.Length > 0)
                {
                    key = item.subCategory;
                }
                else if (item.mainCategory != null && item.mainCategory.Length > 0)
                {
                    key = item.mainCategory;
                }
                if (key != null && key.Length > 0)
                {
                    rawXML.AppendLine("  <hyperlinkParameter>categoryname:"+key+"</hyperlinkParameter>");
                }
            }
            if (item.hyperlink.Equals("4755"))
            {
                string key = "";
                if (item.mainCategory != null && item.mainCategory.Length > 0)
                {
                    key = "site:" + item.mainCategory;
                }
                if (item.subCategory != null && item.subCategory.Length > 0)
                {
                    key = key + "|category:" + item.subCategory;
                }

                if (key != null && key.Length > 0)
                {
                    key = key + "|return:Locked";
                    rawXML.AppendLine("  <hyperlinkParameter>"+key+"</hyperlinkParameter>");
                }
            }
            if (item.hyperlink.Equals("9811"))
            {
                if (item.mainCategory != null && item.mainCategory.Length > 0)
                {
                    rawXML.AppendLine("  <hyperlinkParameter>" + item.mainCategory + "</hyperlinkParameter>");
                }
            }
            if (item.hyperlink.Equals("504")||item.hyperlink.Equals("504"))
            {
                if (item.mainCategory != null && item.mainCategory.Length > 0)
                {
                    rawXML.AppendLine("  <hyperlinkParameter>" + GetParameterValue(item.mainCategory) + "</hyperlinkParameter>");
                }
            }

            rawXML.AppendLine("  <hover>-</hover>");
            rawXML.AppendLine("  <textYOff>0</textYOff>");
            rawXML.AppendLine("  <textXOff>0</textXOff>");

            if (itemNo == 1)
                rawXML.AppendLine("  <onleft>770" + menuCount + "2</onleft>");
            else
                rawXML.AppendLine("  <onleft>770" + (itemNo - 1) + "2</onleft>");
            
            if (item.useLatestMedia.Equals("Moving Pictures") && latestMovingPictures != null && latestMovingPictures.Equals("True"))
            {
                //rawXML.AppendLine("  <textureFocus>hiddenmenu_button_fo_right.png</textureFocus>");
                rawXML.AppendLine("  <onright>91919991</onright>");
            }
            else if (item.useLatestMedia.Equals("TVSeries") && latestTVSeries != null && latestTVSeries.Equals("True"))
            {
                //rawXML.AppendLine("  <textureFocus>hiddenmenu_button_fo_right.png</textureFocus>");
                rawXML.AppendLine("  <onright>91919994</onright>");
            }
            else if (item.useLatestMedia.Equals("Music") && latestMusic != null && latestMusic.Equals("True"))
            {
                //rawXML.AppendLine("  <textureFocus>hiddenmenu_button_fo_right.png</textureFocus>");
                rawXML.AppendLine("  <onright>91919997</onright>");
            }
            else if (item.useLatestMedia.Equals("TV Recordings") && latestTVRecordings != null && latestTVRecordings.Equals("True"))
            {
                //rawXML.AppendLine("  <textureFocus>hiddenmenu_button_fo_right.png</textureFocus>");
                rawXML.AppendLine("  <onright>91919984</onright>");
            }
            else
            {
                /*if (itemNo == 6)
                    rawXML.AppendLine("  <onright>77012</onright>");
                else
                    rawXML.AppendLine("  <onright>770" + (itemNo + 1) + "2</onright>");
                 */
                if (itemNo == menuCount)
                    rawXML.AppendLine("  <onright>77012</onright>");
                else
                    rawXML.AppendLine("  <onright>770" + (itemNo + 1) + "2</onright>");
            }
            rawXML.AppendLine("  <textureFocus>-</textureFocus>");
            string up = "";
            string down = "";
            if (z == 0)
            {
                up = "991899";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "4";
            }
            else if (z == 1)
            {
                up = "770" + itemNo + "3";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "5";
            }
            else if (z == 2)
            {
                up = "770" + itemNo + "4";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "6";
            }
            else if (z == 3)
            {
                up = "770" + itemNo + "5";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "7";
            }
            else if (z == 4)
            {
                up = "770" + itemNo + "6";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "8";
            }
            else if (z == 5)
            {
                up = "770" + itemNo + "7";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "9";
            }
            else if (z == 6)
            {
                up = "770" + itemNo + "8";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "10";
            }
            else if (z == 7)
            {
                up = "770" + itemNo + "9";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "11";
            }
            else if (z == 8)
            {
                up = "770" + itemNo + "10";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "12";
            }
            else if (z == 9)
            {
                up = "770" + itemNo + "11";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "13";
            }
            else if (z == 10)
            {
                up = "770" + itemNo + "12";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "14";
            }
            else if (z == 11)
            {
                up = "770" + itemNo + "13";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "15";
            }
            else if (z == 12)
            {
                up = "770" + itemNo + "14";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "16";
            }
            else if (z == 13)
            {
                up = "770" + itemNo + "15";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "17";
            }
            else if (z == 14)
            {
                up = "770" + itemNo + "16";
                if (isLast)
                    down = "91111";
                else
                    down = "770" + itemNo + "18";
            }

            rawXML.AppendLine("  <onup>" + up + "</onup>");
            rawXML.AppendLine("  <ondown>" + down + "</ondown>");
            rawXML.AppendLine("  <animation effect=\"fade\" start=\"0\" end =\"100\" time=\"100\" delay=\"250\" reversible=\"true\">visible</animation>");
            rawXML.AppendLine("  <animation effect=\"fade\" start=\"100\" end =\"0\" time=\"30\" reversible=\"true\">hidden</animation>");
            //rawXML.AppendLine(visibleTag);
            rawXML.AppendLine(visibleTag.Replace("<visible allowhiddenfocus=\"true\">", "<visible allowhiddenfocus=\"true\">control.isvisible(93761236)+"));
            rawXML.AppendLine("</control>");
            rawXML.AppendLine(" ");

        }

        private string formatXMLChar(string s)
        {
            s = s.Replace("&", "&amp;");
            s = s.Replace("&amp;amp;", "&amp;");
            s = s.Replace("<", "&lt;");
            s = s.Replace(">", "&gt;");
            s = s.Replace("\"", "&quot;");
            s = s.Replace("'", "&#39;");    
            return s;
        }

        private int GetXStartValue()
        {
            int x = 0;
            if (doCenterMenu.Equals("True"))
            {
                switch (menuCount)
                {
                    case 1:
                        x = 800;
                        break;
                    case 2:
                        x = 642;
                        break;
                    case 3:
                        x = 482;
                        break;
                    case 4:
                        x = 321;
                        break;
                    case 5:
                        x = 160;
                        break;
                    case 6:
                        x = 0;
                        break;
                    default:
                        x = 0;
                        break;
                }
            }
            else
            {
                x = 0;
            }
            return x;
        }

        private void writeMenuPart1_Vertical(StreamWriter sw, string text, string prefix, int i)
        {
            int x = 0;
            string xValue = "";
            if (i == 1)
            {
                x = 0;
                xValue = GetXStartValue().ToString();
            }
            else if (i == 2)
            {
                x = 3;
                xValue = (GetXStartValue()+321).ToString();
            }
            else if (i == 3)
            {
                x = 6;
                xValue = (GetXStartValue() + 642).ToString();
            }
            else if (i == 4)
            {
                x = 9;
                xValue = (GetXStartValue() + 963).ToString();
            }
            else if (i == 5)
            {
                x = 12;
                xValue = (GetXStartValue() + 1284).ToString();
            }
            else if (i == 6)
            {
                x = 15;
                xValue = (GetXStartValue() + 1605).ToString();
            }
         
            rawXML.AppendLine("<control>");
            rawXML.AppendLine("  <description>Menu " + i + " Label</description>");
            rawXML.AppendLine("  <id>600" + (x + 2) + "</id>");
            rawXML.AppendLine("  <type>label</type>");
            rawXML.AppendLine("  <posX>" + (Int32.Parse(xValue) + 2) + "</posX>");
            rawXML.AppendLine("  <posY>974</posY>");
            rawXML.AppendLine("  <width>322</width>");
            rawXML.AppendLine("  <height>75</height>");
            rawXML.AppendLine("  <align>center</align>");
            rawXML.AppendLine("  <font>font16BC</font>");
            rawXML.AppendLine("  <textcolor>ffffffff</textcolor>");
            rawXML.AppendLine("  <shadowAngle>45</shadowAngle>");
            rawXML.AppendLine("  <shadowDistance>1</shadowDistance>");
            rawXML.AppendLine("  <shadowColor>ff333333</shadowColor>");
            rawXML.AppendLine("  <label>" + formatXMLChar(text) + "</label>");
            rawXML.AppendLine("  <visible>!Control.HasFocus(7888)+!Control.HasFocus(7999)+!Control.HasFocus(7777)+!Control.HasFocus(73071091)+!Control.isvisible(93761234)</visible>");
            rawXML.AppendLine("</control>");
            rawXML.AppendLine(" ");
        }

          

        private string getBgVisibilityTag_Vertical(string module)
        {
            string s = "";
            int iCount = 0;
            int iMenu = 1;
            if (useMenu1.Equals("True"))
            {
                foreach (menuItem item in menuItems1)
                {
                    if (module.Equals(item.imagePath))
                    {
                        if (s.Length == 0)
                            s = s + "Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";
                        else
                            s = s + "|Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";

                        if (item.useLatestMedia.Equals("Moving Pictures"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                            else
                                s = s + "|Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                        }
                        else if (item.useLatestMedia.Equals("TVSeries"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                            else
                                s = s + "|Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                        }
                        else if (item.useLatestMedia.Equals("Music"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                            else
                                s = s + "|Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                        }
                        else if (item.useLatestMedia.Equals("TV Recordings"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                            else
                                s = s + "|Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu2.Equals("True"))
            {
                foreach (menuItem item in menuItems2)
                {
                    if (module.Equals(item.imagePath))
                    {
                        if (s.Length == 0)
                            s = s + "Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";
                        else
                            s = s + "|Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";

                        if (item.useLatestMedia.Equals("Moving Pictures"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                            else
                                s = s + "|Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                        }
                        else if (item.useLatestMedia.Equals("TVSeries"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                            else
                                s = s + "|Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                        }
                        else if (item.useLatestMedia.Equals("Music"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                            else
                                s = s + "|Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                        }
                        else if (item.useLatestMedia.Equals("TV Recordings"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                            else
                                s = s + "|Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu3.Equals("True"))
            {
                foreach (menuItem item in menuItems3)
                {
                    if (module.Equals(item.imagePath))
                    {
                        if (s.Length == 0)
                            s = s + "Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";
                        else
                            s = s + "|Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";

                        if (item.useLatestMedia.Equals("Moving Pictures"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                            else
                                s = s + "|Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                        }
                        else if (item.useLatestMedia.Equals("TVSeries"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                            else
                                s = s + "|Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                        }
                        else if (item.useLatestMedia.Equals("Music"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                            else
                                s = s + "|Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                        }
                        else if (item.useLatestMedia.Equals("TV Recordings"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                            else
                                s = s + "|Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu4.Equals("True"))
            {
                foreach (menuItem item in menuItems4)
                {
                    if (module.Equals(item.imagePath))
                    {
                        if (s.Length == 0)
                            s = s + "Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";
                        else
                            s = s + "|Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";

                        if (item.useLatestMedia.Equals("Moving Pictures"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                            else
                                s = s + "|Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                        }
                        else if (item.useLatestMedia.Equals("TVSeries"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                            else
                                s = s + "|Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                        }
                        else if (item.useLatestMedia.Equals("Music"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                            else
                                s = s + "|Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                        }
                        else if (item.useLatestMedia.Equals("TV Recordings"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                            else
                                s = s + "|Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu5.Equals("True"))
            {
                foreach (menuItem item in menuItems5)
                {
                    if (module.Equals(item.imagePath))
                    {
                        if (s.Length == 0)
                            s = s + "Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";
                        else
                            s = s + "|Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";

                        if (item.useLatestMedia.Equals("Moving Pictures"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                            else
                                s = s + "|Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                        }
                        else if (item.useLatestMedia.Equals("TVSeries"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                            else
                                s = s + "|Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                        }
                        else if (item.useLatestMedia.Equals("Music"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                            else
                                s = s + "|Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                        }
                        else if (item.useLatestMedia.Equals("TV Recordings"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                            else
                                s = s + "|Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu6.Equals("True"))
            {
                foreach (menuItem item in menuItems6)
                {
                    if (module.Equals(item.imagePath))
                    {
                        if (s.Length == 0)
                            s = s + "Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";
                        else
                            s = s + "|Control.HasFocus(770" + iMenu + "" + (iCount + 3) + ")";

                        if (item.useLatestMedia.Equals("Moving Pictures"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                            else
                                s = s + "|Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)";
                        }
                        else if (item.useLatestMedia.Equals("TVSeries"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                            else
                                s = s + "|Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)";
                        }
                        else if (item.useLatestMedia.Equals("Music"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                            else
                                s = s + "|Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)";
                        }
                        else if (item.useLatestMedia.Equals("TV Recordings"))
                        {
                            if (s.Length == 0)
                                s = s + "Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                            else
                                s = s + "|Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iMenu = 1;
            if (useMenu1.Equals("True"))
            {
                if (module.Equals(textBox15.Text) || module.Equals(comboBox3.SelectedItem))
                {
                    if (s.Length > 0)
                        s = s + "|Control.HasFocus(770" + iMenu + "2)";
                    else
                        s = s + "Control.HasFocus(770" + iMenu + "2)";
                }
                iMenu++;
            }
            if (useMenu2.Equals("True"))
            {
                if (module.Equals(textBox16.Text) || module.Equals(comboBox4.SelectedItem))
                {
                    if (s.Length > 0)
                        s = s + "|Control.HasFocus(770" + iMenu + "2)";
                    else
                        s = s + "Control.HasFocus(770" + iMenu + "2)";
                }
                iMenu++;
            }
            if (useMenu3.Equals("True"))
            {
                if (module.Equals(textBox17.Text) || module.Equals(comboBox12.SelectedItem))
                {
                    if (s.Length > 0)
                        s = s + "|Control.HasFocus(770" + iMenu + "2)";
                    else
                        s = s + "Control.HasFocus(770" + iMenu + "2)";
                }
                iMenu++;
            }
            if (useMenu4.Equals("True"))
            {
                if (module.Equals(textBox18.Text) || module.Equals(comboBox13.SelectedItem))
                {
                    if (s.Length > 0)
                        s = s + "|Control.HasFocus(770" + iMenu + "2)";
                    else
                        s = s + "Control.HasFocus(770" + iMenu + "2)";
                }
                iMenu++;
            }
            if (useMenu5.Equals("True"))
            {
                if (module.Equals(textBox19.Text) || module.Equals(comboBox14.SelectedItem))
                {
                    if (s.Length > 0)
                        s = s + "|Control.HasFocus(770" + iMenu + "2)";
                    else
                        s = s + "Control.HasFocus(770" + iMenu + "2)";
                }
                iMenu++;
            }
            if (useMenu6.Equals("True"))
            {
                if (module.Equals(textBox3.Text) || module.Equals(comboBox15.SelectedItem))
                {
                    if (s.Length > 0)
                        s = s + "|Control.HasFocus(770" + iMenu + "2)";
                    else
                        s = s + "Control.HasFocus(770" + iMenu + "2)";
                }
                iMenu++;
            }
            if (module.IndexOf("weatherbg") > 0)
            {
                if (s.Length > 0)
                    s = s + "|Control.HasFocus(73071091)";
                else
                    s = s + "Control.HasFocus(73071091)";
            }            
            string defMenu = "";
            if (comboBox2.SelectedItem.Equals("Menu 1"))
            {
                if (comboBox11.SelectedItem != null && comboBox11.SelectedItem.Equals("Fanart Plugin"))
                    defMenu = comboBox3.SelectedItem.ToString();
                else
                    defMenu = textBox15.Text;
            }
            else if (comboBox2.SelectedItem.Equals("Menu 2"))
            {
                if (comboBox10.SelectedItem != null && comboBox10.SelectedItem.Equals("Fanart Plugin"))
                    defMenu = comboBox4.SelectedItem.ToString();
                else
                    defMenu = textBox16.Text;
            }
            else if (comboBox2.SelectedItem.Equals("Menu 3"))
            {
                if (comboBox9.SelectedItem != null && comboBox9.SelectedItem.Equals("Fanart Plugin"))
                    defMenu = comboBox12.SelectedItem.ToString();
                else
                    defMenu = textBox17.Text;
            }
            else if (comboBox2.SelectedItem.Equals("Menu 4"))
            {
                if (comboBox8.SelectedItem != null && comboBox8.SelectedItem.Equals("Fanart Plugin"))
                    defMenu = comboBox13.SelectedItem.ToString();
                else
                    defMenu = textBox18.Text;
            }
            else if (comboBox2.SelectedItem.Equals("Menu 5"))
            {
                if (comboBox7.SelectedItem != null && comboBox7.SelectedItem.Equals("Fanart Plugin"))
                    defMenu = comboBox14.SelectedItem.ToString();
                else
                    defMenu = textBox19.Text;
            }
            else if (comboBox2.SelectedItem.Equals("Menu 6"))
            {
                if (comboBox6.SelectedItem != null && comboBox6.SelectedItem.Equals("Fanart Plugin"))
                    defMenu = comboBox15.SelectedItem.ToString();
                else
                    defMenu = textBox3.Text;
            }
            //if (module.Equals("movies"))
            if (module.Equals(defMenu))
            {
                if (s.Length > 0)
                    s = s + "|Control.HasFocus(17)";
                else
                    s = s + "Control.HasFocus(17)";
            }
            //if (module.Equals("movies"))
            /*if (module.Equals(defMenu))
            {
                if (s.Length > 0)
                    s = s + "|Control.HasFocus(91111)";
                else
                    s = s + "Control.HasFocus(91111)";
            }*/
            if (module.Equals(defMenu))
            {
                if (s.Length > 0)
                    s = s + "|Control.HasFocus(98129812)";
                else
                    s = s + "Control.HasFocus(98129812)";
            }
            
            if (s.Length == 0)
                s = "no";
            return s;
        }

        private string getFanartTag(string key, bool firstValue)
        {
            string sout = "";
            if (key.Equals("Random Games") && firstValue)
                sout = "#fanarthandler.games.userdef.backdrop1.any";
            else if (key.Equals("Random Games") && !firstValue)
                sout = "#fanarthandler.games.userdef.backdrop2.any";
            else if (key.Equals("Random Movies") && firstValue)
                sout = "#fanarthandler.movie.userdef.backdrop1.any";
            else if (key.Equals("Random Movies") && !firstValue)
                sout = "#fanarthandler.movie.userdef.backdrop2.any";
            else if (key.Equals("Random Movies Scraper") && firstValue)
                sout = "#fanarthandler.movie.scraper.backdrop1.any";
            else if (key.Equals("Random Movies Scraper") && !firstValue)
                sout = "#fanarthandler.movie.scraper.backdrop2.any";
            else if (key.Equals("Random MovingPictures") && firstValue)
                sout = "#fanarthandler.movingpicture.backdrop1.any";
            else if (key.Equals("Random MovingPictures") && !firstValue)
                sout = "#fanarthandler.movingpicture.backdrop2.any";
            else if (key.Equals("Random Music") && firstValue)
                sout = "#fanarthandler.music.userdef.backdrop1.any";
            else if (key.Equals("Random Music") && !firstValue)
                sout = "#fanarthandler.music.userdef.backdrop2.any";
            else if (key.Equals("Random Music Scraper") && firstValue)
                sout = "#fanarthandler.music.scraper.backdrop1.any";
            else if (key.Equals("Random Music Scraper") && !firstValue)
                sout = "#fanarthandler.music.scraper.backdrop2.any";
            else if (key.Equals("Random Pictures") && firstValue)
                sout = "#fanarthandler.picture.userdef.backdrop1.any";
            else if (key.Equals("Random Pictures") && !firstValue)
                sout = "#fanarthandler.picture.userdef.backdrop2.any";
            else if (key.Equals("Random Plugins") && firstValue)
                sout = "#fanarthandler.plugins.userdef.backdrop1.any";
            else if (key.Equals("Random Plugins") && !firstValue)
                sout = "#fanarthandler.plugins.userdef.backdrop2.any";
            else if (key.Equals("Random TV") && firstValue)
                sout = "#fanarthandler.tv.userdef.backdrop1.any";
            else if (key.Equals("Random TV") && !firstValue)
                sout = "#fanarthandler.tv.userdef.backdrop2.any";
            else if (key.Equals("Random TVSeries") && firstValue)
                sout = "#fanarthandler.tvseries.backdrop1.any";
            else if (key.Equals("Random TVSeries") && !firstValue)
                sout = "#fanarthandler.tvseries.backdrop2.any"; 
            return sout;
        }

   

        

        private void makeMultiImagePart_Vertical(StreamWriter sw)
        {
            Hashtable modules = new Hashtable();

            try
            {
                if (comboBox11.SelectedItem != null && comboBox11.SelectedItem.Equals("Fanart Plugin"))
                    modules.Add(comboBox3.SelectedItem, comboBox3.SelectedItem);
                else
                    modules.Add(textBox15.Text, textBox15.Text);
            }
            catch
            {
                // do nothing
            }
            try
            {
                if (comboBox10.SelectedItem != null && comboBox10.SelectedItem.Equals("Fanart Plugin"))
                    modules.Add(comboBox4.SelectedItem, comboBox4.SelectedItem);
                else
                    modules.Add(textBox16.Text, textBox16.Text);
            }
            catch
            {
                // do nothing
            }
            try
            {
                if (comboBox9.SelectedItem != null && comboBox9.SelectedItem.Equals("Fanart Plugin"))
                    modules.Add(comboBox12.SelectedItem, comboBox12.SelectedItem);
                else
                    modules.Add(textBox17.Text, textBox17.Text);
            }
            catch
            {
                // do nothing
            }
            try
            {
                if (comboBox8.SelectedItem != null && comboBox8.SelectedItem.Equals("Fanart Plugin"))
                    modules.Add(comboBox13.SelectedItem, comboBox13.SelectedItem);
                else
                    modules.Add(textBox18.Text, textBox18.Text);
            }
            catch
            {
                // do nothing
            }
            try
            {
                if (comboBox7.SelectedItem != null && comboBox7.SelectedItem.Equals("Fanart Plugin"))
                    modules.Add(comboBox14.SelectedItem, comboBox14.SelectedItem);
                else
                    modules.Add(textBox19.Text, textBox19.Text);
            }
            catch
            {
                // do nothing
            }
            try
            {
                if (comboBox6.SelectedItem != null && comboBox6.SelectedItem.Equals("Fanart Plugin"))
                    modules.Add(comboBox15.SelectedItem, comboBox15.SelectedItem);
                else
                    modules.Add(textBox3.Text, textBox3.Text);
            }
            catch
            {
                // do nothing
            }

            foreach (menuItem item in menuItems1)
            {
                try
                {
                    modules.Add(item.imagePath, item.imagePath);
                }
                catch
                {
                    // do nothing
                }
            }
            foreach (menuItem item in menuItems2)
            {
                try
                {
                    modules.Add(item.imagePath, item.imagePath);
                }
                catch
                {
                    // do nothing
                }
            }
            foreach (menuItem item in menuItems3)
            {
                try
                {
                    modules.Add(item.imagePath, item.imagePath);
                }
                catch
                {
                    // do nothing
                }
            }
            foreach (menuItem item in menuItems4)
            {
                try
                {
                    modules.Add(item.imagePath, item.imagePath);
                }
                catch
                {
                    // do nothing
                }
            }
            foreach (menuItem item in menuItems5)
            {
                try
                {
                    modules.Add(item.imagePath, item.imagePath);
                }
                catch
                {
                    // do nothing
                }
            }
            foreach (menuItem item in menuItems6)
            {
                try
                {
                    modules.Add(item.imagePath, item.imagePath);
                }
                catch
                {
                    // do nothing
                }
            }
            ICollection valueColl = modules.Values;
            int icount = 0;
            foreach (string s in valueColl)
            {
                if (s.IndexOf("weatherbg") >= 0)
                {
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>BACKGROUND " + icount + "</description>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>1080</height>");
                    rawXML.AppendLine("  <shouldCache>true</shouldCache>");
                    rawXML.AppendLine("  <texture>Animations//weatherbg//#infoservice.weather.today.img.small.filenamewithoutext//weather.jpg</texture>");
                    rawXML.AppendLine("  <visible>" + getBgVisibilityTag_Vertical(s) + "</visible>");
                    rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                }
                else if (s.ToLower().IndexOf("music") >= 0)
                {
                    if (getFilenameOnly(s).IndexOf(".") > 0)
                    {
                        rawXML.AppendLine("<control>");
                        rawXML.AppendLine("  <description>BACKGROUND " + icount + "</description>");
                        rawXML.AppendLine("  <id>0</id>");
                        rawXML.AppendLine("  <type>image</type>");
                        rawXML.AppendLine("  <posX>0</posX>");
                        rawXML.AppendLine("  <posY>0</posY>");
                        rawXML.AppendLine("  <width>1920</width>");
                        rawXML.AppendLine("  <height>1080</height>");
                        rawXML.AppendLine("  <shouldCache>true</shouldCache>");
                        rawXML.AppendLine("  <texture>" + s + "</texture>");
                        rawXML.AppendLine("  <visible>" + getBgVisibilityTag_Vertical(s) + "</visible>");
                        rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                        rawXML.AppendLine("</control>");
                        rawXML.AppendLine(" ");
                    }
                    else if (s.StartsWith("Random"))
                    {
                        rawXML.AppendLine("<control>");
                        rawXML.AppendLine("  <description>BACKGROUND " + icount + "_1</description>");
                        rawXML.AppendLine("  <id>0</id>");
                        rawXML.AppendLine("  <type>image</type>");
                        rawXML.AppendLine("  <posX>0</posX>");
                        rawXML.AppendLine("  <posY>0</posY>");
                        rawXML.AppendLine("  <width>1920</width>");
                        rawXML.AppendLine("  <height>1080</height>");
                        rawXML.AppendLine("  <texture>" + getFanartTag(s, true) + "</texture>");
                        if (s.Equals("Random Music") || s.Equals("Random Music Scraper"))
                        {
                            rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+control.isvisible(91919297)+!control.isvisible(91919294)</visible>");
                        }
                        else
                        {
                            rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+control.isvisible(91919297)</visible>");
                        }
                        rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                        rawXML.AppendLine("</control>");
                        rawXML.AppendLine(" ");
                        rawXML.AppendLine("<control>");
                        rawXML.AppendLine("  <description>BACKGROUND " + icount + "_2</description>");
                        rawXML.AppendLine("  <id>0</id>");
                        rawXML.AppendLine("  <type>image</type>");
                        rawXML.AppendLine("  <posX>0</posX>");
                        rawXML.AppendLine("  <posY>0</posY>");
                        rawXML.AppendLine("  <width>1920</width>");
                        rawXML.AppendLine("  <height>1080</height>");
                        rawXML.AppendLine("  <texture>" + getFanartTag(s, false) + "</texture>");
                        if (s.Equals("Random Music") || s.Equals("Random Music Scraper"))
                        {
                            rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+control.isvisible(91919298)+!control.isvisible(91919294)</visible>");
                        }
                        else
                        {
                            rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+control.isvisible(91919298)</visible>");
                        }
                        rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                        rawXML.AppendLine("</control>");
                        rawXML.AppendLine(" ");
                    }
                    else
                    {
                        rawXML.AppendLine("<control>");
                        rawXML.AppendLine("  <description>BACKGROUND " + icount + "</description>");
                        rawXML.AppendLine("  <id>1200</id>");
                        rawXML.AppendLine("  <type>multiimage</type>");
                        rawXML.AppendLine("  <posX>0</posX>");
                        rawXML.AppendLine("  <posY>0</posY>");
                        rawXML.AppendLine("  <width>1920</width>");
                        rawXML.AppendLine("  <height>1080</height>");
                        rawXML.AppendLine("  <imagepath>" + s + "</imagepath>");
                        rawXML.AppendLine("  <timeperimage>60000</timeperimage>");
                        rawXML.AppendLine("  <fadetime>1000</fadetime>");
                        rawXML.AppendLine("  <loop>yes</loop>");
                        rawXML.AppendLine("  <randomize>True</randomize>");
                        rawXML.AppendLine("  <visible>" + getBgVisibilityTag_Vertical(s) + "</visible>");
                        rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                        rawXML.AppendLine("</control>");
                        rawXML.AppendLine(" ");
                    }
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>FANART BACKGROUND " + icount + "_1</description>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>1080</height>");
                    rawXML.AppendLine("  <texture>#fanarthandler.music.backdrop1.play</texture>");
                    rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+player.hasmedia+control.isvisible(91919295)</visible>");
                    rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>FANART BACKGROUND " + icount + "_2</description>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>1080</height>");
                    rawXML.AppendLine("  <texture>#fanarthandler.music.backdrop2.play</texture>");
                    rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+player.hasmedia+control.isvisible(91919296)</visible>");
                    rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                }
                else if (getFilenameOnly(s).IndexOf(".") > 0)
                {
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>BACKGROUND " + icount + "</description>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>1080</height>");
                    rawXML.AppendLine("  <shouldCache>true</shouldCache>");
                    rawXML.AppendLine("  <texture>" + s + "</texture>");
                    rawXML.AppendLine("  <visible>" + getBgVisibilityTag_Vertical(s) + "</visible>");
                    rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                }
                else if (s.StartsWith("Random"))
                {
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>BACKGROUND " + icount + "_1</description>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>1080</height>");
                    rawXML.AppendLine("  <texture>" + getFanartTag(s, true) + "</texture>");
                    if (s.Equals("Random Music") || s.Equals("Random Music Scraper"))
                    {
                        rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+control.isvisible(91919297)+!control.isvisible(91919294)</visible>");
                    }
                    else
                    {
                        rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+control.isvisible(91919297)</visible>");
                    }
                    rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>BACKGROUND " + icount + "_2</description>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>1080</height>");
                    rawXML.AppendLine("  <texture>" + getFanartTag(s, false) + "</texture>");
                    if (s.Equals("Random Music") || s.Equals("Random Music Scraper"))
                    {
                        rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+control.isvisible(91919298)+!control.isvisible(91919294)</visible>");
                    }
                    else
                    {
                        rawXML.AppendLine("  <visible>[" + getBgVisibilityTag_Vertical(s) + "]+control.isvisible(91919298)</visible>");
                    }
                    rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                }
                else
                {
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>BACKGROUND " + icount + "</description>");
                    rawXML.AppendLine("  <id>1200</id>");
                    rawXML.AppendLine("  <type>multiimage</type>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>1080</height>");
                    rawXML.AppendLine("  <imagepath>" + s + "</imagepath>");
                    rawXML.AppendLine("  <timeperimage>60000</timeperimage>");
                    rawXML.AppendLine("  <fadetime>1000</fadetime>");
                    rawXML.AppendLine("  <loop>yes</loop>");
                    rawXML.AppendLine("  <randomize>True</randomize>");
                    rawXML.AppendLine("  <visible>" + getBgVisibilityTag_Vertical(s) + "</visible>");
                    rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                }
                icount++;
            }
        }

       
       


    

        private bool checkFields()
        {
            bool b = true;

            if (comboBox2.SelectedItem.Equals("Menu 1"))
            {
                if (!useMenu1.Equals("True"))
                {
                    MessageBox.Show("You must enable main menu 1 if you have this as default menu!");
                    b = false;
                }
            }             
            else if (comboBox2.SelectedItem.Equals("Menu 2"))
            {
                if (!useMenu2.Equals("True"))
                {
                    MessageBox.Show("You must enable main menu 2 if you have this as default menu!");
                    b = false;
                }
            }
            else if (comboBox2.SelectedItem.Equals("Menu 3"))
            {
                if (!useMenu3.Equals("True"))
                {
                    MessageBox.Show("You must enable main menu 3 if you have this as default menu!");
                    b = false;
                }
            }
            else if (comboBox2.SelectedItem.Equals("Menu 4"))
            {
                if (!useMenu4.Equals("True"))
                {
                    MessageBox.Show("You must enable main menu 4 if you have this as default menu!");
                    b = false;
                }
            }
            else if (comboBox2.SelectedItem.Equals("Menu 5"))
            {
                if (!useMenu5.Equals("True"))
                {
                    MessageBox.Show("You must enable main menu 5 if you have this as default menu!");
                    b = false;
                }
            }
            else if (comboBox2.SelectedItem.Equals("Menu 6"))
            {
                if (!useMenu6.Equals("True"))
                {
                    MessageBox.Show("You must enable main menu 6 if you have this as default menu!");
                    b = false;
                }
            }
            
            if (useMenu1.Equals("True"))
            {
                if (textBox1.Text.Length < 1 || (textBox15.Text.Length < 1 && comboBox3.SelectedItem.ToString().Length < 1))
                {
                    MessageBox.Show("Text on Menu 1 to 6 (Prefix, Text, Image Name and Image Text) must all have values!");
                    b = false;
                }
                if (used_list.Items.Count < 1)
                {
                    MessageBox.Show("Modules in Menu 1 must all have atleast one subitem!");
                    b = false;
                }
            }
            if (useMenu2.Equals("True"))
            {
                if (textBox2.Text.Length < 1 || (textBox16.Text.Length < 1 && comboBox4.SelectedItem.ToString().Length < 1))
                {
                    MessageBox.Show("Text on Menu 1 to 6 (Prefix, Text, Image Name and Image Text) must all have values!");
                    b = false;
                }
                if (listBox1.Items.Count < 1)
                {
                    MessageBox.Show("Modules in Menu 2 must all have atleast one subitem!");
                    b = false;
                }
            }
            if (useMenu3.Equals("True"))
            {
                if (textBox4.Text.Length < 1 || (textBox17.Text.Length < 1 && comboBox12.SelectedItem.ToString().Length < 1))
                {
                    MessageBox.Show("Text on Menu 1 to 6 (Prefix, Text, Image Name and Image Text) must all have values!");
                    b = false;
                }
                if (listBox2.Items.Count < 1)
                {
                    MessageBox.Show("Modules in Menu 3 must all have atleast one subitem!");
                    b = false;
                }
            }
            if (useMenu4.Equals("True"))
            {
                if (textBox6.Text.Length < 1 || (textBox18.Text.Length < 1 && comboBox13.SelectedItem.ToString().Length < 1))
                {
                    MessageBox.Show("Text on Menu 1 to 6 (Prefix, Text, Image Name and Image Text) must all have values!");
                    b = false;
                }
                if (listBox3.Items.Count < 1)
                {
                    MessageBox.Show("Modules in Menu 4 must all have atleast one subitem!");
                    b = false;
                }
            }
            if (useMenu5.Equals("True"))
            {
                if (textBox8.Text.Length < 1 || (textBox19.Text.Length < 1 && comboBox14.SelectedItem.ToString().Length < 1))
                {
                    MessageBox.Show("Text on Menu 1 to 6 (Prefix, Text, Image Name and Image Text) must all have values!");
                    b = false;
                }
                if (listBox4.Items.Count < 1)
                {
                    MessageBox.Show("Modules in Menu 5 must all have atleast one subitem!");
                    b = false;
                }
            }
            if (useMenu6.Equals("True"))
            {
                if (textBox7.Text.Length < 1 || (textBox3.Text.Length < 1 && comboBox15.SelectedItem.ToString().Length < 1))
                {
                    MessageBox.Show("Text on Menu 1 to 6 (Prefix, Text, Image Name and Image Text) must all have values!");
                    b = false;
                }
                if (listBox5.Items.Count < 1)
                {
                    MessageBox.Show("Modules in Menu 6 must all have atleast one subitem!");
                    b = false;
                } 
            }
                    
            return b;
        }

        private string setDefaultMenu(string layout)
        {
            string s = "";
            
            if (comboBox2.SelectedItem.Equals("Menu 1"))
                s = "77012";
            else if (comboBox2.SelectedItem.Equals("Menu 2"))
                s = "77022";
            else if (comboBox2.SelectedItem.Equals("Menu 3"))
                s = "77032";
            else if (comboBox2.SelectedItem.Equals("Menu 4"))
                s = "77042";
            else if (comboBox2.SelectedItem.Equals("Menu 5"))
                s = "77052";
            else if (comboBox2.SelectedItem.Equals("Menu 6"))
                s = "77062";
            
            return s;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            rawXMLDefineTags = new StringBuilder();
            getDefineTags();
            bgItems = new List<bgItem>();

            if (generateNewBasichome("BasicHome.xml"))
            {
                rawXML = new StringBuilder();
                string filename = path + @"\common.basichome.latest.xml";
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    writeLatest();
                    sw.Write(rawXML.ToString());
                }
                MessageBox.Show("Done generating new BasicHome. Please note that your changes in the menu editor is NOT saved. You will be asked to save your settings when leaving this configuration screen (by pressing the [X] button top right).");
            }
        }

        private void addToDefineTagsHash(Hashtable ht, string property, string value)
        {
            if (ht.Contains(property))
            {
                ht[property] = value;
            }
            else
            {
                ht.Add(property, value);
            }
        }

        private void handleDefineTags(menuItem item, Hashtable ht)
        {
            if (item.imagePath.Equals("Random Games"))
                addToDefineTagsHash(ht, "#useRandomGamesUserFanart:", "Yes");
            if (item.imagePath.Equals("Random Movies Scraper"))
                addToDefineTagsHash(ht, "#useRandomMoviesScraperFanart:", "Yes");
            if (item.imagePath.Equals("Random Movies"))
                addToDefineTagsHash(ht, "#useRandomMoviesUserFanart:", "Yes");
            if (item.imagePath.Equals("Random MovingPictures"))
                addToDefineTagsHash(ht, "#useRandomMovingPicturesFanart:", "Yes");
            if (item.imagePath.Equals("Random Music Scraper"))
                addToDefineTagsHash(ht, "#useRandomMusicScraperFanart:", "Yes");
            if (item.imagePath.Equals("Random Music"))
                addToDefineTagsHash(ht, "#useRandomMusicUserFanart:", "Yes");
            if (item.imagePath.Equals("Random Pictures"))
                addToDefineTagsHash(ht, "#useRandomPicturesUserFanart:", "Yes");
            if (item.imagePath.Equals("Random Plugins"))
                addToDefineTagsHash(ht, "#useRandomPluginsUserFanart:", "Yes");
            if (item.imagePath.Equals("Random TV"))
                addToDefineTagsHash(ht, "#useRandomTVUserFanart:", "Yes");
            if (item.imagePath.Equals("Random TVSeries"))
                addToDefineTagsHash(ht, "#useRandomTVSeriesFanart:", "Yes");
        }

        private void handleDefineTags(string item, Hashtable ht)
        {
            if (item.Equals("Random Games"))
                addToDefineTagsHash(ht, "#useRandomGamesUserFanart:", "Yes");
            if (item.Equals("Random Movies Scraper"))
                addToDefineTagsHash(ht, "#useRandomMoviesScraperFanart:", "Yes");
            if (item.Equals("Random Movies"))
                addToDefineTagsHash(ht, "#useRandomMoviesUserFanart:", "Yes");
            if (item.Equals("Random MovingPictures"))
                addToDefineTagsHash(ht, "#useRandomMovingPicturesFanart:", "Yes");
            if (item.Equals("Random Music Scraper"))
                addToDefineTagsHash(ht, "#useRandomMusicScraperFanart:", "Yes");
            if (item.Equals("Random Music"))
                addToDefineTagsHash(ht, "#useRandomMusicUserFanart:", "Yes");
            if (item.Equals("Random Pictures"))
                addToDefineTagsHash(ht, "#useRandomPicturesUserFanart:", "Yes");
            if (item.Equals("Random Plugins"))
                addToDefineTagsHash(ht, "#useRandomPluginsUserFanart:", "Yes");
            if (item.Equals("Random TV"))
                addToDefineTagsHash(ht, "#useRandomTVUserFanart:", "Yes");
            if (item.Equals("Random TVSeries"))
                addToDefineTagsHash(ht, "#useRandomTVSeriesFanart:", "Yes");
        }

        private void getDefineTags()
        {
            Hashtable ht = new Hashtable();
            addToDefineTagsHash(ht, "#useRandomGamesUserFanart:", "No");
            addToDefineTagsHash(ht, "#useRandomMoviesUserFanart:", "No");
            addToDefineTagsHash(ht, "#useRandomMovingPicturesFanart:", "No");
            addToDefineTagsHash(ht, "#useRandomMusicUserFanart:", "No");
            addToDefineTagsHash(ht, "#useRandomPicturesUserFanart:", "No");
            addToDefineTagsHash(ht, "#useRandomPluginsUserFanart:", "No");
            addToDefineTagsHash(ht, "#useRandomTVUserFanart:", "No");
            addToDefineTagsHash(ht, "#useRandomTVSeriesFanart:", "No");
            if (comboBox11.SelectedItem.Equals("Fanart Plugin") && comboBox3.SelectedItem != null)
            {
                handleDefineTags(comboBox3.SelectedItem.ToString(), ht);
            }
            if (comboBox10.SelectedItem.Equals("Fanart Plugin") && comboBox4.SelectedItem != null)
            {
                handleDefineTags(comboBox4.SelectedItem.ToString(), ht);
            }
            if (comboBox9.SelectedItem.Equals("Fanart Plugin") && comboBox12.SelectedItem != null)
            {
                handleDefineTags(comboBox12.SelectedItem.ToString(), ht);
            }
            if (comboBox8.SelectedItem.Equals("Fanart Plugin") && comboBox13.SelectedItem != null)
            {
                handleDefineTags(comboBox13.SelectedItem.ToString(), ht);
            }
            if (comboBox7.SelectedItem.Equals("Fanart Plugin") && comboBox14.SelectedItem != null)
            {
                handleDefineTags(comboBox14.SelectedItem.ToString(), ht);
            }
            if (comboBox6.SelectedItem.Equals("Fanart Plugin") && comboBox15.SelectedItem != null)
            {
                handleDefineTags(comboBox15.SelectedItem.ToString(), ht);
            }
            foreach (menuItem item in menuItems1)
            {
                handleDefineTags(item, ht);
                
            }
            foreach (menuItem item in menuItems2)
            {
                handleDefineTags(item, ht);
            }
            foreach (menuItem item in menuItems3)
            {
                handleDefineTags(item, ht);
            }
            foreach (menuItem item in menuItems4)
            {
                handleDefineTags(item, ht);
            }
            foreach (menuItem item in menuItems5)
            {
                handleDefineTags(item, ht);
            }
            foreach (menuItem item in menuItems6)
            {
                handleDefineTags(item, ht);
            }

            foreach (DictionaryEntry de in ht)
            {
                rawXMLDefineTags.AppendLine("<define>"+de.Key.ToString() + de.Value.ToString() + "</define>");
            }            

        }

        private bool generateNewBasichome(string filename)
        {
            if (checkFields() == true)
            {
                SetMenuCount();
                string fBasicHomeOrg = path + "\\"+filename;
                //DateTime thisDay = DateTime.Now;
                //string fBasicHomeNew = path + "\\" + filename + "_" + thisDay.ToString("yyyy_M_d_hh_mm_ss") + ".xml";
                //File.Copy(fBasicHomeOrg, fBasicHomeNew);
                rawXML = new StringBuilder();
                rawXMLBg = new StringBuilder();
                using (StreamWriter sw = new StreamWriter(fBasicHomeOrg))
                {
                    rawXML.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
                    rawXML.AppendLine("<window>");
                    rawXML.AppendLine("<id>35</id>");
                    rawXML.AppendLine("<defaultcontrol>" + setDefaultMenu("Vertical") + "</defaultcontrol>");
                    rawXML.AppendLine("<allowoverlay>no</allowoverlay>");
                    rawXML.AppendLine("<disabletopbar>no</disabletopbar>");
                    rawXML.AppendLine("<rememberLastFocusedControl>yes</rememberLastFocusedControl>");
                    rawXML.AppendLine("<define>#LatestMediaHandler:Yes</define>");
                    rawXML.AppendLine("<define>#usePlayFanart:Yes</define>");
                    rawXML.AppendLine("<define>#useSelectedFanart:Yes</define>");
                    rawXML.AppendLine("<define>#backdrops.skin.lastfocused:" + setDefaultMenu("Vertical") + "</define>");                    
                    rawXML.AppendLine("<!-- RANDOM DEFINE TAGS WILL COME HERE -->");
                    rawXML.AppendLine("<controls>");
                    rawXML.AppendLine("<import>common.backdrop.fallback.xml</import>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR VISIBILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>93761234</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("  <visible>no</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");         
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR MENU VISIBILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>93761236</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("  <visible>no</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");                             
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR NAVIGATION CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>93761254</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("  <visible>no</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR NAVIGATION CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>93761264</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("  <visible>no</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");           
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR FANART VISIBILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>91919291</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR FANART VISIBILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>91919292</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR FANART AVAILABILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>91919293</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("  <visible>no</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR FANART 1 RANDOM VISIBILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>91919297</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR FANART 2 RANDOM VISIBILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>91919298</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR FANART 1 PLAY VISIBILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>91919295</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR FANART 2 PLAY VISIBILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>91919296</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>DUMMY CONTROLL FOR FANART AVAILABILITY CONDITION</description>");
                    rawXML.AppendLine("  <type>label</type>");
                    rawXML.AppendLine("  <id>91919294</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1</width>");
                    rawXML.AppendLine("  <visible>no</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    makeMultiImagePart_Vertical(sw);
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>RSS BACKGROUND</description>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>0</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>1080</height>");
                    rawXML.AppendLine("  <shouldCache>true</shouldCache>");
                    rawXML.AppendLine("  <texture>rss_bg.png</texture>");
                    rawXML.AppendLine("  <visible>Control.HasFocus(91111)</visible>");
                    rawXML.AppendLine("  <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<import>common.basichome.latest.xml</import> ");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>Menu Background</description>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>980</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>100</height>");
                    rawXML.AppendLine("  <shouldCache>true</shouldCache>");
                    rawXML.AppendLine("  <texture>menu_bg.png</texture>");
                    rawXML.AppendLine("  <visible>!Control.HasFocus(7888)+!Control.HasFocus(7999)+!Control.HasFocus(7777)+!Control.HasFocus(73071091)+!Control.isvisible(93761234)</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                
                    rawXML.AppendLine("<control><description>feed image</description>");
                    rawXML.AppendLine("  <type>button</type>");
                    rawXML.AppendLine("  <id>91111</id>");
                    rawXML.AppendLine("  <width>36</width>");
                    rawXML.AppendLine("  <height>36</height>");
                    rawXML.AppendLine("  <posY>1044</posY>");
                    rawXML.AppendLine("  <posX>2</posX>");
                    rawXML.AppendLine("  <hyperlink>16001</hyperlink>");
                    rawXML.AppendLine("  <textureFocus>rss_image_focus.png</textureFocus>");
                    rawXML.AppendLine("  <textureNoFocus>rss_image_nofocus.png</textureNoFocus>");
                    rawXML.AppendLine("  <onup>" + setDefaultMenu("Vertical") + "</onup>");
                    rawXML.AppendLine("  <ondown>73071091</ondown>");
                    rawXML.AppendLine("  <onleft>91111</onleft>");
                    rawXML.AppendLine("  <onright>91111</onright>");
                    rawXML.AppendLine("  <keepaspectratio>yes</keepaspectratio>");
                    rawXML.AppendLine("  <visible>plugin.isenabled(InfoService)+![Control.HasFocus(73071091)|Control.isvisible(93761234)]</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>RSS Items</description>");
                    rawXML.AppendLine("  <type>fadelabel</type>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <width>1852</width>");
                    rawXML.AppendLine("  <height>75</height>");
                    rawXML.AppendLine("  <posY>1040</posY>");
                    rawXML.AppendLine("  <posX>68</posX>	");
                    rawXML.AppendLine("  <font>font14</font>");
                    rawXML.AppendLine("  <textcolor>FFFFFFFF</textcolor>");
                    rawXML.AppendLine("  <label>#infoservice.feed.titles</label>");
                    rawXML.AppendLine("  <wrapString> :: </wrapString>");
                    rawXML.AppendLine("  <scrollStartDelaySec>5</scrollStartDelaySec>");                    
                    rawXML.AppendLine("  <animation effect=\"fade\" start=\"0\" end=\"50\" time=\"10\" reversible=\"false\">WindowOpen</animation>");
                    rawXML.AppendLine("  <visible>plugin.isenabled(InfoService)+![Control.HasFocus(73071091)|Control.isvisible(93761234)]</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>RSS Overlay</description>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>1038</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>42</height>");
                    rawXML.AppendLine("  <shouldCache>true</shouldCache>");
                    rawXML.AppendLine("  <texture>rss_overlay.png</texture>");
                    rawXML.AppendLine("  <visible>plugin.isenabled(InfoService)+![Control.HasFocus(73071091)|Control.isvisible(93761234)]</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>Bg Now Playing Top Line</description>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>976</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>7</height>");
                    rawXML.AppendLine("  <shouldCache>true</shouldCache>");
                    rawXML.AppendLine("  <texture>top.png</texture>");
                    rawXML.AppendLine("  <visible>![window.istopmost(0)|window.istopmost(34)]+![Control.HasFocus(73071091)|Control.isvisible(93761234)]</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("<control>");
                    rawXML.AppendLine("  <description>Bg Now Playing Bottom Line</description>");
                    rawXML.AppendLine("  <type>image</type>");
                    rawXML.AppendLine("  <id>0</id>");
                    rawXML.AppendLine("  <posX>0</posX>");
                    rawXML.AppendLine("  <posY>1034</posY>");
                    rawXML.AppendLine("  <width>1920</width>");
                    rawXML.AppendLine("  <height>7</height>");
                    rawXML.AppendLine("  <shouldCache>true</shouldCache>");
                    rawXML.AppendLine("  <texture>top.png</texture>");
                    rawXML.AppendLine("  <visible>![window.istopmost(0)|window.istopmost(34)]+![Control.HasFocus(73071091)|Control.isvisible(93761234)]</visible>");
                    rawXML.AppendLine("</control>");
                    rawXML.AppendLine(" ");                    

                    int iMenu = 1;
                    if (useMenu1.Equals("True"))
                    {
                        writeMenuPart1_Vertical(sw, textBox1.Text, "", iMenu);
                        iMenu++;
                    }
                    if (useMenu2.Equals("True"))
                    {
                        writeMenuPart1_Vertical(sw, textBox2.Text, "", iMenu);
                        iMenu++;
                    }
                    if (useMenu3.Equals("True"))
                    {
                        writeMenuPart1_Vertical(sw, textBox4.Text, "", iMenu);
                        iMenu++;
                    }
                    if (useMenu4.Equals("True"))
                    {
                        writeMenuPart1_Vertical(sw, textBox6.Text, "", iMenu);
                        iMenu++;
                    }
                    if (useMenu5.Equals("True"))
                    {
                        writeMenuPart1_Vertical(sw, textBox8.Text, "", iMenu);
                        iMenu++;
                    }
                    if (useMenu6.Equals("True"))
                    {
                        writeMenuPart1_Vertical(sw, textBox7.Text, "", iMenu);
                        iMenu++;
                    }
                    iMenu = 1;
                    
                    if (useMenu1.Equals("True"))
                    {
                        writeMenuPart2(sw, iMenu, menuItems1);
                        iMenu++;
                    }
                    if (useMenu2.Equals("True"))
                    {
                        writeMenuPart2(sw, iMenu, menuItems2);
                        iMenu++;
                    }
                    if (useMenu3.Equals("True"))
                    {
                        writeMenuPart2(sw, iMenu, menuItems3);
                        iMenu++;
                    }
                    if (useMenu4.Equals("True"))
                    {
                        writeMenuPart2(sw, iMenu, menuItems4);
                        iMenu++;
                    }
                    if (useMenu5.Equals("True"))
                    {
                        writeMenuPart2(sw, iMenu, menuItems5);
                        iMenu++;
                    }
                    if (useMenu6.Equals("True"))
                    {
                        writeMenuPart2(sw, iMenu, menuItems6);
                        iMenu++;
                    }
                    rawXML.AppendLine("");                    
                    rawXML.AppendLine("<import>common.overlays.xml</import>	");
                    rawXML.AppendLine("<import>common.time.xml</import>	");
                    rawXML.AppendLine("<import>basichome.overlay.WolPowerManager.xml</import> ");
                    //rawXML.AppendLine("<import>common.overlay.fanart.xml</import>");
                    rawXML.AppendLine("<import>Backdrop.version.xml</import>    ");
                    rawXML.AppendLine(" ");
                    rawXML.AppendLine("</controls>");
                    rawXML.AppendLine("<animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">WindowOpen</animation>");
                    rawXML.AppendLine("<animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                    rawXML.AppendLine("</window>");
                    String xml = rawXML.ToString();
                    xml = xml.Replace("<!-- RANDOM DEFINE TAGS WILL COME HERE -->", rawXMLDefineTags.ToString());
                    sw.Write(xml);
                }
                return true;
            }
            else
            {
                MessageBox.Show("ERROR!! Nothing was generated because of missing values!");
                return false;
            }
        }



        private void button18_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                int index = listBox1.SelectedIndex;
                menuItem used_item = new menuItem();
                used_item.name = new_name.Text;
                used_item.hyperlink = new_windowid.Text;
                used_item.imagePath = new_imagepath.Text;
                listBox1.Items.RemoveAt(index);
                listBox1.Items.Insert(index, used_item.name);
                menuItems2.RemoveAt(index);
                menuItems2.Insert(index, used_item);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                int index = listBox2.SelectedIndex;
                menuItem used_item = new menuItem();
                used_item.name = new_name.Text;
                used_item.hyperlink = new_windowid.Text;
                used_item.imagePath = new_imagepath.Text;
                listBox2.Items.RemoveAt(index);
                listBox2.Items.Insert(index, used_item.name);

                menuItems3.RemoveAt(index);
                menuItems3.Insert(index, used_item);
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                int index = listBox3.SelectedIndex;
                menuItem used_item = new menuItem();
                used_item.name = new_name.Text;
                used_item.hyperlink = new_windowid.Text;
                used_item.imagePath = new_imagepath.Text;
                listBox3.Items.RemoveAt(index);
                listBox3.Items.Insert(index, used_item.name);

                menuItems4.RemoveAt(index);
                menuItems4.Insert(index, used_item);
            }
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem != null)
            {
                int index = listBox4.SelectedIndex;
                menuItem used_item = new menuItem();
                used_item.name = new_name.Text;
                used_item.hyperlink = new_windowid.Text;
                used_item.imagePath = new_imagepath.Text;
                listBox4.Items.RemoveAt(index);
                listBox4.Items.Insert(index, used_item.name);

                menuItems5.RemoveAt(index);
                menuItems5.Insert(index, used_item);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.Equals("1"))
            {
                tabControl12.SelectedTab = tabControl12.TabPages[0];
            }
            else if (comboBox1.SelectedItem.Equals("2"))
            {
                tabControl12.SelectedTab = tabControl12.TabPages[1];
            }
            else if (comboBox1.SelectedItem.Equals("3"))
            {
                tabControl12.SelectedTab = tabControl12.TabPages[2];
            }
            else if (comboBox1.SelectedItem.Equals("4"))
            {
                tabControl12.SelectedTab = tabControl12.TabPages[3];
            }
            else if (comboBox1.SelectedItem.Equals("5"))
            {
                tabControl12.SelectedTab = tabControl12.TabPages[4];
            }
            else
            {
                tabControl12.SelectedTab = tabControl12.TabPages[5];
            }
        }

        private string getFilenameOnly(string s)
        {
            s.Replace("//", "\\");
            if (s.IndexOf("\\") >= 0)
            {
                s = s.Substring(s.LastIndexOf("\\") + 1);
            }
            return s;
        }

        private void writeLatest()
        {
            rawXML.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
            rawXML.AppendLine("<window>");
            rawXML.AppendLine("<controls>");
            rawXML.AppendLine("");            
            int iID = 0;
            int iCount = 0;
            int iMenu = 1;
            string sPictures = "";
            string sMovingPictures = "";
            string sTVSeries = "";
            string sMusicVis = "";
            string sTVVis = "";

            if (useMenu1.Equals("True"))
            {
                foreach (menuItem item in menuItems1)
                {
                    iID = Int32.Parse("770" +iMenu + "" + (iCount + 3));
                    if (item.useLatestMedia.Equals("Moving Pictures"))
                    {
                        if (sMovingPictures.Length == 0)
                        {
                            sMovingPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMovingPictures = sMovingPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Music"))
                    {
                        if (sMusicVis.Length == 0)
                        {
                            sMusicVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMusicVis = sMusicVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Pictures"))
                    {
                        if (sPictures.Length == 0)
                        {
                            sPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sPictures = sPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TV Recordings"))
                    {
                        if (sTVVis.Length == 0)
                        {
                            sTVVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVVis = sTVVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TVSeries"))
                    {
                        if (sTVSeries.Length == 0)
                        {
                            sTVSeries = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVSeries = sTVSeries + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu2.Equals("True"))
            {
                foreach (menuItem item in menuItems2)
                {
                    iID = Int32.Parse("770" + iMenu + "" + (iCount + 3));
                    if (item.useLatestMedia.Equals("Moving Pictures"))
                    {
                        if (sMovingPictures.Length == 0)
                        {
                            sMovingPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMovingPictures = sMovingPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Music"))
                    {
                        if (sMusicVis.Length == 0)
                        {
                            sMusicVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMusicVis = sMusicVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Pictures"))
                    {
                        if (sPictures.Length == 0)
                        {
                            sPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sPictures = sPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TV Recordings"))
                    {
                        if (sTVVis.Length == 0)
                        {
                            sTVVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVVis = sTVVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TVSeries"))
                    {
                        if (sTVSeries.Length == 0)
                        {
                            sTVSeries = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVSeries = sTVSeries + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu3.Equals("True"))
            {
                foreach (menuItem item in menuItems3)
                {
                    iID = Int32.Parse("770" + iMenu + "" + (iCount + 3));
                    if (item.useLatestMedia.Equals("Moving Pictures"))
                    {
                        if (sMovingPictures.Length == 0)
                        {
                            sMovingPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMovingPictures = sMovingPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Music"))
                    {
                        if (sMusicVis.Length == 0)
                        {
                            sMusicVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMusicVis = sMusicVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Pictures"))
                    {
                        if (sPictures.Length == 0)
                        {
                            sPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sPictures = sPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TV Recordings"))
                    {
                        if (sTVVis.Length == 0)
                        {
                            sTVVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVVis = sTVVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TVSeries"))
                    {
                        if (sTVSeries.Length == 0)
                        {
                            sTVSeries = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVSeries = sTVSeries + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu4.Equals("True"))
            {
                foreach (menuItem item in menuItems4)
                {
                    iID = Int32.Parse("770" + iMenu + "" + (iCount + 3));
                    if (item.useLatestMedia.Equals("Moving Pictures"))
                    {
                        if (sMovingPictures.Length == 0)
                        {
                            sMovingPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMovingPictures = sMovingPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Music"))
                    {
                        if (sMusicVis.Length == 0)
                        {
                            sMusicVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMusicVis = sMusicVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Pictures"))
                    {
                        if (sPictures.Length == 0)
                        {
                            sPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sPictures = sPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TV Recordings"))
                    {
                        if (sTVVis.Length == 0)
                        {
                            sTVVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVVis = sTVVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TVSeries"))
                    {
                        if (sTVSeries.Length == 0)
                        {
                            sTVSeries = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVSeries = sTVSeries + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu5.Equals("True"))
            {
                foreach (menuItem item in menuItems5)
                {
                    iID = Int32.Parse("770" + iMenu + "" + (iCount + 3));
                    if (item.useLatestMedia.Equals("Moving Pictures"))
                    {
                        if (sMovingPictures.Length == 0)
                        {
                            sMovingPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMovingPictures = sMovingPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Music"))
                    {
                        if (sMusicVis.Length == 0)
                        {
                            sMusicVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMusicVis = sMusicVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Pictures"))
                    {
                        if (sPictures.Length == 0)
                        {
                            sPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sPictures = sPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TV Recordings"))
                    {
                        if (sTVVis.Length == 0)
                        {
                            sTVVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVVis = sTVVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TVSeries"))
                    {
                        if (sTVSeries.Length == 0)
                        {
                            sTVSeries = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVSeries = sTVSeries + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;
            if (useMenu6.Equals("True"))
            {
                foreach (menuItem item in menuItems6)
                {
                    iID = Int32.Parse("770" + iMenu + "" + (iCount + 3));
                    if (item.useLatestMedia.Equals("Moving Pictures"))
                    {
                        if (sMovingPictures.Length == 0)
                        {
                            sMovingPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMovingPictures = sMovingPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Music"))
                    {
                        if (sMusicVis.Length == 0)
                        {
                            sMusicVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sMusicVis = sMusicVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("Pictures"))
                    {
                        if (sPictures.Length == 0)
                        {
                            sPictures = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sPictures = sPictures + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TV Recordings"))
                    {
                        if (sTVVis.Length == 0)
                        {
                            sTVVis = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVVis = sTVVis + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    else if (item.useLatestMedia.Equals("TVSeries"))
                    {
                        if (sTVSeries.Length == 0)
                        {
                            sTVSeries = "Control.HasFocus(" + iID + ")";
                        }
                        else
                        {
                            sTVSeries = sTVSeries + "|Control.HasFocus(" + iID + ")";
                        }
                    }
                    iCount++;
                }
                iMenu++;
            }
            iCount = 0;

            if (sMovingPictures.Length > 0 && latestMovingPictures != null && latestMovingPictures.Equals("True"))
            {
                sMovingPictures = "[" + sMovingPictures + "]";
                rawXML.AppendLine("    <!--MovingPicture 1 ******************************************************-->");
                
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture>background.png</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>#latestMediaHandler.movingpicture.latest1.fanart</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919991)</visible>");
                rawXML.AppendLine("        <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>#latestMediaHandler.movingpicture.latest2.fanart</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919992)</visible>");
                rawXML.AppendLine("        <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>#latestMediaHandler.movingpicture.latest3.fanart</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919993)</visible>");
                rawXML.AppendLine("        <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture>default.png</texture>");
                rawXML.AppendLine("        <visible>[Control.HasFocus(91919991)+!string.equals(#latestMediaHandler.movingpicture.latest1.fanart)]|[Control.HasFocus(91919992)+!string.equals(#latestMediaHandler.movingpicture.latest2.fanart)]|[Control.HasFocus(91919993)+!string.equals(#latestMediaHandler.movingpicture.latest3.fanart)]</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345611</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.movingpicture.latest1.title</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.movingpicture.latest1.title)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Latest Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>970</posY>");
                rawXML.AppendLine("        <width>400</width>");
                rawXML.AppendLine("        <height>40</height>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#SkinTranslation.Translations.LatestMovies.Label</label>");
                rawXML.AppendLine("        <font>font18</font>");
                rawXML.AppendLine("        <textcolor>ffffffff</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"90\" end=\"90\" center=\"1734,970\" time=\"0\" condition=\"true\">Conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" delay=\"350\" time=\"50\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[Control.isvisible(98345611)+" + sMovingPictures + "]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Latest Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1670</posX>");
                rawXML.AppendLine("        <posY>970</posY>");
                rawXML.AppendLine("        <width>400</width>");
                rawXML.AppendLine("        <height>40</height>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#SkinTranslation.Translations.LatestMovies.Label</label>");
                rawXML.AppendLine("        <font>font18</font>");
                rawXML.AppendLine("        <textcolor>ffffffff</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"90\" end=\"90\" center=\"1670,970\" time=\"0\" condition=\"true\">Conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834561</id>");
                rawXML.AppendLine("        <posX>1800</posX>");
                rawXML.AppendLine("        <posY>405</posY>");
                rawXML.AppendLine("        <width>112</width>");
                rawXML.AppendLine("        <height>166</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"poster_frame_rounder_corners_background1.png\">#latestMediaHandler.movingpicture.latest1.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]|[Control.isvisible(98345611)+" + sMovingPictures + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1800</posX>");
                rawXML.AppendLine("        <posY>405</posY>");
                rawXML.AppendLine("        <width>112</width>");
                rawXML.AppendLine("        <height>166</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"poster_frame_rounder_corners_background1.png\">defaultposter.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]+!Control.hasthumb(9834561)]|[Control.isvisible(98345611)+" + sMovingPictures + "+!Control.hasthumb(9834561)]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Title Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1468</posX>");
                rawXML.AppendLine("        <posY>674</posY>");
                rawXML.AppendLine("        <width>490</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.movingpicture.latest1.title</label>");
                rawXML.AppendLine("        <font>font14</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"0\" end=\"90\" time=\"0\" condition=\"true\">conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919991)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Genre Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1508</posX>");
                rawXML.AppendLine("        <posY>674</posY>");
                rawXML.AppendLine("        <width>490</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.movingpicture.latest1.genre</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"0\" end=\"90\" time=\"0\" condition=\"true\">conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919991)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	"); 

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919991</id>");
                rawXML.AppendLine("        <posX>1800</posX>");
                rawXML.AppendLine("        <posY>405</posY>");
                rawXML.AppendLine("        <width>112</width>");
                rawXML.AppendLine("        <height>166</height>");
                rawXML.AppendLine("        <textureFocus>poster_frame_rounder_corners_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>poster_frame_rounder_corners.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <hyperlink>96742</hyperlink>");
                rawXML.AppendLine("        <hyperlinkParameter>movieid:#latestMediaHandler.movingpicture.latest1.id</hyperlinkParameter>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");                
                rawXML.AppendLine("        <onup>91919993</onup>");
                rawXML.AppendLine("        <ondown>91919992</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]|[Control.isvisible(98345611)+" + sMovingPictures + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");
                rawXML.AppendLine("");


                rawXML.AppendLine("    <!--MovingPicture 2 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345621</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.movingpicture.latest2.title</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.movingpicture.latest2.title))</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834562</id>");
                rawXML.AppendLine("        <posX>1800</posX>");
                rawXML.AppendLine("        <posY>585</posY>");
                rawXML.AppendLine("        <width>112</width>");
                rawXML.AppendLine("        <height>166</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"poster_frame_rounder_corners_background1.png\">#latestMediaHandler.movingpicture.latest2.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]|[Control.isvisible(98345621)+" + sMovingPictures + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1800</posX>");
                rawXML.AppendLine("        <posY>585</posY>");
                rawXML.AppendLine("        <width>112</width>");
                rawXML.AppendLine("        <height>166</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"poster_frame_rounder_corners_background1.png\">defaultposter.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]+!Control.hasthumb(9834562)]|[Control.isvisible(98345621)+" + sMovingPictures + "+!Control.hasthumb(9834562)]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Title Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1468</posX>");
                rawXML.AppendLine("        <posY>674</posY>");
                rawXML.AppendLine("        <width>490</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.movingpicture.latest2.title</label>");
                rawXML.AppendLine("        <font>font14</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"0\" end=\"90\" time=\"0\" condition=\"true\">conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919992)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Genre Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1508</posX>");
                rawXML.AppendLine("        <posY>674</posY>");
                rawXML.AppendLine("        <width>490</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.movingpicture.latest2.genre</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"0\" end=\"90\" time=\"0\" condition=\"true\">conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919992)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	"); 

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919992</id>");
                rawXML.AppendLine("        <posX>1800</posX>");
                rawXML.AppendLine("        <posY>585</posY>");
                rawXML.AppendLine("        <width>112</width>");
                rawXML.AppendLine("        <height>166</height>");                
                rawXML.AppendLine("        <textureFocus>poster_frame_rounder_corners_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>poster_frame_rounder_corners.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <hyperlink>96742</hyperlink>");
                rawXML.AppendLine("        <hyperlinkParameter>movieid:#latestMediaHandler.movingpicture.latest2.id</hyperlinkParameter>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919991</onup>");
                rawXML.AppendLine("        <ondown>91919993</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]|[Control.isvisible(98345621)+" + sMovingPictures + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");
                rawXML.AppendLine("");

                rawXML.AppendLine("    <!--MovingPicture 3 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345631</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.movingpicture.latest3.title</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.movingpicture.latest3.title)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834563</id>");
                rawXML.AppendLine("        <posX>1800</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>112</width>");
                rawXML.AppendLine("        <height>166</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"poster_frame_rounder_corners_background1.png\">#latestMediaHandler.movingpicture.latest3.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]|[Control.isvisible(98345631)+" + sMovingPictures + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1800</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>112</width>");
                rawXML.AppendLine("        <height>166</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"poster_frame_rounder_corners_background1.png\">defaultposter.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]+!Control.hasthumb(9834563)]|[Control.isvisible(98345631)+" + sMovingPictures + "+!Control.hasthumb(9834563)]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Title Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1468</posX>");
                rawXML.AppendLine("        <posY>674</posY>");
                rawXML.AppendLine("        <width>490</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.movingpicture.latest3.title</label>");
                rawXML.AppendLine("        <font>font14</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"0\" end=\"90\" time=\"0\" condition=\"true\">conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919993)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Genre Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1508</posX>");
                rawXML.AppendLine("        <posY>674</posY>");
                rawXML.AppendLine("        <width>490</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.movingpicture.latest3.genre</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"0\" end=\"90\" time=\"0\" condition=\"true\">conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919993)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	"); 

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919993</id>");
                rawXML.AppendLine("        <posX>1800</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>112</width>");
                rawXML.AppendLine("        <height>166</height>");
                rawXML.AppendLine("        <textureFocus>poster_frame_rounder_corners_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>poster_frame_rounder_corners.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <hyperlink>96742</hyperlink>");
                rawXML.AppendLine("        <hyperlinkParameter>movieid:#latestMediaHandler.movingpicture.latest3.id</hyperlinkParameter>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919992</onup>");
                rawXML.AppendLine("        <ondown>91919991</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919991)|Control.HasFocus(91919992)|Control.HasFocus(91919993)]|[Control.isvisible(98345631)+" + sMovingPictures + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");
                rawXML.AppendLine("");
            }

            if (sTVSeries.Length > 0 && latestTVSeries != null && latestTVSeries.Equals("True"))
            {
                sTVSeries = "[" + sTVSeries + "]";
                rawXML.AppendLine("    <!--TVSeries 1 ******************************************************-->");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>background.png</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>#latestMediaHandler.tvseries.latest1.fanart</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919994)</visible>");
                rawXML.AppendLine("        <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>#latestMediaHandler.tvseries.latest2.fanart</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919995)</visible>");
                rawXML.AppendLine("        <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>#latestMediaHandler.tvseries.latest3.fanart</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919996)</visible>");
                rawXML.AppendLine("        <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>default.png</texture>");
                rawXML.AppendLine("        <visible>[Control.HasFocus(91919994)+!string.equals(#latestMediaHandler.tvseries.latest1.fanart)]|[Control.HasFocus(91919995)+!string.equals(#latestMediaHandler.tvseries.latest2.fanart)]|[Control.HasFocus(91919996)+!string.equals(#latestMediaHandler.tvseries.latest3.fanart)]</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345511</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvseries.latest1.episodeName</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.tvseries.latest1.episodeName)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Latest Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1624</posX>");
                rawXML.AppendLine("        <posY>970</posY>");
                rawXML.AppendLine("        <width>400</width>");
                rawXML.AppendLine("        <height>40</height>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#SkinTranslation.Translations.LatestSeries.Label</label>");
                rawXML.AppendLine("        <font>font18</font>");
                rawXML.AppendLine("        <textcolor>ffffffff</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"90\" end=\"90\" center=\"1624,970\" time=\"0\" condition=\"true\">Conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" delay=\"350\" time=\"50\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[Control.isvisible(98345511)+" + sTVSeries + "]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Latest Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1554</posX>");
                rawXML.AppendLine("        <posY>970</posY>");
                rawXML.AppendLine("        <width>400</width>");
                rawXML.AppendLine("        <height>40</height>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#SkinTranslation.Translations.LatestSeries.Label</label>");
                rawXML.AppendLine("        <font>font18</font>");
                rawXML.AppendLine("        <textcolor>ffffffff</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"90\" end=\"90\" center=\"1554,970\" time=\"0\" condition=\"true\">Conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834551</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>465</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">#latestMediaHandler.tvseries.latest1.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]|[Control.isvisible(98345511)+" + sTVSeries + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>465</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">default.recording.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]+!Control.hasthumb(9834551)]|[Control.isvisible(98345511)+" + sTVSeries + "]+!Control.hasthumb(9834551)]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Series Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1396</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvseries.latest1.serieName</label>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919994)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");
               
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Episode Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1425</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvseries.latest1.episodeName</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919994)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919994</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>465</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <textureFocus>episode.frame_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>episode.frame.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919996</onup>");
                rawXML.AppendLine("        <ondown>91919995</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]|[Control.isvisible(98345511)+" + sTVSeries + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");
                rawXML.AppendLine("");
                
                rawXML.AppendLine("    <!--TVSeries 2 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345521</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvseries.latest2.episodeName</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.tvseries.latest2.episodeName)</visible>");
                rawXML.AppendLine("    </control>	");
                
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834552</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>615</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">#latestMediaHandler.tvseries.latest2.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]|[Control.isvisible(98345521)+" + sTVSeries + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");               

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>615</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">default.recording.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]+!Control.hasthumb(9834552)]|[Control.isvisible(98345521)+" + sTVSeries + "+!Control.hasthumb(9834552)]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Series Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1396</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvseries.latest2.serieName</label>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919995)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Episode Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1425</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvseries.latest2.episodeName</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919995)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919995</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>615</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <textureFocus>episode.frame_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>episode.frame.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919994</onup>");
                rawXML.AppendLine("        <ondown>91919996</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]|[Control.isvisible(98345521)+" + sTVSeries + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");
                rawXML.AppendLine("");

                rawXML.AppendLine("    <!--TVSeries 3 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345531</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvseries.latest3.episodeName</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.tvseries.latest3.episodeName)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834553</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">#latestMediaHandler.tvseries.latest3.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]|[Control.isvisible(98345531)+" + sTVSeries + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">default.recording.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]+!Control.hasthumb(9834553)]|[Control.isvisible(98345531)+" + sTVSeries + "+!Control.hasthumb(9834552)]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Series Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1396</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvseries.latest3.serieName</label>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919996)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Episode Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1425</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvseries.latest3.episodeName</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919996)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919996</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <textureFocus>episode.frame_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>episode.frame.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919995</onup>");
                rawXML.AppendLine("        <ondown>91919994</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919994)|Control.HasFocus(91919995)|Control.HasFocus(91919996)]|[Control.isvisible(98345531)+" + sTVSeries + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");
                rawXML.AppendLine("");
            }

            if (sPictures.Length > 0 && latestPictures != null && latestPictures.Equals("True"))
            {
                sPictures = "[" + sPictures + "]";
                rawXML.AppendLine("    <!--Picture 1 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345711</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.picture.latest1.filename</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.picture.latest1.filename)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Latest Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1654</posX>");
                rawXML.AppendLine("        <posY>970</posY>");
                rawXML.AppendLine("        <width>400</width>");
                rawXML.AppendLine("        <height>40</height>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#SkinTranslation.Translations.LatestPictures.Label</label>");
                rawXML.AppendLine("        <font>font18</font>");
                rawXML.AppendLine("        <textcolor>ffffffff</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"90\" end=\"90\" center=\"1654,970\" time=\"0\" condition=\"true\">Conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,450\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"0,450\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.isvisible(98345711)+" + sPictures + "+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834571</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>465</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">#latestMediaHandler.picture.latest1.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.isvisible(98345711)+" + sPictures + "+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");                

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>465</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture>episode.frame.png</texture>	  ");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.isvisible(98345711)+" + sPictures + "+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <!--Picture 2 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345721</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.picture.latest2.filename</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.picture.latest2.filename)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834572</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>615</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">#latestMediaHandler.picture.latest2.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.isvisible(98345721)+" + sPictures + "+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>615</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture>episode.frame.png</texture>	  ");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.isvisible(98345721)+" + sPictures + "+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <!--Picture 3 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345731</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.picture.latest3.filename</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.picture.latest3.filename)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834573</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">#latestMediaHandler.picture.latest3.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.isvisible(98345731)+" + sPictures + "+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture>episode.frame.png</texture>	  ");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.isvisible(98345731)+" + sPictures + "+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");
  
            }

            if (sMusicVis.Length > 0 && latestMusic != null && latestMusic.Equals("True"))
            {
                sMusicVis = "[" + sMusicVis + "]";
                rawXML.AppendLine("    <!--Music 1 ******************************************************-->");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>background.png</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>#latestMediaHandler.music.latest1.fanart</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919997)</visible>");
                rawXML.AppendLine("        <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>#latestMediaHandler.music.latest2.fanart</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919998)</visible>");
                rawXML.AppendLine("        <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>#latestMediaHandler.music.latest3.fanart</texture>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919999)</visible>");
                rawXML.AppendLine("        <animation effect=\"fade\" time=\"850\">VisibleChange</animation>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>SELECTED BACKGROUND</description>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>0</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1920</width>");
                rawXML.AppendLine("        <height>1080</height>");
                rawXML.AppendLine("        <texture>default.png</texture>");
                rawXML.AppendLine("        <visible>[Control.HasFocus(91919997)+!string.equals(#latestMediaHandler.music.latest1.fanart)]|[Control.HasFocus(91919998)+!string.equals(#latestMediaHandler.music.latest2.fanart)]|[Control.HasFocus(91919999)+!string.equals(#latestMediaHandler.music.latest3.fanart)]</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345811</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.music.latest1.artist</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.music.latest1.artist)</visible>");
                rawXML.AppendLine("    </control>	");
                
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Latest Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1680</posX>");
                rawXML.AppendLine("        <posY>970</posY>");
                rawXML.AppendLine("        <width>400</width>");
                rawXML.AppendLine("        <height>40</height>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#SkinTranslation.Translations.LatestMusic.Label</label>");
                rawXML.AppendLine("        <font>font18</font>");
                rawXML.AppendLine("        <textcolor>ffffffff</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"90\" end=\"90\" center=\"1680,970\" time=\"0\" condition=\"true\">Conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" delay=\"350\" time=\"50\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[Control.isvisible(98345811)+" + sMusicVis + "]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Latest Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1614</posX>");
                rawXML.AppendLine("        <posY>970</posY>");
                rawXML.AppendLine("        <width>400</width>");
                rawXML.AppendLine("        <height>40</height>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#SkinTranslation.Translations.LatestMusic.Label</label>");
                rawXML.AppendLine("        <font>font18</font>");
                rawXML.AppendLine("        <textcolor>ffffffff</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"90\" end=\"90\" center=\"1614,970\" time=\"0\" condition=\"true\">Conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834581</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>420</posY>");
                rawXML.AppendLine("        <width>180</width>");
                rawXML.AppendLine("        <height>180</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"music_frame_rounder_corners_background1.png\">#latestMediaHandler.music.latest1.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]|[Control.isvisible(98345811)+" + sMusicVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>420</posY>");
                rawXML.AppendLine("        <width>180</width>");
                rawXML.AppendLine("        <height>180</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"music_frame_rounder_corners_background1.png\">nothumb.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]+!Control.hasthumb(9834581)]|[Control.isvisible(98345811)+" + sMusicVis + "+!Control.hasthumb(9834581)]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Artist Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1407</posX>");
                rawXML.AppendLine("        <posY>693</posY>");
                rawXML.AppendLine("        <width>495</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.music.latest1.artist</label>");
                rawXML.AppendLine("        <font>font14</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919997)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Album Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1445</posX>");
                rawXML.AppendLine("        <posY>693</posY>");
                rawXML.AppendLine("        <width>495</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.music.latest1.album</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919997)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	"); 

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919997</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>420</posY>");
                rawXML.AppendLine("        <width>180</width>");
                rawXML.AppendLine("        <height>180</height>");
                rawXML.AppendLine("        <textureFocus>music_frame_rounder_corners_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>music_frame_rounder_corners.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919999</onup>");
                rawXML.AppendLine("        <ondown>91919998</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]|[Control.isvisible(98345811)+" + sMusicVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <!--Music 2 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345821</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.music.latest2.artist</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.music.latest2.artist)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834582</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>600</posY>");
                rawXML.AppendLine("        <width>180</width>");
                rawXML.AppendLine("        <height>180</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"music_frame_rounder_corners_background1.png\">#latestMediaHandler.music.latest2.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]|[Control.isvisible(98345821)+" + sMusicVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>600</posY>");
                rawXML.AppendLine("        <width>180</width>");
                rawXML.AppendLine("        <height>180</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"music_frame_rounder_corners_background1.png\">nothumb.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]+!Control.hasthumb(9834582)]|[Control.isvisible(98345821)+" + sMusicVis + "+!Control.hasthumb(9834582)]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Artist Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1407</posX>");
                rawXML.AppendLine("        <posY>693</posY>");
                rawXML.AppendLine("        <width>495</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.music.latest2.artist</label>");
                rawXML.AppendLine("        <font>font14</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919998)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Album Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1445</posX>");
                rawXML.AppendLine("        <posY>693</posY>");
                rawXML.AppendLine("        <width>495</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.music.latest2.album</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919998)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	"); 

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919998</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>600</posY>");
                rawXML.AppendLine("        <width>180</width>");
                rawXML.AppendLine("        <height>180</height>");
                rawXML.AppendLine("        <textureFocus>music_frame_rounder_corners_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>music_frame_rounder_corners.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919997</onup>");
                rawXML.AppendLine("        <ondown>91919999</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]|[Control.isvisible(98345821)+" + sMusicVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <!--Music 3 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345831</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.music.latest2.artist</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.music.latest3.artist)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834583</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>780</posY>");
                rawXML.AppendLine("        <width>180</width>");
                rawXML.AppendLine("        <height>180</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"music_frame_rounder_corners_background1.png\">#latestMediaHandler.music.latest3.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]|[Control.isvisible(98345831)+" + sMusicVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>780</posY>");
                rawXML.AppendLine("        <width>180</width>");
                rawXML.AppendLine("        <height>180</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture mask=\"music_frame_rounder_corners_background1.png\">nothumb.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]+!Control.hasthumb(9834583)]|[Control.isvisible(98345831)+" + sMusicVis + "+!Control.hasthumb(9834583)]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Artist Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1407</posX>");
                rawXML.AppendLine("        <posY>693</posY>");
                rawXML.AppendLine("        <width>495</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.music.latest3.artist</label>");
                rawXML.AppendLine("        <font>font14</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919999)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Album Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1445</posX>");
                rawXML.AppendLine("        <posY>693</posY>");
                rawXML.AppendLine("        <width>495</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.music.latest3.album</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919999)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	"); 

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919999</id>");
                rawXML.AppendLine("        <posX>1734</posX>");
                rawXML.AppendLine("        <posY>780</posY>");
                rawXML.AppendLine("        <width>180</width>");
                rawXML.AppendLine("        <height>180</height>");
                rawXML.AppendLine("        <textureFocus>music_frame_rounder_corners_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>music_frame_rounder_corners.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919998</onup>");
                rawXML.AppendLine("        <ondown>91919997</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919997)|Control.HasFocus(91919998)|Control.HasFocus(91919999)]|[Control.isvisible(98345831)+" + sMusicVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

            }

            if (sTVVis.Length > 0 && latestTVRecordings != null && latestTVRecordings.Equals("True"))
            {
                sTVVis = "[" + sTVVis + "]";
                rawXML.AppendLine("    <!--TV Recordings 1 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345911</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvrecordings.latest1.title</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.tvrecordings.latest1.title)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Latest Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1624</posX>");
                rawXML.AppendLine("        <posY>970</posY>");
                rawXML.AppendLine("        <width>400</width>");
                rawXML.AppendLine("        <height>40</height>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#SkinTranslation.Translations.LatestRecordings.Label</label>");
                rawXML.AppendLine("        <font>font18</font>");
                rawXML.AppendLine("        <textcolor>ffffffff</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"90\" end=\"90\" center=\"1624,970\" time=\"0\" condition=\"true\">Conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" delay=\"350\" time=\"50\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[Control.isvisible(98345511)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Latest Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1554</posX>");
                rawXML.AppendLine("        <posY>970</posY>");
                rawXML.AppendLine("        <width>400</width>");
                rawXML.AppendLine("        <height>40</height>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#SkinTranslation.Translations.LatestRecordings.Label</label>");
                rawXML.AppendLine("        <font>font18</font>");
                rawXML.AppendLine("        <textcolor>ffffffff</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" start=\"90\" end=\"90\" center=\"1554,970\" time=\"0\" condition=\"true\">Conditional</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834591</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>465</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">#latestMediaHandler.tvrecordings.latest1.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345911)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>465</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">default.recording.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345911)+" + sTVVis + "]]+!Control.hasthumb(9834591)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>465</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture>episode.frame.png</texture>	  ");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345911)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Series Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1396</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvrecordings.latest1.title</label>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919984)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Episode Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1425</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvrecordings.latest1.genre</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919984)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919984</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>465</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <textureFocus>episode.frame_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>episode.frame.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919986</onup>");
                rawXML.AppendLine("        <ondown>91919985</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345911)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");                
                rawXML.AppendLine("    </control>");


                rawXML.AppendLine("    <!--TV Recordings 2 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345921</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvrecordings.latest2.title</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.tvrecordings.latest2.title)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834592</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>615</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">#latestMediaHandler.tvrecordings.latest2.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345921)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>615</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">default.recording.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345921)+" + sTVVis + "]]+!Control.hasthumb(9834592)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>615</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture>episode.frame.png</texture>	  ");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345921)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Series Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1396</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvrecordings.latest2.title</label>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919985)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Episode Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1425</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvrecordings.latest2.genre</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919985)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919985</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>615</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <textureFocus>episode.frame_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>episode.frame.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919984</onup>");
                rawXML.AppendLine("        <ondown>91919986</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345921)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");
 
                rawXML.AppendLine("    <!--TV Recordings 3 ******************************************************-->");
                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Dummy label</description>");
                rawXML.AppendLine("        <type>label</type>");
                rawXML.AppendLine("        <id>98345931</id>");
                rawXML.AppendLine("        <posX>3000</posX>");
                rawXML.AppendLine("        <posY>0</posY>");
                rawXML.AppendLine("        <width>1</width>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvrecordings.latest3.title</label>");
                rawXML.AppendLine("        <visible>string.equals(#latestMediaHandler.tvrecordings.latest3.title)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>9834593</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">#latestMediaHandler.tvrecordings.latest3.thumb</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345931)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Cover Art</description>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <keepaspectratio>yes</keepaspectratio>	  ");
                rawXML.AppendLine("        <zoom>yes</zoom>");
                rawXML.AppendLine("        <zoomfromtop>yes</zoomfromtop>	  ");
                rawXML.AppendLine("        <centered>yes</centered>");
                rawXML.AppendLine("        <texture mask=\"episode.frame.background.png\">default.recording.png</texture>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345931)+" + sTVVis + "]]+!Control.hasthumb(9834593)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <type>image</type>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <shouldCache>true</shouldCache>");
                rawXML.AppendLine("        <texture>episode.frame.png</texture>	  ");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible>[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345931)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Series Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1396</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvrecordings.latest3.title</label>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919986)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>	");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Episode Label</description>");
                rawXML.AppendLine("        <type>fadelabel</type>");
                rawXML.AppendLine("        <id>0</id>");
                rawXML.AppendLine("        <posX>1425</posX>");
                rawXML.AppendLine("        <posY>670</posY>");
                rawXML.AppendLine("        <width>420</width>");
                rawXML.AppendLine("        <align>left</align>");
                rawXML.AppendLine("        <scrollStartDelaySec>3600</scrollStartDelaySec>");
                rawXML.AppendLine("        <label>#latestMediaHandler.tvrecordings.latest3.genre</label>");
                rawXML.AppendLine("        <font>font13</font>");
                rawXML.AppendLine("        <textcolor>ffa9d0f7</textcolor>");
                rawXML.AppendLine("        <shadowAngle>45</shadowAngle>");
                rawXML.AppendLine("        <shadowDistance>1</shadowDistance>");
                rawXML.AppendLine("        <shadowColor>ff333333</shadowColor>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"rotate\" end=\"90\" time=\"0\">Hidden</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"0\" end=\"100\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"50\">Hidden</animation>");
                rawXML.AppendLine("        <visible>Control.HasFocus(91919986)+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");

                rawXML.AppendLine("    <control>");
                rawXML.AppendLine("        <description>Play Button</description>");
                rawXML.AppendLine("        <type>button</type>");
                rawXML.AppendLine("        <id>91919986</id>");
                rawXML.AppendLine("        <posX>1678</posX>");
                rawXML.AppendLine("        <posY>765</posY>");
                rawXML.AppendLine("        <width>242</width>");
                rawXML.AppendLine("        <height>142</height>");
                rawXML.AppendLine("        <textureFocus>episode.frame_play.png</textureFocus>");
                rawXML.AppendLine("        <textureNoFocus>episode.frame.png</textureNoFocus>");
                rawXML.AppendLine("        <label>-</label>");
                rawXML.AppendLine("        <onleft>#backdrops.skin.lastfocused</onleft>");
                rawXML.AppendLine("        <onright>#backdrops.skin.lastfocused</onright>");
                rawXML.AppendLine("        <onup>91919985</onup>");
                rawXML.AppendLine("        <ondown>91919984</ondown>");
                rawXML.AppendLine("        <font>font12</font>");
                rawXML.AppendLine("        <animation effect=\"fade\" start=\"100\" end=\"0\" time=\"350\">WindowClose</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"450,0\" end=\"0,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Visible</animation>");
                rawXML.AppendLine("        <animation effect=\"slide\" start=\"0,0\" end=\"450,0\" tween=\"quadratic\" easing=\"in\" time=\"350\">Hidden</animation>");
                rawXML.AppendLine("        <visible allowhiddenfocus=\"true\">[[Control.HasFocus(91919984)|Control.HasFocus(91919985)|Control.HasFocus(91919986)]|[Control.isvisible(98345921)+" + sTVVis + "]]+!Control.isvisible(93761234)</visible>");
                rawXML.AppendLine("    </control>");
            }
            
            rawXML.AppendLine("");
            rawXML.AppendLine("</controls>");
            rawXML.AppendLine("</window>");
  
        }
        private void used_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (used_list.SelectedIndex >= 0)
            {
                int index = used_list.SelectedIndex;                
                new_name.Text = menuItems1[index].name;
                new_windowid.Text = menuItems1[index].hyperlink;
                comboBox17.SelectedItem = menuItems1[index].useLatestMedia;
                comboBox1.SelectedItem = "1";
                try
                {
                    HandleParameters();
                }
                catch
                { }
                if (menuItems1[index].mainCategory != null && menuItems1[index].mainCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox18.SelectedItem = menuItems1[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox21.SelectedItem = menuItems1[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("9811"))
                    {
                        comboBox23.SelectedItem = menuItems1[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("504") || new_windowid.Text.Equals("504"))
                    {
                        comboBox22.SelectedItem = menuItems1[index].mainCategory;
                    }
                }
                if (menuItems1[index].subCategory != null && menuItems1[index].subCategory.Length > 0)
                {                    
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox19.SelectedItem = menuItems1[index].subCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox20.SelectedItem = menuItems1[index].subCategory;
                    }
                }
                if (menuItems1[index].imagePath.StartsWith("Random"))
                {
                    comboBox5.SelectedItem = "Fanart Plugin";
                    comboBox16.SelectedItem = menuItems1[index].imagePath;
                }
                else if (getFilenameOnly(menuItems1[index].imagePath).IndexOf(".") > 0)
                {
                    comboBox5.SelectedItem = "Single Image";
                    new_imagepath.Text = menuItems1[index].imagePath;
                }
                else
                {
                    comboBox5.SelectedItem = "Multiple Images";
                    new_imagepath.Text = menuItems1[index].imagePath;
                }                
                listBox1.SelectedIndex = -1;
                listBox2.SelectedIndex = -1;
                listBox3.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
                if (menuItems1[index].name.Equals("Multishortcut"))
                {
                    new_windowid.Enabled = true;
                }

                updateIndex = index;
                comboBox1.Enabled = false;                
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                int index = listBox1.SelectedIndex;
                new_name.Text = menuItems2[index].name;
                new_windowid.Text = menuItems2[index].hyperlink;
                comboBox17.SelectedItem = menuItems2[index].useLatestMedia;
                comboBox1.SelectedItem = "2";
                try
                {
                    HandleParameters();
                }
                catch
                { }
                if (menuItems2[index].mainCategory != null && menuItems2[index].mainCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox18.SelectedItem = menuItems2[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox21.SelectedItem = menuItems2[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("9811"))
                    {
                        comboBox23.SelectedItem = menuItems2[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("504") || new_windowid.Text.Equals("504"))
                    {
                        comboBox22.SelectedItem = menuItems2[index].mainCategory;
                    }
                }
                if (menuItems2[index].subCategory != null && menuItems2[index].subCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox19.SelectedItem = menuItems2[index].subCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox20.SelectedItem = menuItems2[index].subCategory;
                    }
                }
                if (menuItems2[index].imagePath.StartsWith("Random"))
                {
                    comboBox5.SelectedItem = "Fanart Plugin";
                    comboBox16.SelectedItem = menuItems2[index].imagePath;
                }
                else if (getFilenameOnly(menuItems2[index].imagePath).IndexOf(".") > 0)
                {
                    comboBox5.SelectedItem = "Single Image";
                    new_imagepath.Text = menuItems2[index].imagePath;
                }
                else
                {
                    comboBox5.SelectedItem = "Multiple Images";
                    new_imagepath.Text = menuItems2[index].imagePath;
                }
                used_list.SelectedIndex = -1;
                listBox2.SelectedIndex = -1;
                listBox3.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
                if (menuItems2[index].name.Equals("Multishortcut"))
                {
                    new_windowid.Enabled = true;
                }
//                listBox1.Items.RemoveAt(index);
//                menuItems2.RemoveAt(index);
                updateIndex = index;
                comboBox1.Enabled = false;                
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                int index = listBox2.SelectedIndex;
                new_name.Text = menuItems3[index].name;
                new_windowid.Text = menuItems3[index].hyperlink;
                comboBox17.SelectedItem = menuItems3[index].useLatestMedia;
                comboBox1.SelectedItem = "3";
                try
                {
                    HandleParameters();
                }
                catch
                { }
                if (menuItems3[index].mainCategory != null && menuItems3[index].mainCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox18.SelectedItem = menuItems3[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox21.SelectedItem = menuItems3[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("9811"))
                    {
                        comboBox23.SelectedItem = menuItems3[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("504") || new_windowid.Text.Equals("504"))
                    {
                        comboBox22.SelectedItem = menuItems3[index].mainCategory;
                    }
                }
                if (menuItems3[index].subCategory != null && menuItems3[index].subCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox19.SelectedItem = menuItems3[index].subCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox20.SelectedItem = menuItems3[index].subCategory;
                    }
                }
                if (menuItems3[index].imagePath.StartsWith("Random"))
                {
                    comboBox5.SelectedItem = "Fanart Plugin";
                    comboBox16.SelectedItem = menuItems3[index].imagePath;
                }
                else if (getFilenameOnly(menuItems3[index].imagePath).IndexOf(".") > 0)
                {
                    comboBox5.SelectedItem = "Single Image";
                    new_imagepath.Text = menuItems3[index].imagePath;
                }
                else
                {
                    comboBox5.SelectedItem = "Multiple Images";
                    new_imagepath.Text = menuItems3[index].imagePath;
                }
                used_list.SelectedIndex = -1;
                listBox1.SelectedIndex = -1;
                listBox3.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
                if (menuItems3[index].name.Equals("Multishortcut"))
                {
                    new_windowid.Enabled = true;
                }
//                listBox2.Items.RemoveAt(index);
//                menuItems3.RemoveAt(index);
                updateIndex = index;
                comboBox1.Enabled = false;                
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex >= 0)
            {
                int index = listBox3.SelectedIndex;
                new_name.Text = menuItems4[index].name;
                new_windowid.Text = menuItems4[index].hyperlink;
                comboBox17.SelectedItem = menuItems4[index].useLatestMedia;
                comboBox1.SelectedItem = "4";
                try
                {
                    HandleParameters();
                }
                catch
                { }
                if (menuItems4[index].mainCategory != null && menuItems4[index].mainCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox18.SelectedItem = menuItems4[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox21.SelectedItem = menuItems4[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("9811"))
                    {
                        comboBox23.SelectedItem = menuItems4[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("504") || new_windowid.Text.Equals("504"))
                    {
                        comboBox22.SelectedItem = menuItems4[index].mainCategory;
                    }
                }
                if (menuItems4[index].subCategory != null && menuItems4[index].subCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox19.SelectedItem = menuItems4[index].subCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox20.SelectedItem = menuItems4[index].subCategory;
                    }
                }
                if (menuItems4[index].imagePath.StartsWith("Random"))
                {
                    comboBox5.SelectedItem = "Fanart Plugin";
                    comboBox16.SelectedItem = menuItems4[index].imagePath;
                }
                else if (getFilenameOnly(menuItems4[index].imagePath).IndexOf(".") > 0)
                {
                    comboBox5.SelectedItem = "Single Image";
                    new_imagepath.Text = menuItems4[index].imagePath;
                }
                else
                {
                    comboBox5.SelectedItem = "Multiple Images";
                    new_imagepath.Text = menuItems4[index].imagePath;
                }
                used_list.SelectedIndex = -1;
                listBox1.SelectedIndex = -1;
                listBox2.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
                if (menuItems4[index].name.Equals("Multishortcut"))
                {
                    new_windowid.Enabled = true;
                }
//                listBox3.Items.RemoveAt(index);
//                menuItems4.RemoveAt(index);
                updateIndex = index;
                comboBox1.Enabled = false;                
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex >= 0)
            {
                int index = listBox4.SelectedIndex;
                new_name.Text = menuItems5[index].name;
                new_windowid.Text = menuItems5[index].hyperlink;
                comboBox17.SelectedItem = menuItems5[index].useLatestMedia;
                comboBox1.SelectedItem = "5";
                try
                {
                    HandleParameters();
                }
                catch
                { }
                if (menuItems5[index].mainCategory != null && menuItems5[index].mainCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox18.SelectedItem = menuItems5[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox21.SelectedItem = menuItems5[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("9811"))
                    {
                        comboBox23.SelectedItem = menuItems5[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("504") || new_windowid.Text.Equals("504"))
                    {
                        comboBox22.SelectedItem = menuItems5[index].mainCategory;
                    }
                }
                if (menuItems5[index].subCategory != null && menuItems5[index].subCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox19.SelectedItem = menuItems5[index].subCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox20.SelectedItem = menuItems5[index].subCategory;
                    }
                }
                if (menuItems5[index].imagePath.StartsWith("Random"))
                {
                    comboBox5.SelectedItem = "Fanart Plugin";
                    comboBox16.SelectedItem = menuItems5[index].imagePath;
                }
                else if (getFilenameOnly(menuItems5[index].imagePath).IndexOf(".") > 0)
                {
                    comboBox5.SelectedItem = "Single Image";
                    new_imagepath.Text = menuItems5[index].imagePath;
                }
                else
                {
                    comboBox5.SelectedItem = "Multiple Images";
                    new_imagepath.Text = menuItems5[index].imagePath;
                }
                used_list.SelectedIndex = -1;
                listBox1.SelectedIndex = -1;
                listBox2.SelectedIndex = -1;
                listBox3.SelectedIndex = -1;
                listBox5.SelectedIndex = -1;
                if (menuItems5[index].name.Equals("Multishortcut"))
                {
                    new_windowid.Enabled = true;
                }
//                listBox4.Items.RemoveAt(index);
//                menuItems5.RemoveAt(index);
                updateIndex = index;
                comboBox1.Enabled = false;                
            }
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button22_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at top
            if (listBox5.SelectedItem != null && listBox5.SelectedIndex > 0)
            {
                int index = listBox5.SelectedIndex;
                Object listItem = listBox5.SelectedItem;
                menuItem mnuItem = menuItems6[index];
                listBox5.Items.RemoveAt(index);
                menuItems6.RemoveAt(index);
                listBox5.Items.Insert(index - 1, listItem);
                menuItems6.Insert(index - 1, mnuItem);
                listBox5.SelectedIndex = index - 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at bottom
            if (listBox5.SelectedItem != null && listBox5.SelectedIndex < listBox5.Items.Count - 1)
            {
                int index = listBox5.SelectedIndex;
                Object listItem = listBox5.SelectedItem;
                menuItem mnuItem = menuItems6[index];
                listBox5.Items.RemoveAt(index);
                menuItems6.RemoveAt(index);
                if (index < listBox5.Items.Count)
                {
                    listBox5.Items.Insert(index + 1, listItem);
                    menuItems6.Insert(index + 1, mnuItem);
                }
                else
                {
                    listBox5.Items.Add(listItem);
                    menuItems6.Add(mnuItem);
                }
                listBox5.SelectedIndex = index + 1;
                new_name.Text = "";
                new_imagepath.Text = "";
                new_windowid.Text = "";
                updateIndex = 999;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            removeToolStripMenuItem_Click6(sender, e);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem != null)
            {
                int index = listBox5.SelectedIndex;
                menuItem used_item = new menuItem();
                used_item.name = new_name.Text;
                used_item.hyperlink = new_windowid.Text;
                used_item.imagePath = new_imagepath.Text;
                listBox5.Items.RemoveAt(index);
                listBox5.Items.Insert(index, used_item.name);

                menuItems6.RemoveAt(index);
                menuItems6.Insert(index, used_item);
            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedIndex >= 0)
            {
                int index = listBox5.SelectedIndex;
                new_name.Text = menuItems6[index].name;
                new_windowid.Text = menuItems6[index].hyperlink;
                comboBox17.SelectedItem = menuItems6[index].useLatestMedia;
                comboBox1.SelectedItem = "6";
                try
                {
                    HandleParameters();
                }
                catch
                { }
                if (menuItems6[index].mainCategory != null && menuItems6[index].mainCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox18.SelectedItem = menuItems6[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox21.SelectedItem = menuItems6[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("9811"))
                    {
                        comboBox23.SelectedItem = menuItems6[index].mainCategory;
                    }
                    else if (new_windowid.Text.Equals("504") || new_windowid.Text.Equals("504"))
                    {
                        comboBox22.SelectedItem = menuItems6[index].mainCategory;
                    }
                }
                if (menuItems6[index].subCategory != null && menuItems6[index].subCategory.Length > 0)
                {
                    if (new_windowid.Text.Equals("96742"))
                    {
                        comboBox19.SelectedItem = menuItems6[index].subCategory;
                    }
                    else if (new_windowid.Text.Equals("4755"))
                    {
                        comboBox20.SelectedItem = menuItems6[index].subCategory;
                    }
                }
                if (menuItems6[index].imagePath.StartsWith("Random"))
                {
                    comboBox5.SelectedItem = "Fanart Plugin";
                    comboBox16.SelectedItem = menuItems6[index].imagePath;
                }
                else if (getFilenameOnly(menuItems6[index].imagePath).IndexOf(".") > 0)
                {
                    comboBox5.SelectedItem = "Single Image";
                    new_imagepath.Text = menuItems6[index].imagePath;
                }
                else
                {
                    comboBox5.SelectedItem = "Multiple Images";
                    new_imagepath.Text = menuItems6[index].imagePath;
                }
                used_list.SelectedIndex = -1;
                listBox1.SelectedIndex = -1;
                listBox2.SelectedIndex = -1;
                listBox3.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                if (menuItems6[index].name.Equals("Multishortcut"))
                {
                    new_windowid.Enabled = true;
                }
                //                listBox4.Items.RemoveAt(index);
                //                menuItems5.RemoveAt(index);
                updateIndex = index;
                comboBox1.Enabled = false;                
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


      
  

        private string getBackdropMenuEditorVisibilityTag(string tag, int itemNo)
        {
            string currId = "6" + itemNo + "700";
            if (tag.IndexOf(currId) >= 0)
                tag = tag + "|Control.HasFocus(6070" + (itemNo - 1) + ")|Control.HasFocus(6080" + (itemNo - 1) + ")";
            return tag;
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedItem.Equals("Multiple Images"))
            {
                label14.Text = "Backdrop Path:";
                new_imagepath.Visible = true;
                comboBox16.Visible = false;
                button1.Visible = true;
            }
            else if (comboBox5.SelectedItem.Equals("Fanart Plugin"))
            {
                label14.Text = "Backdrop Id:";
                new_imagepath.Visible = false;
                comboBox16.Visible = true;
                button1.Visible = false;
            }
            else
            {
                label14.Text = "Backdrop Name:";
                new_imagepath.Visible = true;
                comboBox16.Visible = false;
                button1.Visible = true;
            }
        }

        private void new_imagepath_TextChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void textBox15_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox16_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox17_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox18_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox19_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            
        }

        private void new_imagepath_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedItem.Equals("Multiple Images"))
            {
                label4.Text = "Backdrop Path:";
                textBox3.Visible = true;
                comboBox15.Visible = false;
                button23.Visible = true;
            }
            else if (comboBox6.SelectedItem.Equals("Fanart Plugin"))
            {
                label4.Text = "Backdrop Id:";
                textBox3.Visible = false;
                comboBox15.Visible = true;
                button23.Visible = false;
            }
            else
            {
                label4.Text = "Backdrop Name:";
                textBox3.Visible = true;
                comboBox15.Visible = false;
                button23.Visible = true;
            }
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox11.SelectedItem.Equals("Multiple Images"))
            {
                label20.Text = "Backdrop Path:";
                textBox15.Visible = true;
                comboBox3.Visible = false;
                button5.Visible = true;
            }
            else if (comboBox11.SelectedItem.Equals("Fanart Plugin"))
            {
                label20.Text = "Backdrop Id:";
                textBox15.Visible = false;
                comboBox3.Visible = true;
                button5.Visible = false;
            }
            else
            {
                label20.Text = "Backdrop Name:";
                textBox15.Visible = true;
                comboBox3.Visible = false;
                button5.Visible = true;
            }            
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox10.SelectedItem.Equals("Multiple Images"))
            {
                label21.Text = "Backdrop Path:";
                textBox16.Visible = true;
                comboBox4.Visible = false;
                button9.Visible = true;
            }
            else if (comboBox10.SelectedItem.Equals("Fanart Plugin"))
            {
                label21.Text = "Backdrop Id:";
                textBox16.Visible = false;
                comboBox4.Visible = true;
                button9.Visible = false;
            }
            else
            {
                label21.Text = "Backdrop Name:";
                textBox16.Visible = true;
                comboBox4.Visible = false;
                button9.Visible = true;
            }

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox9.SelectedItem.Equals("Multiple Images"))
            {
                label22.Text = "Backdrop Path:";
                textBox17.Visible = true;
                comboBox12.Visible = false;
                button13.Visible = true;
            }
            else if (comboBox9.SelectedItem.Equals("Fanart Plugin"))
            {
                label22.Text = "Backdrop Id:";
                textBox17.Visible = false;
                comboBox12.Visible = true;
                button13.Visible = false;
            }
            else
            {
                label22.Text = "Backdrop Name:";
                textBox17.Visible = true;
                comboBox12.Visible = false;
                button13.Visible = true;
            }            
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.SelectedItem.Equals("Multiple Images"))
            {
                label23.Text = "Backdrop Path:";
                textBox18.Visible = true;
                comboBox13.Visible = false;
                button18.Visible = true;
            }
            else if (comboBox8.SelectedItem.Equals("Fanart Plugin"))
            {
                label23.Text = "Backdrop Id:";
                textBox18.Visible = false;
                comboBox13.Visible = true;
                button18.Visible = false;
            }
            else
            {
                label23.Text = "Backdrop Name:";
                textBox18.Visible = true;
                comboBox13.Visible = false;
                button18.Visible = true;
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.SelectedItem.Equals("Multiple Images"))
            {
                label24.Text = "Backdrop Path:";
                textBox19.Visible = true;
                comboBox14.Visible = false;
                button19.Visible = true;
            }
            else if (comboBox7.SelectedItem.Equals("Fanart Plugin"))
            {
                label24.Text = "Backdrop Id:";
                textBox19.Visible = false;
                comboBox14.Visible = true;
                button19.Visible = false;
            }
            else
            {
                label24.Text = "Backdrop Name:";
                textBox19.Visible = true;
                comboBox14.Visible = false;
                button19.Visible = true;
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox18_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox12_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox18_Enter_1(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox5.SelectedItem.Equals("Multiple Images"))
            {
                FolderBrowserDialog openFD = new FolderBrowserDialog();
                openFD.Description = "Select Thumbnail Picture";
                openFD.SelectedPath = new_imagepath.Text;
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    new_imagepath.Text = openFD.SelectedPath;
                }
            }
            else if (comboBox5.SelectedItem.Equals("Fanart Plugin"))
            {
                new_imagepath.Visible = false;
                comboBox16.Visible = true;
            }
            else
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.InitialDirectory = Config.GetFolder(Config.Dir.Skin) + @"\Backdrop\Media";
                openFD.Title = "Select Thumbnail Picture";
                openFD.FileName = new_imagepath.Text;
                openFD.Filter = "PNG Images|*.png";
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    new_imagepath.Text = openFD.FileName;
                }
            }
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox11.SelectedItem.Equals("Multiple Images"))
            {
                FolderBrowserDialog openFD = new FolderBrowserDialog();
                openFD.Description = "Select Thumbnail Picture";
                openFD.SelectedPath = textBox15.Text;
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox15.Text = openFD.SelectedPath;
                }
            }
            else if (comboBox11.SelectedItem.Equals("Fanart Plugin"))
            {
                textBox15.Visible = false;
                comboBox3.Visible = true;
            }
            else
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.InitialDirectory = Config.GetFolder(Config.Dir.Skin) + @"\Backdrop\Media";
                openFD.Title = "Select Thumbnail Picture";
                openFD.FileName = textBox15.Text;
                openFD.Filter = "PNG Images|*.png";
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox15.Text = openFD.FileName;
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (comboBox6.SelectedItem.Equals("Multiple Images"))
            {
                FolderBrowserDialog openFD = new FolderBrowserDialog();
                openFD.Description = "Select Thumbnail Picture";
                openFD.SelectedPath = textBox3.Text;
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox3.Text = openFD.SelectedPath;
                }
            }
            else if (comboBox6.SelectedItem.Equals("Fanart Plugin"))
            {
                textBox3.Visible = false;
                comboBox15.Visible = true;
            }
            else
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.InitialDirectory = Config.GetFolder(Config.Dir.Skin) + @"\Backdrop\Media";
                openFD.Title = "Select Thumbnail Picture";
                openFD.FileName = textBox3.Text;
                openFD.Filter = "PNG Images|*.png";
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox3.Text = openFD.FileName;
                }
            }
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            if (comboBox7.SelectedItem.Equals("Multiple Images"))
            {
                FolderBrowserDialog openFD = new FolderBrowserDialog();
                openFD.Description = "Select Thumbnail Picture";
                openFD.SelectedPath = textBox19.Text;
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox19.Text = openFD.SelectedPath;
                }
            }
            else if (comboBox7.SelectedItem.Equals("Fanart Plugin"))
            {
                textBox19.Visible = false;
                comboBox14.Visible = true;
            }
            else
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.InitialDirectory = Config.GetFolder(Config.Dir.Skin) + @"\Backdrop\Media";
                openFD.Title = "Select Thumbnail Picture";
                openFD.FileName = textBox19.Text;
                openFD.Filter = "PNG Images|*.png";
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox19.Text = openFD.FileName;
                }
            }
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            if (comboBox8.SelectedItem.Equals("Multiple Images"))
            {
                FolderBrowserDialog openFD = new FolderBrowserDialog();
                openFD.Description = "Select Thumbnail Picture";
                openFD.SelectedPath = textBox18.Text;
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox18.Text = openFD.SelectedPath;
                }
            }
            else if (comboBox8.SelectedItem.Equals("Fanart Plugin"))
            {
                textBox18.Visible = false;
                comboBox13.Visible = true;
            }
            else
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.InitialDirectory = Config.GetFolder(Config.Dir.Skin) + @"\Backdrop\Media";
                openFD.Title = "Select Thumbnail Picture";
                openFD.FileName = textBox18.Text;
                openFD.Filter = "PNG Images|*.png";
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox18.Text = openFD.FileName;
                }
            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (comboBox9.SelectedItem.Equals("Multiple Images"))
            {
                FolderBrowserDialog openFD = new FolderBrowserDialog();
                openFD.Description = "Select Thumbnail Picture";
                openFD.SelectedPath = textBox17.Text;
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox17.Text = openFD.SelectedPath;
                }
            }
            else if (comboBox9.SelectedItem.Equals("Fanart Plugin"))
            {
                textBox17.Visible = false;
                comboBox12.Visible = true;
            }
            else
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.InitialDirectory = Config.GetFolder(Config.Dir.Skin) + @"\Backdrop\Media";
                openFD.Title = "Select Thumbnail Picture";
                openFD.FileName = textBox17.Text;
                openFD.Filter = "PNG Images|*.png";
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox17.Text = openFD.FileName;
                }
            }
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox10.SelectedItem.Equals("Multiple Images"))
            {
                FolderBrowserDialog openFD = new FolderBrowserDialog();
                openFD.Description = "Select Thumbnail Picture";
                openFD.SelectedPath = textBox16.Text;
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox16.Text = openFD.SelectedPath;
                }
            }
            else if (comboBox10.SelectedItem.Equals("Fanart Plugin"))
            {
                textBox16.Visible = false;
                comboBox4.Visible = true;
            }
            else
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.InitialDirectory = Config.GetFolder(Config.Dir.Skin) + @"\Backdrop\Media";
                openFD.Title = "Select Thumbnail Picture";
                openFD.FileName = textBox16.Text;
                openFD.Filter = "PNG Images|*.png";
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                }
                else
                {
                    textBox16.Text = openFD.FileName;
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                useMenu2 = "True";
                //tabControl12.TabPages[2].Enabled = true;
            }
            else
            {
                useMenu2 = "False";
                //useMenu3 = "False";
                //tabControl12.TabPages[2].Enabled = false;
                //checkBox3.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                useMenu1 = "True";
           //     tabControl12.TabPages[1].Enabled = true;
            }
            else
            {
                useMenu1 = "False";
             //   useMenu2 = "False";
             //   tabControl12.TabPages[1].Enabled = false;
             //   checkBox1.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                useMenu3 = "True";
               // tabControl12.TabPages[3].Enabled = true;
            }
            else
            {
                useMenu3 = "False";
                //useMenu4 = "False";
                //tabControl12.TabPages[3].Enabled = false;
                //checkBox4.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                useMenu4 = "True";
                //tabControl12.TabPages[4].Enabled = true;
            }
            else
            {
                useMenu4 = "False";
                //useMenu5 = "False";
                //tabControl12.TabPages[4].Enabled = false;
                //checkBox5.Checked = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                useMenu5 = "True";
                //tabControl12.TabPages[5].Enabled = true;
            }
            else
            {
                useMenu5 = "False";
                //useMenu6 = "False";
                //tabControl12.TabPages[5].Enabled = false;
                //checkBox6.Checked = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                useMenu6 = "True";
            }
            else
            {
                useMenu6 = "False";
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                doCenterMenu = "True";
            }
            else
            {
                doCenterMenu = "False";
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                doHideClock = "True";
            }
            else
            {
                doHideClock = "False";
            }
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void comboBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox19.SelectedIndex = -1;
            HandleMovingPictureSubCategories();
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void comboBox21_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox20.SelectedIndex = -1;
            comboBox20.Items.Clear();
            if (comboBox21.SelectedItem != null)
            {
                ArrayList al = null;
                try
                {
                    string filename = Path.Combine(Path.Combine(Config.GetFolder(Configuration.Config.Dir.Plugins), @"windows\OnlineVideos"), "OnlineVideos.dll");
                    if (IsAssemblyAvailable("OnlineVideos", new Version(0, 27, 0, 0), filename))
                    {
                        al = OnlineVideoHandler.GetCategories(comboBox21.SelectedItem.ToString());
                    }                    
                }
                catch { }
                foreach (string s in al)
                {
                    comboBox20.Items.Add(s);
                }
                if (al != null)
                {
                    al.Clear();
                }
                al = null;
            }
        }




    }

    public class menuItem
    {
        public string hyperlink;
        public string imagePath;
        public string name;
        public string useLatestMedia;
        public int id;
        public string mainCategory;
        public string subCategory;
    }

    public class bgItem
    {
        public int id;
        public string imagePath;
    }

}









