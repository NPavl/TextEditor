using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// обслуживает нашу форму кода

namespace TextEditor
{
    // опишем команды и свойства через интерфейс к коорму будт обращаться управляющий код 
    // согласно нашему паттерну MVP
    public interface IMainForm
    {
        string FilePath { get; } // к данному члену типа будет иметь доступ Presenter 
                                 // данное свойство будет возвращать тот тип файла коорое выбрал пользователь
                                 // оно у нас только для чтения что логично Presenter не может задавать это 
                                 // свойство он можт только считывать его значение.
        string Content { get; set; } // данно свойство и для чтения и для запись. Presenter будет выставлять 
        // это свойство а Form отображать содержимое а потом после того как пользователь внес туда необходимые 
        // изменения Presenter будет получать его 
        void SetSymbolCount(int count); // этот метод будет устанавливать то количство символов коорый 
        // содержит наш файл, его также будет задавать наш Presenter потом что именно он будет передавать 
        // в модель содержимое файла с тем чтобы модель его обсчитала а потом получив эту цифору он будет 
        // он будет давать форму команде отобразить ее.

        // три события через которые формам будет уведомлять Presenter о том что в ней что то произошло
        // во первых форма скажет Presenter(у) что пользователь нажал на кнопку открыть файл FileOpenClick
        // сохранить файл FileSaveClick и третья форма что пользовател внес изменения содержимое файла. 
        event EventHandler FileOpenClick;
        event EventHandler FileSaveClick;
        event EventHandler ContentChanged;
    }

    // реализация нашего интерфейса 
    public partial class MainForm : Form, IMainForm
    {
        public MainForm()
        {
            InitializeComponent();
            butOpenFile.Click += butOpenFile_Click; // butOpenFile кнопка открытия файла добавляем ее событие 
            // Click далее += и нажимаем клавишу tab и среда VS создаст нам как обработчик события так и 
            // подвяжет его к этому событию. 
            // аналогично оформляем подписку на дугие события: 
            butSaveFile.Click += butSave_Click;
            fldContent.TextChanged += fldContent_TextChanged;
            butSelectFile.Click += butSelectFile_Click;
            numFont.ValueChanged += numFont_ValueChanged;

        }   

        #region Проброс событий 
        // пользователь кликает на кнопку мы попадаем в обработчик ButOpenFile_Click в нем мы проверяем 
        // есть ли подписчики у события FileOpenClick если они есть мы вызываем их . тоесть происходит 
        // проброс с метода клика на кнопке FileOpenClick мы вызываем событие FileOpenClick 
        private void butOpenFile_Click(object sender, EventArgs e) // обработчик событие окрытия файла 
        {
            if (FileOpenClick != null) FileOpenClick(this, EventArgs.Empty); // как толко пользователь 
            // кликнет на кнопку открыть файл он попадет сюда.
            // Empty - значит пустые аргументы.
        }

        // аналогично создаем для других событий создаюся аналогичным образом:
        private void butSave_Click(object sender, EventArgs e) // обрабочик события сохранения файла 
        {
            if (FileOpenClick != null) FileOpenClick(this, EventArgs.Empty); 
        }

        private void fldContent_TextChanged(object sender, EventArgs e) // обработчик события изменения содержимого
        {
            if (ContentChanged != null) ContentChanged(this, EventArgs.Empty); 
        }
        #endregion

        #region Реализация интерфейса IMainForm
        // обьявим свойства через которые будет взаимодествовать Presenter будет взаимодействовать с нашей
        // формой, таким образом мы экранировать взаимодействие элементов на прямую и оборачивать их вот 
        // в такие свойства и методы описанные в интерфейсе:

        // открывам свойства filePath которое возвращает содержимое нашего екстового поля на форму 
        public string FilePath
        {
            get { return fldFilePath.Text; }
        }

        // открываем на ружу свойство Content которое как возвращает так и устанавливает содержимое 
        // нашего тектового поля которое отбражает содержимое файла
        public string Content
        {
            get { return fldFilePath.Text; }
            set { fldContent.Text = value; }
        }  

        // открываем наружу метод SetSymbolCount который устанавливает значение элимента управления
        // label кооре занимается отображеним количества символов в содержимом файла.   
        public void SetSymbolCount(int count)
        {
            lblSymbolCount.Text = count.ToString();  
        }

        // никакие аргументы мы не будем передавать в наши события используем простую форму обработчика 
        // без праметров EventHandler:

        public event EventHandler FileOpenClick; 
        public event EventHandler FileSaveClick;
        public event EventHandler ContentChanged;

        #endregion


        // третья часть реализовываем собственный код формы коорый занимаетс лишь внутреннми элиментами управления 


        // описание метода butSelectFile_Click: 
        // мы должны обработать клик на кнопке обработать файл, и подвяжем его к событию клик: 
        // данный обработчик - он вызывает стандартный диалог для открытия файла и проставляет в нем
        // необходиые фильтры , по умолчани мы будем отображать в окне выбора файла только файлы с 
        // расширением txt но пользовать также может выбрать опцию отображат все файлы , далее если позовать 
        // выбрал файл и нажал на опцию ОК мы утанавливаем нашему тексовому полю коорый хранит путь к файлу 
        // этот самый путь (са путь мы берем из диалога выбора файла) и далее мы опять таки выбираем событие 
        // открытия файла - затем что мы хоим чтобы пользовател выбрав файл сразу же получил его содержание 
        // в нашем текстовом поле а не нажимал дополнительно кнопку открыть иенно поэтому после выбора файла 
        // сразуже генерируем событие окрытия файла.

        private void butSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fldFilePath.Text = dlg.FileName;
                if (FileOpenClick != null)
                {
                    FileOpenClick(this, EventArgs.Empty);
                }
            }
        }

        // данный обработчик для выбора шрифта: 

        private void numFont_ValueChanged(object sender, EventArgs e)
        {
            fldContent.Font = new Font("Calibri", (float)numFont.Value);
        }
    }
}
