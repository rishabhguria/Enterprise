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

				<xsl:if test ="COL2 != 'Symbol' and COL2 != 'CASH' and number(COL4)">

					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL10)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<Symbol>
							<xsl:value-of select ="COL2"/>
						</Symbol>

						<AccountName>
							<xsl:value-of select="COL1"/>
						</AccountName>

						<LongName>
							<xsl:value-of select ="COL2"/>
						</LongName>

						<Quantity>
							<xsl:value-of select ="COL4"/>
						</Quantity>

						<AllocationBasedOn>
							<xsl:value-of select ="'Symbol'"/>
						</AllocationBasedOn>

						<OrderSideTagValue>
							<xsl:choose>
								<xsl:when test="COL3 = 'BL'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL3 = 'SL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
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
							<xsl:value-of select ="'SymbolSide'"/>
						</AllocationSchemeKey>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


