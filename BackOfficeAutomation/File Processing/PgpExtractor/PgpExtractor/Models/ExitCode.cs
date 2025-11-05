namespace PgpExtractor.Models
{
    public enum ExitCode
    {
        Success = 0,             // Success
        ProcessingError = 1,     // Error during decryption process of any file
        OtherError = 2, // Other exceptions
    }
}
