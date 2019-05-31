using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace oopCreateObject
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            List<Type> types = new List<Type>()
            {
                typeof(Liver),
                typeof(Spleen),
                typeof(Brains),
                typeof(Bonebrains),
                typeof(Heart),
                typeof(Lungs),
                typeof(Nose)
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(types, new CRUD()));
        }
    }
}
