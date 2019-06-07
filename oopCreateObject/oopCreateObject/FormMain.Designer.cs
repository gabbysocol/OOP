namespace oopCreateObject
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnAdd = new System.Windows.Forms.Button();
            this.cmbxClassCreate = new System.Windows.Forms.ComboBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.listViewObjects = new System.Windows.Forms.ListView();
            this.comboBoxChooseSerializer = new System.Windows.Forms.ComboBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.comboBoxPlugin = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(51, 39);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(56, 43);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add New";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cmbxClassCreate
            // 
            this.cmbxClassCreate.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.cmbxClassCreate.FormattingEnabled = true;
            this.cmbxClassCreate.Location = new System.Drawing.Point(12, 12);
            this.cmbxClassCreate.MaxDropDownItems = 7;
            this.cmbxClassCreate.Name = "cmbxClassCreate";
            this.cmbxClassCreate.Size = new System.Drawing.Size(121, 21);
            this.cmbxClassCreate.TabIndex = 1;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(51, 88);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(56, 43);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnDelete.Location = new System.Drawing.Point(51, 137);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(56, 43);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // listViewObjects
            // 
            this.listViewObjects.LabelEdit = true;
            this.listViewObjects.Location = new System.Drawing.Point(163, 12);
            this.listViewObjects.MultiSelect = false;
            this.listViewObjects.Name = "listViewObjects";
            this.listViewObjects.Size = new System.Drawing.Size(460, 283);
            this.listViewObjects.TabIndex = 4;
            this.listViewObjects.UseCompatibleStateImageBehavior = false;
            // 
            // comboBoxChooseSerializer
            // 
            this.comboBoxChooseSerializer.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.comboBoxChooseSerializer.FormattingEnabled = true;
            this.comboBoxChooseSerializer.Location = new System.Drawing.Point(12, 186);
            this.comboBoxChooseSerializer.MaxDropDownItems = 7;
            this.comboBoxChooseSerializer.Name = "comboBoxChooseSerializer";
            this.comboBoxChooseSerializer.Size = new System.Drawing.Size(121, 21);
            this.comboBoxChooseSerializer.TabIndex = 5;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(12, 213);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(56, 43);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(77, 213);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(56, 43);
            this.buttonLoad.TabIndex = 7;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // comboBoxPlugin
            // 
            this.comboBoxPlugin.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.comboBoxPlugin.FormattingEnabled = true;
            this.comboBoxPlugin.Location = new System.Drawing.Point(12, 262);
            this.comboBoxPlugin.MaxDropDownItems = 7;
            this.comboBoxPlugin.Name = "comboBoxPlugin";
            this.comboBoxPlugin.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPlugin.TabIndex = 8;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(635, 295);
            this.Controls.Add(this.comboBoxPlugin);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.comboBoxChooseSerializer);
            this.Controls.Add(this.listViewObjects);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.cmbxClassCreate);
            this.Controls.Add(this.btnAdd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "CreateObjects";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cmbxClassCreate;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListView listViewObjects;
        private System.Windows.Forms.ComboBox comboBoxChooseSerializer;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.ComboBox comboBoxPlugin;
    }
}

