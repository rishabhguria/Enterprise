<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime firstWednesday= new DateTime(year, month, 1);
    while (firstWednesday.DayOfWeek != DayOfWeek.Wednesday)
    {
    firstWednesday = firstWednesday.AddDays(1);
    }
    return firstWednesday.ToString();
    }
  </msxsl:script>


  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='F' or $varMonth='1'">
        <xsl:value-of select ="'JAN'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='G' or $varMonth ='2'">
        <xsl:value-of select ="'FEB'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='H' or $varMonth ='3'">
        <xsl:value-of select ="'MAR'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='J' or $varMonth ='4'">
        <xsl:value-of select ="'APR'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='K' or $varMonth ='5'">
        <xsl:value-of select ="'MAY'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='M' or $varMonth ='6'">
        <xsl:value-of select ="'JUN'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='N' or $varMonth ='7'">
        <xsl:value-of select ="'JUL'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Q' or $varMonth ='8'">
        <xsl:value-of select ="'AUG'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='U' or $varMonth ='9'">
        <xsl:value-of select ="'SEP'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='V' or $varMonth ='10'">
        <xsl:value-of select ="'OCT'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='X' or $varMonth ='11'">
        <xsl:value-of select ="'NOV'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Z' or $varMonth ='12'">
        <xsl:value-of select ="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template name="GetCurrencyID">
    <xsl:param name="CurrencyName"/>

    <xsl:choose>
      <xsl:when test ="$CurrencyName='USD'">
        <xsl:value-of select ="1"/>
      </xsl:when>
      <xsl:when test ="$CurrencyName='EUR'">
        <xsl:value-of select ="8"/>
      </xsl:when>
      <xsl:when test ="$CurrencyName='JPY'">
        <xsl:value-of select ="3"/>
      </xsl:when>
      <xsl:when test ="$CurrencyName='GBP'">
        <xsl:value-of select ="4"/>
      </xsl:when>
      <xsl:when test ="$CurrencyName='CAD'">
        <xsl:value-of select ="7"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="0"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">


        <!--IF NOT CONTAIN HEADER ROW -->
        <xsl:if test="COL3 != '*' and COL3 != 'Expiration'">

          <PositionMaster>

            <xsl:variable name="varDay">
              <xsl:value-of select="substring-before(substring-after(COL3,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select="substring-before(COL3,'/')"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select="substring-after(substring-after(COL3,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varMonthCode">
              <xsl:call-template name="MonthCode">
                <xsl:with-param name="varMonth" select="$varMonth"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varLeadCcyID">
              <xsl:call-template name="GetCurrencyID">
                <xsl:with-param name="CurrencyName" select="COL1"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varVsCcyID">
              <xsl:call-template name="GetCurrencyID">
                <xsl:with-param name="CurrencyName" select="COL2"/>
              </xsl:call-template>
            </xsl:variable>
            
            <TickerSymbol>
              <xsl:choose>
                <xsl:when test="COL1 = 'CAD'">
                  <xsl:value-of select="concat(COL2,'-',COL1, ' ', $varDay, $varMonthCode, $varYear)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat(COL1,'-',COL2, ' ', $varDay, $varMonthCode, $varYear)"/>
                </xsl:otherwise>
              </xsl:choose>
            </TickerSymbol>


            <ExpirationDate>
              <xsl:value-of select="COL3"/>
            </ExpirationDate>

            <UnderLyingSymbol>
              <xsl:value-of select="COL4"/>
            </UnderLyingSymbol>

            <LeadCurrencyID>
              <xsl:value-of select="$varLeadCcyID"/>
            </LeadCurrencyID>

            <VsCurrencyID>
              <xsl:value-of select="$varVsCcyID"/>
            </VsCurrencyID>

            <StrikePrice>
              <xsl:value-of select="0"/>
            </StrikePrice>

            <PutOrCall>
              <xsl:value-of select="2"/>
            </PutOrCall>

            <AUECID>
              <xsl:value-of select="33"/>
            </AUECID>


            <IDCOOptionSymbol>
              <xsl:value-of select="''"/>
            </IDCOOptionSymbol>


            <DividendYield>
              <xsl:value-of select ="0"/>
            </DividendYield>

            <Dividend>
              <xsl:value-of select ="0"/>
            </Dividend>

            <DividendAmtRate>
              <xsl:value-of select ="0"/>
            </DividendAmtRate>

            <DivDistributionDate>
              <xsl:value-of select ="'1/1/0001'"/>
            </DivDistributionDate>

            <RequestedSymbology>
              <xsl:value-of select="0"/>
            </RequestedSymbology>

            <Multiplier>
              <xsl:value-of select="1"/>
            </Multiplier>

            <LongName>
              <xsl:value-of select="COL5"/>
            </LongName>

            <AssetCategory>
              <xsl:value-of select="'FX Forward'"/>
            </AssetCategory>

            <Symbol_PK>
              <xsl:value-of select ="0"/>
            </Symbol_PK>

            <ReutersSymbol>
              <xsl:value-of select="''"/>
            </ReutersSymbol>

            <BloombergSymbol>
              <xsl:value-of select="''"/>
            </BloombergSymbol>

            <ISINSymbol>
              <xsl:value-of select="''"/>
            </ISINSymbol>

            <SedolSymbol>
              <xsl:value-of select="''"/>
            </SedolSymbol>

            <OSIOptionSymbol>
              <xsl:value-of select="''"/>
            </OSIOptionSymbol>

            <OPRAOptionSymbol>
              <xsl:value-of select="''"/>
            </OPRAOptionSymbol>

            <CusipSymbol>
              <xsl:value-of select="COL6"/>
            </CusipSymbol>

            <SymbolType>
              <xsl:value-of select="'0'"/>
            </SymbolType>

            <Delta>
              <xsl:value-of select="'1'"/>
            </Delta>

            <AccrualBasisID>
              <xsl:value-of select="'0'"/>
            </AccrualBasisID>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>