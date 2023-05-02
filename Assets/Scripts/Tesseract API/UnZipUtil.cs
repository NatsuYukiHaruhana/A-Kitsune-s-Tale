using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

public class UnZipUtil {
    public static void CreateTarGZ_FromDirectory(string tgzFilename, string sourceDirectory) {
        Stream outStream = File.Create(tgzFilename);
        Stream gzoStream = new GZipOutputStream(outStream);
        TarArchive tarArchive = TarArchive.CreateOutputTarArchive(gzoStream);

        tarArchive.RootPath = sourceDirectory.Replace('\\', '/');
        if (tarArchive.RootPath.EndsWith("/")) { 
            tarArchive.RootPath = tarArchive.RootPath.Remove(tarArchive.RootPath.Length - 1);
        }

        AddDirectoryFilesToTar(tarArchive, sourceDirectory, true);

        tarArchive.Close();
    }

    public static void AddDirectoryFilesToTar(TarArchive tarArchive, string sourceDirectory, bool recurse) {
        TarEntry tarEntry = TarEntry.CreateEntryFromFile(sourceDirectory);
        tarArchive.WriteEntry(tarEntry, false);

        string[] filenames = Directory.GetFiles(sourceDirectory);
        foreach (string filename in filenames) {
            tarEntry = TarEntry.CreateEntryFromFile(filename);
            tarArchive.WriteEntry(tarEntry, true);
        }

        if (recurse) {
            string[] directories = Directory.GetDirectories(sourceDirectory);
            foreach (string directory in directories) {
                AddDirectoryFilesToTar(tarArchive, directory, recurse);
            }
        }
    }

    public static void ExtractTGZ(string gzArchiveName, string destFolder) {
        Stream inStream = File.OpenRead(gzArchiveName);
        Stream gzipStream = new GZipInputStream(inStream);

        TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
        tarArchive.ExtractContents(destFolder);
        tarArchive.Close();

        gzipStream.Close();
        inStream.Close();
    }
}