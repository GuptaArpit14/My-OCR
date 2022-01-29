using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Patagames.Ocr;
using Patagames.Ocr.Enums;

namespace My_OCR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        List<string> Files = new List<string>();
        string Filename;
        private void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "png(*.png)|*.png|jpeg(*.jpeg)|*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    Files.Add(filename);
                    Locations.Text += filename;
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var api = OcrApi.Create())
            {
                api.Init(Languages.English);
                //Create the renderer to PDF file output. The extension will be added automatically
                using (var renderer = OcrPdfRenderer.Create("multipage_pdf_file"))
                {
                    renderer.BeginDocument("Result");
                    api.ProcessPages(@Filename, renderer);
                    renderer.EndDocument();
                }
                Final_output.Text = "PDF ready";
            }
        }

        private void Save_Location_File(object sender, RoutedEventArgs e)
        {
            string filename;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            filename = sfd.FileName;
            Filename = filename;
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (string File in Files)
                {
                    sw.WriteLine(File);
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "png(*.png)|*.png|jpeg(*.jpeg)|*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                using (var api = OcrApi.Create())
                {
                    api.Init(Languages.English);
                    string plainText = api.GetTextFromImage(openFileDialog.FileName);
                    string filename;
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.ShowDialog();
                    sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    filename = sfd.FileName;
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        sw.WriteLine(plainText);
                    }
                    Final_output.Text = "txt file ready";
                }
            }
        }
    }
}
