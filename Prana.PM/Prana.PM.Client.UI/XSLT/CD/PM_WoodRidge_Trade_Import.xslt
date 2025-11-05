<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template match="/NewDataSet">

		<DocumentElement>

			<xsl:for-each select="//PositionMaster">

				<xsl:if test ="number(COL6)">

					<PositionMaster>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>


						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME!=''">
								<AccountName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>



						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<!--<xsl:value-of select="COL2"/>-->
							<xsl:choose>
								<xsl:when test="PRANA_SYMBOL!=''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL2"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<PBSymbol>
							<xsl:value-of select="COL2"/>
						</PBSymbol>

						<xsl:variable name="varQty">
							<xsl:value-of select="COL6"/>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$varQty &lt; 0">
									<xsl:value-of select="$varQty*-1"/>
								</xsl:when>
								<xsl:when test="$varQty &gt; 0">
									<xsl:value-of select="$varQty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL5='Long'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL5='Short'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="$varAvgPrice &lt; 0">
									<xsl:value-of select="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test="$varAvgPrice &gt; 0">
									<xsl:value-of select="$varAvgPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<PositionStartDate>
							<xsl:value-of select ="COL3"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select="COL4"/>
						</PositionSettlementDate>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
