using System;
using System.IO;
//using Ionic.Zip;


namespace MCDomain.Common
{
    /// <summary>
    /// ���������� ������ ��� ������ � ������ IDocumentImageContainer
    /// �������� Images ���������������, ��� ���������� ����� ������� ZIP
    /// ��� ������ � ������� ������������ DotNetZip: http://dotnetzip.codeplex.com
    /// </summary>
    //public static class DocumentImageContainerExtensions
    //{
    //    /// <summary>
    //    /// ����������� ���������� Images � ���� Stream
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
    //    /// ��������� ���������� ������� ���� Stream � �������� Images
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
    //    /// ���������� ���������� Images � ���� ������� ���� ZipInputStream
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <returns></returns>
    //    public static ZipInputStream ImageToZipStream(this IDocumentImageContainer source)
    //    {
    //        if (source == null) throw new ArgumentNullException("source");
    //        return new ZipInputStream(source.ImageToStream());                        
    //    }

    //    /// <summary>
    //    /// ���������, ����������� �� � �������� Images ���������� ����� ������� ZIP
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <returns></returns>
    //    public static bool IsZip(this IDocumentImageContainer source)
    //    {
    //        if (source == null) throw new ArgumentNullException("source");

    //        return ZipFile.IsZipFile(source.ImageToZipStream(), false);
    //    }

    //    /// <summary>
    //    /// ������������� ���������� �������� Images � �������� �������
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <param name="path"></param>
    //    public static void ExtractToFolder(this IDocumentImageContainer source, string path)
    //    {
    //        if (source == null) throw new ArgumentNullException("source");
    //        if (path == null) throw new ArgumentNullException("path");
            
            
            

    //    }
    //    /// <summary>
    //    /// ���������� �������� ����������� ��������� ���� � �������� ����� � �������� Images
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