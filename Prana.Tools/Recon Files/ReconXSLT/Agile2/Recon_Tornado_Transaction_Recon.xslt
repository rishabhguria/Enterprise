<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
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

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and not(contains(COL6,'FOAXX'))">
					<!--<xsl:if test="number($Position) and COL6!='38142B609' and COL2!='EXW'">-->

					<PositionMaster>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="string-length(COL6)=21">
									<xsl:value-of select="'Option'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BTIG_PORTAL'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" select="COL6"/>

						<Symbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="Symbol != ''">
									<xsl:value-of select="normalize-space($Symbol)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$Symbol"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="concat(substring($Symbol,1,21),'U')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</IDCOOptionSymbol>

						<xsl:variable name="PB_FUND_NAME" select="'Toronado Fund LP'"/>

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

						<Quantity>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPX">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPX &gt; 0">
									<xsl:value-of select="$AvgPX"/>

								</xsl:when>
								<xsl:when test="$AvgPX &lt; 0">
									<xsl:value-of select="$AvgPX * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>

						<xsl:variable name="Side" select="COL4"/>

						<Side>
							<xsl:choose>

								<xsl:when test="$Asset='Option'">
									<xsl:choose>

										<xsl:when test="$Side='BUY'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>

										<xsl:when test="$Side='SEL'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>

										<xsl:when test="$Side='BTC'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>

										<xsl:when test="$Side='SSL'">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test="$Side='BUY'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>

										<xsl:when test="$Side='SSL'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
										<xsl:when test="$Side='SEL'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										
										<xsl:when test="$Side='BTC'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<TradeDate>
							<xsl:value-of select="COL2"/>
						</TradeDate>

						<xsl:variable name="COL12">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL13">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Commission" select="COL12"/>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission * (-1)"/>

								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission* (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue "/>

								</xsl:when>
								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue* (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>