<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL10) and substring(COL18,1,4) != 'Cash'">
					<PositionMaster>

						<xsl:variable name = "PB_NAME" >
							<xsl:value-of select ="'NorthernTrust'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL22"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="contains(substring(COL24,1,1),'C')">
									<xsl:value-of select="'CUSIP'"/>
								</xsl:when>
								<xsl:when test="contains(substring(COL24,1,1),'S')">
									<xsl:value-of select="'SEDOL'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="contains(substring(COL24,1,1),'C')">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:when test="contains(substring(COL24,1,1),'S')">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="contains(substring(COL24,1,1),'C')">
									<xsl:value-of select ="substring(COL24,2)"/>
								</xsl:when>

								<xsl:when test="contains(substring(COL24,1,1),'S')">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								
								<xsl:when test="contains(substring(COL24,1,1),'C')">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:when test="contains(substring(COL24,1,1),'S')">
									<xsl:value-of select ="substring(COL24,2)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>

						<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</FundName>

						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="number(COL10)"/>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<SideTagValue>
							<xsl:choose>
								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="5"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="1"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name ="varCostBasis" select ="number(COL4)"/>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<PositionStartDate>
							<xsl:value-of select ="COL3"/>
						</PositionStartDate>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
