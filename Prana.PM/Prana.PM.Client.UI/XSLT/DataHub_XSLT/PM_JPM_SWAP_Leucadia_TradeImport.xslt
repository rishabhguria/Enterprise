<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL23) and COL20!='Cancelled'">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'JPM'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL17"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL17,' ')"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<xsl:when test="$PRANA_SUFFIX_NAME!=''">
									<xsl:value-of select="concat(substring-before(COL17,' '),$PRANA_SUFFIX_NAME)"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL3"/>
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

						<xsl:variable name="PRANA_STRATEGY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name=$PB_NAME]/StrategyData[@PranaFundName=$PRANA_FUND_NAME]/@PranaStrategy"/>
						</xsl:variable>

						<Strategy>
							<xsl:value-of select ="$PRANA_STRATEGY_NAME"/>
						</Strategy>

						<PositionStartDate>
							<xsl:value-of select ="COL12"/>
						</PositionStartDate>

						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="COL23"/>
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

						<xsl:variable name="varSide" select="COL22"/>



						<SideTagValue>
							<xsl:choose>
								<xsl:when test ="$varSide ='B'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>

								<xsl:when test ="$varSide ='S'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>


						<xsl:variable name ="varCostBasis">
							<xsl:value-of select="COL41"/>
						</xsl:variable>

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

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<!--CFD Entries-->

						<IsSwapped>
							<xsl:value-of select ="1"/>
						</IsSwapped>

						<SwapDescription>
							<xsl:value-of select ="'SWAP'"/>
						</SwapDescription>

						<DayCount>
							<xsl:value-of select ="365"/>
						</DayCount>

						<ResetFrequency>
							<xsl:value-of select ="'Monthly'"/>
						</ResetFrequency>

						<OrigTransDate>
							<xsl:value-of select ="COL12"/>
						</OrigTransDate>

						<xsl:variable name="varPrevMonth">
							<xsl:choose>
								<xsl:when test ="number(substring-before(COL12,'/')) = 1">
									<xsl:value-of select ="12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="number(substring-before(COL12,'/'))-1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name ="varYear">
							<xsl:choose>
								<xsl:when test ="number(substring-before(COL12,'/')) = 1">
									<xsl:value-of select ="number(substring-after(substring-after(COL12,'/'),'/')) -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="number(substring-after(substring-after(COL12,'/'),'/'))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<FirstResetDate>
							<xsl:value-of select ="concat($varPrevMonth,'/28/',$varYear)"/>
						</FirstResetDate>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
