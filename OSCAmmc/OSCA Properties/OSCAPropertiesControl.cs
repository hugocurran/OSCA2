using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OSCASnapin.ConfigManager;

namespace OSCASnapin
{
    public partial class OSCAPropertiesControl : UserControl
    {
        private enum privRole
        {
            user,
            admin
        }

        private Configuration config;
        private OSCAPropertyPage propertyPage;
        

        public OSCAPropertiesControl(OSCAPropertyPage PropertyPage, Mode Mode)
        {
            propertyPage = PropertyPage;
            config = Configuration.Instance;

            InitializeComponent();

            if (Mode == Mode.AdminMode)
            {
                clbAdminPriv.Enabled = true;
                clbUserPriv.Enabled = true;
            }
            else
            {
                clbAdminPriv.Enabled = false;
                clbUserPriv.Enabled = false;
            }


        }

        /// <summary>
        /// Populate control from the Configuration object
        /// </summary>
        public void RefreshData(InitialisationData initData)
        {
            initPriv(privRole.admin, initData);
            initPriv(privRole.user, initData);
            propertyPage.Dirty = false;
        }

        /// <summary>
        /// Update the node with the controls values
        /// </summary>
        /// <param name="scopeNode">Node being updated by property page</param>
        public void UpdateData(InitialisationData initData)
        {
            int index = 0;
            foreach (object item in clbUserPriv.Items)
            {
                initData.UserPriv[item.ToString()] = clbUserPriv.GetItemChecked(index);
                index++;
            }
            index = 0;
            foreach (object item in clbAdminPriv.Items)
            {
                initData.AdminPriv[item.ToString()] = clbAdminPriv.GetItemChecked(index);
                index++;
            }

            propertyPage.Dirty = false;
        }

        /// <summary>
        /// Check during UserProptertyPage.OnApply to ensure that changes can be Applied
        /// </summary>
        /// <returns>returns true if changes are valid</returns>
        public bool CanApplyChanges()
        {
            //bool result = false;

            //if (DisplayName.Text.Trim().Length == 0)
            //{
            //    MessageBoxParameters messageBoxParameters = new MessageBoxParameters();
            //    messageBoxParameters.Text = "Display Name cannot be blank";
            //    scopePropertyPage.ParentSheet.ShowDialog(messageBoxParameters);
            //
            //    // MessageBox.Show("Display Name cannot be blank");
            //}
            //else
            //{
            //    result = true;
            //}
            //return result;
            return true;
        }


        #region Privileges Tab

        /// <summary>
        /// Setup the initial privileges
        /// </summary>
        /// <param name="role"></param>
        private void initPriv(privRole role, InitialisationData initData)
        {
            switch (role)
            {
                case privRole.user:

                    CheckedListBox.ObjectCollection uitem = clbUserPriv.Items;
                    int uindex = 0;
                    foreach (KeyValuePair<string, bool> entry in initData.UserPriv)
                    {
                        uitem.Insert(uindex, entry.Key);
                        clbUserPriv.SetItemChecked(uindex, entry.Value);
                        uindex++;
                    }
                    break;

                case privRole.admin:
                    CheckedListBox.ObjectCollection aitem = clbAdminPriv.Items;
                    int aindex = 0;
                    foreach (KeyValuePair<string, bool> entry in initData.AdminPriv)
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

    }
}
