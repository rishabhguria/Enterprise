Set NOCOUNT On;

Delete From T_W_Indices 
Delete From T_W_BatchPreferences 
Delete From T_W_RegistrationClientMapping 
Delete From T_W_Registration 
Delete From T_W_ClientFundMapping 
Delete From T_W_Funds 
Delete From T_W_Clients 

Insert Into T_W_Indices (Symbol,Quotes,IsVAMI,IsPeriod,IsBenchmark,EODSymbol,Source)
Select '$PX1-EEB','CAC 40','1','0','0','$PX1-EEB',0
Union Select '$COMPQ','NASDAQ COMPOSITE','1','0','0','$COMPQ',0
Union Select '$INDU','DOW JONES INDUSTRIAL AVERAGE','1','0','0','$INDU',0
Union Select '$UKX-FTSE','FTSE 100','1','0','0','$UKX-FTSE',0
Union Select '$HSI-HKG','HANG SENG INDEX','1','0','0','$HSI-HKG',0
Union Select '$N225-NKI','NIKKEI 225','1','0','0','$N225-NKI',0
Union Select '$OEX','S&P 100','1','0','0','$OEX',0
Union Select 'HCX A0','S&P 500 HEALTHCARE','1','0','0','HCX A0',0
Union Select 'XLB','MATERIALS SELECT SECTOR SPDR FUND','1','0','0','XLB',0
Union Select 'XLE','ENERGY SELECT SECTOR SPDR FUND','1','0','0','XLE',0
Union Select 'XLF','FINANCIAL SELECT SECTOR SPDR FUND','1','0','0','XLF',0
Union Select 'XLI','INDUSTRIAL SELECT SECTOR SPDR FUND','1','0','0','XLI',0
Union Select 'XLK','TECHNOLOGY SELECT SECTOR SPDR FUND','1','0','0','XLK',0
Union Select 'XLP','CONSUMER STAPLES SELECT SECTOR SPDR FUND','1','0','0','XLP',0
Union Select 'XLU','UTILITIES STAPLES SELECT SECTOR SPDR FUND','1','0','0','XLU',0
Union Select 'XLV','HEALTH CARE SELECT SECTOR SPDR FUND','1','0','0','XLV',0
Union Select 'XLY','CONSUMER DISCRETIONARY SELECT SECTOR SPDR FUND','1','0','0','XLY',0
Union Select '$SPX','S&P 500 INDEX','1','1','1','$SPX',0
Union Select '$KRX','KBW REGIONAL BANKING INDEX','1','0','0','$KRX',0
Union Select '$BANK','NASDAQ BANKING','1','0','0','$BANK',0
Union Select '$BKX','KBW BANK INDEX','1','0','0','$BKX',0
Union Select 'PXT A0','S&P 500 TOTAL RETURN','1','0','0',Null,0
Union Select 'LIB M3-BA','US DOLLAR LIBOR','0','0','0','LIB M3-BA',0
Union Select '$RUT','RUSSELL 2000','1','0','0','$RUT',0
Union Select 'XBI','SPDR S&P BIOTECH ETF','1','0','0',Null,0 
Union Select '$TSX50.','50%TSX/50% Cash','1','0','0',Null,1

Insert Into T_W_BatchPreferences (ReportID)
Select 'Touch'

Insert Into T_W_Funds 
(PranaFundID) 
Select CompanyFundID From T_CompanyFunds 

Update D Set StartDate = Case When S.CashMgmtStartDate Is Null Then Null Else DateAdd(d,1,S.CashMgmtStartDate) End 
From T_W_Funds D Left Outer Join T_CashPreferences S On D.PranaFundID = S.FundID 

Insert Into T_W_Clients 
(ClientName,ClientAlias) 
Select '{@client@}','{@client@}'  

Insert Into T_W_ClientFundMapping 
(ClientID,TouchFundID) 
Select ClientID,TouchFundID From T_W_Clients,T_W_Funds 

Insert Into T_W_Registration 
(RegistrationName) 
Select '{@client@}' 

Insert Into T_W_RegistrationClientMapping 
(RegistrationID,ClientID) 
Select D.RegistrationID,S.ClientID From T_W_Registration D,T_W_Clients S