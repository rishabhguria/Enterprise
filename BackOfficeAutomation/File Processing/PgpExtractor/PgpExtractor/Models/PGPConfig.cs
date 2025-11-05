namespace PgpExtractor.Models
{
    public class PGPConfig
    {
        public string InputFolderPath { get; set; }
        public string PrivateKeyPath { get; set; }
        public string Passphrase { get; set; }
    }
}
