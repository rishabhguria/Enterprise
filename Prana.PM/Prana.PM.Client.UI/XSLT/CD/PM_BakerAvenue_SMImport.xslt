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


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">


        <!--IF NOT CONTAIN HEADER ROW -->
        <xsl:if test ="COL2 !=  'Asset'">

          <PositionMaster>


            <TickerSymbol>
              <xsl:value-of select="COL4"/>
            </TickerSymbol>
              

            <IDCOOptionSymbol>
              <xsl:value-of select="''"/>
            </IDCOOptionSymbol>

            <UnderLyingSymbol>
              <xsl:value-of select="COL4"/>
            </UnderLyingSymbol>

            <StrikePrice>
              <xsl:value-of select="0"/>
            </StrikePrice>

            <AUECID>
              <xsl:choose>
                <xsl:when test="COL2 = 'Mutual Funds'" >
                  <xsl:value-of select="81"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="80"/>
                </xsl:otherwise>
              </xsl:choose>
            </AUECID>

            <ExpirationDate>
              <xsl:choose>
                <xsl:when test="COL2 = 'Mutual Funds'" >
                  <xsl:value-of select="'01/01/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat(substring(COL11,5,2),'/',substring(COL11,7,2),'/',substring(COL11,1,4))"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExpirationDate>

            <MaturityDate>
              <xsl:choose>
                <xsl:when test="COL2 = 'Mutual Funds'" >
                  <xsl:value-of select="'01/01/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat(substring(COL11,5,2),'/',substring(COL11,7,2),'/',substring(COL11,1,4))"/>
                </xsl:otherwise>
              </xsl:choose>
            </MaturityDate>

            <IssueDate>
              <xsl:choose>
                <xsl:when test="COL2 = 'Mutual Funds'" >
                  <xsl:value-of select="'01/01/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat(substring(COL7,5,2),'/',substring(COL7,7,2),'/',substring(COL7,1,4))"/>
                </xsl:otherwise>
              </xsl:choose>
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

			  <IsZero>
				  <xsl:choose>
					  <xsl:when test="COL2 != 'Mutual Funds' and COL14 = 0" >
						  <xsl:value-of select="'TRUE'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'FALSE'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IsZero>

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
              <xsl:value-of select="COL6"/>
            </LongName>

            <!--<UnderLyingSymbol>
							  <xsl:value-of select="$varSymbol"/>
						  </UnderLyingSymbol>-->


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
                <xsl:when test="COL2 = 'Mutual Funds'" >
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="number(COL14)"/>
                </xsl:otherwise>
              </xsl:choose>
            </Coupon>

            <FirstCouponDate>
              <xsl:choose>
                <xsl:when test="COL2 = 'Mutual Funds'" >
                  <xsl:value-of select="'01/01/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat(substring(COL8,5,2),'/',substring(COL8,7,2),'/',substring(COL8,1,4))"/>
                </xsl:otherwise>
              </xsl:choose>
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
              <xsl:value-of select="''"/>
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
