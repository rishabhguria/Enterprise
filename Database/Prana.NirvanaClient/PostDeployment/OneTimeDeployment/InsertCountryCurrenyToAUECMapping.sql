truncate table T_DefaultAUECMapping

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='AED' AND CountryName ='United Arab Emirates' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='ADX-Equity')) then 'ADX-Equity' 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='DFM-Equity')) then 'DFM-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='NQD-Equity')) then 'NQD-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='ARS' AND CountryName ='Argentina' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='BUE-Equity')) then 'BUE-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='AUD' AND CountryName ='Australia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='ASX-Equity')) then 'ASX-Equity' 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='CXA-Equity')) then 'CXA-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='BGN' AND CountryName ='Bulgaria' AND 
ExchangeIdentifier =case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='BUL-Equity')) then 'BUL-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='BHD' AND CountryName ='Bahrain' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='BAH-Equity')) then 'BAH-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='BRL' AND CountryName ='Brazil' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='BSP-Equity')) then 'BSP-Equity' 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='SMA-Equity')) then 'SMA-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='CAD' AND CountryName ='Canada' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='CN-Equity')) then 'CN-Equity' 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='QNL-Equity')) then 'QNL-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='TC-Equity')) then 'TC-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='CHF' AND CountryName ='Switzerland' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='BRN-Equity')) then 'BRN-Equity' 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='SWS-Equity')) then 'SWS-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='SWX-Equity')) then 'SWX-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='CLP' AND CountryName ='Chile' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='SGO-Equity')) then 'SGO-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='CNY' AND CountryName ='China' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='SHE-Equity')) then 'SHE-Equity' 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='SHG-Equity')) then 'SHG-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='COP' AND CountryName ='Colombia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='BVC-Equity')) then 'BVC-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='CZK' AND CountryName ='Czech Republic' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='PRA-Equity')) then 'PRA-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='EGP' AND CountryName ='Egypt' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='EGX-Equity')) then 'EGX-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='EUR' AND CountryName ='Austria' AND 
ExchangeIdentifier =  case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='VIE-Equity')) then 'VIE-Equity' 
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='EUR' AND CountryName ='Belgium' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='BELs-Equity')) then 'BELs-Equity' 
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='EUR' AND CountryName ='Cyprus' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='CYS-Equity')) then 'CYS-Equity' 
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='EUR' AND CountryName ='Denmark' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='CSE-Equity')) then 'CSE-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='EUR' AND CountryName ='Estonia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='OMB-Equity')) then 'OMB-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='EUR' AND CountryName ='Finland' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='HEL-Equity')) then 'HEL-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='EUR' AND CountryName ='France' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='EEB-Equity')) then 'EEB-Equity'
else '' END

INSERT INTO T_DefaultAUECMapping SELECT o.CountryID, u.CurrencyID, a.AUECID FROM T_Currency u, T_Country o, T_AUEC a WHERE CurrencySymbol ='EUR' AND CountryName ='Germany' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='DBG-Equity')) then 'DBG-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='DBR-Equity')) then 'DBR-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='STU-Equity')) then 'STU-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='EUR' AND CountryName ='Greece' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='ATH-Equity')) then 'ATH-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='EUR' AND CountryName ='Ireland' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='DUB-Equity')) then 'DUB-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='EUR' AND CountryName ='Italy' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='MIL-Equity')) then 'MIL-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='EUR' AND CountryName ='Slovenia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='LJS-Equity')) then 'LJS-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='EUR' AND CountryName ='Spain' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='MAC-Equity')) then 'MAC-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='MRE-Equity')) then 'MRE-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='GBP' AND CountryName ='England' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='LON-Equity')) then 'LON-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='LSIN-Equity')) then 'LSIN-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='GBP' AND CountryName ='France' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='FTSE-Equity')) then 'FTSE-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='GBX' AND CountryName ='England' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'LSEP')) then 'LSEP'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='HKD' AND CountryName ='Hong Kong' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'HKG-Equity')) then 'HKG-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'SEHK-Equity')) then 'SEHK-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='HRK' AND CountryName ='Croatia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'ZAG-Equity')) then 'ZAG-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='HUF' AND CountryName ='Hungary' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'BUD-Equity')) then 'BUD-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='IDR' AND CountryName ='Indonesia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'JKT-Equity')) then 'JKT-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='ILS' AND CountryName ='Israel' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'TAE-Equity')) then 'TAE-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'TASE-Equity')) then 'TASE-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='INR' AND CountryName ='India' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'BOM-Equity')) then 'BOM-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'NSE-Equity')) then 'NSE-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='JOD' AND CountryName ='Jordan' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'AMM-Equity')) then 'AMM-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='JPY' AND CountryName ='Japan' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='JAQ-Equity')) then 'JAQ-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='NAG-Equity')) then 'NAG-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='OSE-Equity')) then 'OSE-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='KES' AND CountryName ='Kenya' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'NAI-Equity')) then 'NAI-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='KRW' AND CountryName ='South Korea' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='KOE-Equity')) then 'KOE-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='KON-Equity')) then 'KON-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier='KOR-Equity')) then 'KOR-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='KWF' AND CountryName ='Kuwait' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'KUW-Equity')) then 'KUW-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='LKR' AND CountryName ='SriLanka' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'COL-Equity')) then 'COL-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='MAD' AND CountryName ='Morocco' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'CAS-Equity')) then 'CAS-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='MXN' AND CountryName ='Brazil' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'MEX-Equity')) then 'MEX-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='MXN' AND CountryName ='United States' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'CHX-Equity')) then 'CHX-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='MYR' AND CountryName ='Malaysia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'KLS-Equity')) then 'KLS-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='NGN' AND CountryName ='Nigeria' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'NSA-Equity')) then 'NSA-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='NOK' AND CountryName ='Norway' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'OSL-Equity')) then 'OSL-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='NZD' AND CountryName ='Australia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'NZX-Equity')) then 'NZX-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='OMR' AND CountryName ='Oman' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'MSM-Equity')) then 'MSM-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='PEN' AND CountryName ='Peru' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'LIM-Equity')) then 'LIM-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='PHP' AND CountryName ='Philippines' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'PHS-Equity')) then 'PHS-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='PLN' AND CountryName ='Poland' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'WAR-Equity')) then 'WAR-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'WSC-Equity')) then 'WSC-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='QAR' AND CountryName ='Qatar' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'DSM-Equity')) then 'DSM-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='RON' AND CountryName ='Romania' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'BVB-Equity')) then 'BVB-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='RSD' AND CountryName ='Serbia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'BEL-Equity')) then 'BEL-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='RUB' AND CountryName ='Russia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'MIS-Equity')) then 'MIS-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'MOS-Equity')) then 'MOS-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='SAR' AND CountryName ='Saudi Arabia' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'SAU-Equity')) then 'SAU-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='SEK' AND CountryName ='Sweden' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'OMX-Equity')) then 'OMX-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='SGD' AND CountryName ='Singapore' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'SES-Equity')) then 'SES-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'SESDAQ-Equity')) then 'SESDAQ-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='THB' AND CountryName ='Thailand' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'SET-Equity')) then 'SET-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='TRY' AND CountryName ='Turkey' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'IST-Equity')) then 'IST-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'TUR-Equity')) then 'TUR-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='TWD' AND CountryName ='Taiwan' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'GTS-Equity')) then 'GTS-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'TAI-Equity')) then 'TAI-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='USD' AND CountryName ='United States' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'NASD-Equity')) then 'NASD-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'AMEX-Equity')) then 'AMEX-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'NYSE-Equity')) then 'NYSE-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='VES' AND CountryName ='Venezula' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'CAR-Equity')) then 'CAR-Equity'
else '' END 

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='VND' AND CountryName ='Vietnam' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'HCM-Equity')) then 'HCM-Equity'
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'HNX-Equity')) then 'HNX-Equity'
else '' END

insert into T_DefaultAUECMapping select o.CountryID ,u.CurrencyID , a.AUECID from T_Currency u, T_Country o , T_AUEC a where CurrencySymbol ='ZAR' AND CountryName ='South Africa' AND 
ExchangeIdentifier = case 
when exists(select * from T_CompanyAUEC where AUECID=( select AUECID from T_AUEC where ExchangeIdentifier= 'JSE-Equity')) then 'JSE-Equity'
else '' END 