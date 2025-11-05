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
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test ="number(COL4) and COL2 != 'CORE-CASH'">

					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>
						
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Automailer']/FundData[@PranaFund=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<!--<Symbol>
							<xsl:value-of select ="COL2"/>
						</Symbol>-->

						<!--<Bloomberg>
							<xsl:value-of select="concat(COL2,' ','EQUITY')"/>
						</Bloomberg>-->

						<Symbol>
							<xsl:value-of select ="translate(normalize-space(COL2),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
						</Symbol>

						<FundName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME != ''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>

								</xsl:otherwise>
							</xsl:choose>
						</FundName>

						<LongName>
							<xsl:value-of select ="COL3"/>
						</LongName>

						<Quantity>
							<xsl:value-of select ="COL4"/>
						</Quantity>

						<AllocationBasedOn>
							<xsl:value-of select ="'Symbol'"/>
						</AllocationBasedOn>

						<OrderSideTagValue>
							<xsl:choose>
								<xsl:when test="COL3 = 'Buy' and contains(COL2,'O:') = false">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL3 = 'BUY' and contains(COL2,'O:') = false">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								
								
								
								<xsl:when test="COL3 = 'BL' and contains(COL2,'O:') != false">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="COL3 = 'Sell' and contains(COL2,'O:') = false">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								
								<xsl:when test="COL3 = 'SELL' and contains(COL2,'O:') = false">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								
								
								<xsl:when test="COL3 = 'SL' and contains(COL2,'O:') != false">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:when test="COL3 = 'Sell Short' and contains(COL2,'O:') = false">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="COL3 = 'SS' and contains(COL2,'O:') != false">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:when test="COL3 = 'Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrderSideTagValue>						

						<AllocationSchemeKey>
							<xsl:value-of select ="'SymbolSide'"/>
						</AllocationSchemeKey>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


