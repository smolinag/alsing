﻿using System.Windows.Forms;

namespace GenArt.Classes
{
    public static class FileUtil
    {
        public static string DnaExtension = "dna files (*.dna)|*.dna|xml files (*.xml)|*.xml|All files (*.*)|*.*";
        public static string ProjectExtension = "dnaproj files (*.dnaproj)|*.dnaproj|xml files (*.xml)|*.xml|All files (*.*)|*.*";
        public static string ImgExtension =
            "gif files (*.gif)|*.gif|bmp files (*.bmp)|*.bmp|jpg files (*.jpg)|*.jpg|jpeg files (*.jpeg)|*.jpeg|All files (*.*)|*.*";

        public static string XmlExtension = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        public static string GetSaveFileName(string filter)
        {
            var dialog = new SaveFileDialog {Filter = filter};
            if (!dialog.ShowDialog().Equals(DialogResult.Cancel))
                return dialog.FileName;
            return null;
        }

        public static string GetOpenFileName(string filter)
        {
            var dialog = new OpenFileDialog {Filter = filter};
            if (!dialog.ShowDialog().Equals(DialogResult.Cancel))
                return dialog.FileName;
            return null;
        }
    }
}