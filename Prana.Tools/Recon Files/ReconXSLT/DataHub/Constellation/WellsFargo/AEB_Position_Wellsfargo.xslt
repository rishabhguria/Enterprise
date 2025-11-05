<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>

	<xsl:template match="/">




		<DocumentElement>

			<xsl:for-each select ="//Comparision">


				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity) and contains(COL2,'Cash')!='true'">
						<PositionMaster>

							<xsl:variable name ="varPBName">
								<xsl:value-of select ="'Wells Fargo'"/>
							</xsl:variable>
							<xsl:variable name="PB_FUND_NAME" select="'AEB Harvest'"/>
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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



							<xsl:variable name ="PB_COMPANY">
								<xsl:value-of select ="COL4"/>
							</xsl:variable>
							<xsl:variable name="PRANA_SYMBOL">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name="PB_SUFFIX_NAME">
								<xsl:value-of select="substring-after(COL3,' ')"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
							</xsl:variable>


							<!--<xsl:variable name="Symbol" select="substring-before(COL9,' ')"/>-->
							<xsl:variable name="Symbol">
								<xsl:choose>
									<xsl:when test="contains(COL3,' ')">
										<xsl:value-of select="substring-before(COL3,' ')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL3"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL!=''">
										<xsl:value-of select="$PRANA_SYMBOL"/>
									</xsl:when>

									<xsl:when test ="$Symbol!=''">
										<xsl:value-of select ="concat($Symbol,$PRANA_SUFFIX_NAME)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$PB_COMPANY"/>
									</xsl:otherwise>
								</xsl:choose>

							</Symbol>

							<Asset>
								<xsl:choose>
									<xsl:when test="COL2='Equities'">
										<xsl:value-of select="'Equity'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Asset>

							<Quantity>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="$Quantity"/>

									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="$Quantity * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>

							<xsl:variable name="AvgPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL9"/>
								</xsl:call-template>
							</xsl:variable>


							<MarkPrice>
								<xsl:choose>
									<xsl:when test="$AvgPrice &gt; 0">
										<xsl:value-of select="$AvgPrice"/>

									</xsl:when>
									<xsl:when test="$AvgPrice &lt; 0">
										<xsl:value-of select="$AvgPrice * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>

								</xsl:choose>
							</MarkPrice>

							<!--<xsl:variable name="Side" select="COL1"/>-->

							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="'Buy'"/>

									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="'Sell short'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>

							<PBSymbol>
								<xsl:value-of select="$PB_COMPANY"/>
							</PBSymbol>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<xsl:variable name="varMarketValue">
								<xsl:value-of select="COL13"/>
							</xsl:variable>

							<xsl:variable name="varMarketValueBase">
								<xsl:value-of select="COL12"/>
							</xsl:variable>


							<MarketValue>
								<xsl:choose>
									<xsl:when test ="number($varMarketValue) ">
										<xsl:value-of select="$varMarketValue"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValue>

							<MarketValueBase>
								<xsl:choose>
									<xsl:when test ="number($varMarketValueBase) ">
										<xsl:value-of select="$varMarketValueBase"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>


							<!--<BaseCurrency>
							<xsl:value-of select="COL5"/>
						</BaseCurrency>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>-->

						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>

							
							<FundName>
								<xsl:value-of select="''"/>
							</FundName>




							<Symbol>
								<xsl:value-of select="''"/>

							</Symbol>

							<Asset>
								<xsl:value-of select="''"/>
							</Asset>

							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

						

							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>

							<!--<xsl:variable name="Side" select="COL1"/>-->

							<Side>
								<xsl:value-of select="''"/>
							</Side>

							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

						


							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>


							<!--<BaseCurrency>
							<xsl:value-of select="COL5"/>
						</BaseCurrency>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>-->

						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>
			
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>