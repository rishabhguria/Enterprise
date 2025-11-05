<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				
				<xsl:variable name ="NetPosition">
					<xsl:value-of select ="COL22"/>
				</xsl:variable>
				
				<xsl:if test ="number($NetPosition) and COL9!='Dividends' and COL10!='Cancel'">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select ="COL30"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME and @Currency=$PB_CURRENCY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL26,'.')"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<xsl:when test="$PRANA_SUFFIX_NAME!=''">
									<xsl:value-of select="concat(substring-before(COL26,'.'),$PRANA_SUFFIX_NAME)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
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
							<xsl:value-of select ="COL15"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select ="COL16"/>
						</PositionSettlementDate>
						
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

						<xsl:variable name="varSide" select="COL7"/>

						<xsl:variable name="varCheck" select="COL8"/>

						
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varCheck='Open'">
									<xsl:choose>
										<xsl:when test ="$varSide ='Buy'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>

										<xsl:when test ="$varSide ='Sell'">
											<xsl:value-of select ="'5'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$varSide ='Buy'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>

										<xsl:when test ="$varSide ='Sell'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
							
						</SideTagValue>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select="COL42"/>
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

						<xsl:variable name="Commission">
							<xsl:value-of select="translate(COL51,',','')"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($Commission) &lt; 0'>
									<xsl:value-of select ="$Commission*-1"/>
								</xsl:when>
								<xsl:when test ='number($Commission) &gt; 0'>
									<xsl:value-of select ='$Commission'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>


						<!--<xsl:variable name="StampDuty">
							<xsl:value-of select="translate(COL29,',','')"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test ='number($StampDuty) &lt; 0'>
									<xsl:value-of select ="$StampDuty*-1"/>
								</xsl:when>
								<xsl:when test ='number($StampDuty) &gt; 0'>
									<xsl:value-of select ='$StampDuty'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>-->

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
							<xsl:value-of select ="COL15"/>
						</OrigTransDate>

						<xsl:variable name="varPrevMonth">
							<xsl:choose>
								<xsl:when test ="number(substring-before(COL15,'/')) = 1">
									<xsl:value-of select ="12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="number(substring-before(COL15,'/'))-1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name ="varYear">
							<xsl:choose>
								<xsl:when test ="number(substring-before(COL15,'/')) = 1">
									<xsl:value-of select ="number(substring-after(substring-after(COL15,'/'),'/')) -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="number(substring-after(substring-after(COL15,'/'),'/'))"/>
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
