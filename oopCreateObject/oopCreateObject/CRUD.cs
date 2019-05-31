using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace oopCreateObject
{
    public interface IFORCRUD
    {
        Form CreateForm(Object element, List<Object> elements, bool flag);
        void ElementDelete(Object element, List<Object> elements);
    }

    public class CRUD: IFORCRUD
    {
        private readonly Color blue;

        public void ElementDelete(Object item, List<Object> items)
        {
            //objects with other object
            var ownerList = items.Where(itm => (itm.GetType()
            .GetProperties()
            .Where(fld => ((fld.PropertyType == item.GetType() || fld.PropertyType.BaseType == item.GetType())))).ToList().Count > 0).ToList();

            foreach (var owner in ownerList)
            {
                foreach (var fld in owner.GetType().GetProperties().Where(fld => (fld.PropertyType == item.GetType())).ToList())
                {
                    if ((fld.GetValue(owner) != null) && (fld.GetValue(owner).Equals(item)))
                    {
                        fld.SetValue(owner, null);
                    }
                }
            }
            items.Remove(item);
        }

        private void TrySetValue(PropertyInfo propertyInfo, object item, object value)
        {
            var buf = propertyInfo.GetValue(item);
            try
            {
                propertyInfo.SetValue(item, Convert.ChangeType(value, propertyInfo.PropertyType));
            }
            catch
            {
                propertyInfo.SetValue(item, buf);
                MessageBox.Show(propertyInfo.Name + ": Incorrect field value");
            }
        }

        public Form CreateForm(Object element, List<Object> elements, bool flag)
        {
            const int formWidth = 400;
            const int width = 90;
            const int height = 20;

            PropertyInfo[] fields = element.GetType().GetProperties();

            // for form
            Form form = new Form
            {
                Text = string.Concat("Add new organ ", element.GetType().ToString()),
                BackColor = blue,
                Size = new Size(formWidth, height * (fields.Length + 6)),
            };

            // for all fields
            for (int i = 0; i < fields.Length; i++)
            {
                Label label = new Label
                {
                    Text = string.Concat(" ", fields[i].PropertyType.Name, " "),
                    Width = width,
                    Height = height,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(15, height * (i + 1)),
                };
                form.Controls.Add(label);

                Type fieldType = fields[i].PropertyType;
                // for bool fields
                if (((fieldType.IsPrimitive) && (!fieldType.IsEnum)) || (fieldType == typeof(string)))
                {
                    if (fieldType == typeof(bool))
                    {
                        RadioButton radioButton = new RadioButton
                        {
                            Name = string.Concat("", fields[i].Name.ToString()),
                            Text = "Yes",
                            Width = width,
                            Height = height,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Location = new Point(15 + label.Width, height * (i + 1)),
                            Checked = (bool)fields[i].GetValue(element)
                        };
                        form.Controls.Add(radioButton);
                    }
                    // for string fields
                    else
                    {
                        TextBox text = new TextBox
                        {
                            Name = string.Concat("", fields[i].Name.ToString()),  
                            Width = width,
                            Text = fields[i].GetValue(element).ToString(),
                            Location = new Point(15 + label.Width, height * (i + 1)),
                        };
                        form.Controls.Add(text);
                    }
                }
                // for enum fields
                else if (fields[i].PropertyType.IsEnum)
                {
                    ComboBox combobox = CreateComboBox(fields[i].Name,
                                                        new Point(15 + label.Width, height * (i + 1)),
                                                        width,
                                                        fields[i].PropertyType.GetEnumNames(),
                                                        (int)(fields[i].GetValue(element)));
                    form.Controls.Add(combobox);

                }
                // for enum fields
                else if ((fields[i].PropertyType.IsClass))
                {
                    ComboBox combobox = CreateComboBox(fields[i].Name, new Point(15 + label.Width, height * (i + 1)),
                                                       width,
                                                       fields[i].PropertyType, fields[i].GetValue(element),elements);
                    form.Controls.Add(combobox);
                }
            };

            Button btn = new Button
            {
                Text = "OK",
                Location = new Point(form.Width / 2 - form.Width / 9 * 2, (fields.Length + 2) * height),
                Width = form.Width / 5,
                DialogResult = DialogResult.OK,
            };

            EventHandler eventAdd = (object sender, EventArgs e) =>
            {
                Button button = (Button)sender;
                Form formEvent = button.FindForm();

                if ((formEvent == null) || (form == null) || (element == null) || (elements == null))
                    return;
                else
                {
                    foreach (var property in form.Controls.OfType<TextBox>().ToList())
                    {
                        if (fields.ToList().Where(field => field.Name == property.Name).Count() != 0)
                        {
                            PropertyInfo fi = fields.ToList().Where(field => field.Name == property.Name).First();
                            TrySetValue(fi, element, property.Text);
                        }
                    }

                    foreach (var property in form.Controls.OfType<RadioButton>().ToList())
                    {
                        if (fields.ToList().Where(field => field.Name == property.Name).Count() != 0)
                        {
                            PropertyInfo fi = fields.ToList().Where(field => field.Name == property.Name).First();
                            TrySetValue(fi, element, property.Checked);
                        }
                    }

                    //enum
                    foreach (var property in form.Controls.OfType<ComboBox>().ToList())
                    {
                        if (fields.ToList().Where(field => field.Name == property.Name).Count() != 0)
                        {
                            PropertyInfo fi = fields.ToList().Where(field => field.Name == property.Name).First();
                            var buf = fi.GetValue(element);

                            if (property.SelectedIndex == -1)
                                continue;

                            if (fi.PropertyType.IsEnum)
                            {
                                try
                                {
                                    fi.SetValue(element, property.SelectedIndex);
                                }
                                catch
                                {
                                    fi.SetValue(element, buf);
                                    MessageBox.Show(fi.Name + ": Incorrect field value");
                                }
                            }
                            else
                            {
                                List<object> suitableItems = elements.Where(sitem => ((sitem.GetType() == fi.PropertyType) || (sitem.GetType().BaseType == fi.PropertyType))).ToList();
                                try
                                {
                                    fi.SetValue(element, suitableItems[property.SelectedIndex]);
                                }
                                catch
                                {
                                    fi.SetValue(element, buf);
                                    MessageBox.Show(fi.Name + ": Incorrect field value");
                                }
                            }
                        }
                    }
                }
            };

            btn.Click += eventAdd;
            form.Controls.Add(btn);

            return form;
        }

        private ComboBox CreateComboBox(string name, Point point, int width, string[] values, int currentValue)
        {
            ComboBox combobox = new ComboBox();
            combobox.Name = name;
            combobox.SelectionStart = 0;
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.Location = point;
            combobox.Width = width;
            combobox.Items.AddRange(values);
            combobox.SelectedIndex = currentValue;
            return combobox;
        }

        // for deep object
        private ComboBox CreateComboBox(string name, Point point, int width, Type itemType, Object currentItem, List<Object> allItems)
        {
            ComboBox combobox = new ComboBox();
            combobox.Name = name;
            combobox.SelectionStart = 0;
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.Location = point;
            combobox.Width = width;

            List<object> suitableItems = allItems.Where(s_item => ((s_item.GetType() == itemType) || (s_item.GetType().BaseType == itemType))).ToList();
            for (int j = 0; j < suitableItems.Count; j++)
            {
                combobox.Items.Add(suitableItems[j].ToString());
            }

            int index = -1;
            if (currentItem != null)
            {
                for (int j = 0; j < suitableItems.Count; j++)
                {
                    if (currentItem.Equals(suitableItems[j]))
                    {
                        index = j;
                        break;
                    }
                }
                combobox.SelectedIndex = index;
            }
            combobox.SelectedIndex = index;
            return combobox;
        }
    }
}
