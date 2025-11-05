<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:if test ="number(COL16) and COL10!='Cash and Cash Equivalents'">

					<PositionMaster>

						<xsl:variable name ="PB_NAME">
							<xsl:value-of select ="'BNY'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL11"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="COL12!='*'">
									<xsl:value-of select ="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<CUSIP>
							<xsl:choose>
								<xsl:when test ="COL12!='*'">
									<xsl:value-of select ="COL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>

						<SMRequest>
							<xsl:value-of select="'True'"/>
						</SMRequest>
						
						<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="COL2"/>
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

						<xsl:variable name ="varNetPosition">
							<xsl:value-of select ="number(COL16)"/>
						</xsl:variable>

						<Quantity>
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
						</Quantity>


						<Side>
							<xsl:choose >
								<xsl:when test ="$varNetPosition &lt;0">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>
								<xsl:when test ="$varNetPosition &gt;0">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name ="varCoastBasis">
							<xsl:value-of select ="number(COL14)"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="$varCoastBasis &gt;0">
									<xsl:value-of select ="$varCoastBasis"/>
								</xsl:when>
								<xsl:when test ="$varCoastBasis &lt;0">
									<xsl:value-of select ="$varCoastBasis*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<TradeDate>
							<xsl:value-of select ="COL8"/>
						</TradeDate>

						<xsl:variable name ="MarketValue">
							<xsl:value-of select ="number(COL20)"/>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test ="$MarketValue &gt;0">
									<xsl:value-of select ="$MarketValue"/>
								</xsl:when>
								<xsl:when test ="$MarketValue &lt;0">
									<xsl:value-of select ="$MarketValue*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>

		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
