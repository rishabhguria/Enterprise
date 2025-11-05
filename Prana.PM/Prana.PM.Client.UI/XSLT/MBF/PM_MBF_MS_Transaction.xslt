<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

        <xsl:variable name ="varInstrument">
          <xsl:value-of select ="COL84"/>
        </xsl:variable>

        <xsl:if test ="($varInstrument = 'Equity' and COL8 = '015A') or ($varInstrument = 'FUTURES TRADES' and COL8='TRD') or (($varInstrument ='Call - Listed' or $varInstrument ='Put - Listed') And COL8 = '015A') ">
        <!--<xsl:if test ="$varInstrument = 'Equity' and COL8 = '015A'">-->


          <PositionMaster>
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="translate(COL3,'&quot;','')"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='MS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME = ''">
                <FundName>
                  <xsl:value-of select="''"/>
                </FundName>
              </xsl:when>
              <xsl:otherwise>
                <FundName>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </FundName>
              </xsl:otherwise>
            </xsl:choose>

            <PBAssetType>
              <xsl:value-of select="$varInstrument"/>
            </PBAssetType>

            <xsl:choose>
              <xsl:when test="COL34 &lt; 0">
                <NetPosition>
                  <xsl:value-of select="COL34 * (-1)"/>
                </NetPosition>
              </xsl:when>
              <xsl:when test="COL34 &gt; 0">
                <NetPosition>
                  <xsl:value-of select="COL34"/>
                </NetPosition>
              </xsl:when>
              <xsl:otherwise>
                <NetPosition>
                  <xsl:value-of select="0"/>
                </NetPosition>
              </xsl:otherwise>
            </xsl:choose>

            <!--Side-->
            <xsl:choose>
              <xsl:when test="COL9='BUY LONG' and $varInstrument = 'Equity' ">
                <SideTagValue>
                  <xsl:value-of select="'1'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL9='SELL LONG' and $varInstrument = 'Equity'">
                <SideTagValue>
                  <xsl:value-of select="'2'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL9='SELL SHORT' and $varInstrument = 'Equity'">
                <SideTagValue>
                  <xsl:value-of select="'5'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL9='BUY TO COVER' and $varInstrument = 'Equity' ">
                <SideTagValue>
                  <xsl:value-of select="'B'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL9='BUY LONG' and ($varInstrument ='Call - Listed' or $varInstrument ='Put - Listed')">
                <SideTagValue>
                  <xsl:value-of select="'A'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL9='SELL LONG' and ($varInstrument ='Call - Listed' or $varInstrument ='Put - Listed') ">
                <SideTagValue>
                  <xsl:value-of select="'D'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL9='SELL SHORT' and ($varInstrument ='Call - Listed' or $varInstrument ='Put - Listed') ">
                <SideTagValue>
                  <xsl:value-of select="'C'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL9='BUY TO COVER' and ($varInstrument ='Call - Listed' or $varInstrument ='Put - Listed') ">
                <SideTagValue>
                  <xsl:value-of select="'B'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL9='BUY' and $varInstrument = 'FUTURES TRADES' ">
                <SideTagValue>
                  <xsl:value-of select="'1'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL9='SELL' and $varInstrument = 'FUTURES TRADES' ">
                <SideTagValue>
                  <xsl:value-of select="'2'"/>
                </SideTagValue>
              </xsl:when>             
              <xsl:otherwise>
                <SideTagValue>
                  <xsl:value-of select="''"/>
                </SideTagValue>
              </xsl:otherwise>
            </xsl:choose>

            <!-- Position Date mapped with the column 5 -->
            <xsl:choose>
              <xsl:when test="COL36='Trade Date' or COL36='*'">
                <PositionStartDate>
                  <xsl:value-of select="''"/>
                </PositionStartDate>
              </xsl:when>
              <xsl:otherwise>
                <PositionStartDate>
                  <xsl:value-of select="COL36"/>
                </PositionStartDate>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number(COL71))">
                <CostBasis>
                  <xsl:value-of select="COL71"/>
                </CostBasis>
              </xsl:when>
              <xsl:otherwise>
                <CostBasis>
                  <xsl:value-of select="0"/>
                </CostBasis>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="COL73 &lt; 0">
                <Commission>
                  <xsl:value-of select="COL73*(-1)"/>
                </Commission>
              </xsl:when>
              <xsl:when test="COL73 &gt; 0">
                <Commission>
                  <xsl:value-of select="COL73"/>
                </Commission>
              </xsl:when>
              <xsl:otherwise>
                <Commission>
                  <xsl:value-of select="0"/>
                </Commission>
              </xsl:otherwise>
            </xsl:choose>
            
            <xsl:choose>
              <xsl:when test="COL74 &lt; 0">
                <MiscFees>
                  <xsl:value-of select="COL74*(-1)"/>
                </MiscFees>
              </xsl:when>
              <xsl:when test="COL74 &gt; 0">
                <MiscFees>
                  <xsl:value-of select="COL74"/>
                </MiscFees>
              </xsl:when>
              <xsl:otherwise>
                <MiscFees>
                  <xsl:value-of select="0"/>
                </MiscFees>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:variable name="PB_COMPANY_NAME" select="translate(COL17,'&quot;','')"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:when>
							<xsl:when test="$varInstrument = 'Equity'">
								<Symbol>
									<xsl:value-of select="COL19"/>
								</Symbol>
							</xsl:when>
              <xsl:when test="$varInstrument ='Call - Listed' or $varInstrument ='Put - Listed'">
                <Symbol>
                  <xsl:value-of select="translate(COL19,'/',' ')"/>
                </Symbol>
              </xsl:when>
              <xsl:when test="$varInstrument = 'FUTURES TRADES'">
                <xsl:variable name = "varLength" >
                  <xsl:value-of select="string-length(COL19)"/>
                </xsl:variable>
                <xsl:variable name = "varAfter" >
                  <xsl:value-of select="substring(COL19,($varLength)-1,2)"/>
                </xsl:variable>
                <xsl:variable name = "varBefore" >
                  <xsl:value-of select="substring(COL19,1,($varLength)-2)"/>
                </xsl:variable>
                <Symbol>
                  <xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
                </Symbol>
              </xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL19"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

            <PBSymbol>
              <xsl:value-of select="COL19"/>
            </PBSymbol>

            <CounterPartyID>
              <xsl:value-of select ="2"/>
            </CounterPartyID>

          </PositionMaster>
        </xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>