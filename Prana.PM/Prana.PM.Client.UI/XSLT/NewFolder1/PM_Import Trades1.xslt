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
				<xsl:if test="number(COL6)">
					<PositionMaster>

						
								<FundName>
									<xsl:value-of select='COL3'/>
								</FundName>
												
						<!--QUANTITY-->
						<xsl:choose>
							<xsl:when test="COL6 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL6*(-1)"/>
								</Quantity>
							</xsl:when>

							<xsl:when test="COL6 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL6"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="boolean(number(COL7))">
								<CostBasis>
									<xsl:value-of select="COL7"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<!--Side-->
						<xsl:choose>
							<xsl:when test="COL5='sell to open'">
								<Side>
									<xsl:value-of select="'C'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL5='Buy to open'">
								<Side>
									<xsl:value-of select="'A'"/>
								</Side>
							</xsl:when>
							
							
							<xsl:when test="COL5='BUY'">
								<Side>
									<xsl:value-of select="'1'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL5='SELL'">
								<Side>
									<xsl:value-of select="'2'"/>
								</Side>
							</xsl:when>
							
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<!--SYMBOL-->
						<xsl:variable name="AssetType">
							<xsl:value-of select="COL35"/>
						</xsl:variable>
						<xsl:variable name="PB_COMPANY_NAME" select="COL18"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='CITI']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						
								<Symbol>
									<xsl:value-of select='COL2'/>
								</Symbol>
							

						

						<!--COMMISSION-->
						
					<Date>
						<xsl:value-of select="COL1"/>
					</Date>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>