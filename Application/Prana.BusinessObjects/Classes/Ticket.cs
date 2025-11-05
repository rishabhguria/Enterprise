namespace Prana.BusinessObjects
{
    public enum EnumTicketType
    {
        OPTradingTicket = 1,
        OGTradingTicket = 2
    }
    /// <summary>
    /// Summary description for Ticket.
    /// </summary>
    public class Ticket
    {
        int _ticketID = int.MinValue;
        string _ticketName = string.Empty;
        string _displayName = string.Empty;
        //		int _type 	= int.MinValue;
        EnumTicketType _ticketType = EnumTicketType.OGTradingTicket;




        public int TicketID
        {
            get
            {
                return _ticketID;
            }

            set
            {
                _ticketID = value;
            }
        }

        public string TicketName
        {
            get
            {
                return _ticketName;
            }

            set
            {
                _ticketName = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }

            set
            {
                _displayName = value;
            }
        }


        public EnumTicketType TicketType
        {
            get
            {
                return _ticketType;
            }

            set
            {
                _ticketType = value;
            }
        }
    }


}
