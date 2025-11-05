namespace Prana.OptionCalculator.Common
{
    /// <summary>
    /// Constants class , esigbin class has defines for basic taken from dbcbin.h
    /// </summary>
    public class eSignBin
    {
        // Added to support Tick Server options
        public static readonly string NO_OPTIONS = "";
        public static readonly string QUOTEONLY = "QUOTEONLY=T";
        public static readonly string TRADESONLY = "TRADESONLY=T";
        public static readonly string FORMT = "FORMT=T";
        // You can combine options for Tick Server requests
        // for example:  "FORMT=T QUOTEONLY=T"
        // NOTE:  All Tick server requests submitted through the eSignal Control API
        //        are segmented requests.  There is no need to specify the SEGMENT=T
        //        option; it is implicit.
        public const short SC1BITVALUE = 8;
        public const short SC2BITVALUE = 4;
        public const short SC3BITVALUE = 2;
        public const short SC4BITVALUE = 1;

        public const short CATEGORY_STOCK = 66;
        public const short CATEGORY_FUTURE = 72;
        public const short CATEGORY_FUTUREOPTION = 73;
        public const short CATEGORY_CURRENCYOPTION = 74;
        public const short CATEGORY_STOCKOPTION = 71;
        public const short CATEGORY_INDICE = 65;
        public const short CATEGORY_MUTUALFUND = 75;
        public const short CATEGORY_MONEYFUND = 76;
        public const short CATEGORY_GAINER = 85;
        public const short CATEGORY_HEADLINE = 86;
        public const short CATEGORY_MESSAGE = 49;
        public const short CATEGORY_NALERT = 77;
        public const short CATEGORY_PASS = 54;
        public const short CATEGORY_NSTOCK = 56;
        public const short CATEGORY_NINE = 57;
        public const short CATEGORY_FILE = 70;
        public const short CATEGORY_INFO = 90;
        public const short CATEGORY_ASCIIROS = 54;
        public const short CATEGORY_BINARYROS = 55;
        public const short SUBCATEGORY_FX = 70;
        public const short SUBCATEGORY_GB = 71;
        public const short SUBCATEGORY_FUTURE = 72;
        public const short SUBCATEGORY_FUTUREOPTION = 73;
        public const short SUBCATEGORY_CURRENCYOPTION = 74;
        public const short SUBCATEGORY_HK = 75;
        public const short SUBCATEGORY_BOND = 89;
        public const short SUBCATEGORY_FUNDAMENTAL = 88;
        // ' extra status bits
        public const short ROSBANKRUPTCY = 0x100;
        public const short ROSPENDINGDELISTING = 0x200;
        // ' general bitmap for new data
        //Can not implicitly convert type 'uint' to 'int'
        public const int BMNEW = unchecked((int)0x80000000);
        // '/* defines for status byte 1 */
        public static readonly short ROSREALTIMETRADEORBIDASK = 0x2;
        public static readonly short ROSBIDASKMORECURRENT = 0x1;
        public const short ROSOPEN = 0x4;
        public const short ROSSETTLEMENT = 0x8; // '/* for futures */
        public const short ROSOPTVOLASDATE = 0x8; // '/* for stock options */
        public const short ROSREJECTEDTRADE = 0x8; // '/* for hong kong stocks */
        public const short ROSTICKMASK = 0x30;
        public const short ROSTICKBITUP = 0x0;
        public const short ROSTICKBITDOWN = 0x10;
        public const short ROSTICKCALCULATE = 0x20;
        // '/* defines for status byte 2 */
        public const short ROSLOWSETBYASK = 0x1; // '/* for futures */
        public const short ROSEXDIVIDEND = 0x1; // '/* for stocks */
        public const short ROSHIGHSETBYBID = 0x2;
        public const short ROSNOMINAL = 0x4;
        public const short ROSSESSIONIDMASK = 0x38; // '/* for futures */
        // '/* defines for status byte 3 */
        public const short ROSINVALIDATETRADE = 0x1; //'/* for stock options */
        public const short ROSINVALIDATEBIDASK = 0x2; // '/* for stock options */
        public const short ROSCONTINUOUSCONTRACT = 0x4; // '/* for futures */
        public const short ROSDELETECONTRACT = 0x8; //'/* for futures */
        public const short ROSFUTVOLASDATE = 0x10; //'/* for futures */
        public const short ROSFUTFASTMARKET = 0x20; //'/* for futures */
        public const short ROSNOMINALDAYCLOSE = 0x20; //'/* for hongkong stocks */
        // '/* defines for status byte 4 */
        public const short WROSBONDTB = 0x1; // '/* for bmi bonds (display as tbills */
        public const short WROSBANKRUPTCY = 0x1; ///* for stocks */
        public const short WROSBONDTN = 0x2; ///* for bmi bonds (display as tnotes */
        public const short WROSPENDINGDELISTING = 0x2; ///* for stocks */
        public const short WROSHASOPTIONS = 0x4; ///* for stocks (does not come from ros */
        public const short WROSUPC = 0x8; ///* for stocks, upc indicator */
        public const short WROSVALIDUPC = 0x80; ///* this bit is on if upc indicator is valid (for internal winros usage only) */
        // '/* defines for status byte 5 */
        public const short ROSFREE = 0x2; ///* for futures free contracts */
        public const short ROSLOWCOST = 0x4; ///* for lowcost cme contracts */
                                             ///* bit 0x40 is used for some BB flags */
                                             ///* defines for status byte 6 */
                                             ///* look for bid tick in bit 0x10 only if bit 0x20 is on (ROSBIDTICKVALID */
        public const short WROSRTTRADE = 0x8; ///* for all.... */
        public const short WROSRTBID = 0x10; ///* for all.... */
        public const short WROSRTASK = 0x20; ///* for all.... */
        public const short WROSBIDCLOSE = 0x1;
        public const short WROSASKCLOSE = 0x2;
        ///* defines for morestatus field (includes stickbits and passthru bits */
        ///* stickybits...winros turns them off on various circumstances */
        ///* formt and trading halted turned off on next rt trade */
        public const int ROSFORMT = unchecked((int)0x80000000);
        public const int ROSTRADINGHALTED = 0x40000000;
        ///* passthru bits via fnet */
        public const short ROSPREMARKET = unchecked((short)0x8000);
        public const short ROSTICKNONE = 32;
        public const short ROSTICKUP = 43;
        public const short ROSTICKDOWN = 45;
        public const short SHEETTICKNONE = 95;
        ///* Item identifier for daily request */
        public const string PASS = "PASS";
        public const string HEADLINE = "HEADLINE";
        public const string NEWSALERT = "NEWSALERT";
        public const string WILDCARD = "*";
        public const string LIST_ITEM = "LIST_";
        public const string ALL_ITEM = "ALL_";
        public const string HEADLINE_ITEM = "HL_";
        public const string MESSAGE_ITEM = "MESSAGE_";
        public const string NEWSALERT_ITEM = "NEWSALERT";
        public const string PASS_ITEM = "PASS_";
        public const string ROS_ITEM = "ROSCOMMAND_";
        public const string WR_ITEM = "$WR";
        public const string REQUEST_ITEM = "REQUEST_";
        ///* Items for global output */
        public const string ALL_STOCK = "ALL_STOCK";
        public const string ALL_FUTURE = "ALL_FUTURE";
        public const string ALL_FUTUREOPTION = "ALL_FUTUREOPTION";
        public const string ALL_CURRENCYOPTION = "ALL_CURRENCYOPTION";
        public const string ALL_INDICE = "ALL_INDICE";
        public const string ALL_MUTUAL = "ALL_MUTUAL";
        public const string ALL_MONEY = "ALL_MONEY";
        public const string ALL_STOCKOPTION = "ALL_STOCKOPTION";
        public const string ALL_SPORT = "ALL_SPORT";
        public const string ALL_HEADLINE = "ALL_HEADLINE";
        public const string ALL_FILE = "ALL_FILE";
        public const string ALL_GAINER = "ALL_GAINER";
        public const string ALL_NEWSALERT = "ALL_NEWSALERT";
        public const string ALL_PASS = "ALL_PASS";
        public const string ALL_NEWSSTORY = "ALL_NEWSSTORY";
        public const string ALL_CLOSE = "ALL_CLOSE";
        public const string ALL_NYSE = "ALL_NYSE";
        public const string ALL_AMEX = "ALL_AMEX";
        public const string ALL_NASD = "ALL_NASD";
        public const string ALL_CME = "ALL_CME";
        public const string ALL_COMX = "ALL_COMX";
        public const string ALL_CBT = "ALL_CBT";
        public const string ALL_KCBT = "ALL_KCBT";
        public const string ALL_NYME = "ALL_NYME";
        public const string ALL_MIDA = "ALL_MIDA";
        public const string ALL_CEC = "ALL_CEC";
        public const string ALL_LIF1 = "ALL_LIF1";
        public const string ALL_TC = "ALL_TC";
        public const string ALL_VC = "ALL_VC";
        public const string ALL_MC = "ALL_MC";
        public const string ALL_AC = "ALL_AC";
        public const string ALL_OC = "ALL_OC";
        ///* CHANGEBETA5                                      ' some _aaa were changed to _aa, eg. _LIFE -> _LF */
        public const string ALL_LF = "ALL_LF";
        public const string ALL_MA = "ALL_MA";
        public const string ALL_LM = "ALL_LM";
        public const string ALL_LC = "ALL_LC";
        public const string ALL_IP = "ALL_IP";
        public const string ALL_SI = "ALL_SI";
        public const string ALL_AT = "ALL_ATA";
        ///* ENDCHANGEBAET5 */
        public const string ALL_FX = "ALL_FX";
        public const string ALL_GB = "ALL_GB";
        public const string ALL_TS = "ALL_TS";
        public const string ALL_TG = "ALL_TG";
        public const string ALL_TO = "ALL_TO";
        public const string ALL_SA = "ALL_SA";
        public const string ALL_SM = "ALL_SM";
        public const string ALL_SZ = "ALL_SZ";
        public const string ALL_SH = "ALL_SH";
        public const string ALL_MX = "ALL_MX";
        public const string ALL_HK = "ALL_HK";
        public const string ALL_LSE = "ALL_LSE";
        public const string ALL_WCE = "ALL_WCE";
        public const string ALL_DTB = "ALL_DTB";
        public const string ALL_BOND = "ALL_BOND";
        public const string ALL_DC = "ALL_ZB";
        public const string ALL_OTHER = "ALL_OTHER";
        public const string ALL_QUESTION = "ALL_QUESTION";
        ///* Items for global output */
        public const string LIST_STOCK = "LIST_STOCK";
        public const string LIST_FUTURE = "LIST_FUTURE";
        public const string LIST_FUTUREOPTION = "LIST_FUTUREOPTION";
        public const string LIST_CURRENCYOPTION = "LIST_CURRENCYOPTION";
        public const string LIST_INDICE = "LIST_INDICE";
        public const string LIST_MUTUAL = "LIST_MUTUAL";
        public const string LIST_MONEY = "LIST_MONEY";
        public const string LIST_STOCKOPTION = "LIST_STOCKOPTION";
        public const string LIST_SPORT = "LIST_SPORT";
        public const string LIST_HEADLINE = "LIST_HEADLINE";
        public const string LIST_FILE = "LIST_FILE";
        public const string LIST_GAINER = "LIST_GAINER";
        public const string LIST_NEWSALERT = "LIST_NEWSALERT";
        public const string LIST_PASS = "LIST_PASS";
        public const string LIST_NEWSSTORY = "LIST_NEWSSTORY";
        public const string LIST_CLOSE = "LIST_CLOSE";
        public const string LIST_NYSE = "LIST_NYSE";
        public const string LIST_AMEX = "LIST_AMEX";
        public const string LIST_NASD = "LIST_NASD";
        public const string LIST_CME = "LIST_CME";
        public const string LIST_COMX = "LIST_COMX";
        public const string LIST_CBT = "LIST_CBT";
        public const string LIST_KCBT = "LIST_KCBT";
        public const string LIST_NYME = "LIST_NYME";
        public const string LIST_MIDA = "LIST_MIDA";
        public const string LIST_CEC = "LIST_CEC";
        public const string LIST_TC = "LIST_TC";
        public const string LIST_VC = "LIST_VC";
        public const string LIST_MC = "LIST_MC";
        public const string LIST_AC = "LIST_AC";
        public const string LIST_OC = "LIST_OC";
        ///* CHANGEBETA5' some _aaa were changed to _aa, eg. _LIFE -> _LF */
        public const string LIST_LF = "LIST_LF";
        public const string LIST_MA = "LIST_MA";
        public const string LIST_LM = "LIST_LM";
        public const string LIST_LC = "LIST_LC";
        public const string LIST_IP = "LIST_IP";
        public const string LIST_SI = "LIST_SI";
        public const string LIST_AT = "LIST_AT";
        ///* ENDCHANGEBETA5 */
        public const string LIST_FX = "LIST_FX";
        public const string LIST_GB = "LIST_GB";
        public const string LIST_TS = "LIST_TS";
        public const string LIST_TG = "LIST_TG";
        public const string LIST_TO = "LIST_TO";
        public const string LIST_SA = "LIST_SA";
        public const string LIST_SM = "LIST_SM";
        public const string LIST_SZ = "LIST_SZ";
        public const string LIST_SH = "LIST_SH";
        public const string LIST_MX = "LIST_MX";
        public const string LIST_HK = "LIST_HK";
        public const string LIST_LSE = "LIST_LSE";
        public const string LIST_WCE = "LIST_WCE";
        public const string LIST_DTB = "LIST_DTB";
        public const string LIST_BOND = "LIST_BOND";
        public const string LIST_DC = "LIST_ZB";
        public const string LIST_OTHER = "LIST_OTHER";
        public const string LIST_QUESTION = "LIST_QUESTION";
        public const string DBC_NOSYMBOL = "NOSYMBOL";
        public const string ROSCOMMAND_U = "ROSCOMMAND_U";
        public const string ROSCOMMAND_IDS = "ROSCOMMAND_IDS";
        public const string ROSCOMMAND_PERFORMANCE = "ROSCOMMAND_PERFORM";
        public const string ROSCOMMAND_GLOBALS = "ROSCOMMAND_GLOBALS";
        public const string ROSCOMMAND_REMOTEDIR = "ROSCOMMAND_REMOTE";
        public const string ROSCOMMAND_NEWS_PROFILE = "ROSCOMMAND_NP";
        public const string ROSCOMMAND_TICK_PROFILE = "ROSCOMMAND_TP";
        public const string ROSCOMMAND_OTHER_PROFILE = "ROSCOMMAND_OP";
        public const string ROSCOMMAND_NEWS_PROFILEUSER = "ROSCOMMAND_NPU";
        public const string ROSCOMMAND_TICK_PROFILEUSER = "ROSCOMMAND_TP_EXT";
        public const string ROSCOMMAND_OTHER_PROFILEUSER = "ROSCOMMAND_OPU";
        public const string ROSCOMMAND_HIST_PROFILE = "ROSCOMMAND_HP";
        public const string ROSCOMMAND_HIST_PROFILEUSER = "ROSCOMMAND_HPU";
        public const string ROSCOMMAND_CLOSE = "ROSCOMMAND_CLOSE";
        public const string ROSCOMMAND_AMCLOSE = "ROSCOMMAND_AMCLOSE";
        public const string ROSCOMMAND_INFO = "ROSCOMMAND_INFO";
        public const string ROSCOMMAND_DEFAULT = "ROSCOMMAND_DEFAULT";
        public const string ROSCOMMAND_OPT = "ROSCOMMAND_OPT";
        public const string ROSCOMMAND_STORY = "ROSCOMMAND_STORY";
        public const string ROSCOMMAND_ACTION = "ROSCOMMAND_ACTION";
        public const string ROSCOMMAND_SNAP = "ROSCOMMAND_SNAP";
        ///* Item for status message request */
        public const string MESSAGE_STATUS = "MESSAGE_STATUS";
        public const string MESSAGE_IDANDSERVICES = "MESSAGE_IDS";
        public const string MESSAGE_PERFORMANCE = "MESSAGE_PERFORM";
        public const string MESSAGE_GLOBALSANDCONNECTED = "MESSAGE_GLOBALS";
        public const string MESSAGE_REMOTEDIR = "MESSAGE_REMOTEDIR";
        public const string MESSAGE_NODATA = "MESSAGE_NODATA";
        public const string MESSAGE_NOTAUTHORIZED = "MESSAGE_NOTAUTH";
        public const string MESSAGE_BADSYMBOL = "MESSAGE_BADSYM";
        public const string MESSAGE_NEEDPASSWORD = "MESSAGE_NEEDPSW";
        public const string MESSAGE_NOTAUTHSOL = "MESSAGE_NOTSOL";
        public const string MESSAGE_NOTAUTHORIZEDUSER = "MESSAGE_NOTAUTHUSER";
        public const string MESSAGE_ENDOFDATA = "MESSAGE_ENDOFDATA";
        public const string MESSAGE_ENDOFSERVERLIST = "MESSAGE_ENDOFSERVERLIST";
        public const string MESSAGE_DISCONNECT = "MESSAGE_DISCONNECT";
        ///* Message Type defines */
        public const short WRMESSAGE_MESSAGE = 0;
        public const short WRMESSAGE_WARNING = 1;
        public const short WRMESSAGE_ERROR = 2;
        ///* Message Id defines and explanation */
        ///* No arguments used for the following */
        public const short WRMESSAGE_ID_ENDOFDATA = 1; ///* Signifies end of a list_ data. No arguments used */
        public const short WRMESSAGE_ID_STATUS = 2; ///* Status message. Arguments used, ARG1 and ARG2 */
        public const short WRMESSAGE_ID_INVALIDSYMBOL = 3; ///* Invalid symbol entered via dde request or advise */
        public const short WRMESSAGE_ID_NODATA = 4; ///* No data for symbol in a request */
        public const short WRMESSAGE_ID_NOPASSWORDFORMORESYMBOLS = 5; ///* No password for shifting into global output from receiver */
        public const short WRMESSAGE_ID_ROSRECORD = 6; ///* Indicates that the message is a ros record in old ascii format */
        public const short WRMESSAGE_ID_FILENAME = 7; ///* Indicates filename of received file */
        public const short WRMESSAGE_ID_FILEPATH = 8; ///* Indicates filepath -including computer name or ftp address */
        public const short WRMESSAGE_ID_NEWRECORD = 9; ///* Indicates new record for winsock support */
        public const short WRMESSAGE_ID_NOTAUTHORIZED = 10; ///* Indicates need authorization for kind of symbol */
        public const short WRMESSAGE_ID_NOTAUTHSOL = 11; ///* Indicates signal online service and symbol limit has been reachec */
        public const short WRMESSAGE_ID_NEEDHIGHERSYMBOLCOUNT = 12; ///* Need higher symbol count in password NEW 2/11/97 */
        public const short WRMESSAGE_ID_NEEDHIGHERSYMBOLTYPE = 13; ///* Need higher symbol type auth. symbol count password doesnt allow wildcard or globals */
        public const short WRMESSAGE_ID_NOTAUTHORIZEDUSER = 14; ///* Not authorized for user authorized in SOL */
        public const short WRMESSAGE_ID_ENDOFSERVERLIST = 15; ///* Client winros received and end of list from server winros */
        public const short WRMESSAGE_ID_DISCONNECT = 16; ///* Client should disconnect, Arg1 will give the reason */
        // ' the following is used for wrmessage_id_status
        //    /* ARG1 = status bits. ( Notice these defines of bits are the same as STAT1 (system status byte) shifted
        //        left 8 bits, ored with STAT2 (application status byte) of the ROS manual of old. */
        ///* The following defines are for ARG1 of wrmessage_id_status message */
        ///* ARG1 is a bitmap with the following bits assigned */
        public const short ROSMESSAGE_NOPASSWORD = 0x2000; ///* No password or password error */
        public const short ROSMESSAGE_FATAL = 0x1000; ///* Fatal Receiver error */
        public const short ROSMESSAGE_EXPIRE = 0x800; ///* Password has expired */
        public const short ROSMESSAGE_GRACE = 0x400; ///* Grace for password */
        public const short ROSMESSAGE_ADJUST = 0x200; ///* Bad CRC on packet to receiver -- Reception */
        public const short ROSMESSAGE_OK = 0x100; ///* Good header receiver on packet to receiver-- Reception */
        public const short ROSMESSAGE_BADDOWNLOAD = 0x10; ///* Receiver download checksum error */
        public const short ROSMESSAGE_ROSPRESENT = 0x1; ///* Ros downloaded into receiver */
                                                        ///* The client should delete its headlines database, if it has any if the folowing bit is set */
        public const int ROSMESSAGE_DELETEHEADLINES = unchecked((int)0x80000000); ///* Winros has deleted its headlines database */
                                                                                  ///* The client should delete its newsalerts database, if it has any if the folowing bit is set */
        public const int ROSMESSAGE_DELETENEWSALERTS = 0x40000000; ///* Winros has deleted its newsalerts database */
                                                                   ///* The client is informed by the following bit that LIST_ items will not responed until this bit is cleared */
        public const int ROSMESSAGE_DATABASERESET = 0x20000000; ///* Winros is resetting its database */
                                                                ///* The client is informed by the following bit that the User should checkout winros */
                                                                ///* Something has happened with the receiver,password, or comm */
        public const int ROSMESSAGE_CHECKOUTWINROS = 0x10000000;
        ///*  Winros has determined that it is out of disk space on its primary drive */
        public const int ROSMESSAGE_DISKFULL = 0x8000000;
        ///*  This state mask masks off states that reset after outputting once */
        public const int MESSAGE_ARG1_STATEMASK = 0xFFFFFF;
        ///* ARG2 is a value with the following meanings */
        public const short MAXMODES = 21;
        public const short SIGNALMODEOK = 0;
        public const short SIGNALMODESTARTUP = 1;
        public const short SIGNALMODEUNITTEST = 2;
        public const short SIGNALMODEDOWNLOAD = 3;
        public const short SIGNALMODENOUNIT = 4;
        public const short SIGNALMODEERROR = 5;
        public const short SIGNALMODETIMEOUT = 6;
        public const short SIGNALMODESUSPENSION = 7;
        public const short SIGNALMODEFINETUNE = 8;
        public const short SIGNALMODENORECEIVER = 9;
        public const short SIGNALMODEBADDOWNLOAD = 10;
        public const short SIGNALMODECOMMTIMEOUT = 11;
        public const short SIGNALMODEDISCONNECTED = 12;
        public const short SIGNALMODERECONNECTING = 13;
        public const short SIGNALMODEDELAYAFTERDOWNLOAD = 14;
        public const short SIGNALMODEACTIVATINGROS = 15;
        public const short SIGNALMODEDELAYAFTERU = 16;
        public const short SIGNALMODESENDINGPASSWORD = 17;
        public const short SIGNALMODEINTERRUPTED = 18;
        public const short SIGNALMODEWRONGBOX = 19;
        public const short SIGNALMODEAUTOSCAN = 20;

        //K is the key formatting character
        //A means that the input is eSignal format 
        //B means that the input is IDCO-22
        //Z means that we want to get OSI-21 and IDCO-22 format in the snapshot
        public const string OPTIONREQUESTFORMAT_ESIGNAL = @"[F\|KAZ]";
        public const string OPTIONREQUESTFORMAT_IDCO = @"[F\|KBZ]";

        ///*
        // ExchangeListed:         Exchange groups:    Exchange Name:
        // NYSE                            "ALL_NYSE"      New York Stock Exchange
        // AMEX                            "ALL_AMEX"      American Stock Exchange
        // NASD                            "ALL_NASD"      Nasdaq Stock Exchange
        // CME                         "ALL_CME"       Chicago Mercantile
        // COMX                            "ALL_COMX"      Comex
        // CBT                         "ALL_CBT"       Chicage Board of Trade
        // KCBT                            "ALL_KCBT"      Kansas City Board of Trade
        // NYME                            "ALL_NYME"      New York Mercantile Exchange
        // MIDA                            "ALL_MIDA"      Mid-Am
        // CEC                         "ALL_CEC"       Coffee, Cotton, Sugar, Cocao
        // TC                          "ALL_TC"        Toronto Stock
        // VC                          "ALL_VC"        Vancouver Stock
        // MC                          "ALL_MC"        Montreal Stock
        // AC                          "ALL_AC"        Alberta Stock
        // OC                          "ALL_OC"        Ontario Stock
        // LIFE                            "ALL_LF"        London International Futures
        // MATF                            "ALL_MA"        Paris
        // LME                         "ALL_LM"        London Metal
        // LCE                         "ALL_LC"        London Commodity
        // IPE                         "ALL_IP"        International Petroleum
        // SIMX                            "ALL_SI"        Singapore
        // ATA                         "ALL_AT"        Amsterdam
        // FX                          "ALL_FX"        CrossMar FX
        // GV                          "ALL_GV"        Garvin Cash Bonds
        // TS                              "ALL_TS"        Tokyo Sugar
        // TG                              "ALL_TG"        Tokyo Grain
        // TO                              "ALL_TO"        Tokyo Commodity
        // SA                              "ALL_SA"        Shanghai Agricultural
        // SM                              "ALL_SM"        Shanghai Metals
        // SZ                              "ALL_SZ"        Shezhuan
        // MX                              "ALL_MX"        Mexico
        // Exchange traded exchanges
        // AMEX                                                American Stock Exchange
        // BSE                                                 Boston Stock Exchange
        // CSE                                                 Cincinatti Stock Exchange
        // MSE                                                 Midwest Stock Exchange
        // NYSE                                                    New York Stock Exchange
        // INST Instanet
        // PSE                                                 Pacific Stock Exchange
        // NASD Nasdaq
        // CBOE                                                    Chicago Board Options Exchange
        // PHIL                                                        Philadelphia Stock Exchange
        // */
    }
}
