using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TextEditor.BL;

#region Text Editor  MVP 
// will use the MVP pattern (View-Presenter-Model)
#endregion

namespace TextEditor
{
    static class Program
    {
        
        [STAThread]
        static void Main()
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            MainForm form = new MainForm();
            MessageService service = new MessageService();
            FileManager manager = new FileManager();

            
            MainPresenter presenter = new MainPresenter(form, manager, service);
            
            Application.Run(form);

        }
    }
    
}
