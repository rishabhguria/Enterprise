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

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='01' ">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='02' ">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='03'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='04' ">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='05' ">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='06'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='07' ">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='08' ">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='09' ">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' ">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12'">
        <xsl:value-of select ="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="MonthNo">
    <xsl:param name="varMonth"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='F' or $varMonth='f'">
        <xsl:value-of select ="'01'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='G' or $varMonth ='g'">
        <xsl:value-of select ="'02'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='H' or $varMonth ='h'">
        <xsl:value-of select ="'03'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='J' or $varMonth ='j'">
        <xsl:value-of select ="'04'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='K' or $varMonth ='k'">
        <xsl:value-of select ="'05'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='M' or $varMonth ='m'">
        <xsl:value-of select ="'06'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='N' or $varMonth ='n'">
        <xsl:value-of select ="'07'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Q' or $varMonth ='q'">
        <xsl:value-of select ="'08'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='U' or $varMonth ='u'">
        <xsl:value-of select ="'09'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='V' or $varMonth ='v'">
        <xsl:value-of select ="'10'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='X' or $varMonth ='x'">
        <xsl:value-of select ="'11'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Z' or $varMonth ='z'">
        <xsl:value-of select ="'12'"/>
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
        <xsl:if test="number(COL4)">

          <PositionMaster>

          

            <xsl:variable name="varDay">
              <xsl:choose>
                <xsl:when test="string-length(COL3) = 1">
                  <xsl:value-of select="concat('0',COL3)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL3"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varMonthCode">
              <xsl:call-template name="MonthCode">
                <xsl:with-param name="varMonth" select="substring(COL24,12,2)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name ="varMonthNo">
              <xsl:call-template name="MonthNo">
                <xsl:with-param name="varMonth" select="COL2"/>
              </xsl:call-template>
            </xsl:variable>
					 

		
					  <TickerSymbol>
              <xsl:value-of select="concat(COL1,' ',substring(COL6,4,1),COL2,$varDay,'-LME')"/>
            </TickerSymbol>


            <ExpirationDate>
						  <xsl:value-of select="concat($varMonthNo,'/',$varDay,'/',COL6)"/>
					  </ExpirationDate>

					  <UnderLyingSymbol>
              <xsl:value-of select="concat(COL1,' ',substring(COL6,4,1),COL2,$varDay,'-LME')"/>
            </UnderLyingSymbol>

            <StrikePrice>
              <xsl:value-of select="0"/>
            </StrikePrice>

            <PutOrCall>
              <xsl:value-of select="2"/>
            </PutOrCall>

            <AUECID>
						  <xsl:value-of select="95"/>
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
              <xsl:value-of select="COL4"/>
            </Multiplier>

            <LongName>
              <xsl:value-of select="concat('LM', COL1,'P ',COL6,$varMonthNo,$varDay,' Comdty')"/>
            </LongName>

            <!--<UnderLyingSymbol>
							<xsl:value-of select="$varSymbol"/>
						</UnderLyingSymbol>-->

            <AssetCategory>
              <xsl:value-of select="'Future'"/>
            </AssetCategory>

            <Symbol_PK>
              <xsl:value-of select ="0"/>
            </Symbol_PK>

            <ReutersSymbol>
              <xsl:value-of select="''"/>
            </ReutersSymbol>

            <BloombergSymbol>
              <xsl:value-of select="concat('LM', COL1,'P ',COL6,$varMonthNo,$varDay,' Comdty')"/>
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
              <xsl:value-of select="''"/>
            </CusipSymbol>

            <SymbolType>
              <xsl:value-of select="'0'"/>
            </SymbolType>

            <Delta>
              <xsl:value-of select="'1'"/>
            </Delta>

            <!--<PutOrCall>
							<xsl:choose>
								<xsl:when test="$varCallPutCode='P'">
									<xsl:value-of select="0"/>
								</xsl:when>

								<xsl:when test="$varCallPutCode='C'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="2"/>
								</xsl:otherwise>
							</xsl:choose>
						</PutOrCall>-->

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