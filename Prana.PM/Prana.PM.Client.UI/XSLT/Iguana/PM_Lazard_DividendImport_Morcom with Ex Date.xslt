<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL12) ">

          <!--TABLE-->
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'Morcom'"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varDividend">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varPayoutDate">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="varRecordDate">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varExDate">
              <xsl:value-of select="COL9"/>
            </xsl:variable>

            <!--FundNameSection -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL4"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $varPBName]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

            <PBSymbol>
              <xsl:value-of select="COL6"/>
            </PBSymbol>

            <Symbol>
              <xsl:value-of select="substring-before(substring-after(COL6, 'SYMBOL'),')')"/>
            </Symbol>

            <Dividend>
              <xsl:value-of select="$varDividend"/>
            </Dividend>

            <PayoutDate>
              <xsl:value-of select="$varPayoutDate"/>
            </PayoutDate>

            <RecordDate>
              <xsl:value-of select="$varRecordDate"/>
            </RecordDate>

            <ExDate>
              <xsl:value-of select="$varExDate"/>
            </ExDate>

            <Description>
				<xsl:choose>
					<xsl:when test ="number(COL12) &lt; 0">
						<xsl:value-of select ="'DIV CHARGED'"/>
					</xsl:when>
					<xsl:when test ="number(COL12) &gt; 0">
						<xsl:value-of select ="'DIV RECEIVED'"/>
					</xsl:when>
				</xsl:choose>
            </Description>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>