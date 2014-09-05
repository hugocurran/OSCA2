using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSCASnapin.ConfigManager;

namespace OSCASnapin.ConfigManager
{
    public partial class CAPropertiesControl : UserControl
    {
        private enum privRole
        {
            user,
            admin
        }

        private Configuration config;
        private CAPropertyPage propertyPage;
        

        public CAPropertiesControl(CAPropertyPage PropertyPage)
        {
            propertyPage = PropertyPage;
            config = Configuration.Instance;

            InitializeComponent();
        }

        /// <summary>
        /// Populate control from the Configuration object
        /// </summary>
        public void RefreshData(CANode caNode)
        {
            initPriv(privRole.admin);
            initPriv(privRole.user);
            propertyPage.Dirty = false;
        }

        /// <summary>
        /// Update the node with the controls values
        /// </summary>
        /// <param name="scopeNode">Node being updated by property page</param>
        public void UpdateData(CANode caNode)
        {
            propertyPage.Dirty = false;
        }

        /// <summary>
        /// Check during UserProptertyPage.OnApply to ensure that changes can be Applied
        /// </summary>
        /// <returns>returns true if changes are valid</returns>
        public bool CanApplyChanges()
        {
            return true;
        }

        /// <summary>
        /// Notifies the PropertyPage that info has changed and that the PropertySheet can change the 
        /// buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayName_TextChanged(object sender, System.EventArgs e)
        {
            propertyPage.Dirty = true;
        }

        #region Privileges Tab

        /// <summary>
        /// Setup the initial privileges
        /// </summary>
        /// <param name="role"></param>
        private void initPriv(privRole role)
        {
            switch (role)
            {
                case privRole.user:

                    CheckedListBox.ObjectCollection uitem = clbUserPriv.Items;
                    int uindex = 0;
                    foreach (KeyValuePair<string, bool> entry in config.InitData.UserPriv)
                    {
                        uitem.Insert(uindex, entry.Key);
                        clbUserPriv.SetItemChecked(uindex, entry.Value);
                        uindex++;
                    }
                    break;

                case privRole.admin:
                    CheckedListBox.ObjectCollection aitem = clbAdminPriv.Items;
                    int aindex = 0;
                    foreach (KeyValuePair<string, bool> entry in config.InitData.AdminPriv)
                    {
                        aitem.Insert(aindex, entry.Key);
                        clbAdminPriv.SetItemChecked(aindex, entry.Value);
                        aindex++;
                    }
                    break;
            }
        }

        /// <summary>
        /// Notifies the PropertyPage that info has changed and that the PropertySheet can change the 
        /// buttons
        /// </summary>
        private void ItemCheck(object sender, ItemCheckEventArgs e)
        {
            propertyPage.Dirty = true;
        }



        #endregion

        #region Apply

        private void butApply_Click(object sender, EventArgs e)
        {
            // First update the CA policy

            // Second do the privilege map
            int index = 0;
            foreach (object item in clbUserPriv.Items)
            {
                config.InitData.UserPriv[item.ToString()] = clbUserPriv.GetItemChecked(index);
                index++;
            }
            index = 0;
            foreach (object item in clbAdminPriv.Items)
            {
                config.InitData.AdminPriv[item.ToString()] = clbAdminPriv.GetItemChecked(index);
                index++;
            }

            // Thirdly apply the policy to the privilege map


        }

        #endregion

    }
}
