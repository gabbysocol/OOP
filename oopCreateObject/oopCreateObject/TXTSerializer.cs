using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;

namespace oopCreateObject
{
    class TXTSerializer : ISerializer
    {
        public string FileExtension { get; } = ".txt";
        public TXTSerializer()
        {
        }

        public void Serialize(Object itemList, Stream fileName)
        {
            string objInfo = String.Empty;
            TXTSerializerFormatter textFormatter = new TXTSerializerFormatter();
            objInfo = textFormatter.GetObjectInfo(itemList);
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.Write(objInfo);
            }
        }

        public Object Deserialize(Stream fileName)
        {
            string obj = String.Empty;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                obj = streamReader.ReadToEnd();
            }
            TXTDeserializerFormatter jojoParser = new TXTDeserializerFormatter();
            object deserializedObject = jojoParser.ParseJojoObject(obj);
            return deserializedObject;
        }
    }

    public class TXTSerializerFormatter
    {
        private List<Container> _referenceList = new List<Container>();
        private int _idCounter = 1;

        public char _arrayOpenSymbol = '[';
        public char _arrayCloseSymbol = ']';
        public char _objectOpenSymbol = '{';
        public char _objectCloseSymbol = '}';
        public char _objectDelimeter = ',';
        public string _arrayAttribute = "values";
        public string _typeAttribute = "type";
        public string _idAttribute = "id";
        public string _referenceAttribute = "atr";
        public string tab = "  ";

        private string FormatList(Object objectList, string tabs)
        {
            StringBuilder listFormatter = new StringBuilder();

            listFormatter.AppendLine(tabs + '"' + _arrayAttribute + '"' + ":" + tab + _arrayOpenSymbol);
            bool firstFlag = true;
            foreach (object obj in (List<Object>)objectList)
            {
                if (firstFlag)
                    firstFlag = false;
                else
                    listFormatter.AppendLine(_objectDelimeter.ToString());

                listFormatter.Append(tabs + tab);
                listFormatter.Append(FormatObject(obj, tabs + tab));
            }
            listFormatter.AppendLine();
            listFormatter.AppendLine(tabs + _arrayCloseSymbol);

            return listFormatter.ToString();
        }

        private string FormatReference(Object obj, string tabs)
        {
            StringBuilder referenceFormatter = new StringBuilder();

            referenceFormatter.AppendLine(_objectOpenSymbol.ToString());
            referenceFormatter.AppendLine(tabs + tab + '"' + _referenceAttribute + '"' + ':' + tab + FindID(obj));
            referenceFormatter.Append(tabs + _objectCloseSymbol);

            return referenceFormatter.ToString();
        }

        private string FormatObject(Object obj, string tabs = "")
        {
            StringBuilder objectFormatter = new StringBuilder();

            if (IsAlreadyExist(obj))
                return FormatReference(obj, tabs);
            else
                _referenceList.Add(new Container(obj, _idCounter));

            objectFormatter.AppendLine(_objectOpenSymbol.ToString());
            objectFormatter.AppendLine(tabs + tab + '"' + _typeAttribute + '"' + ":" + tab + '"' + obj.GetType().FullName + '"' + _objectDelimeter);
            objectFormatter.AppendLine(tabs + tab + '"' + _idAttribute + '"' + ":" + tab + (_idCounter++).ToString() + _objectDelimeter);

            if (obj.GetType().GetInterface("ICollection") != null || (obj.GetType().GetInterface("IEnumerable`1") != null))
                objectFormatter.Append(FormatList(obj, tabs + tab));
            else
            {
                bool firstFlag = true;
                foreach (var property in obj.GetType().GetProperties())
                {
                    if (firstFlag)
                        firstFlag = false;
                    else
                        objectFormatter.AppendLine(_objectDelimeter.ToString());

                    objectFormatter.Append(FormatProperty(property, obj, tabs));
                }
                objectFormatter.AppendLine();
            }
            objectFormatter.Append(tabs + _objectCloseSymbol);

            return objectFormatter.ToString();
        }

        private int FindID(Object obj)
        {
            if (_referenceList.Count > 0)
            {
                for (int i = 0; i < _referenceList.Count; i++)
                {
                    if (_referenceList[i].container.Equals(obj))
                        return _referenceList[i].id;
                }
            }
            return -1;
        }

        private bool IsAlreadyExist(Object obj)
        {
            return FindID(obj) != -1;
        }

        private string FormatProperty(PropertyInfo property, Object obj, string tabs)
        {
            StringBuilder propertyFormatter = new StringBuilder();
            propertyFormatter.Append(tabs + tab + '"' + property.Name + '"' + ':' + tab);

            if ((property.PropertyType.IsPrimitive) || (property.PropertyType.IsEnum) || (property.PropertyType == typeof(string)) || (property.PropertyType.IsValueType))
            {
                if (property.PropertyType == typeof(string))
                {
                    propertyFormatter.Append('"' + property.GetValue(obj).ToString() + '"');
                }
                else if (property.PropertyType.IsEnum)
                {
                    propertyFormatter.Append(((int)property.GetValue(obj)).ToString());
                }
                else
                    propertyFormatter.Append(property.GetValue(obj).ToString());
            }
            else if (property.PropertyType.IsClass)
            {
                object nestedObject = property.GetValue(obj);
                if (nestedObject != null)
                    propertyFormatter.Append(FormatObject(nestedObject, tabs + tab));
                else
                    propertyFormatter.Append("null");
            }

            return propertyFormatter.ToString();
        }

        public string GetObjectInfo(Object item)
        {
            _idCounter = 1;
            return FormatObject(item);
        }
    }

    public class Container
    {
        public Container(object obj, int id)
        {
            this.container = obj;
            this.id = id;
        }
        public Container() { }

        public int id = 0;
        public object container = null;
    }

    public class TXTDeserializerFormatter
    {
        private List<Container> _referenceList = new List<Container>();

        public bool CutNextToken(ref string svList, string separator, out string token)
        {

            token = "";
            if (svList == "")
            {
                return false;
            }

            List<char> openBrakets = new List<char>() { '{', '[' };
            List<char> closeBrakets = new List<char>() { '}', ']' };
            int i = 0;
            int j = 0;
            Stack<char> brStack = new Stack<char>();
            bool quotes = false;

            while (i < svList.Length)
            {
                if (svList[i] == '"')
                    quotes = !quotes;

                if (openBrakets.Contains(svList[i]))
                {
                    brStack.Push(svList[i]);
                }
                else if (closeBrakets.Contains(svList[i]))
                {
                    brStack.Pop();
                }

                if (svList[i] == separator[j])
                {
                    if ((j == (separator.Length - 1)) && (!quotes) && (brStack.Count == 0))
                    {
                        token = svList.Substring(0, i - (separator.Length - 1)).Trim();
                        svList = svList.Substring(i + 1).Trim();
                        return true;
                    }
                    else
                    {
                        if ((j == (separator.Length - 1)) && ((quotes) || (brStack.Count != 0)))
                            j = 0;
                        else
                            j++;
                    }
                }
                else
                    j = 0;
                i++;
            }

            token = svList.Trim();
            svList = "";
            return true;
        }

        private Object CreateClassInstance(string className)
        {
            return Activator.CreateInstance(Type.GetType(className));
        }

        private void SetPropertyValue(Object obj, string propertyName, object propertyValue)
        {
            if (obj == null)
                return;

            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            try
            {
                if ((propertyInfo.PropertyType.IsPrimitive) || (propertyInfo.PropertyType.IsEnum) || (propertyInfo.PropertyType == typeof(string)) || (propertyInfo.PropertyType.IsValueType))
                {
                    if (propertyInfo.PropertyType.IsEnum)
                        propertyInfo.SetValue(obj, Convert.ToInt32(propertyValue));
                    else
                        propertyInfo.SetValue(obj, Convert.ChangeType(propertyValue, propertyInfo.PropertyType));
                }
                else
                {
                    propertyInfo.SetValue(obj, propertyValue);
                }

            }
            catch
            {
                Debug.WriteLine("Bad value: " + propertyValue + " for property " + propertyName);
            }
        }

        public string CutTillMatchinPare(char openSymbol, ref string svList, char closeSymbol)
        {
            if (svList == null || svList.Length <= 1)
                return "";

            Stack<char> brStack = new Stack<char>();
            if (svList[0] == openSymbol)
            {
                int i = 0;
                while (i < svList.Length)
                {
                    if (svList[i] == openSymbol)
                        brStack.Push(openSymbol);

                    if (svList[i] == closeSymbol)
                        brStack.Pop();

                    if ((svList[i] == closeSymbol) && (brStack.Count == 0))
                        break;

                    i++;
                }

                if (i < svList.Length)
                {
                    string result = svList.Substring(1, i - 1).Trim();
                    svList = svList.Substring(i + 1).Trim();
                    return result;
                }
            }
            return "";
        }

        private void FillObjectList(ref object list, string values)
        {
            var collection = (IList)list;
            if (collection != null)
            {
                var objectsString = CutTillMatchinPare('[', ref values, ']');
                while (CutNextToken(ref objectsString, ",", out string token))
                {
                    collection.Add(ParseJojoObject(token));
                }
            }
            list = collection;
        }

        private Container FindReferenceByID(int id)
        {
            if (_referenceList.Count > 0)
            {
                for (int i = 0; i < _referenceList.Count; i++)
                {
                    if (_referenceList[i].id == id)
                        return _referenceList[i];
                }
            }
            return null;
        }

        private void SetUpObject(ref Container objectContainer, string key, string value)
        {
            switch (key)
            {
                case "atr":
                    objectContainer = FindReferenceByID(Convert.ToInt32(value));
                    break;
                case "id":
                    objectContainer.id = Convert.ToInt32(value);
                    break;
                case "type":
                    objectContainer.container = CreateClassInstance(value);
                    break;
                case "values":
                    FillObjectList(ref objectContainer.container, value);
                    break;
                default:
                    if ((value.Length > 0) && (value[0] == '{'))
                        SetPropertyValue(objectContainer.container, key, ParseJojoObject(value));
                    else
                        SetPropertyValue(objectContainer.container, key, value);
                    break;
            }
        }

        public Object ParseJojoObject(string obj)
        {
            string objectForParse = obj;
            string currentObjectString = CutTillMatchinPare('{', ref objectForParse, '}');

            Container container = new Container();
            while (CutNextToken(ref currentObjectString, ",", out string token))
            {
                string key, value;
                CutNextToken(ref token, ":", out key);
                CutNextToken(ref token, ":", out value);
                key = key.Trim('"');
                value = value.Trim('"');
                SetUpObject(ref container, key, value);
            }

            _referenceList.Add(container);

            return container.container;
        }
    }
}
