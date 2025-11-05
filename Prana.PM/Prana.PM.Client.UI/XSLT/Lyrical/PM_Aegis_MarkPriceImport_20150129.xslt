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


  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CHF'">
        <xsl:value-of select="'-SWX'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'EUR'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'GBP'">
        <xsl:value-of select="'-LON'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'SEK'">
        <xsl:value-of select="'-OMX'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="MonthCode">
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
  <xsl:template name="GetMonth">
    <xsl:param name="varMonth"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='Jan'">
        <xsl:value-of select ="1"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Feb'">
        <xsl:value-of select ="2"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Mar' ">
        <xsl:value-of select ="3"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Apr'">
        <xsl:value-of select ="4"/>
      </xsl:when>
      <xsl:when test ="$varMonth='May'">
        <xsl:value-of select ="5"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Jun'">
        <xsl:value-of select ="6"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Jul'">
        <xsl:value-of select ="7"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Aug'">
        <xsl:value-of select ="8"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Sep' ">
        <xsl:value-of select ="9"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Oct'">
        <xsl:value-of select ="10"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Nov' ">
        <xsl:value-of select ="11"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Dec'">
        <xsl:value-of select ="12"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="ConvertBBCodetoTicker">
    <xsl:param name="varBBCode"/>

    <xsl:variable name="varRoot">
      <xsl:value-of select="substring-before($varBBCode,' ')"/>
    </xsl:variable>

    <xsl:variable name="varExYear">
      <xsl:value-of select="substring(substring-after($varBBCode,' '),7,2)"/>
    </xsl:variable>

    <xsl:variable name="varPutCall">
      <xsl:value-of select="substring(substring-after(substring-after($varBBCode,' '),' '),1,1)"/>
    </xsl:variable>

    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(substring(substring-after(substring-after($varBBCode,' '),' '),2),'#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring(substring-after($varBBCode,' '),4,2)"/>
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

    <xsl:variable name="varExMonth">
      <xsl:value-of select="substring(substring-after($varBBCode,' '),1,2)"/>
    </xsl:variable>

    <xsl:variable name="varMonthCode">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="varMonth" select="$varExMonth"/>
        <xsl:with-param name="varPutCall" select="$varPutCall"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name='varThirdFriday'>
      <xsl:value-of select='my:Now(number(concat("20",$varExYear)),number($varExMonth))'/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
        <xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike))"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike,'D',$varExpiryDay))"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL12)">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="COL10"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= 'GS']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varSuffix">
              <xsl:call-template name="GetSuffix">
                <xsl:with-param name="Suffix" select="COL7"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMarkPrice">
              <xsl:value-of select="COL13"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL6"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <PBSymbol>
              <xsl:value-of select="COL10"/>
            </PBSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number($varMarkPrice)">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

            <Date>
              <xsl:value-of select="''"/>
            </Date>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>