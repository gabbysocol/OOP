using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

// ctrl+k ctrl+c
// ctrl+k ctrl+u

namespace oopCreateObject
{
    public partial class FormMain : Form
    {
        public Form FormEdit = null;
        public List<object> listOrgans = new List<object>();
        public List<Type> listBase = new List<Type>();
        public IFORCRUD IForCrud = null;
        public static bool flag = false;

        public FormMain(List<Type> typesOrgan, IFORCRUD fORCRUD)
        {
            InitializeComponent();
            listBase = typesOrgan;
            IForCrud = fORCRUD;

            // initialization list
            listOrgans.AddRange(new List<object>
            {
                new Liver() { },
                new Spleen() { },
                new Brains() { },
                new Bonebrains() { },
                new Heart() { },
                new Lungs() { },
                new Nose() { }
            });
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            foreach (var element in listBase)
            {
                string typeString = element.Name;

                if (element.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                {
                    typeString = displayNameAttribute.DisplayName;
                }

                cmbxClassCreate.Items.Add(typeString);
            }
            cmbxClassCreate.SelectedIndex = 0;
            cmbxClassCreate.DropDownStyle = ComboBoxStyle.DropDownList;
            listViewObjects.View = View.Details;
            ListOrganShow(listViewObjects, listOrgans);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ConstructorInfo constructorInfo = listBase[cmbxClassCreate.SelectedIndex].GetConstructor(new Type[] { });
            object element = constructorInfo.Invoke(new object[] { });
            listOrgans.Add(element);

            if (element != null)
            {
                FormEdit = IForCrud.CreateForm(element, listOrgans, flag);

                FormEdit.ShowDialog();
                FormEdit.Dispose();
            }               

            ListOrganShow(listViewObjects, listOrgans);
        }

        public void ListOrganShow(Object sender, List<Object> organs)
        {
            ListView listView = (ListView)sender;
            listView.Clear();
            listView.Columns.Add("Organ", 200);
            listView.Columns.Add("Owner", 200);
            listView.Columns.Add("Days", 60);

            for (int i = 0; i < organs.Count; i++)
            {
                Type itemType = organs[i].GetType();
                object name = organs[i].ToString();
                var temp = (BiologicalSystem) organs[i];

                string typeStringOwner = itemType.Name;

                if (itemType.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                {
                    typeStringOwner = displayNameAttribute.DisplayName;
                }

                var listItem = new ListViewItem(new string[] { typeStringOwner, name.ToString(), temp.GetDays() });
                listView.Items.Add(listItem);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            object element = null;
            if (listViewObjects.SelectedIndices.Count > 0)
                element = listOrgans[listViewObjects.SelectedIndices[0]];

            if (element != null)
            {
                FormEdit = IForCrud.CreateForm(element, listOrgans, flag);

                FormEdit.ShowDialog();
                FormEdit.Dispose();
            }

            ListOrganShow(listViewObjects, listOrgans);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            object element = null;
            if (listViewObjects.SelectedIndices.Count > 0)
                element = listOrgans[listViewObjects.SelectedIndices[0]];

            if (element != null)
            {
                IForCrud.ElementDelete(element, listOrgans);
                ListOrganShow(listViewObjects, listOrgans);
            }
        }
    }
}