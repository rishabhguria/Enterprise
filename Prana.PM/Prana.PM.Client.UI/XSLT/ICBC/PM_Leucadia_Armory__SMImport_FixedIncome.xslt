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

  <xsl:template name="GetMonth">
    <xsl:param name="varMonth"/>
    <xsl:param name="varPutCall"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='JAN' and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='FEB' and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAR' and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='APR' and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAY' and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUN' and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUL' and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='AUG' and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='SEP' and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='OCT' and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='NOV' and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='DEC' and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JAN' and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='FEB' and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAR' and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='APR' and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAY' and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUN' and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUL' and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='AUG' and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='SEP' and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='OCT' and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='NOV' and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='DEC' and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>
    <xsl:param name="varPutCall"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='01' and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='02' and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='03' and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='04' and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='05' and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='06' and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='07' and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='08' and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='09' and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11' and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12' and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='01' and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='02' and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='03' and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='04' and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='05' and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='06' and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='07' and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='08' and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='09' and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11' and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12' and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="substring-after-last">
    <xsl:param name="string" />
    <xsl:param name="delimiter" />
    <xsl:choose>
      <xsl:when test="contains($string, $delimiter)">
        <xsl:call-template name="substring-after-last">
          <xsl:with-param name="string"
            select="substring-after($string, $delimiter)" />
          <xsl:with-param name="delimiter" select="$delimiter" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$string" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">


        <!--IF NOT CONTAIN HEADER ROW -->
        <xsl:if test ="COL1 !=  'Trade Date'">

          <PositionMaster>

            <xsl:variable name="varExpirationDate">
              <xsl:call-template name="substring-after-last">
                <xsl:with-param name="string" select="COL4"/>
                <xsl:with-param name="delimiter" select="' '"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name ="varCoupon">
              <xsl:value-of select ="substring-before(substring-after(normalize-space(COL4),' '),'%')"/>
            </xsl:variable>

            <TickerSymbol>
              <xsl:value-of select="COL5"/>
            </TickerSymbol>


            <IDCOOptionSymbol>
              <xsl:value-of select="''"/>
            </IDCOOptionSymbol>

            <UnderLyingSymbol>
              <xsl:value-of select="COL5"/>
            </UnderLyingSymbol>

            <StrikePrice>
              <xsl:value-of select="0"/>
            </StrikePrice>

            <AUECID>
              <xsl:value-of select="80"/>
            </AUECID>

            <ExpirationDate>
              <xsl:value-of select="$varExpirationDate"/>
            </ExpirationDate>

            <MaturityDate>
              <xsl:value-of select="$varExpirationDate"/>
            </MaturityDate>

            <IssueDate>
              <xsl:value-of select="''"/>
            </IssueDate>

            <PutOrCall>
              <xsl:value-of select="2"/>
            </PutOrCall>


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
              <xsl:value-of select="COL4"/>
            </LongName>

            <Symbol_PK>
              <xsl:value-of select ="0"/>
            </Symbol_PK>

            <ReutersSymbol>
              <xsl:value-of select="''"/>
            </ReutersSymbol>

            <BloombergSymbol>
              <xsl:value-of select="''"/>
            </BloombergSymbol>

            <Coupon>
				<xsl:choose>
					<xsl:when test ="number($varCoupon)">
						<xsl:value-of select="$varCoupon"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>

					</xsl:otherwise>
				</xsl:choose>
            </Coupon>

            <IsZero>
              <xsl:value-of select="'TRUE'"/>
            </IsZero>

            <FirstCouponDate>
              <xsl:value-of select="COL1"/>
            </FirstCouponDate>

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
              <xsl:value-of select="COL5"/>
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
</xsl:stylesheet>
