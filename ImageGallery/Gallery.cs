using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ImageGallery
{
    class Gallery
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(Constants.Author);
                // get all pictures from 'bildes' folder
                var pictures = ReadFiles();
                Console.WriteLine("Ievadiet galerijas nosaukumu:");
                string name = Console.ReadLine();
                // Create HTML string
                var htmlPage = ComposeHtml(pictures, name);
                // Save index.html file
                File.WriteAllText(@".\bildes\index.html", htmlPage);
                Console.WriteLine("Galerija izveidota.");
                // open index.html in default system browser (Windows only)
                OpenPage(@".\bildes\index.html");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Mape 'bildes' neeksiste.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Mape 'bildes' nesatur bildes.");
            }
        }

        private static List<String> ReadFiles()
        {
            var files = Directory.GetFiles(@".\bildes", "*.*", SearchOption.TopDirectoryOnly);
            var imageFiles = new List<string>();
            foreach (string filename in files)
            {
                if (Regex.IsMatch(filename, @".jpg|.jpeg|.png|.gif|.tiff|.bmp|.svg$"))
                    imageFiles.Add(filename);
            }
            if (imageFiles.Count == 0)
            {
                throw new FileNotFoundException();
            }
            return imageFiles;
        }

        private static void OpenPage(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        private static string ComposeHtml(List<String> imageFileNames, string galleryName)
        {
            // Page header start
            string html = Constants.HtmlHeaderStart;

            // Page css style start/end.
            html += 
                "<style>" +
                "div.gallery img {width: 100%; height: auto;}" +
                ".responsive {padding: 0 6px; float: left; width: 25%; }" +
                "@media only screen and (max-width: 500px) {" +
                ".responsive {width: 100%;}}" +
                "</style>";

            // Page header end
            html += Constants.HtmlHeaderEnd;

            // Page body start
            html += Constants.HtmlBodyStart;

            // Adding Header
            html += "<h1>" + galleryName + "</h1>";

            // Compose gallery for each picture
            foreach (string oneImage in imageFileNames)
            {
                html +=
                    "<div class=\"responsive\">" +
                    "<div class=\"gallery\">" +
                    "<a target=\"_blank\" href=\"" + Path.GetFileName(oneImage) + "\">" +
                    "<img src=\"" + Path.GetFileName(oneImage) + "\" width=\"600\" height=\"400\">" +
                    "</a>" +
                    "</div></div>";
            }

            // Page body end
            html += Constants.HtmlBodyEnd;

            return html;
        }
    }
}