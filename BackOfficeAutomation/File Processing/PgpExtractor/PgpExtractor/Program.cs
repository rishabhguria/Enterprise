using CsvHelper;
using CsvHelper.Configuration;
using PgpCore;
using PgpExtractor;
using PgpExtractor.Models;
using System.Globalization;

public class Program
{
    private static ExitCode exitCode = ExitCode.Success;
    private const string PgpExtension = ".pgp";
    public static async Task Main(string[] args)
    {
        using var logger = new Logger();
        bool hasIssues = false;

        try
        {
            if (args.Length == 0)
            {
                throw new ArgumentNullException("No arguments passed to PgpExtractor.");
            }

            string configFilePath = args[0];

            if (string.IsNullOrEmpty(configFilePath))
            {
                throw new ArgumentNullException("Config file path not passed.");
            }

            var pgpConfigs = new List<PGPConfig>();

            using (var reader = new StreamReader(configFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true
            }))
            {
                pgpConfigs = csv.GetRecords<PGPConfig>().ToList();
            }

            await Parallel.ForEachAsync(pgpConfigs, async (pgpConfig, cancellationToken) =>
            {
                try
                {
                    // Validate file paths
                    if (string.IsNullOrWhiteSpace(pgpConfig.PrivateKeyPath) ||
                        string.IsNullOrWhiteSpace(pgpConfig.InputFolderPath))
                    {
                        throw new ArgumentException("Invalid configuration paths in PGP config.");
                    }

                    FileInfo privateKey = new FileInfo(pgpConfig.PrivateKeyPath);
                    if (!privateKey.Exists)
                    {
                        throw new FileNotFoundException("Private key file not found.", pgpConfig.PrivateKeyPath);
                    }

                    // Initialize encryption keys and PGP
                    EncryptionKeys encryptionKeys = new EncryptionKeys(privateKey, pgpConfig.Passphrase);
                    PGP pgp = new PGP(encryptionKeys);

                    // Get PGP files from the input folder
                    var pgpFiles = Directory.GetFiles(pgpConfig.InputFolderPath).Where(i => Path.GetExtension(i).ToLower() == PgpExtension);
                    if (pgpFiles.Any())
                    {
                        await Parallel.ForEachAsync(pgpFiles, async (file, innerCancellationToken) =>
                        {
                            try
                            {
                                FileInfo inputFile = new FileInfo(@file);
                                string outputFilePath = Path.Combine(pgpConfig.InputFolderPath, Path.GetFileNameWithoutExtension(file));
                                FileInfo outputFile = new FileInfo(outputFilePath);

                                // Decrypt the PGP file
                                await pgp.DecryptAsync(inputFile, outputFile);
                                logger.ConsoleLog($"Successfully decrypted PGP file: {inputFile.Name}");
                                logger.Log($"Successfully decrypted PGP file: {inputFile.Name}");

                                // Delete the original file after successful decryption
                                File.Delete(file);
                                logger.ConsoleLog($"Successfully deleted PGP file: {inputFile.Name}");
                                logger.Log($"Successfully deleted PGP file: {inputFile.Name}");
                            }
                            catch (Exception ex)
                            {
                                hasIssues = true;
                                string msg = $"Error Decrypting file {file}: {ex.Message}";
                                logger.Log(msg);
                               
                                logger.ConsoleLog(msg, ConsoleColor.Red);
                            }

                        });
                    }
                }
                catch (Exception ex)
                {
                    hasIssues = true;
                    string msg = $"Error Decrypting files for folder {pgpConfig.InputFolderPath}: {ex.Message}";
                    logger.Log(msg);
                    logger.ConsoleLog(msg, ConsoleColor.Red);
                }
            });

            if (hasIssues) 
                exitCode = ExitCode.ProcessingError;
        }
        catch (Exception ex)
        {
            exitCode = ExitCode.OtherError;
            string msg = $"Error: {ex.Message}";
            logger.Log(msg);
            logger.ConsoleLog(msg, ConsoleColor.Red);
        }

        Environment.Exit((int)exitCode);
    }

}
