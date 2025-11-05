using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ThirdPartyGnuPG
    {
        /// <summary>
        /// 
        /// </summary>
        private int gnuPGId;
        /// <summary>
        /// Gets or sets the gnu PG id.
        /// </summary>
        /// <value>The gnu PG id.</value>
        /// <remarks></remarks>
        public int GnuPGId
        {
            get
            {
                return gnuPGId;
            }
            set
            {
                gnuPGId = value;
            }
        }
        private string _extensionToAdd;
        /// <summary>
        /// get or set the extension to add column from T_ThirdPartyGnuPG
        /// </summary>
        public string ExtensionToAdd
        {
            get { return _extensionToAdd; }
            set { _extensionToAdd = value; }
        }

        private string _GnuPGName = "";
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        /// <remarks></remarks>
        public string GnuPGName
        {
            get { return _GnuPGName; }
            set { _GnuPGName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string homeDirectory = "C:\\GnuPG";
        /// <summary>
        /// Gets or sets the home directory.
        /// </summary>
        /// <value>The home directory.</value>
        /// <remarks></remarks>
        public string HomeDirectory
        {
            get
            {
                return homeDirectory;
            }
            set
            {
                homeDirectory = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private bool useCmdBatch = true;
        /// <summary>
        /// Gets or sets a value indicating whether [use CMD batch].
        /// </summary>
        /// <value><c>true</c> if [use CMD batch]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool UseCmdBatch
        {
            get
            {
                return useCmdBatch;
            }
            set
            {
                useCmdBatch = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private bool useCmdYes = true;
        /// <summary>
        /// Gets or sets a value indicating whether [use CMD yes].
        /// </summary>
        /// <value><c>true</c> if [use CMD yes]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool UseCmdYes
        {
            get
            {
                return useCmdYes;
            }
            set
            {
                useCmdYes = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private bool useCmdArmor = true;
        /// <summary>
        /// Gets or sets a value indicating whether [use CMD armor].
        /// </summary>
        /// <value><c>true</c> if [use CMD armor]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool UseCmdArmor
        {
            get
            {
                return useCmdArmor;
            }
            set
            {
                useCmdArmor = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private VerboseLevel verboseLevel = VerboseLevel.Verbose;
        /// <summary>
        /// Gets or sets the verbose level.
        /// </summary>
        /// <value>The verbose level.</value>
        /// <remarks></remarks>
        public VerboseLevel VerboseLevel
        {
            get
            {
                return verboseLevel;
            }
            set
            {
                verboseLevel = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string recipient = "";
        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        /// <remarks></remarks>
        public string Recipient
        {
            get
            {
                return recipient;
            }
            set
            {
                recipient = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string originator = "";
        /// <summary>
        /// Gets or sets the originator.
        /// </summary>
        /// <value>The originator.</value>
        /// <remarks></remarks>
        public string Originator
        {
            get
            {
                return originator;
            }
            set
            {
                originator = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string passPhrase = "";
        /// <summary>
        /// Gets or sets the pass phrase.
        /// </summary>
        /// <value>The pass phrase.</value>
        /// <remarks></remarks>
        public string PassPhrase
        {
            get
            {
                return passPhrase;
            }
            set
            {
                passPhrase = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string passPhraseDescriptor = "";
        /// <summary>
        /// Gets or sets the pass phrase descriptor.
        /// </summary>
        /// <value>The pass phrase descriptor.</value>
        /// <remarks></remarks>
        public string PassPhraseDescriptor
        {
            get
            {
                return passPhraseDescriptor;
            }
            set
            {
                passPhraseDescriptor = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int timeout = 10000;
        /// <summary>
        /// Gets or sets the time out.
        /// </summary>
        /// <value>The time out.</value>
        /// <remarks></remarks>
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        Commands command = Commands.Encrypt;

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        /// <remarks></remarks>
        public Commands Command
        {
            get { return command; }
            set { command = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _enabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ThirdPartyGnuPG"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
    }
}
