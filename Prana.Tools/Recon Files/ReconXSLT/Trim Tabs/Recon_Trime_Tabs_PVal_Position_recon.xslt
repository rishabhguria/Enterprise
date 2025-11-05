<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name ="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL6"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JPM'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Symbol" select="''"/>

						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="string-length(COL1) ='7'">
									<xsl:value-of select="'SEDOL'"/>
								</xsl:when>
								<xsl:when test="string-length(COL1) &gt; '7'">
									<xsl:value-of select="'CUSIP'"/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>

						<!--<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								--><!--<xsl:when test="$varSymbol ='SEDOL'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSymbol ='CUSIP'">
									<xsl:value-of select="''"/>
								</xsl:when>--><!--

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>-->


						<SEDOL>
							<xsl:choose>
								<!--<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>-->

								<xsl:when test="$varSymbol ='SEDOL'">
									<xsl:value-of select="COL1"/>
								</xsl:when>
								<!--<xsl:when test="$varSymbol ='CUSIP'">
									<xsl:value-of select="''"/>
								</xsl:when>-->

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</SEDOL>

						<CUSIP>
							<xsl:choose>
								<!--<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>-->

								<!--<xsl:when test="$varSymbol ='SEDOL'">
									<xsl:value-of select="''"/>
								</xsl:when>-->
								<xsl:when test="$varSymbol ='CUSIP'">
									<xsl:value-of select="COL1"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</CUSIP>

						<xsl:variable name="PB_FUND_NAME" select="COL16"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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


						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterParty>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID)">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterParty>

						<Quantity>
							<xsl:choose>
								<xsl:when  test="number($varQuantity)">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>



						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>
						<AvgPrice>
							<xsl:choose>
								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>

								</xsl:when>
								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPrice>


						<xsl:variable name="varMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="$varMarketValue &gt; 0">
									<xsl:value-of select="$varMarketValue"/>

								</xsl:when>
								<xsl:when test="$varMarketValue &lt; 0">
									<xsl:value-of select="$varMarketValue * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValueBase>

						<xsl:variable name ="Side" select="COL11"/>
						<Side>
							<xsl:choose>
								<xsl:when  test="$varQuantity &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when  test="$varQuantity &lt; 0">
									<xsl:value-of select="'Sell Short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>


						<Date>
							<xsl:value-of select ="''"/>
						</Date>


						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

