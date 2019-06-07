using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Data;
using IRealiseCrypto;

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
        private string cipherKey;
        public static bool flag = false;
        private List<ISerializer> ListSerializers = new List<ISerializer>()
        {
            new BinarySerializer(),
            new JSONSerializer(),
            new TXTSerializer()
        };

        private readonly string pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        private List<ICrypto> Plugins = new List<ICrypto>();

        public FormMain(List<Type> typesOrgan, IFORCRUD fORCRUD)
        {
            InitializeComponent();
            listBase = typesOrgan;
            IForCrud = fORCRUD;
            cipherKey = "751003OBJECTOP";

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

            foreach (var item in ListSerializers)
            {
                string typeString = item.GetType().Name;

                if (item.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    typeString = displayNameAttribute.DisplayName;

                comboBoxChooseSerializer.Items.Add(typeString);
            }

            if (comboBoxChooseSerializer.Items.Count != 0)
                comboBoxChooseSerializer.SelectedIndex = 0;

            comboBoxChooseSerializer.DropDownStyle = ComboBoxStyle.DropDownList;

            LoadPlugins(Plugins);
            foreach (var item in Plugins)
            {
                string typeString = item.GetType().Name;

                if (item.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    typeString = displayNameAttribute.DisplayName;
                comboBoxPlugin.Items.Add(typeString);
            }
            comboBoxPlugin.SelectedIndex = 0;
            comboBoxPlugin.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void LoadPlugins(List<ICrypto> pluginList)
        {
            var pluginsPath = "Plugins";
            var files = Directory.GetFiles(pluginsPath, "*.dll");

            foreach (var file in files)
            {
                Assembly assembly;

                try { assembly = Assembly.LoadFrom(file); }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    continue;
                }

                var types = assembly.GetTypes()
                    .Where(type => type.IsClass && type.GetInterface(nameof(ICrypto)) != null);

                foreach (var type in types)
                {
                    var plugin = Activator.CreateInstance(type) as ICrypto;
                    if (plugin == null)
                    {
                        Console.WriteLine("Null");
                        continue;
                    }
                    pluginList.Add(plugin);
                }
            }
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

        private string ChooseFileSave()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"D:\";
            saveFileDialog.ShowDialog();
            return saveFileDialog.FileName;
        }


/// CHANGE  save + !!!!!!!!!!!!!!!!!!!
///         Load +
/// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string destinationFileName = ChooseFileSave();
            ICrypto coder = (comboBoxPlugin.SelectedIndex != -1) ? Plugins[comboBoxPlugin.SelectedIndex] : null;

            if ((coder != null) && MessageBox.Show("Code?", 
                                  "Save?", 
                                  MessageBoxButtons.YesNo,
                                  MessageBoxIcon.Question) == DialogResult.Yes)

                coder = Plugins[comboBoxPlugin.SelectedIndex];
            else
                coder = null;

            ISerializer serializer = ListSerializers[comboBoxChooseSerializer.SelectedIndex];
            if (destinationFileName.Length == 0)
            {
                if (serializer.FileExtension != Path.GetExtension(destinationFileName))
                {
                    MessageBox.Show("You Not Choose File!", 
                    "Attention", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information, 
                    MessageBoxDefaultButton.Button1, 
                    MessageBoxOptions.DefaultDesktopOnly);
                   
                    return;
                }
            }
            else
            {
                if (serializer.FileExtension != Path.GetExtension(destinationFileName))
                {
                    destinationFileName += serializer.FileExtension;
                }
                if ((coder != null) && coder.Expansion != Path.GetExtension(destinationFileName))
                {
                    destinationFileName += coder.Expansion;
                }
            }

            if ((coder != null))
            {
                using (FileStream ms = new FileStream("temp.txt", FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                {
                    ms.SetLength(0);
                    serializer.Serialize(listOrgans, ms);
                }
                using (FileStream ms = new FileStream("temp.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (FileStream fs = new FileStream(destinationFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fs.SetLength(0);
                    coder.EncryptStream(ms, fs, cipherKey);
                }
            }
            else
            {
                using (FileStream file = new FileStream(destinationFileName, FileMode.OpenOrCreate))
                    serializer.Serialize(listOrgans, file);
            }

            //serializer.Serialize(listOrgans, destinationFileName);
            ListOrganShow(listViewObjects, listOrgans);   
        }

        private ISerializer ChooseSerializer(string ext)
        {
            foreach (var serializer in ListSerializers)
            {
                if (ext == serializer.FileExtension)
                    return serializer;
            }
            return null;
        }

        private ICrypto ChooseDecoder(string ext)
        {
            foreach (var encoder in Plugins)
            {
                if (ext == encoder.Expansion)
                    return encoder;
            }
            return null;
        }

        private string ChooseFileLoad()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"D:\";
            openFileDialog.ShowDialog();
            return openFileDialog.FileName;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string destinationFileName = ChooseFileLoad();
            ISerializer serializer;

            ICrypto coder = null;
            // for usual serialization
            if (destinationFileName.Length == 0)
            {
                MessageBox.Show("You Not Choose File!",
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            else
            {
                coder = ChooseDecoder(Path.GetExtension(destinationFileName));
                serializer = ChooseSerializer(Path.GetExtension(destinationFileName));

                if (coder != null)
                {
                    serializer = ChooseSerializer(Path.GetExtension(destinationFileName.Replace(coder.Expansion, "")));
                }

                if (serializer == null)
                {
                    MessageBox.Show("Error extension");
                    return;
                }

                serializer = ChooseSerializer(Path.GetExtension(destinationFileName));
                if (serializer == null)
                {
                    MessageBox.Show("Error extension");
                    return;
                }
            }

            // for decode
            if (coder != null)
            {
                using (FileStream ms = new FileStream("temp.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (FileStream fs = new FileStream(destinationFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ms.SetLength(0);
                    coder.DecryptStream(fs, ms, cipherKey);
                }
                using (FileStream ms = new FileStream("temp.txt", FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                {
                    listOrgans = (List<Object>)serializer.Deserialize(ms);
                }
            }
            else
            {
                using (FileStream file = new FileStream(destinationFileName, FileMode.OpenOrCreate))
                    listOrgans = (List<Object>)serializer.Deserialize(file);
            }

            //listOrgans = (List<Object>)serializer.Deserialize(destinationFileName);
            ListOrganShow(listViewObjects, listOrgans);
        }
    }
}