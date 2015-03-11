using System;
using System.IO;
//using Ionic.Zip;


namespace MCDomain.Common
{
    /// <summary>
    /// Определяет методы для работы с типами IDocumentImageContainer
    /// Свойство Images рассматривается, как содержищее архив формата ZIP
    /// Для работы с архивом используется DotNetZip: http://dotnetzip.codeplex.com
    /// </summary>
    //public static class DocumentImageContainerExtensions
    //{
    //    /// <summary>
    //    /// Преобразует содержимое Images к типу Stream
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <returns>Stream</returns>
    //    public static Stream ImageToStream(this IDocumentImageContainer source)
    //    {
    //        if (source == null) throw new ArgumentNullException("source");


    //        var stream = new MemoryStream(source.Images);
    //        return stream;
    //    }

        
    //    /// <summary>
    //    /// Сохраняет содержимое объекта типа Stream в свойство Images
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <param name="stream"></param>
    //    public static void StreamToImage(this IDocumentImageContainer source, System.IO.Stream stream)
    //    {
    //        if (source == null) throw new ArgumentNullException("source");
    //        if (stream == null) throw new ArgumentNullException("stream");

    //        stream.Write(source.Images, 0, (int) stream.Length);
    //    }

    //    /// <summary>
    //    /// Возвращает содержимое Images в виде объекта типа ZipInputStream
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <returns></returns>
    //    public static ZipInputStream ImageToZipStream(this IDocumentImageContainer source)
    //    {
    //        if (source == null) throw new ArgumentNullException("source");
    //        return new ZipInputStream(source.ImageToStream());                        
    //    }

    //    /// <summary>
    //    /// Проверяет, содерджится ли в свойстве Images корректный архив формата ZIP
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <returns></returns>
    //    public static bool IsZip(this IDocumentImageContainer source)
    //    {
    //        if (source == null) throw new ArgumentNullException("source");

    //        return ZipFile.IsZipFile(source.ImageToZipStream(), false);
    //    }

    //    /// <summary>
    //    /// Распаковывает содержимое свойства Images в заданный каталог
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <param name="path"></param>
    //    public static void ExtractToFolder(this IDocumentImageContainer source, string path)
    //    {
    //        if (source == null) throw new ArgumentNullException("source");
    //        if (path == null) throw new ArgumentNullException("path");
            
            
            

    //    }
    //    /// <summary>
    //    /// Производит упаковку содержимого заданного пути и помещает архив в свойство Images
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <param name="path"></param>
    //    public static void ZipFolderToImage(this IDocumentImageContainer source, string path)
    //    {
    //        if (source == null) throw new ArgumentNullException("source");
    //        if (path == null) throw new ArgumentNullException("path");
    //    }
    //}
}