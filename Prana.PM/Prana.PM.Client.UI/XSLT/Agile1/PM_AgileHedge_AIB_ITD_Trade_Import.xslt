<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL11)">

					<PositionMaster>
				

						<xsl:variable name = "PB_NAME" >
							<xsl:value-of select="'BTIG'"/>
						</xsl:variable>

						<xsl:variable name = "PB_Symbol_NAME" >
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Symbol_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@CompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<PBSymbol>
							<xsl:value-of select ="COL10"/>
						</PBSymbol>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_Symbol_NAME!=''">
									<xsl:value-of select="$PRANA_Symbol_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL9"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<xsl:variable name ="varSide">
							<xsl:value-of select ="COL8"/>
						</xsl:variable>


						<SideTagValue>

							<xsl:choose>
								<xsl:when test ="$varSide='BY'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>

								<xsl:when test ="$varSide='SL'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>

								<xsl:when test ="$varSide='SS'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>
								
								<xsl:when test ="$varSide='CS'">
									<xsl:value-of select ="'B'"/>
								</xsl:when>

								<xsl:when test ="$varSide='RM'">
									<xsl:value-of select ="'RM'"/>
								</xsl:when>

								<xsl:when test ="$varSide='SD'">
									<xsl:value-of select ="'SD'"/>
								</xsl:when>

								<xsl:when test ="$varSide='TN'">
									<xsl:value-of select ="'TN'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
								
							</xsl:choose>
							
						</SideTagValue>


						<xsl:variable name ="varNetPosition">
							<xsl:value-of select ="number(COL11)"/>
						</xsl:variable>

						<NetPosition>

							<xsl:choose>
								<xsl:when test ="$varNetPosition &lt;0">
									<xsl:value-of select ="$varNetPosition*-1"/>
								</xsl:when>
								<xsl:when test ="$varNetPosition &gt;0">
									<xsl:value-of select ="$varNetPosition"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</NetPosition>


						<xsl:variable name ="varAvgPrice">
							<xsl:value-of select ="number(COL12)"/>
						</xsl:variable>


						<CostBasis>
							<xsl:choose>
								<xsl:when test ="$varAvgPrice &lt; 0">
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ="$varAvgPrice &gt;0">
									<xsl:value-of select ="$varAvgPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</CostBasis>

						<PositionStartDate>
							<xsl:value-of select="COL4"/>
						</PositionStartDate>
						

						<!--<FXConversionMethodOperator>
							<xsl:value-of select="'D'"/>
						</FXConversionMethodOperator>

						<FXRate>
							<xsl:value-of select="COL25"/>
						</FXRate>-->

						<xsl:variable name ="varCommission">
							<xsl:value-of select="COL17"/>
						</xsl:variable>

						<xsl:variable name ="varSecFees">
							<xsl:value-of select="COL14"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select ="$varCommission"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select ="$varCommission*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<StampDuty>
							<xsl:choose>
								<xsl:when test="$varSecFees &gt; 0">
									<xsl:value-of select ="$varSecFees"/>
								</xsl:when>
								<xsl:when test="$varSecFees &lt; 0">
									<xsl:value-of select ="$varSecFees*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>
						<Fees>
							<xsl:choose>
								<xsl:when test="COL15 &gt; 0">
									<xsl:value-of select ="COL15"/>
								</xsl:when>
								<xsl:when test="COL15 &lt; 0">
									<xsl:value-of select ="COL15*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
