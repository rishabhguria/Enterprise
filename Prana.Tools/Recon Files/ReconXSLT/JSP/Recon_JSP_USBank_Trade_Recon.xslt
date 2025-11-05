<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
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
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL20"/>
					</xsl:call-template>
				</xsl:variable>
					<xsl:variable name="varSide" select="COL41"/>
				<xsl:if test="number($Quantity) and (COL7 !='USBGFS3') and (contains($varSide, 'SOLD') or contains($varSide, 'PURCHASED') )">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'USBank'"/>
						</xsl:variable>
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL7)" />
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<xsl:when test="$varSymbol!='' or $varSymbol!='*'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME">
							<xsl:value-of select="COL5"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<Quantity>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>

								<xsl:when test="$Quantity &lt; 0">
									<xsl:value-of select="$Quantity * -1"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>


						<xsl:variable name="varDate">
							<xsl:value-of select="COL2"/>
						</xsl:variable>
						<TradeDate>
							<xsl:value-of select ="$varDate"/>
						</TradeDate>

						<xsl:variable name="varSettlementDate">
							<xsl:value-of select="COL3"/>
						</xsl:variable>
						<SettlementDate>
							<xsl:value-of select ="$varSettlementDate"/>
						</SettlementDate>


						
						<Side>
							<xsl:choose>
								<xsl:when  test="contains($varSide, 'SOLD')">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>

								<xsl:when  test="contains($varSide, 'PURCHASED')">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>


						<xsl:variable name="varAvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL21"/>
							</xsl:call-template>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$varAvgPrice &gt; 0">
									<xsl:value-of select="$varAvgPrice"/>
								</xsl:when>
								<xsl:when test="$varAvgPrice &lt; 0">
									<xsl:value-of select="$varAvgPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<xsl:variable name="varTotalCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL24 + number(COL28)"/>
							</xsl:call-template>
						</xsl:variable>
						<TotalCommissionandFees>
							<xsl:choose>
								<xsl:when test="$varTotalCommission &gt; 0">
									<xsl:value-of select="$varTotalCommission"/>
								</xsl:when>
								<xsl:when test="$varTotalCommission &lt; 0">
									<xsl:value-of select="$varTotalCommission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCommissionandFees>


						<xsl:variable name="varNetAmountLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL22"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$varNetAmountLocal &gt; 0">
									<xsl:value-of select="$varNetAmountLocal"/>
								</xsl:when>
								<xsl:when test="$varNetAmountLocal &lt; 0">
									<xsl:value-of select="$varNetAmountLocal * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="varNetAmountBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="0"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$varNetAmountBase &gt; 0">
									<xsl:value-of select="$varNetAmountBase"/>
								</xsl:when>
								<xsl:when test="$varNetAmountBase &lt; 0">
									<xsl:value-of select="$varNetAmountBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>