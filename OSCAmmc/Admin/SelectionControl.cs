using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.ManagementConsole;

namespace OSCASnapin
{
   // <summary>
    /// The selection control class.
    /// </summary>
    public partial class SelectionControl : UserControl, IFormViewControl
    {
        AdminProperties adminProperties = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public SelectionControl()
        {
            // Initialize the control.
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            //Set up the list.
            UserListView.View = System.Windows.Forms.View.Details;

            ColumnHeader userColumnHeader = new ColumnHeader();
            userColumnHeader.Text = "User";
            userColumnHeader.Width = 200;
            UserListView.Columns.Add(userColumnHeader);

            ColumnHeader birthdayColumnHeader = new ColumnHeader();
            birthdayColumnHeader.Text = "BirthDay";
            birthdayColumnHeader.Width = 200;
            UserListView.Columns.Add(birthdayColumnHeader);
        }
        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="parentSelectionFormView"></param>
        void IFormViewControl.Initialize(FormView parentSelectionFormView)
        {
            adminProperties = (AdminProperties)parentSelectionFormView;

            // Add the actions
            adminProperties.SelectionData.ActionsPaneItems.Clear();
            adminProperties.SelectionData.ActionsPaneItems.Add(new Microsoft.ManagementConsole.Action("Show Selection", 
                "Shows the Names of the selected Items in the FormView's ListView.", -1, "ShowSelection"));
        }
        /// <summary>
        /// Populate the list with the sample data.
        /// </summary>
        /// <param name="users"></param>
        public void RefreshData(string[][] users)
        {
            // Clear the list.
            UserListView.Items.Clear();

            // Populate the list using the sample data.
            foreach (string[] user in users)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = user[0];
                listViewItem.SubItems.Add(user[1]);
                UserListView.Items.Add(listViewItem);
            }
        }
        /// <summary>
        /// Show the selected items.
        /// </summary>
        public void ShowSelection()
        {
            if (UserListView.SelectedItems == null)
            {
                MessageBox.Show("There are no items selected");
            }
            else
            {
                MessageBox.Show("Selected Users: \n" + GetSelectedUsers());
            }
        }
        /// <summary>
        /// Update the context.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UserListView.SelectedItems.Count == 0)
            {
                adminProperties.SelectionData.Clear();
            }
            else
            {
                // Update MMC with the current selection information
                adminProperties.SelectionData.Update(GetSelectedUsers(), UserListView.SelectedItems.Count > 1, null, null);

                // Update the title of the selected data menu in the actions pane
                adminProperties.SelectionData.DisplayName = ((UserListView.SelectedItems.Count == 1) ? UserListView.SelectedItems[0].Text : "Selected Objects");
            }
        }
        /// <summary>
        /// Build a string of selected users.
        /// </summary>
        /// <returns></returns>
        private string GetSelectedUsers()
        {
            StringBuilder selectedUsers = new StringBuilder();

            foreach (ListViewItem listViewItem in UserListView.SelectedItems)
            {
                selectedUsers.Append(listViewItem.Text + "\n");
            }

            return selectedUsers.ToString();
        }
        /// <summary>
        /// Handle mouse clicks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Check selected items for a right-click.
                bool rightClickedOnSelection = false;

                ListViewItem rightClickedItem = UserListView.GetItemAt(e.X, e.Y);
                if (rightClickedItem == null || rightClickedItem.Selected == false)
                {
                    rightClickedOnSelection = false;
                }
                else
                {
                    rightClickedOnSelection = true;
                }

                // Show the context menu.
                adminProperties.ShowContextMenu(PointToScreen(e.Location), rightClickedOnSelection);
            }
        }
    }
}
