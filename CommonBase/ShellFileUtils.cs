using System;
using System.Diagnostics;
using System.IO;

namespace CommonBase
{
    /// <summary>
    /// Предоставляет набор утилит для работы с сервисами оболочки Windows  
    /// </summary>
    public class ShellFileUtils
    {
        /// <summary>
        /// Создаёт экземпляр окна Explorer для отображения файлов каталога. Файл, переданный в качестве аргумента - подсвечивается
        /// </summary>
        /// <param name="pFilename"></param>
        /// <returns></returns>
        public static bool LaunchFolderView(string pFilename)
        {
            var lResult = false;

            // Check the file exists
            if (File.Exists(pFilename))
            {
                // Check the folder we get from the file exists
                // this function would just get "C:\Hello" from
                // an input of "C:\Hello\World.txt"
                var lFolder = Path.GetDirectoryName(pFilename); //FileSystemHelpers.GetPathFromQualifiedPath(p_Filename);

                // Check the folder exists
                if (lFolder != null && Directory.Exists(lFolder))
                {
                    try
                    {
                        // Start a new process for explorer
                        // in this location     
                        var lPsi = new ProcessStartInfo
                            {
                                FileName = "explorer",
                                Arguments = string.Format("{0},/select", pFilename),
                                UseShellExecute = true
                            };

                        var lNewProcess = new Process {StartInfo = lPsi};
                        lNewProcess.Start();

                        // No error
                        lResult = true;
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Не удалось показать файл.", exception);
                    }
                }
            }

            return lResult;
        }

    }
}
