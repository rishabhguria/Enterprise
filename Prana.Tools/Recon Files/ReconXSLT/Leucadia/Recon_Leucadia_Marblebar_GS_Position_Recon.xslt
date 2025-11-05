<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name ="Quantity">
					<xsl:value-of select ="COL10"/>
				</xsl:variable>

				<xsl:if test ="number($Quantity) and not(contains(COL4,'RTS/'))">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select ="COL12"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME and $PB_CURRENCY_NAME = @Currency]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL6,'.')"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varExFactor">
							<xsl:value-of select="substring(COL6,string-length(substring-before(COL6,'.')),1)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL6!=''">
									<xsl:choose>

										<xsl:when test="$varExFactor='a'">
											<xsl:value-of select="concat(substring(COL6,1,string-length(substring-before(COL6,'.'))-1),'_a',$PRANA_SUFFIX_NAME,'/SWAP')"/>
										</xsl:when>

										<xsl:when test="$varExFactor='b'">
											<xsl:value-of select="concat(substring(COL6,1,string-length(substring-before(COL6,'.'))-1),'_b',$PRANA_SUFFIX_NAME,'/SWAP')"/>
										</xsl:when>

										<xsl:when test="$PRANA_SUFFIX_NAME!=''">
											<xsl:value-of select="concat(substring-before(COL6,'.'),$PRANA_SUFFIX_NAME,'/SWAP')"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="concat(COL6,'/SWAP')"/>
										</xsl:otherwise>
									</xsl:choose>
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

						<Quantity>

							<xsl:choose>

								<xsl:when test ="number($Quantity)">
									<xsl:value-of select ="$Quantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Quantity>


						<ISINSymbol>
							<xsl:value-of select ="COL7"/>
						</ISINSymbol>

						<CurrencySymbol>
							<xsl:value-of select ="COL12"/>
						</CurrencySymbol>

						<xsl:variable name="varSide" select="COL3"/>

						<Side>
							<xsl:choose>
								<xsl:when test ="$varSide ='Long'">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>

								<xsl:when test ="$varSide ='Short'">
									<xsl:value-of select ="'Sell'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name ="MarkPrice">
							<xsl:value-of select="number(COL14)"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test ="$MarkPrice &lt;0">
									<xsl:value-of select ="$MarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$MarkPrice &gt;0">
									<xsl:value-of select ="$MarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<xsl:variable name ="MarketValue">
							<xsl:value-of select="number(COL20)"/>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>

						

								<xsl:when test ="$MarketValue ">
									<xsl:value-of select ="$MarketValue"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValue>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

