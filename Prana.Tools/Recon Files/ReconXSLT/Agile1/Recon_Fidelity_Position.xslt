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


  <xsl:template name="ConvertBBCodetoTicker">
    <xsl:param name="varBBCode"/>

    <xsl:variable name="varRoot">
      <xsl:value-of select="substring-before($varBBCode,' ')"/>
    </xsl:variable>

    <xsl:variable name="varPutCall">
      <xsl:value-of select="substring(substring-after(substring-after(substring-after($varBBCode,' '),' '),' '), 1, 1)"/>
    </xsl:variable>

    <xsl:variable name="varExYear">
      <xsl:value-of select="substring(substring-after(substring-after($varBBCode,' '),' '), 7, 2)"/>
    </xsl:variable>

    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(substring-before(substring(substring-after(substring-after($varBBCode,' '),' '),11), ' '), '#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring(substring-after(substring-after($varBBCode,' '),' '), 4, 2)"/>
    </xsl:variable>

    <xsl:variable name="varExMonth">
      <xsl:value-of select="substring(substring-after(substring-after($varBBCode,' '),' '), 1, 2)"/>
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


    <xsl:variable name="varMonthCode">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="varMonth" select="$varExMonth"/>
        <xsl:with-param name="varPutCall" select="$varPutCall"/>
      </xsl:call-template>
    </xsl:variable>

    <!--<xsl:variable name='varThirdFriday'>
      <xsl:value-of select='my:Now(number(concat("20",$varExYear)),number($varExMonth))'/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
        <xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike))"/>
      </xsl:when>
      <xsl:otherwise>-->
        <xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike,'D',$varExpiryDay))"/>
      <!--</xsl:otherwise>
    </xsl:choose>-->

  </xsl:template>
  
  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'HK'">
        <xsl:value-of select="'-HKG'"/>
      </xsl:when>

      <xsl:when test="$Suffix = 'JP'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>

      <xsl:when test="$Suffix = 'CN'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>

      <xsl:when test="$Suffix = 'LN'">
        <xsl:value-of select="'-LON'"/>
      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">

        <xsl:variable name="varPBName">
          <xsl:value-of select="'Agile'"/>
        </xsl:variable>

        <xsl:variable name="varMarkPrice">
          <xsl:value-of select="COL18"/>
        </xsl:variable>

        <xsl:if test="number(COL11)">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name= $varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL9"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='AQL']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varRoot">
              <xsl:value-of select="substring-before(COL3, ' ')"/>
            </xsl:variable>

            <xsl:variable name="varZeroString">
              <xsl:call-template name="noofzeros">
                <xsl:with-param name="count" select="(4- string-length($varRoot))"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varSuffix">
              <xsl:call-template name="GetSuffix">
                <xsl:with-param name="Suffix" select="substring-before(substring-after(COL3, ' '), ' ')"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="varMarketValue">
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name="varMarketValueBase">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>

            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL5"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <!--Side-->

            <Side>
              <xsl:choose>
                <xsl:when test="COL11 &gt; 0">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="COL11 &lt; 0 ">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <!--QUANTITY-->

            <Quantity>
              <xsl:choose>
                <xsl:when test="number($varNetPosition)">
                  <xsl:value-of select="$varNetPosition"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="number($varMarkPrice) ">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

           <MarketValue>
              <xsl:choose>
                <xsl:when test ="number($varMarketValue) ">
                  <xsl:value-of select="$varMarketValue"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test ="number($varMarketValueBase) ">
                  <xsl:value-of select="$varMarketValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>

            <CurrencySymbol>
              <xsl:value-of select="COL20"/>
            </CurrencySymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration from lower to upper case conversion-->

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
