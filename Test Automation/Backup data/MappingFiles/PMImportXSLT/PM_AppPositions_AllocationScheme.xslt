<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test ="COL1 != 'Symbol'">

					<PositionMaster>

						<!--<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>-->

						<!--<xsl:variable name ="varSymbolCode">
              <xsl:value-of select ="substring-after(COL11,' ')"/>
            </xsl:variable>
            <xsl:variable name="TickerSuffixCode">
              <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='SANSATO']/SymbolData[@PBSuffixCode=$varSymbolCode]/@TickerSuffixCode"/>
            </xsl:variable>
            <xsl:variable name ="varHKGSymbol">
              <xsl:call-template name="noofzeros">
                <xsl:with-param name="count" select="(4) - string-length(substring-before(COL11,' '))" />
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL" >
              <xsl:value-of select="COL11"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SANSATO']/SymbolData[@PBCompanyName=$PB_SYMBOL]/@PranaSymbol"/>
            </xsl:variable>-->

						<!--<Symbol>
				  <xsl:choose>
					  <xsl:when test ="$PRANA_SYMBOL != ''">
						  <xsl:value-of select ="$PRANA_SYMBOL"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'AU'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-',$TickerSuffixCode)"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'HK'">
						  <xsl:value-of select="concat($varHKGSymbol,substring-before(COL11,' '),'-',$TickerSuffixCode)"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'IJ'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-','JKT')"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'IN'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'.',$TickerSuffixCode)"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'JP'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-',$TickerSuffixCode)"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'KS'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-',$TickerSuffixCode)"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'MK'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-',$TickerSuffixCode)"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'NZ'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-','NZX')"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'SP'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-',$TickerSuffixCode)"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'TB'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-',$TickerSuffixCode)"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'TT'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-',$TickerSuffixCode)"/>
					  </xsl:when>
					  <xsl:when test ="$varSymbolCode = 'PM'">
						  <xsl:value-of select="concat(substring-before(COL11,' '),'-','PHS')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="COL11"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>-->
						<Symbol>
							<xsl:value-of select ="COL1"/>
						</Symbol>

						<FundName>
							<xsl:value-of select="COL2"/>
						</FundName>

						<!--<LongName>
              <xsl:value-of select ="COL9"/>
            </LongName>-->

						<!--<SEDOL>
              <xsl:value-of select ="COL10"/>
            </SEDOL>-->

						<!--<Bloomberg>
              <xsl:value-of select ="COL11"/>
            </Bloomberg>-->

						<Quantity>
							<xsl:value-of select ="COL3"/>
						</Quantity>

						<!--<RoundLot>
              <xsl:value-of select ="COL14"/>
            </RoundLot>-->

						<AllocationBasedOn>
							<xsl:value-of select ="'Symbol'"/>
						</AllocationBasedOn>

						<!--<TargetAllocationPct>
							<xsl:value-of select ="COL4"/>
						</TargetAllocationPct>-->
						
						<PB>
              <xsl:value-of select ="COL5"/>
            </PB>
						
						<OrderSideTagValue>
              <xsl:choose>
                <xsl:when test="COL12 = 'BC'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="COL12 = 'BL'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="COL12 = 'SS'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="COL12 = 'SL'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL12"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrderSideTagValue>

						<!--<TradeType>
              <xsl:value-of select="normalize-space(COL6)"/>
            </TradeType>-->

						<!--<Currency>
              <xsl:value-of select ="COL7"/>
            </Currency>

            <PB>
              <xsl:value-of select ="COL4"/>
            </PB>

            <SMMappingReq>
              <xsl:value-of select="'SecMasterMapping.xml'"/>
            </SMMappingReq>-->

						<AllocationSchemeKey>
							<xsl:value-of select ="'Symbol'"/>
						</AllocationSchemeKey>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


