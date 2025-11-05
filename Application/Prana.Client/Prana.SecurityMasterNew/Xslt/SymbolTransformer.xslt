<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now1(int year, int month)
    {
    DateTime thirdWednesday= new DateTime(year, month, 15);
    while (thirdWednesday.DayOfWeek != DayOfWeek.Wednesday)
    {
    thirdWednesday = thirdWednesday.AddDays(1);
    }
    return thirdWednesday.ToString();
    }
  </msxsl:script>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now2(int year, int month)
    {
    DateTime firstWednesday= new DateTime(year, month, 1);
    while (firstWednesday.DayOfWeek != DayOfWeek.Wednesday)
    {
    firstWednesday = firstWednesday.AddDays(1);
    }
    return firstWednesday.ToString();
    }
  </msxsl:script>

  <xsl:template name="MonthCodeEQOPT">
    <xsl:param name="varMonth"/>
    <xsl:param name="varPutCall"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth = 1 and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=2 and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 3 and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 4 and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 5 and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 6 and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 7 and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=8 and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 9 and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 10 and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 11 and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 12 and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 1 and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 2 and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 3 and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 4 and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 5 and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth =6 and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 7 and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 8 and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 9 and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 10 and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 11 and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonth = 12 and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="temp_MonthExpireCode">
    <xsl:param name="param_MonthExpireCode"/>
    <xsl:choose>
      <xsl:when test ="$param_MonthExpireCode='01'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='02'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='03'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='04'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='05'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='06'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='07'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='08'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='09'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='10'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='11'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthExpireCode='12'">
        <xsl:value-of select ="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="$param_MonthExpireCode"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="tempCurrencyCode">
    <xsl:param name="paramCurrencySymbol"/>
    <!-- 1 characters for metal code -->
    <!--  e.g. A represents A = aluminium-->
    <xsl:choose>
      <xsl:when test ="$paramCurrencySymbol='USD'">
        <xsl:value-of select ="'1'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='HKD'">
        <xsl:value-of select ="'2'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='JPY'">
        <xsl:value-of select ="'3'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='GBP'">
        <xsl:value-of select ="'4'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='AED'">
        <xsl:value-of select ="'5'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='BRL'">
        <xsl:value-of select ="'6'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='CAD'">
        <xsl:value-of select ="'7'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='EUR'">
        <xsl:value-of select ="'8'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='NOK'">
        <xsl:value-of select ="'9'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='SGD'">
        <xsl:value-of select ="'10'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='MUL'">
        <xsl:value-of select ="'11'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='ZAR'">
        <xsl:value-of select ="'12'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='SEK'">
        <xsl:value-of select ="'13'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='AUD'">
        <xsl:value-of select ="'14'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='CNY'">
        <xsl:value-of select ="'15'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='KRW'">
        <xsl:value-of select ="'16'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='BDT'">
        <xsl:value-of select ="'17'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='THB'">
        <xsl:value-of select ="'18'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='dong'">
        <xsl:value-of select ="'19'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='GBX'">
        <xsl:value-of select ="'20'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='INR'">
        <xsl:value-of select ="'21'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='CHF'">
        <xsl:value-of select ="'23'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='CLP'">
        <xsl:value-of select ="'24'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='COP'">
        <xsl:value-of select ="'25'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='CZK'">
        <xsl:value-of select ="'26'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='DKK'">
        <xsl:value-of select ="'27'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='GHS'">
        <xsl:value-of select ="'28'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='HUF'">
        <xsl:value-of select ="'29'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='IDR'">
        <xsl:value-of select ="'30'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='ILS'">
        <xsl:value-of select ="'31'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='ISK'">
        <xsl:value-of select ="'32'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='KZT'">
        <xsl:value-of select ="'33'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='LVL'">
        <xsl:value-of select ="'34'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='MXN'">
        <xsl:value-of select ="'35'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='NZD'">
        <xsl:value-of select ="'36'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='PEN'">
        <xsl:value-of select ="'37'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='PLN'">
        <xsl:value-of select ="'38'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='RON'">
        <xsl:value-of select ="'40'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='RUB'">
        <xsl:value-of select ="'41'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='SKK'">
        <xsl:value-of select ="'42'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='TRY'">
        <xsl:value-of select ="'43'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='ARS'">
        <xsl:value-of select ="'44'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='UYU'">
        <xsl:value-of select ="'45'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='TWD'">
        <xsl:value-of select ="'46'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='BMD'">
        <xsl:value-of select ="'47'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='EEK'">
        <xsl:value-of select ="'48'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='GEL'">
        <xsl:value-of select ="'49'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='MYR'">
        <xsl:value-of select ="'51'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='SIT'">
        <xsl:value-of select ="'52'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='XAF'">
        <xsl:value-of select ="'53'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='XOF'">
        <xsl:value-of select ="'54'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='AZN'">
        <xsl:value-of select ="'55'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='PKR'">
        <xsl:value-of select ="'56'"/>
      </xsl:when>
      <xsl:when test ="$paramCurrencySymbol='PHP'">
        <xsl:value-of select ="'57'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="temp_MonthNumberFromCode">
    <xsl:param name="param_MonthNumberFromCode"/>
    <xsl:choose>
      <xsl:when test ="$param_MonthNumberFromCode='F'">
        <xsl:value-of select ="'01'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='G'">
        <xsl:value-of select ="'02'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='H'">
        <xsl:value-of select ="'03'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='J'">
        <xsl:value-of select ="'04'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='K'">
        <xsl:value-of select ="'05'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='M'">
        <xsl:value-of select ="'06'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='N'">
        <xsl:value-of select ="'07'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='Q'">
        <xsl:value-of select ="'08'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='U'">
        <xsl:value-of select ="'09'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='V'">
        <xsl:value-of select ="'10'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='X'">
        <xsl:value-of select ="'11'"/>
      </xsl:when>
      <xsl:when test ="$param_MonthNumberFromCode='Z'">
        <xsl:value-of select ="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="$param_MonthNumberFromCode"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'JP'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'SW'">
        <xsl:value-of select="'-SWX'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'BB'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CN'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'LN'">
        <xsl:value-of select="'-LON'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'SS'">
        <xsl:value-of select="'-OMX'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'GY'">
        <xsl:value-of select="'-MUN'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'HK'">
        <xsl:value-of select="'-HKG'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'GR'">
        <xsl:value-of select="'-FRA'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'NO'">
        <xsl:value-of select="'-OSL'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'BZ'">
        <xsl:value-of select="'-BSP'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'IM'">
        <xsl:value-of select="'-MIL'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CH'">
        <xsl:value-of select="'-SHG'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'BZ'">
        <xsl:value-of select="'-BSP'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>


      <xsl:for-each select="//SecMasterRequest">

        <xsl:if test="TickerSymbol ='' or TickerSymbol ='*'">
          <SecMasterRequest>

            <xsl:variable name="varException">
              <xsl:choose>
                <xsl:when test="contains(COL28,'XXXXXXX')!= false">
                  <xsl:value-of select="'XXXXXXX'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name ="varCOL35">
              <xsl:value-of select ="BloombergSymbol"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:choose>
                <xsl:when test="string-length(substring-after($varCOL35, ' ')) = 2">
                  <xsl:value-of select="'EQT'"/>
                </xsl:when>
                <xsl:when test="(substring(substring-after(substring-after($varCOL35, ' '),' '),1,1) = 'P' or substring(substring-after(substring-after($varCOL35, ' '),' '),1,1) = 'C') and number(substring(substring-after(substring-after($varCOL35, ' '),' '),2))">
                  <xsl:value-of select="'EOPT'"/>
                </xsl:when>
                <xsl:when test="string-length(substring-before(normalize-space($varCOL35),' ')) = 1 and number(substring-before(substring-after(substring-after(normalize-space($varCOL35),' '),' '),' ')) and string-length(substring-before(substring-after(substring-after(normalize-space($varCOL35),' '),' '),' ')) &lt; 5">
                  <xsl:value-of select="'FOPT'"/>
                </xsl:when>
                <xsl:when test="string-length(substring-before(normalize-space($varCOL35),' ')) != 1 and number(substring-before(substring-after(normalize-space($varCOL35),' '),' ') and string-length(substring-before(substring-after(normalize-space($varCOL35),' '),' ')) &lt; 6)">
                  <xsl:value-of select="'FOPT'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'FUT'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <!--<xsl:variable name="varBBCode">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
									<xsl:value-of select="substring-before(normalize-space($varCOL35),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring(substring-before(normalize-space($varCOL35),' '),1,string-length(substring-before(normalize-space($varCOL35),' '))-2)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->

            <xsl:variable name="varBBCode">
              <xsl:choose>
                <xsl:when test="$varAssetType='FUT'">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
                      <xsl:value-of select="substring-before(normalize-space($varCOL35),' ')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring(substring-before(normalize-space($varCOL35),' '),1,string-length(substring-before(normalize-space($varCOL35),' '))-2)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
                      <xsl:value-of select="substring-before(normalize-space($varCOL35),' ')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring(substring-before(normalize-space($varCOL35),' '),1,string-length(substring-before(normalize-space($varCOL35),' '))-3)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <!--<xsl:variable name ="varBBKey">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
									<xsl:value-of select="substring-after(substring-after(normalize-space($varCOL35),' '),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-after(normalize-space($varCOL35),' ')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->

            <xsl:variable name ="varBBKey">
              <xsl:choose>
                <xsl:when test="$varAssetType='FUT'">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
                      <xsl:value-of select="translate(substring-after(substring-after(normalize-space($varCOL35),' '),' '),$varSmall,$varCapital)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="translate(substring-after(normalize-space($varCOL35),' '),$varSmall,$varCapital)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
                      <xsl:value-of select="translate(substring-after(substring-after(substring-after(normalize-space($varCOL35),' '),' '),' '),$varSmall,$varCapital)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="translate(substring-after(substring-after(normalize-space($varCOL35),' '),' '),$varSmall,$varCapital)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varUnderlying">
              <xsl:choose>
                <xsl:when test="$varAssetType = 'EOPT'">
                  <xsl:value-of select="substring-before($varCOL35, ' ')"/>
                </xsl:when>
                <xsl:when test="$varException != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@UnderlyingCode"/>
                </xsl:when>
                <xsl:when test="$varBBCode != '' and $varBBKey != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@UnderlyingCode"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varBBCode"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varStrikeMul">
              <xsl:choose>
                <xsl:when test="$varException != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@StrikeMul"/>
                </xsl:when>
                <xsl:when test="$varBBCode != '' and $varBBKey != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@StrikeMul"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varExpFlag">
              <xsl:choose>
                <xsl:when test="$varException != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@ExpFlag"/>
                </xsl:when>
                <xsl:when test="$varBBCode != '' and $varBBKey != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExpFlag"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varMulFlag">
              <xsl:choose>
                <xsl:when test="$varException != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@MulFlag"/>
                </xsl:when>
                <xsl:when test="$varBBCode != '' and $varBBKey != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@MulFlag"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varExchangeCode">
              <xsl:choose>
                <xsl:when test="$varException != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@ExchangeCode"/>
                </xsl:when>
                <xsl:when test="$varBBCode != '' and $varBBKey != ''">
                  <xsl:value-of select="document('BBCodeMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExchangeCode"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varBBSymbolBeforeKey">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
                  <xsl:value-of select="substring-before(substring-after(normalize-space($varCOL35),' '),' ')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(normalize-space($varCOL35),' ')"/>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:variable>

            <xsl:variable name="varStrikeEOPT">
              <xsl:choose>
                <xsl:when test="$varAssetType = 'EOPT'">
                  <xsl:value-of select="format-number(substring(substring-after(substring-after($varCOL35, ' '),' '),2),'#.00')"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varExpiryEOPT">
              <xsl:value-of select="substring-before(substring-after(normalize-space($varCOL35),' '),' ')"/>
            </xsl:variable>

            <xsl:variable name="varPutCall">
              <xsl:value-of select="substring(substring-after(substring-after($varCOL35, ' '),' '),1,1)"/>
            </xsl:variable>

            <xsl:variable name="varExDay">
              <xsl:value-of select="substring($varExpiryEOPT,4,2)"/>
            </xsl:variable>

            <xsl:variable name="varExMonth">
              <xsl:value-of select="substring($varExpiryEOPT,1,2)"/>
            </xsl:variable>

            <xsl:variable name="varExYear">
              <xsl:value-of select="substring($varExpiryEOPT,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varMonthCode">
              <xsl:call-template name="MonthCodeEQOPT">
                <xsl:with-param name="varMonth" select="$varExMonth"/>
                <xsl:with-param name="varPutCall" select="$varPutCall"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varExpiryDay">
              <xsl:choose>
                <xsl:when test="substring($varExDay,1,1)= '0'">
                  <xsl:value-of select="substring($varExDay,2,1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varExDay"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name='varThirdFriday'>
              <xsl:choose>
                <xsl:when test='number($varExYear) and number($varExMonth)'>
                  <xsl:value-of select='my:Now(number(concat("20",$varExYear)),number($varExMonth))'/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select='""'/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varStrikeFUT">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(normalize-space($varCOL35),' ')) = 1">
                  <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space($varCOL35),' '),' '),' ')"/>
                </xsl:when>
                <xsl:when test="string-length(substring-before(normalize-space($varCOL35),' ')) != 1">
                  <xsl:value-of select="substring-before(substring-after(normalize-space($varCOL35),' '),' ')"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varStrikeFUTOPT">
              <xsl:choose>
                <xsl:when test="$varStrikeMul = ''">
                  <xsl:value-of select="$varStrikeFUT"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varStrikeFUT*($varStrikeMul)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varMonthExpireCode">
              <xsl:value-of select ="substring($varBBSymbolBeforeKey,((string-length($varBBSymbolBeforeKey) - 2) + 1),2)"/>
            </xsl:variable>

            <xsl:variable name="varMonthExpireCodeOPT">
              <xsl:value-of select ="substring($varBBSymbolBeforeKey,((string-length($varBBSymbolBeforeKey) - 3) + 1),3)"/>
            </xsl:variable>


            <!--
					<CODEDISPLAY>
						<xsl:value-of select ="substring-before(my:Now2(number(concat('201',substring($varMonthExpireCodeOPT,2,1))),number($varMonthNo1)),' ')"/>
					</CODEDISPLAY>-->

            <xsl:variable name="varSuffix">
              <xsl:call-template name="GetSuffix">
                <xsl:with-param name="Suffix" select="substring-after($varCOL35, ' ')"/>
              </xsl:call-template>
            </xsl:variable>

            <ConvertedSymbol>
              <xsl:choose>

                <!--For Equities-->
                <xsl:when test="$varAssetType = 'EQT'">
                  <xsl:choose>
                    <xsl:when test="substring-after($varCOL35, ' ') = 'US'">
                      <xsl:value-of select="substring-before($varCOL35, ' ')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat(substring-before($varCOL35, ' '), $varSuffix)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>

                <!--For Equity Options-->
                <xsl:when test="$varAssetType = 'EOPT'">
                  <xsl:choose>
                    <xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
                      <xsl:value-of select="normalize-space(concat('O:', $varUnderlying, ' ', $varExYear,$varMonthCode,$varStrikeEOPT))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="normalize-space(concat('O:', $varUnderlying, ' ', $varExYear,$varMonthCode,$varStrikeEOPT,'D',$varExpiryDay))"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>

                <!--For Future Options-->
                <xsl:when test="$varAssetType = 'FOPT'">
                  <xsl:choose>
                    <xsl:when test ="$varExchangeCode != '-LME' and $varExpFlag != '1'">
                      <xsl:value-of select="concat($varUnderlying, ' ',$varMonthExpireCodeOPT, $varStrikeFUTOPT, $varExchangeCode)"/>
                    </xsl:when>
                    <xsl:when test ="$varExchangeCode != '-LME' and $varExpFlag = '1'">
                      <xsl:value-of select="concat($varUnderlying, ' ',substring($varMonthExpireCodeOPT,2,1),substring($varMonthExpireCodeOPT,1,1),substring($varMonthExpireCodeOPT,3,1), $varStrikeFUTOPT, $varExchangeCode)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <!-- Get Month number from month code-->
                      <xsl:variable name="varMonthNo">
                        <xsl:call-template name="temp_MonthNumberFromCode">
                          <xsl:with-param name="param_MonthNumberFromCode" select="substring($varMonthExpireCodeOPT,1,1)"/>
                        </xsl:call-template>
                      </xsl:variable>
                      <!-- Get First Wednesday-->
                      <xsl:variable name ="varExpireDate">
                        <xsl:value-of select =" substring-before(my:Now2(number(concat('201',substring($varMonthExpireCodeOPT,2,1))),number($varMonthNo)),' ')"/>
                      </xsl:variable>
                      <!-- PBD 3J3P2000-LME-->
                      <xsl:value-of select="concat($varUnderlying,' ',substring($varMonthExpireCodeOPT,2,1),substring($varMonthExpireCodeOPT,1,1),concat('0',substring-before(substring-after($varExpireDate,'/'),'/')),substring($varMonthExpireCodeOPT,3,1),$varStrikeFUTOPT,$varExchangeCode)"/>
                      <!--<xsl:value-of select="concat($varUnderlying, ' ',$varMonthExpireCodeOPT, $varStrikeFUTOPT, $varExchangeCode)"/>-->
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>

                <!--For Futures-->
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varExpFlag = '0'">
                      <xsl:value-of select="concat($varUnderlying,' ',$varMonthExpireCode,$varExchangeCode)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:choose>
                        <xsl:when test ="$varExchangeCode = '-LME'">
                          <!-- Get Month number from month code-->
                          <xsl:variable name="varMonthNo">
                            <xsl:call-template name="temp_MonthNumberFromCode">
                              <xsl:with-param name="param_MonthNumberFromCode" select="substring($varMonthExpireCode,1,1)"/>
                            </xsl:call-template>
                          </xsl:variable>
                          <!-- Get Third Wednesday-->
                          <xsl:variable name ="varExpireDate">
                            <xsl:value-of select =" substring-before(my:Now1(number(concat('201',substring($varMonthExpireCode,2,1))),number($varMonthNo)),' ')"/>
                          </xsl:variable>
                          <!-- ZSD 20K3-LME-->
                          <xsl:value-of select="concat($varUnderlying,' ',substring($varMonthExpireCode,2,1),substring($varMonthExpireCode,1,1),substring-before(substring-after($varExpireDate,'/'),'/'),$varExchangeCode)"/>
                          <!--<xsl:value-of select="concat($varUnderlying,' ',$varExpireDate,$varMonthExpireCode,$varExchangeCode)"/>-->
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="normalize-space(concat($varUnderlying,' ',substring($varMonthExpireCode,2,1),substring($varMonthExpireCode,1,1),$varExchangeCode))"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </ConvertedSymbol>


            <RequestedSymbol>
              <xsl:value-of select ="BloombergSymbol"/>
            </RequestedSymbol>



            <xsl:choose>
              <xsl:when test="$varAssetType = 'FOPT' and $varExchangeCode = '-LME'">
                <xsl:variable name="varMonthNo">
                  <xsl:call-template name="temp_MonthNumberFromCode">
                    <xsl:with-param name="param_MonthNumberFromCode" select="substring($varMonthExpireCodeOPT,1,1)"/>
                  </xsl:call-template>
                </xsl:variable>

                <xsl:variable name ="varFutLMEExpireDate">
                  <xsl:value-of select =" substring-before(my:Now2(number(concat('201',substring($varMonthExpireCodeOPT,2,1))),number($varMonthNo)),' ')"/>
                </xsl:variable>
                <ExpirationDate>
                  <xsl:value-of select="$varFutLMEExpireDate"/>
                </ExpirationDate>
              </xsl:when>
              <xsl:otherwise>
                <ExpirationDate>
                  <xsl:value-of select="'01/01/0001'"/>
                </ExpirationDate>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test ="$varAssetType = 'FOPT' and $varExchangeCode = '-LME'">

                <xsl:variable name="varMonthNo">
                  <xsl:call-template name="temp_MonthNumberFromCode">
                    <xsl:with-param name="param_MonthNumberFromCode" select="substring($varMonthExpireCodeOPT,1,1)"/>
                  </xsl:call-template>
                </xsl:variable>

                <xsl:variable name ="thirdWednesday">
                  <xsl:value-of select ="my:Now1(number(concat('201',substring($varMonthExpireCodeOPT,2,1))),number($varMonthNo))"/>
                </xsl:variable>

                <xsl:variable name ="thirdWednesdayDate">
                  <xsl:value-of select ="substring-before(substring-after($thirdWednesday,'/'),'/')"/>
                </xsl:variable>
                <UnderlyingSymbol>
                  <xsl:value-of select="translate(concat($varUnderlying,' ',substring($varMonthExpireCodeOPT,2,1),substring($varMonthExpireCodeOPT,1,1),$thirdWednesdayDate,'-LME'),$varSmall,$varCapital)"/>
                </UnderlyingSymbol>
              </xsl:when>
              <xsl:otherwise>
                <UnderlyingSymbol>
                  <xsl:value-of select ="''"/>
                </UnderlyingSymbol>
              </xsl:otherwise>
            </xsl:choose>


            <ConvertedSymbology>
              <xsl:value-of select ="0"/>
            </ConvertedSymbology>


          </SecMasterRequest>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


