<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position" select="COL2"/>

				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Citrine'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME">
							<xsl:value-of select ="normalize-space(COL4)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" select ="normalize-space(COL4)"/>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL8)"/>

						<xsl:variable name ="PRANA_FUND_NAME">
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

						<xsl:variable name="Asset" select="normalize-space(COL1)"/>

						<xsl:variable name="Side" select="normalize-space(COL5)"/>

						<SideTagValue>
							<xsl:choose>

								<xsl:when test="$Asset='Future'">
									<xsl:choose>


										<xsl:when test="$Side='Buy'">
											<xsl:value-of select="'1'"/>
										</xsl:when>

										<xsl:when test="$Side='Sell'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								
								<xsl:when test="$Asset='FutureOption'">
									<xsl:choose>

										<xsl:when test="$Side='Buy'">
											<xsl:value-of select="'A'"/>
										</xsl:when>

										<xsl:when test="$Side='Sell'">
											<xsl:value-of select="'C'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<NetPosition>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetPosition>

						<xsl:variable name="Cost" select="number(COL6)"/>

						<CostBasis>
							<xsl:choose>

								<xsl:when test="$Cost &gt; 0">
									<xsl:value-of select="$Cost"/>
								</xsl:when>

								<xsl:when test="$Cost &lt; 0">
									<xsl:value-of select="$Cost * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<PositionStartDate>
							<xsl:value-of select="COL3"/>
						</PositionStartDate>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Fees" select="number(COL7)"/>

						<Fees>
							<xsl:choose>

								<xsl:when test="$Fees &gt; 0">
									<xsl:value-of select="$Fees"/>
								</xsl:when>

								<xsl:when test="$Fees &lt; 0">
									<xsl:value-of select="$Fees * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Fees>
						
						<xsl:variable name="PB_COUNTERPARTY_NAME" select="normalize-space(COL10)"/>

						<xsl:variable name="PRANA_COUNTERPARTY_ID">
							<xsl:value-of select="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PBBroker=$PB_COUNTERPARTY_NAME]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_COUNTERPARTY_ID)">
									<xsl:value-of select="$PRANA_COUNTERPARTY_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<CurrencyID>
							<xsl:choose>
								<xsl:when test="number(COL9)">
									<xsl:value-of select="COL9"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CurrencyID>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>