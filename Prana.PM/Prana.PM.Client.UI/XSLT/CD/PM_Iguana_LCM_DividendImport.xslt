<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template name="MonthNo">
		<xsl:param name="varMonth"/>

		<xsl:choose>
			<xsl:when test ="$varMonth='Jan'">
				<xsl:value-of select ="'01'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Feb'">
				<xsl:value-of select ="'02'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Mar'">
				<xsl:value-of select ="'03'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Apr'">
				<xsl:value-of select ="'04'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='May'">
				<xsl:value-of select ="'05'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Jun'">
				<xsl:value-of select ="'06'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Jul'">
				<xsl:value-of select ="'07'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Aug'">
				<xsl:value-of select ="'08'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Sep'">
				<xsl:value-of select ="'09'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Oct'">
				<xsl:value-of select ="'10'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Nov'">
				<xsl:value-of select ="'11'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Dec'">
				<xsl:value-of select ="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:if test="COL16 = 'DIV' or COL16 = 'DIS' or COL16 = 'ADI'">
          <!--TABLE-->
          <PositionMaster>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Lazard']/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRIC">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

			  <xsl:variable name ="varMonthTrade">
				  <xsl:call-template name ="MonthNo">
					  <xsl:with-param name="varMonth" select="substring-before(COL5,' ')"/>
				  </xsl:call-template>
			  </xsl:variable>

            <xsl:variable name="varDividend">
              <xsl:value-of select="COL14*(-1)"/>
            </xsl:variable>

            <xsl:variable name="varPayoutDate">
				<xsl:value-of select ="concat($varMonthTrade,'/',substring(COL5,5,2),'/',substring(COL5,8,4))"/>
			</xsl:variable>

            <xsl:variable name="varExDate">
				<xsl:value-of select ="concat($varMonthTrade,'/',substring(COL5,5,2),'/',substring(COL5,8,4))"/>
			</xsl:variable>

            <!--FundNameSection -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL4"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name = 'Lazard']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>


            <AccountName>
				<xsl:choose>
					<xsl:when test ="$PRANA_FUND_NAME= ''">
						<xsl:value-of select='$PB_FUND_NAME'/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select='$PRANA_FUND_NAME'/>

					</xsl:otherwise>
				</xsl:choose>
            </AccountName>

            <FundID>
              <xsl:value-of select="''"/>
            </FundID>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_Symbol_NAME = ''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_Symbol_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="COL11"/>
            </PBSymbol>

            <Dividend>
              <xsl:value-of select="$varDividend"/>
            </Dividend>

            <PayoutDate>
              <xsl:value-of select="$varPayoutDate"/>
            </PayoutDate>

            <ExDate>
              <xsl:value-of select="$varExDate"/>
            </ExDate>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>